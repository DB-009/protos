using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GETP_Controller : MonoBehaviour
{


    public GETP_GameManager gameStateManager;

 
    public Transform myTrans;


    public float minSwipeDistY;
    public float minSwipeDistX;
    private Vector2 startPos;

    public Slider hpSlider;
  
    public PlayerClasses playerClass;

    public CameraTracking cam;

    public Rigidbody rb;

    //player variables for movement
    public bool isController, isGrounded, dblJumped, cannonBackward,canMove,matchStarted;
    public float jumpHeight;


    public float jumpForce = 700f;

    public bool isJumping, canDblJump;
    public bool doubleJump = false;
    public bool isDoubleJumping = false;
    public float doubleJumpedAt;
    //wall jump variables

    //raycast variables
    public float upCheck, horCheck, downCheck;

    public bool allowZmovement;
    public enum directionMoving { not, left, right, up, down };
    public directionMoving dir;



    public enum PlayerClasses
    {
        Fighter,
        Mage,
        Healer,
        Archer,
    }



    //Player Character stats
    public float str, intel, def, vit, spd, hp, mhp, mp, mmp, lvl, baseHP, baseMp, mvspd;

    //player Game Stats
    public float myID;



    public Vector3 initPos;


    public float rTimer1start, rTimer1end, rTimer2start, rTimer2end;

    public bool inBattle, drawn;


    public float xDis, yDis;


    //movement
    public float fwdSpd = 0, sidSpd = 0;

    public bool lockedOnTarget;

    public int curPath;
    public List<int> pathList = new List<int>();
    public List<GameObject> pathListObjs = new List<GameObject>();

    public GameObject trigger;


    public GameObject bulletPrefab;
    public float bulForce, fireRate, lastShotAt, bulletAcceptedDistance;

    public int kills;

    public objGen eneGen1, eneGen2;

    public enum gunType
    {
        normal,
        rapidFire,
        spreadShot,
        triShot,
        hadoken,
    }

    public gunType curGun;

    public float spreadShotAngle,jetPack_fuel,jetPack_rate, jetPack_force,gunUpgradedAt,gunUpgradeTime;
    public bool jetPack,gunDoubleUpgrade;

    public float screen_half = Screen.width / 2;

    public float diedAT,respawnTime;
    public bool isALive;

    public void Awake()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.myTrans = this.transform;
        this.hp = this.vit * this.lvl + this.baseHP;
        this.mp = this.intel * this.lvl + this.baseMp;
        this.mhp = this.hp;
        this.mmp = this.mp;


  
        if(gameStateManager != null)
        {
            this.hpSlider = this.gameStateManager.hpSlider1;


            this.hpSlider.maxValue = this.mhp;
            this.hpSlider.value = this.hp;

            if (this.cam == null)
                this.cam = Camera.main.GetComponent<CameraTracking>();

            this.gameStateManager.left.myHero = this;
            this.gameStateManager.right.myHero = this;

            this.gameStateManager.shootLeft.myHero = this;
            this.gameStateManager.shootRight.myHero = this;

            foreach (GameObject disGen in this.gameStateManager.gens)
            {
                disGen.GetComponent<objGen>().targetPlayer = this.gameObject;
            }


        }
      


        upCheck = gameObject.transform.localScale.y / 2 + .2f;
        downCheck = upCheck;

    }





    public void CmdShoot(int dire)
    {

       

        
   

        float force = bulForce;

        GameObject bulz = bulletPrefab;



        if (curGun == gunType.normal)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x +dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;

        }
        else if (curGun == gunType.spreadShot)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x +dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x +dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);
            var tileCreated3 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x +dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;

            tileCreated.GetComponent<Rigidbody>().velocity = ((myTrans.right + new Vector3(0, 0, spreadShotAngle)) * dire) * bulForce;

            tileCreated2.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;


            tileCreated3.GetComponent<Rigidbody>().velocity = ((myTrans.right + new Vector3(0, 0, -spreadShotAngle)) * dire) * bulForce;



        }
        else if (curGun == gunType.triShot)
        {
            var tileCreated = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z ) + new Vector3(0, 0, -.5f), Quaternion.identity) as GameObject;
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z ), myTrans.rotation);
            var tileCreated3 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z ) - new Vector3(0, 0, -.5f), Quaternion.identity) as GameObject;

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;

            tileCreated2.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;


            tileCreated3.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;


            if (gunDoubleUpgrade == true)
            {
                var tileCreated4 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z) + new Vector3(0, 0, 1), Quaternion.identity) as GameObject;

                tileCreated4.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated4.GetComponent<projectileLife>().playerBullet = true;

                var tileCreated5 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z) - new Vector3( 0,0, 1), Quaternion.identity) as GameObject;

                tileCreated5.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated5.GetComponent<projectileLife>().playerBullet = true;


    
            tileCreated4.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;


            tileCreated5.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;



            }


        }


        lastShotAt = Time.time;
    }





    public void CmdShootVer(int dire)
    {






        float force = bulForce;

        GameObject bulz = bulletPrefab;



        if (curGun == gunType.normal)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x , myTrans.position.y, myTrans.position.z + dire), myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (myTrans.forward * dire) * bulForce;
            Debug.Log("for" + myTrans.forward * dire);
        }
        else if (curGun == gunType.spreadShot)
        {

            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x, myTrans.position.y, myTrans.position.z + dire), myTrans.rotation);
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x , myTrans.position.y, myTrans.position.z + dire), myTrans.rotation);
            var tileCreated3 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x , myTrans.position.y, myTrans.position.z + dire), myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;



            tileCreated.GetComponent<Rigidbody>().velocity = ((myTrans.forward + new Vector3(spreadShotAngle,0,0) )* dire) * bulForce;

            tileCreated2.GetComponent<Rigidbody>().velocity = (myTrans.forward * dire) * bulForce;


            tileCreated3.GetComponent<Rigidbody>().velocity = ((myTrans.forward + new Vector3(-spreadShotAngle, 0, 0)) * dire) * bulForce;



        }
        else if (curGun == gunType.triShot)
        {
            var tileCreated = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x, myTrans.position.y, myTrans.position.z + dire) + new Vector3( -.5f,0, 0), Quaternion.identity) as GameObject;
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x, myTrans.position.y, myTrans.position.z + dire), myTrans.rotation);
            var tileCreated3 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x, myTrans.position.y, myTrans.position.z + dire) - new Vector3( -.5f,0, 0), Quaternion.identity) as GameObject;

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (myTrans.forward * dire) * bulForce;

            tileCreated2.GetComponent<Rigidbody>().velocity = (myTrans.forward * dire) * bulForce;


            tileCreated3.GetComponent<Rigidbody>().velocity = (myTrans.forward * dire) * bulForce;


            if (gunDoubleUpgrade == true)
            {
                var tileCreated4 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x, myTrans.position.y, myTrans.position.z + dire) + new Vector3( 1,0, 0), Quaternion.identity) as GameObject;

                tileCreated4.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated4.GetComponent<projectileLife>().playerBullet = true;

                var tileCreated5 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x, myTrans.position.y, myTrans.position.z + dire) - new Vector3( 1,0, 0), Quaternion.identity) as GameObject;

                tileCreated5.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated5.GetComponent<projectileLife>().playerBullet = true;


                tileCreated4.GetComponent<Rigidbody>().velocity = (myTrans.forward * dire) * bulForce;


                tileCreated5.GetComponent<Rigidbody>().velocity = (myTrans.forward * dire) * bulForce;



            }


        }


        lastShotAt = Time.time;
    }





    public void Update()
    {

        //reset input remove this for accelartion based movement if remove add limit
        //fwdSpd = 0;
        //sidSpd = 0;

  
            if (hp <= 0 && isALive == true)
            {
            canMove = false;
            diedAT = Time.time;
            isALive = false;
            this.transform.gameObject.SetActive(false);

        }
        //Wall jump functions


        if (isController == true)
            {
                if (canMove != false)
                {

                    _touchControls();

                    //inputs
                    OnDown();
                    OnHold();
                    OnUp();



                    SpeedUpates();

                }
                else
                {
                    _touchControls();

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        StageProgression();

                    }
                }



                HorColCheck();
            ZetaColCheck();
            }


        








    }

    void FixedUpdate()
    {


            if (jetPack)
            {
                rb.AddForce(Physics.gravity * rb.mass);
                rb.useGravity = false;
            }

            // rb.AddForce(new Vector3(sidSpd, fwdSpd, 0),ForceMode.Force);//Add a force to my players RigidBody.
            //this.gameObject.transform.Translate(sidSpd, 0, fwdSpd);
            this.gameObject.transform.Translate(sidSpd * Time.fixedDeltaTime, 0, fwdSpd * Time.fixedDeltaTime);
            //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
        

    }








    void _touchControls()
    {


        //#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];



            switch (touch.phase)

            {

                case TouchPhase.Began:



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

                           
                           // dirText.text = "spike";

                            // Spike();
                        }
                        else
                        {
                            Debug.Log("Up Swype");
                            if(matchStarted == false)
                            StageProgression();
                            //  fwdSpd = mvspd;
                            //myPlayer.dirFacing = Player.PlayerDirection.N;
                           
                           // dirText.text = "jump";
                           //  Jump();

                        }




                    }
                    else if (swipeDistHorizontal > swipeDistVertical)

                    {

                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);

                        if (swipeValue < 0)
                        {
                            Debug.Log("left Swype");

                                      //  sidSpd = -mvspd;
                                                         // dir = directionMoving.left;
                                                         // dirText.text = "left";
                        }
                        else
                        {
                            Debug.Log("right Swype");
                        
                            // dir = directionMoving.right;
                            // dirText.text = "right";


                        }




                    }


                    break;
            }
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


    private void HorColCheck()
    {
        RaycastHit hitL, hitR;
        bool isLeft = Physics.Raycast(this.transform.position, -this.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(this.transform.position, this.transform.right, out hitR, horCheck);
        Debug.DrawRay(this.transform.position, -this.transform.right, Color.green);
        Debug.DrawRay(this.transform.position, this.transform.right, Color.green);

        if (isRight == true)
        {
           // Debug.Log("There is right next to me!");
            if (hitR.transform.gameObject.tag == "Player" || hitR.transform.gameObject.tag == "enemy" || hitR.transform.gameObject.tag != "border")
                isRight = false;
            else
            {
                if (sidSpd > 0)
                    sidSpd = 0;
            }



        }
        else if (isLeft == true)
        {
           // Debug.Log("There is left next to me!");
            if (hitL.transform.gameObject.tag == "Player" || hitL.transform.gameObject.tag == "enemy" || hitL.transform.gameObject.tag != "border")
                isLeft = false;
            else
            {
                if (sidSpd < 0)
                    sidSpd = 0;
            }


        }

    }




    private void VerColCheck()
    {

        RaycastHit hitL, hitR;
        bool isLeft = Physics.Raycast(this.transform.position, -this.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(this.transform.position, this.transform.right, out hitR, horCheck);


        if (isRight == true)
        {
          


        }
        else if (isLeft == true)
        {
          


        }


    }



    private void ZetaColCheck()
    {
        RaycastHit hitL, hitR;
        bool behind = Physics.Raycast(this.transform.position, -this.transform.forward, out hitR, horCheck);
        bool infront = Physics.Raycast(this.transform.position, this.transform.forward, out hitL, horCheck);
        Debug.DrawRay(this.transform.position, -this.transform.forward, Color.green);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);


        if (behind == true)
        {
             if (hitR.transform.gameObject.tag == "Player" || hitR.transform.gameObject.tag == "enemy" || hitR.transform.gameObject.tag != "border")
                    behind = false;
            else
            {
                if (fwdSpd < 0)
                    fwdSpd = 0;
            }
            //else
            //Debug.Log("There is something next to me! " + hitR.transform.name);


        }
        else if (infront == true)
        {

            if (hitL.transform.gameObject.tag == "Player" || hitL.transform.gameObject.tag == "enemy" || hitL.transform.gameObject.tag != "border")
                infront = false;
            else
            {
                if (fwdSpd > 0)
                    fwdSpd = 0;
            }

            //else
            //Debug.Log("There is something next to me! " +hitL.transform.name);
        }

    }





    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {


            isGrounded = true;
            isJumping = false;
            isDoubleJumping = false;
            doubleJump = false;


            dir = directionMoving.not;


        }


        if (col.gameObject.tag == "obstacle")
        {

            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "bouncePad")
        {
            rb.AddForce(jumpHeight * xDis, jumpHeight * yDis, 0, ForceMode.Impulse);
            dblJumped = false;
        }
        else if (col.gameObject.tag == "buff")
        {
            col.gameObject.GetComponent<GETP_buff>().BuffEffect(this.gameObject);
            Destroy(col.gameObject);
            Debug.Log("buff grabbed by player remove from list bug");

        }
        else if (col.gameObject.tag == "enemy")
        {
            CpuAi disCpu = col.gameObject.GetComponent<CpuAi>();
            if (disCpu.attacking == true)
            {
                Debug.Log("you got hit by chargin ene;");
                hp -= 25;
                hpSlider.value = hp;
               
                disCpu.attacking = false;
                disCpu.lastAttack = Time.time;
                disCpu.GetComponent<Renderer>().material = disCpu.normalMat;

                if(disCpu.eneType == CpuAi.enemyType.flyer)
                {
                    disCpu.retreating = true;
                }
                else
                {
                    rb.AddForce(new Vector3(0, disCpu.chargeForce, 0), ForceMode.Impulse);
                }
            }
            else
            {
                Debug.Log("you got hit by enemy;");
                hp -= 10;
                hpSlider.value = hp;

            }

        }
    }

    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = false;

            isDoubleJumping = false;
        }
    }





    public void OnTriggerEnter(Collider col)
    {
     if(col.tag == "QuadrantController")
        {
            if (col.GetComponent<QuadrantController>().cntrlType == QuadrantController.controlType.quadControl)
            {
                Debug.Log("press space movement haulted");
                canMove = false;
            }
            else if (col.GetComponent<QuadrantController>().cntrlType == QuadrantController.controlType.goal)
                this.gameObject.SetActive(false);

        }
     


        }


    public void OnTriggerStay(Collider col)
    {

      if (col.tag == "StageController")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //if(kills == eneGen1.amount + eneGen2.amount)
               // {
                //    Debug.Log("space ship takeoff");

               // }
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
    
    }

    public void OnMouseDown()
    {


    }


  

    public void StageProgression()
    {
        if (matchStarted == false)
        {
            matchStarted = true;
            Debug.Log("match started");

          
            //this.transform.position = pathListObjs[0].transform.position+new Vector3(0,1,0);
            cam.transform.position = this.transform.position+new Vector3(0,cam.yPos,0);

            canMove = true;
        }

    }



    public void Jump()
    {



        if (!doubleJump && !isGrounded && jetPack == false)//double jump
                {
                    if (canDblJump == true)
                    {
                        doubleJump = true;
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.AddForce(new Vector2(0f, jumpForce));

                        isDoubleJumping = true;
                        doubleJumpedAt = Time.time;
                    }
                }
                else if (isGrounded && !doubleJump && jetPack == false)//normal jump
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0f, jumpForce));

                    isJumping = true;
                }




    }


    public void JetPack()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0f, jetPack_force));
        jetPack_fuel -= jetPack_rate;
        if(jetPack_fuel <=0)
        {
            rb.useGravity = true;
            jetPack = false;
            jetPack_fuel = 100;
        }
    }


   

    public void UIActions(int action)
    {

        if (action == 0)
            {
         
            if (Time.time >= lastShotAt + fireRate)
                    CmdShoot(-1);

            
            }
            else if (action == 1)
            {
        

            if (Time.time >= lastShotAt + fireRate)
                    CmdShoot(1);

             }
            else if (action == 4)
            {


                if (Time.time >= lastShotAt + fireRate)
                CmdShootVer(1);

            }
            else if (action == 5)
            {


                if (Time.time >= lastShotAt + fireRate)
                CmdShootVer(-1);

            }
        else if (action == 2)
            {

                    if (sidSpd == -mvspd)
                    {

                        sidSpd = 0;
                    }
                    else
                        sidSpd = -mvspd;
                
            }
            else if (action == 3)
            {

                    if (sidSpd == mvspd)
                    {

                        sidSpd = 0;
                    }
                    else
                            sidSpd = mvspd;
                


            }

        


    }












    public void OnDown()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            //  fwdSpd = mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.N;
            //dir = directionMoving.up;
            // dirText.text = "jump";
            //  Jump();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //  fwdSpd = -mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.S;

            // dir = directionMoving.down;
            // dirText.text = "spike";

            // Spike();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {


            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
          

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

            // dir = directionMoving.right;
            // dirText.text = "right";

            //  sidSpd = mvspd;


        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            // dir = directionMoving.right;
            // dirText.text = "right";
            if ( jetPack == false)
                Jump();

        }





    }

    public void OnHold()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if ( jetPack == true)
            {
                JetPack();
            }
        }


        if (Input.GetKey(KeyCode.A))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
 
                sidSpd = -mvspd;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;

                sidSpd = mvspd;

        }

        if (Input.GetKey(KeyCode.W))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";

            if (allowZmovement == true)
            {

              
                    fwdSpd = mvspd;

            }

        }
        else if (Input.GetKey(KeyCode.S))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (allowZmovement == true)
            {

           
                    fwdSpd = -mvspd;
            }


        }
    }

    public void OnUp()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
        
                sidSpd = 0;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
      
                sidSpd = 0;

        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";

            if (allowZmovement == true)
            {

   
                    fwdSpd = 0;

            }

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (allowZmovement == true)
            {

          
                    fwdSpd = 0;
            }


        }
    }



}


