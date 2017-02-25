using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SlingShotPlayer : MonoBehaviour
{

    public GameManager gameStateManager;

    public PlayerClasses playerClass;

    public CameraTracking cam;

    public Rigidbody rb;

    //player variables for movement
    public bool isController,isGrounded, doubleJump, canDblJump, dblJumped, cannonBackward;
    public float jumpForce, doubleJumpedAt;

    //wall jump variables
    public float wallTimeStart, wallTimeLimit, lastWallExitTIme, wallAllowedStoreTime;

    //cannon variables
    public float timeBetweenCanon, LastCannonAt, noGravityTimeLimit;

    //raycast variables
    public float upCheck,horCheck,downCheck;

    public List<GameObject> lastCannon = new List<GameObject>();

    public bool onWall , jumpThrough;
    public GameObject lastWall;



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


    public float xDis,yDis;


    //movement
    public float fwdSpd = 0, sidSpd = 0;

    public bool lockedOnTarget;


    private int numOfTrajectoryPoints = 30;
    public GameObject trajectoryPrefab;

    public List<GameObject> trajectoryPoints = new List<GameObject>();

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

    public float spreadShotAngle, jetPack_fuel, jetPack_rate, jetPack_force, gunUpgradedAt, gunUpgradeTime;

    public bool jetPack, gunDoubleUpgrade, isDoubleJumping, isJumping,isAndroid;

    public void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        this.hp = this.vit * lvl + baseHP;
        this.mp = this.intel * lvl + baseMp;
        this.mhp = this.hp;
        this.mmp = this.mp;


        initPos = this.transform.position;
    }

    public void Update()
    {

        //reset input remove this for accelartion based movement if remove add limit
        fwdSpd = 0;
        sidSpd = 0;

        if (hp <= 0)
        {
            Debug.Log("you died;");
            transform.position = initPos;
            hp = mhp;
        }
        //Wall jump functions
        //if player has been holding wall for x time make em fall
        if (onWall == true)
        {
            if(Time.time >= wallTimeStart + wallTimeLimit)
            {
                Debug.Log("free fall");
                onWall = false;
                rb.isKinematic = false;
                lastWallExitTIme = Time.time;
            }
        }
        else//delete wall after x amount of seconds so he can regrab
        {
            if (Time.time >= lastWallExitTIme + wallAllowedStoreTime && lastWallExitTIme != 0)
            {
                Debug.Log("deleted wall");
                lastWall = null;
            }
        }

        if (rb.useGravity == false && rb.isKinematic == false )
        {
            if (Time.time >= LastCannonAt + noGravityTimeLimit)
            {

                rb.useGravity = true;
            }
        }

        if(isController == true)
        {
            OnDown();
            OnHold();
            OnUp();

            if (Input.GetKey(KeyCode.Mouse0) && isAndroid != true)//combo / button mash inputs should be  on GetKeyDown
            {

                //Debug.Log("in combo");
                ShotDetect();
                

            }


        }

        HorColCheck();
        VerColCheck();



    }

    void FixedUpdate()
    {

                if (onWall == false)//its this players turn in battle system
                {




            // rb.AddForce(new Vector3(sidSpd, fwdSpd, 0),ForceMode.Force);//Add a force to my players RigidBody.
            //this.gameObject.transform.Translate(sidSpd, 0, fwdSpd);
            this.gameObject.GetComponent<Rigidbody>().AddForce(sidSpd, fwdSpd, 0, ForceMode.Acceleration);
                    //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
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
        else if (action == 2)
        {
            if ( isGrounded == true)
            {
                if (sidSpd == mvspd * -1)
                {
                    sidSpd = 0;
                }
                else
                    sidSpd = mvspd * -1;
            }
        }
        else if (action == 3)
        {
            if (isGrounded == true)
            {
                if (sidSpd == mvspd)
                {

                    sidSpd = 0;
                }
                else
                    sidSpd = mvspd;
            }
        }
    }




    public void ShotDetect()
    {
        Ray targetPoint = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = Vector3.zero;

        Vector3 pos = targetPoint.GetPoint(0);


        direction = pos - new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - yDis, Camera.main.transform.position.z);

        direction.Normalize();
        // Debug.Log("Dir = " + direction + " true x = " + direction.x + " true y = " + direction.y);
        int tempXdis = 0;
        if (direction.x < 0)
        {
            if (Time.time >= lastShotAt + fireRate)
                CmdShoot(-1);

        }
        else if (direction.x > 0)
        {

            if (Time.time >= lastShotAt + fireRate)
                CmdShoot(1);
        }
    }

    public void CmdShoot(int dire)
    {






        float force = bulForce;

        GameObject bulz = bulletPrefab;



        if (curGun == gunType.normal)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z), this.transform.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (this.transform.right * dire) * bulForce;

        }
        else if (curGun == gunType.spreadShot)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            var tileCreated3 = (GameObject)Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z), this.transform.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;

            tileCreated.GetComponent<Rigidbody>().velocity = ((this.transform.right + new Vector3(0, 0, spreadShotAngle)) * dire) * bulForce;

            tileCreated2.GetComponent<Rigidbody>().velocity = (this.transform.right * dire) * bulForce;


            tileCreated3.GetComponent<Rigidbody>().velocity = ((this.transform.right + new Vector3(0, 0, -spreadShotAngle)) * dire) * bulForce;



        }
        else if (curGun == gunType.triShot)
        {
            var tileCreated = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z) + new Vector3(0, .5f, 0), Quaternion.identity) as GameObject;
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z), this.transform.rotation);
            var tileCreated3 = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z) - new Vector3(0, .5f, 0), Quaternion.identity) as GameObject;

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (this.transform.right * dire) * bulForce;

            tileCreated2.GetComponent<Rigidbody>().velocity = (this.transform.right * dire) * bulForce;


            tileCreated3.GetComponent<Rigidbody>().velocity = (this.transform.right * dire) * bulForce;


            if (gunDoubleUpgrade == true)
            {
                var tileCreated4 = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z) + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated4.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated4.GetComponent<projectileLife>().playerBullet = true;

                var tileCreated5 = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x + dire, this.transform.position.y, this.transform.position.z) - new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated5.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated5.GetComponent<projectileLife>().playerBullet = true;



                tileCreated4.GetComponent<Rigidbody>().velocity = (this.transform.right * dire) * bulForce;


                tileCreated5.GetComponent<Rigidbody>().velocity = (this.transform.right * dire) * bulForce;



            }


        }


        lastShotAt = Time.time;
    }


    public void JetPack()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0f, jetPack_force),ForceMode.Force);
        jetPack_fuel -= jetPack_rate;
        if (jetPack_fuel <= 0)
        {
            rb.useGravity = true;
            jetPack = false;
            jetPack_fuel = 100;
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
                rb.AddForce(new Vector2(0f, jumpForce),ForceMode.Impulse);

                isDoubleJumping = true;
                doubleJumpedAt = Time.time;
            }
        }
        else if (isGrounded && !doubleJump && jetPack == false)//normal jump
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode.Impulse);

            isJumping = true;
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
          
                Jump();

        }





    }

    public void OnHold()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (jetPack == true)
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



        }
        else if (Input.GetKey(KeyCode.S))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;



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


        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;



        }
    }


    private void HorColCheck()
    {
        RaycastHit hitL,hitR;
        bool isLeft = Physics.Raycast(this.transform.position, -this.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(this.transform.position, this.transform.right, out hitR, horCheck);


        if (isRight == true )
        {
            //Debug.Log("There is something next to me!");
            if (hitR.transform.gameObject.tag == "Player" || hitR.transform.gameObject.tag == "enemy" || hitR.transform.gameObject.tag == "edge")
                isRight = false;
            else
            {
                if (sidSpd > 0)
                 sidSpd = 0;
            }


            
        }
        else if (isLeft == true)
        {
            //Debug.Log("There is something next to me!");
            if (hitL.transform.gameObject.tag == "Player" || hitL.transform.gameObject.tag == "enemy" || hitL.transform.gameObject.tag == "edge")
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
        RaycastHit hitUp,hitDown;
        bool isUp = Physics.Raycast(this.transform.position, this.transform.up, out hitUp, upCheck);
        bool isDown = Physics.Raycast(this.transform.position, -this.transform.up, out hitDown, upCheck);


        if (isDown == true)
        {
            //Debug.Log("There is something underneath me!");
            //Check what you are stood on e.g hit.collider.gameObject.layer   etc..
         
               this.gameObject.GetComponent<SphereCollider>().isTrigger = false;

            
            isGrounded = true;
            dblJumped = false;
        }
        else if(isUp == true)
        {
            if (hitUp.transform.gameObject.tag == "passablePlat")
            {
                //Debug.Log("There is something Above  me!");
                jumpThrough = true;
                this.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            }

        }
        else {
            jumpThrough = false;
            isGrounded = false;
            
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


          


        }


        if (col.gameObject.tag == "obstacle")
        {

            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "bouncePad")
        {
            rb.AddForce(jumpForce * xDis, jumpForce * yDis, 0, ForceMode.Impulse);
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
                Debug.Log("you got hit by chargin ene; add in hpslider");
                hp -= 25;
               // hpSlider.value = hp;

                disCpu.attacking = false;
                disCpu.lastAttack = Time.time;
                disCpu.GetComponent<Renderer>().material = disCpu.normalMat;

                if (disCpu.eneType == CpuAi.enemyType.flyer)
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
                Debug.Log("you got hit by enemy add in slider;");
                hp -= 10;
                //hpSlider.value = hp;

            }

        }
    }

    public void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            Debug.Log("Eh colsaty;");
            if (lastWall != col.gameObject)
            {
                rb.isKinematic = true;
                wallTimeStart = Time.time;
                onWall = true;
                lastWall = col.gameObject;
            }
        }
    }


    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            // isGrounded = false;
            //dblJumped = false;
        }

        else if (col.collider.tag == "wall" && isGrounded == false)//if the object you collided withs tag is ground your player is on the floor
        {
            rb.isKinematic = false;
            onWall = false;
            dblJumped = false;
            rb.useGravity = true;

        }
    }



    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "SpeedUp")
        {
            //access the buffZone
            Debug.Log("Speed up");
            rb.AddForce(new Vector3(sidSpd * 1.5f, 0, 0), ForceMode.Impulse);

        }
        if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone

            col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false, this.gameObject);

        }
        if (col.gameObject.tag == "edge")
        {
            //access the buffZone

            trigger = col.gameObject;

        }
    }


    public void OnTriggerStay(Collider col)
    {

        if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone

            if (col.GetComponent<buffZone>().type != buffZone.Type.speedChange)
            {

                col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false, this.gameObject);

            }

        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "edge")
        {
            //access the buffZone

            trigger = null;

        }
    }

    public void OnMouseDown()
    {
        if(this.gameObject != gameStateManager.objectControlling)
        {
            gameStateManager.objectControlling.GetComponent<SlingShotPlayer>().isController = false;
            gameStateManager.objectControlling = this.gameObject;

            this.isController = true;
        }
      
    }
}







