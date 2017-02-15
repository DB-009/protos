using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class _touch : MonoBehaviour
{

    public float minSwipeDistY;
    public float minSwipeDistX;
    private Vector2 startPos;
    public Camera targetCam;

    //-------------------------
    public Vector3 initPos;


    //Player Character stats
    public float str, intel, def, vit, spd, hp, mhp, mp, mmp, lvl, baseHP, baseMp, mvspd;

    //player Game Stats
    public float myID;


    public Rigidbody2D rb;
    //movement
    public float fwdSpd = 0, sidSpd = 0;
    public enum directionMoving { not, left, right, up, down };
    public directionMoving dir,shotDir;

    //public float maxSpeed = 5f;

    bool grounded = false;
    public bool isMoving;

    public float jumpForce;

    //-------------------------

    public GameObject bulletPrefab;
    public float bulForce, fireRate, lastShotAt, bulletAcceptedDistance;

    public int wepID;
    public int comboCur, hits, kills;
    public float lastComboAt, comboStarted, allowedComboTime, comboLimit;

    //temporary combo system
    public GameObject comboPrefabA, comboPrefabB, comboPrefabC;



    //-----------------
    public float camOffsetY, yDis;

    public bool zoomOUtOnJump,playZoomedOut;
    public float zoomLvl,initialZoomZ, JumpZoomIncrementZ, initialZoomX, JumpZoomIncrementX , JumpZoomIncrementY, initialZoomY, curZ, CurX , CurY;

    //------------------






    public AudioClip JumpSfx;

    public bool SfxTrigger = false;

    public bool isJumping;
    public bool doubleJump = false;
    public bool isDoubleJumping = false;
    public float doubleJumpedAt;

    public Text dirText;


    //wall jump variables
    public float wallTimeStart, wallTimeLimit, lastWallExitTIme, wallAllowedStoreTime;
    public bool onWall, jumpThrough;
    public GameObject lastWall;

    //wall jump 2d fixes
    public bool tempSpeedUp;
    public float initGravity;


    //cannon variables
    public float timeBetweenCanon, LastCannonAt, noGravityTimeLimit;

    //raycast variables
    public float upCheck, horCheck, downCheck;

    public List<GameObject> lastCannon = new List<GameObject>();



    void Awake()
    {

        targetCam = Camera.main;
        targetCam.GetComponent<CameraTracking>().activePlayer = this.gameObject;


        initPos = this.transform.position;
        initialZoomZ = targetCam.orthographicSize;
        initialZoomX = targetCam.GetComponent<CameraTracking>().xPos;
        initialZoomY = targetCam.GetComponent<CameraTracking>().yPos;
        zoomLvl = 2;
        curZ = initialZoomZ;
        CurX = initialZoomX;
        CurY = initialZoomY;

        rb = this.GetComponent<Rigidbody2D>();


        minSwipeDistY = Screen.height * .25f;
        minSwipeDistX = Screen.width * .25f;
    }




    void Update()
    {
        fwdSpd = 0;
        if(tempSpeedUp == false)
        sidSpd = 0;

        if (playZoomedOut == true)
        {
            
             //   targetCam.orthographicSize = initialZoomZ + JumpZoomIncrementZ;
              //  targetCam.GetComponent<CameraTracking>().xPos = initialZoomX + JumpZoomIncrementX;
            
        }


        if (isJumping  == true)
        {
            //change zoom level while jumping
            if (zoomOUtOnJump == true)
            {
             //   targetCam.orthographicSize = initialZoomZ + JumpZoomIncrementZ;
             //   targetCam.GetComponent<CameraTracking>().xPos = initialZoomX + JumpZoomIncrementX;
            }
        }

        if (isDoubleJumping == true)
        {

            //change zoom level while jumping
            if (zoomOUtOnJump == true)
            {
              //  targetCam.orthographicSize = initialZoomZ + JumpZoomIncrementZ;
               // targetCam.GetComponent<CameraTracking>().xPos = initialZoomX + JumpZoomIncrementX;
            }

            if (Time.time >= doubleJumpedAt + 1)
            {
                isDoubleJumping = false;


            }

        }



        //Wall jump functions
        //if player has been holding wall for x time make em fall
        if (onWall == true)
        {
            if (Time.time >= wallTimeStart + wallTimeLimit)
            {
                Debug.Log("free fall");
                onWall = false;
                rb.isKinematic = false;
                lastWallExitTIme = Time.time;
            }
        }
        else//delete wall after x amount of seconds so he can regrab
        {
            if (Time.time >= lastWallExitTIme + wallAllowedStoreTime && lastWallExitTIme != 0 && lastWall != null)
            {
                Debug.Log("deleted wall");
                lastWall = null;
                lastWallExitTIme = 0;
            }
        }

        if (rb.gravityScale == 0 && rb.isKinematic == false && onWall == false)
        {
            if (Time.time >= LastCannonAt + noGravityTimeLimit)
            {
               if( Time.time >= lastWallExitTIme + .5f && lastWall!=null)
                {
                    rb.gravityScale = initGravity;
                    Debug.Log("hmm fall" + Time.time);
                    tempSpeedUp = false;
                }
            }
        }






        _touchControls();
        detectMove();
        //SpeedUpates();

        

        DetectAttack();



        if ((sidSpd > 0 || sidSpd < 0) || (fwdSpd >0 || fwdSpd <0))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }


    }


    void FixedUpdate()
    {

        if (grounded)
            doubleJump = false;

        //float move = 1;
        //rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

        if(rb!= null && onWall == false)
        {
            rb.AddForce(new Vector3(sidSpd, 0), ForceMode2D.Force);//Add a force to my players RigidBody.
                                                                   //this.gameObject.transform.Translate(sidSpd, 0, fwdSpd);
                                                                   //this.gameObject.transform.Translate(sidSpd * Time.fixedDeltaTime, 0, fwdSpd * Time.fixedDeltaTime);
                                                                   //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
        }





    }

    /*
	IEnumerator Flanking(){
		yield return new WaitForSeconds(0.2f);
		Flank = false;
		FlankTime = false;
		yield return new WaitForSeconds(2);
		FlankTime = true;
	}
	*/

 void _touchControls()
    {


        //#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];



            switch (touch.phase)

            {

                case TouchPhase.Began:

                    startPos = touch.position;

                    break;



                case TouchPhase.Ended:

                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeDistVertical > swipeDistHorizontal)

                    {

                        float swipeValue = Mathf.Sign(touch.position.y - startPos.y);

                        if (swipeValue < 0)
                        {
                            Debug.Log("DOwn Swype");

                            dir = directionMoving.down;
                            dirText.text = "spike";

                            Spike();
                        }
                        else
                        {
                            Debug.Log("Up Swype");
                            //  fwdSpd = mvspd;
                            //myPlayer.dirFacing = Player.PlayerDirection.N;
                            dir = directionMoving.up;
                            dirText.text = "jump";
                            Jump();

                        }

                      


                    }
                    else if (swipeDistHorizontal > swipeDistVertical)

                    {

                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);

                        if (swipeValue < 0)
                        {
                            Debug.Log("left Swype");
                            //  sidSpd = -mvspd;
                            dir = directionMoving.left;
                            dirText.text = "left";
                        }
                        else
                        {
                            Debug.Log("right Swype");

                            dir = directionMoving.right;
                            dirText.text = "right";


                        }

                        Debug.Log("Touch flank");



                    }
                    break;
            }
        }
    }

    public void detectMove()
    {


        if (Input.GetKeyDown(KeyCode.W))
        {
            //  fwdSpd = mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.N;
            
            dirText.text = "jump";
           


            //jump
            if (grounded == true && onWall == false)
                Jump();
            else if (isDoubleJumping == false && onWall == false)
            {
                Jump();
                isDoubleJumping = true;
            }
            else if (onWall == true)
            {//if my player is grounded do whats in the barckets

                rb.isKinematic = false;
                tempSpeedUp = true;
                if (sidSpd < 0)
                    sidSpd = -mvspd;
                else if (sidSpd > 0)
                    sidSpd = mvspd;
                Debug.Log("wall jump" + Time.time);
                onWall = false;
                lastWallExitTIme = Time.time;
                rb.AddForce(new Vector2(sidSpd, jumpForce), ForceMode2D.Impulse);
                

            }


        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //  fwdSpd = -mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.S;

         
            dirText.text = "spike";

            Spike();
        }

        if (Input.GetKey(KeyCode.A))
        {


              sidSpd = -mvspd;
          
            dirText.text = "left";



        }
        else if (Input.GetKey(KeyCode.D))
        {

           
            dirText.text = "right";

              sidSpd = mvspd;


        }


    }

    public void SpeedUpates()
    {
        if (dir == directionMoving.left)
        {
            sidSpd = -mvspd;
        }
        else if (dir == directionMoving.right)
        {
            sidSpd = mvspd;

        }
        else if (dir == directionMoving.up)
        {
            fwdSpd = mvspd;

        }
        else if (dir == directionMoving.down)
        {
            fwdSpd = -mvspd;

        }
    }

    public void Jump()
    {
     

        if ((grounded || !doubleJump))
        {

                if (!doubleJump && !grounded)//double jump
                {
                    doubleJump = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                
                    isDoubleJumping = true;
                    doubleJumpedAt = Time.time;

                }
                else if (grounded && !doubleJump)//normal jump
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);

                isJumping = true;
                }

            



        }
    }


    public void Spike()
    {



            if (doubleJump )//double jump
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0f, -jumpForce),ForceMode2D.Impulse);
                Debug.Log("Add spiked at tiem updates like jumped at");
               

            }
            else if (isJumping == true || grounded == false)//normal jump
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0f, -jumpForce), ForceMode2D.Impulse);
                Debug.Log("Add spiked at tiem updates like jumped at");

            }
            else if(grounded == true)
        {
            Debug.Log("Should add dash to down spike here");

        }






    }


    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {


            grounded = true;
            isJumping = false;
            isDoubleJumping = false;
            doubleJump = false;


            dir = directionMoving.not;



            //change zoom level while jumping
            if (zoomOUtOnJump == true && playZoomedOut == false)
            {
            //    targetCam.orthographicSize = initialZoomZ ;
            //    targetCam.GetComponent<CameraTracking>().xPos = initialZoomX;
            }
        }
        else if (col.collider.tag == "wall" && grounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {

            if (lastWall != col.gameObject)
            {
                Debug.Log("Eh enterd;");

                rb.isKinematic = true;
                rb.gravityScale = 0;
                onWall = true;
                wallTimeStart = Time.time;
                
                lastWall = col.gameObject;
            }
        }
    }

    public void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.tag == "wall" && grounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            if (lastWall != col.gameObject)
            {
                Debug.Log("Eh colsaty;");

                rb.isKinematic = true;
                rb.gravityScale = 0;
                wallTimeStart = Time.time;
                onWall = true;
                lastWall = col.gameObject;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            grounded = false;

            isDoubleJumping = false;
        }
        else if (col.collider.tag == "wall" && grounded == false)//if the object you collided withs tag is ground your player is on the floor
        {
          

        }
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Respawn")
        {
            rb.velocity = Vector3.zero;
            transform.position = initPos;
            dir = directionMoving.not;

        }
    }


    public void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Respawn")
        {
            rb.velocity = Vector3.zero;
            dir = directionMoving.not;
            transform.position = initPos;

        }
    }



    public void DetectAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))//combo / button mash inputs should be  on GetKeyDown
        {
            if (wepID == 1)
            {
                Debug.Log("in combo");
                Combo();
            }

        }

        if (Input.GetKey(KeyCode.Mouse0))//guns  and rapid fire should be called on Hold mouse fire
        {
           // if (wepID == 0)
           // {
            //    if (Time.time >= lastShotAt + fireRate)
            //        Shoot(bulletPrefab, bulForce);
           // }

        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (wepID == 0)
            {
                wepID = 1;
            }
            else if (wepID == 1)
            {
                wepID = 0;
            }
        }

    }





    public void Shoot(GameObject bulz, float force, directionMoving shotDirection)
    {
        /*
        {
        RAYCAST SHOT DONT DELETE

        // Get the point along the ray that hits the calculated distance.
        Ray targetPoint = targetCam.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = Vector3.zero;

        Vector3 pos = targetPoint.GetPoint(0);



        direction = pos - new Vector3(targetCam.transform.position.x, targetCam.transform.position.y - yDis, targetCam.transform.position.z);

        direction.Normalize();
        //Debug.Log("Dir = " + direction + " true x = " + direction.x + " true y = " + direction.y);

        if (direction.x < 0 && direction.x > -bulletAcceptedDistance)
        {
            if (direction.x > -bulletAcceptedDistance)
            {

                if (direction.y < 0 && direction.y > -bulletAcceptedDistance)
                {

                    Debug.Log("1 left side - bottom y - slow shot");

                }
                else if (direction.y > 0 && direction.y < bulletAcceptedDistance)
                {

                    Debug.Log("2 left side - top y - slow Shot");
                }
                else
                {
                    Debug.Log("quick shot; doesnt matter");

                }
            }
            else
            {
                Debug.Log("quick shot; doesnt matter");

            }

        }
        else if (direction.x > 0 && direction.x < bulletAcceptedDistance)
        {

            if (direction.x < bulletAcceptedDistance)
            {
                if (direction.y < 0 && direction.y > -bulletAcceptedDistance)
                {

                    Debug.Log("3 right side - bottom y - slow shot");

                }
                else if (direction.y > 0 && direction.y < bulletAcceptedDistance)
                {

                    Debug.Log("4 right side - top y - slow Shot");
                }
                else
                {
                    Debug.Log("quick shot; doesnt matter");

                }
            }


        }
        else
        {
            Debug.Log("quick shot; doesnt matter");
        }

        Vector3 bulPosition = Vector3.zero;
        if (isMoving == true)
        {
            if (sidSpd < 0 && direction.x < 0)
            {
                bulPosition = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd < 0 && direction.x > 0)
            {
                bulPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x > 0)
            {
                bulPosition = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x < 0)
            {
                bulPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            }
        }
        else
        {
            bulPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }

        GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;




        Quaternion targetRotation = Quaternion.LookRotation(pos - tileCreated.transform.position);

        // Smoothly rotate towards the target point.
        tileCreated.transform.rotation = targetRotation;




        tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, direction.y, 0) * force, ForceMode.Impulse);
        lastShotAt = Time.time;
        }
        */


        Vector3 bulPosition = Vector3.zero;
        float dire = 1;
 

                if(shotDirection == directionMoving.left)
                {
                    bulPosition = new Vector3(this.transform.position.x - 1, this.gameObject.transform.position.y, this.transform.position.z);
                    dire = -1;
                }
                else if (shotDirection == directionMoving.right)
                {
                    bulPosition = new Vector3(this.transform.position.x + 1, this.gameObject.transform.position.y, this.transform.position.z);
                    dire = +1;
                }
            
  


        GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;


        tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);
        lastShotAt = Time.time;
    }





    public void Combo()
    {
        GameObject comboObj = comboPrefabA;




        if (comboCur == 0)//com just started
        {
            comboStarted = Time.time;
            comboObj = comboPrefabA;

            lastComboAt = Time.time;
            comboCur++;
        }
        else
        {


            if (comboCur == 1)
            {
                comboObj = comboPrefabB;
            }
            else if (comboCur == 2)
            {
                comboObj = comboPrefabC;
            }

            if (Time.time > lastComboAt + allowedComboTime)//if not in time to continue combo start new combo
            {
                comboStarted = Time.time;
                lastComboAt = Time.time;
                comboObj = comboPrefabA;
                comboCur = 1;
            }
            else if (comboCur == comboLimit - 1)//if this is last combo before restart
            {
                comboStarted = Time.time;
                lastComboAt = Time.time;
                comboCur = 0;
            }
            else//else just add contniue combo
            {
                lastComboAt = Time.time;
                comboCur++;
            }



        }

        // Get the point along the ray that hits the calculated distance.
        Ray targetPoint = targetCam.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = Vector3.zero;

        Vector3 pos = targetPoint.GetPoint(0);


        direction = pos - new Vector3(targetCam.transform.position.x, targetCam.transform.position.y - camOffsetY, targetCam.transform.position.z);

        direction.Normalize();
        Debug.Log("Dir = " + direction + " true x = " + direction.x + " true y = " + direction.y);
        int tempXdis = 0;
        if (direction.x < 0)
        {


            tempXdis = -1;



        }
        else if (direction.x > 0)
        {

            tempXdis = +1;


        }

        Vector3 objPos = Vector3.zero;
        if (isMoving == true)
        {
            if (sidSpd < 0 && direction.x < 0)
            {
                objPos = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd < 0 && direction.x > 0)
            {
                objPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x > 0)
            {
                objPos = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x < 0)
            {
                objPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            }
        }
        else
        {
            objPos = new Vector3(this.transform.position.x + tempXdis, this.transform.position.y, this.transform.position.z);
        }

        GameObject tileCreated = GameObject.Instantiate(comboObj, objPos, Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;




        Quaternion targetRotation = Quaternion.LookRotation(pos - tileCreated.transform.position);

        // Smoothly rotate towards the target point.
        tileCreated.transform.rotation = targetRotation;





    }






}

