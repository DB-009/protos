using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlatformControls : MonoBehaviour
{

    //Boolean variables can be true or false you'd use this to see wheather something is happening or a variable is set.

    public int playerID;
    public Slider hpSlider,staminaSlider;
    public bool isGrounded;//is my player colliding with the floor?
    public bool isJumping;//is my player jumping or can he jump?
    public bool onWall,jumpThrough;
    public GameObject lastWall;
    //RigidBody is basically a physical object in unitys engine lets us know it takes up space, and  allows it to use gravity

    public Rigidbody rb;//Container variable for my players Rigidbody 
    public float  sidSpd;//how fast is my player moving left or right (Left this will be negative, RIght this willbe positive)

    public bool isController;

    //float is basically a number but it can be negative , positive or a decimal number.

    public float moveSpd;//A variable were going to ue later on in the tutorial to increase players speed or decrease it

    public int str, def, intelligence, hp, mhp, level ;

    public float jumpForce;//how much force is the player using to jump. gravity is always pushing down on player so he needs X amount of force to jump

    public bool isMorphed,isMoving,canPerformAct;
    public GameObject normalMode, morphMode;
    public SphereCollider normalCollider;
    public SphereCollider morphCollider;

    public CameraTracking myCam;
    public GameObject initialSpawnPos;

    public GameObject bulletPrefab;
    public float bulForce, fireRate , lastShotAt , bulletAcceptedDistance;

    public int wepID;
    public int comboCur, hits, kills;
    public float lastComboAt, comboStarted, allowedComboTime, comboLimit;

    //temporary combo system
    public GameObject comboPrefabA, comboPrefabB, comboPrefabC;

    public float yDis;


    public enum gunType
    {
        normal,
        rapidFire,
        spreadShot,
        triShot,
        hadoken,
    }

    public gunType curGun;

    public float spreadShotAngle, jetPack_fuel, jetPack_rate, jetPack_force, gunUpgradedAt, gunUpgradeTime, dashDistance;
    public float stamina, maxStamina, staminaRegen, staminaLoss;
    public bool jetPack, gunDoubleUpgrade,isDashing;

    public enum directionMoving { not, left, right, up, down };
    public directionMoving dir, shotDir;

    //Timers
    public float walkTimer, impulseTimer, dashTimer, lastDash, lastImpulse;

    //walking 
    public float walkTime, startWalkTime, allowedWalkingTime;

    public int impulses, dashes;

    public Vector3 targetReposition;

    //wall jump variables
    public float wallTimeStart, wallTimeLimit, lastWallExitTIme, wallAllowedStoreTime;

    public float upCheck, horCheck, downCheck;


    public List<Material> mats = new List<Material>();


    //Built in unity function  Use this for initialization any variable that cannot be empty. Like rigidbody for example. this runs once the game loads or the stage is loaded
    void Awake()
    {
        //set the RigidBOdy of the player inside my RB variable.
        rb = GetComponent<Rigidbody>();///GetComponent<> Basically grabs any component attached to the current object. (object this script is on)
        hpSlider.maxValue = mhp;
        hpSlider.value = hp;

        staminaSlider.maxValue = maxStamina;
       staminaSlider.value = stamina;
    }

    //Built in unity function  Update is called once per frame
    //most math based equations go here. You should not be moving your character or any object in update when working with Physics or rigidbodies 
    void Update()
    {
        sidSpd = 0;
        if (stamina <= 0)
        {
            
            canPerformAct = false;
            rb.useGravity = true;
            rb.isKinematic = false;
            this.GetComponent<Renderer>().material = mats[1];
            onWall = false;
            
        }
        else
        {
            canPerformAct = true;
            this.GetComponent<Renderer>().material = mats[0];
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
                rb.useGravity = true;
            }
        }
        else//delete wall after x amount of seconds so he can regrab
        {
            if (Time.time >= lastWallExitTIme + wallAllowedStoreTime && lastWallExitTIme != 0 && lastWall != null)
            {
                Debug.Log("deleted wall");
                lastWall = null;
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        HorColCheck();
        VerColCheck();

        if (canPerformAct == true)
        {
            MoveObj();
      

            if (Input.GetKeyDown(KeyCode.Mouse0))//combo / button mash inputs should be  on GetKeyDown
            {
                if (wepID == 1)
                {
                    //Debug.Log("in combo");
                    Combo();
                }

            }
            else if (Input.GetKeyDown(KeyCode.Z))
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

        if (stamina < maxStamina)
        {

            UpdateStamina(staminaRegen * Time.deltaTime, false);
        }


    }





    //Built in unity function  runs on a fixed time scale cam be changed in setting. All physics based movements should go in here
    void FixedUpdate()
    {
        if (isController == true && onWall == false)
        {
            if (isDashing == true)
            {
                //this.transform.position = targetReposition;
                // rb.MovePosition(targetReposition);

                Vector3 direction = (targetReposition - transform.position).normalized;
                rb.MovePosition(transform.position + new Vector3 (direction.x,0,0) * (moveSpd * dashDistance) * Time.fixedDeltaTime);

                float temp = (targetReposition - transform.position).magnitude;
                if (temp <= .6f)
                {
                    isDashing = false;
                }

            }
            else
            rb.AddForce(new Vector3(sidSpd, 0, 0));//Add a force to my players RigidBody.
                                                   //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
        }
    }


    //custom move function
    void MoveObj()
    {
        sidSpd = 0;
        if (isController == true)
        {

            // sidSpd = Input.GetAxis("Horizontal") * moveSpd;//Input.GetAxis will grab the Axis "Horizontal" in this case.
            // to create axis  hit the Edit tab > Project Setting > Input

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (dir == directionMoving.left && onWall == false)
                    playerDash(0);

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {

                if (dir == directionMoving.right && onWall == false)
                    playerDash(1);
            }


            if (Input.GetKey(KeyCode.A))
            {
               
                    sidSpd = -moveSpd;
                    dir = directionMoving.left;
                dashTimer = Time.time + 1;
                UpdateStamina(staminaLoss * Time.deltaTime, true);
            }
            else if (Input.GetKey(KeyCode.D))
            {

                    sidSpd = moveSpd;
                    dir = directionMoving.right;
                dashTimer = Time.time + 1;
                UpdateStamina(staminaLoss * Time.deltaTime, true);

            }


            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {//Input.GetKeyDown() waits for the user to press a key once
                if (isGrounded == true && onWall == false)
                {//if my player is grounded do whats in the barckets
                    isJumping = true;
                    rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);//Add a force to my players RigidBody.
                    UpdateStamina(staminaLoss , true);

                }
                else if (onWall == true)
                {//if my player is grounded do whats in the barckets
                    rb.isKinematic = false;

                    if (sidSpd < 0)
                        sidSpd = -moveSpd;
                    else if (sidSpd > 0)
                        sidSpd = moveSpd;
                    else
                    {
                        rb.useGravity = true;
                        rb.isKinematic = false;
                    }

                    onWall = false;
                    lastWallExitTIme = Time.time;
                    rb.AddForce(new Vector3(sidSpd, jumpForce, 0), ForceMode.Impulse);//Add a force to my players RigidBody.
                    UpdateStamina(staminaLoss, true);

                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (isMorphed == false)//if not morphed , morph
                {
                    isMorphed = true;
                    normalMode.SetActive(false);
                    normalCollider.enabled = false;

                    morphCollider.enabled = true;
                    morphMode.SetActive(true);

                }
                else//if morphed, morph back
                {
                    isMorphed = false;
                    morphCollider.enabled = false;
                    morphMode.SetActive(false);

                    normalMode.SetActive(true);
                    normalCollider.enabled = true;

                }

            }

            if(sidSpd > 0 || sidSpd < 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            //motion check

            if ((sidSpd != 0))//in motion
            {
                if (startWalkTime == 0)//if no startWalk time then this is new motion
                {
                    startWalkTime = Time.time;
                    // Debug.Log("new motion");
                }
                else//must be an old motion
                {
                    //Debug.Log("old motion ");
                    walkTime = Time.time - startWalkTime;
                }

                walkTimer = 0;
            }
            else if ( sidSpd == 0)//not in motion
            {

                if (walkTimer == 0 && startWalkTime != 0)//if motion was started and you just stopped moving
                {
                    walkTimer = Time.time + allowedWalkingTime;

                    //Debug.Log("motion timer started");
                }
                else if (Time.time >= walkTimer && walkTimer != 0)//if its been a second since you stopped moving
                {
                    startWalkTime = 0;
                    walkTime = 0;
                    walkTimer = 0;
                    dir = directionMoving.not;

                    rb.velocity = Vector3.zero;//stopping all motion
                    rb.angularVelocity = Vector3.zero;//stopping all motion
                                                      // Debug.Log("motion timer ended");
                }

            }


            if (Time.time >= lastDash + 1)
            {
                isDashing = false;

            }

        }

        //elaborate here
    }


    private void HorColCheck()
    {
        RaycastHit hitL, hitR;
        bool isLeft = Physics.Raycast(this.transform.position, -this.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(this.transform.position, this.transform.right, out hitR, horCheck);


        if (isRight == true)
        {
            Debug.Log("There is something next to me!" + hitR.transform.gameObject.tag);
            if (hitR.transform.gameObject.tag == "Player"
                || hitR.transform.gameObject.tag == "enemy" 
                || hitR.transform.gameObject.tag == "edge"
                || hitR.transform.gameObject.tag == "Combo")
                isRight = false;
            else
            {
                if (sidSpd > 0)
                    sidSpd = 0;

                isDashing = false;

            }



        }
        else if (isLeft == true)
        {
            Debug.Log("There is something next to me! "+ hitL.transform.gameObject.tag);
            if (hitL.transform.gameObject.tag == "Player"
                || hitL.transform.gameObject.tag == "enemy" 
                || hitL.transform.gameObject.tag == "edge"
                || hitL.transform.gameObject.tag == "Combo"
                )
                isLeft = false;
            else
            {
                if (sidSpd < 0)
                    sidSpd = 0;

                isDashing = false;
            }


        }

    }

    private void VerColCheck()
    {
        RaycastHit hitUp, hitDown;
        bool isUp = Physics.Raycast(this.transform.position, this.transform.up, out hitUp, upCheck);
        bool isDown = Physics.Raycast(this.transform.position, -this.transform.up, out hitDown, upCheck);


        if (isDown == true)
        {
            Debug.Log("There is something underneath me! " + hitDown.transform.gameObject.tag);
            //Check what you are stood on e.g hit.collider.gameObject.layer   etc..

            this.gameObject.GetComponent<SphereCollider>().isTrigger = false;


        }
        else if (isUp == true) 
        {
            if (hitUp.transform.gameObject.tag == "passablePlat")
            {
                Debug.Log("There is something Above  me! " + hitUp.transform.gameObject.tag);
                jumpThrough = true;
                this.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            }

        }


    }

    ///COLLISIONS 
    /// 
    /// 
    /// 	
    //Built in unity function to detect When a collision has occured
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("you hit something " + col.collider.tag);//basic print message for debugging purposes
        if (col.collider.tag == "ground")//if the object you collided withs tag is ground your player is on the floor
        {
            isGrounded = true;///so grounded must be true because Player has hit the floor.
            lastWall = null;
            isJumping = false;
        }
        else if (col.collider.tag == "wall" && isGrounded == false && onWall == false && canPerformAct == true)//if the object you collided withs tag is ground your player is on the floor
        {
            if (isDashing == true)
                isDashing = false;

            if (lastWall != col.gameObject)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                wallTimeStart = Time.time;
                onWall = true;
                lastWall = col.gameObject;
            }
        }


    }


    public void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "wall" && isGrounded == false && onWall == false && canPerformAct == true)//if the object you collided withs tag is ground your player is on the floor
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
        if (col.collider.tag == "ground")//if the object you collided withs tag is ground your player is on the floor
        {
            isGrounded = true;///so grounded must be true because Player has hit the floor.
            
            isJumping = false;
        }
    }
    //Built in unity function to detect When a collision has ended
    void OnCollisionExit(Collision col)
    {
        Debug.Log("you left something ");//basic print message for debugging purposes
        if (col.collider.tag == "ground")//if the object you WERE colliding withs tag is ground your player is leaving the floor
        {
            isGrounded = false;//Player is no longer grounded

        }
        else if (col.collider.tag == "wall" && isGrounded == false)//if the object you collided withs tag is ground your player is on the floor
        {
            rb.isKinematic = false;
            onWall = false;
       
            rb.useGravity = true;

        }

    }


    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("you hit something " + col.tag);//basic print message for debugging purposes
        if (col.tag == "trackArea")//if the object you collided withs tag is ground your player is on the floor
        {
            myCam.canTrack = true;
        }


    }

    void OnTriggerExit(Collider col)
    {
        //Debug.Log("you hit something " + col.tag);//basic print message for debugging purposes
        if (col.tag == "trackArea")//if the object you collided withs tag is ground your player is on the floor
        {
            myCam.canTrack = false;
        }


    }




    public void Shoot(GameObject bulz, float force, directionMoving shotDirection)
    {



        Vector3 bulPosition = Vector3.zero;
        float dire = 1;


        if (shotDirection == directionMoving.left)
        {
            bulPosition = new Vector3(this.transform.position.x - 1, this.gameObject.transform.position.y, this.transform.position.z);
            dire = -1;
        }
        else if (shotDirection == directionMoving.right)
        {
            bulPosition = new Vector3(this.transform.position.x + 1, this.gameObject.transform.position.y, this.transform.position.z);
            dire = +1;
        }


        if (curGun == gunType.normal)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);


        }
        else if (curGun == gunType.spreadShot)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;
            GameObject tileCreated2 = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;
            GameObject tileCreated3 = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(dire, spreadShotAngle, 0) * force, ForceMode.Impulse);

            tileCreated2.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);


            tileCreated3.GetComponent<Rigidbody>().AddForce(new Vector3(dire, -spreadShotAngle, 0) * force, ForceMode.Impulse);

        }
        else if (curGun == gunType.triShot)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition + new Vector3(0, -.5f, 0), Quaternion.identity) as GameObject;
            GameObject tileCreated2 = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;
            GameObject tileCreated3 = GameObject.Instantiate(bulz, bulPosition - new Vector3(0, -.5f, 0), Quaternion.identity) as GameObject;

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);

            tileCreated2.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);


            tileCreated3.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);

            if (gunDoubleUpgrade == true)
            {
                GameObject tileCreated4 = GameObject.Instantiate(bulz, bulPosition + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated4.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated4.GetComponent<projectileLife>().playerBullet = true;

                GameObject tileCreated5 = GameObject.Instantiate(bulz, bulPosition - new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated5.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated5.GetComponent<projectileLife>().playerBullet = true;


                tileCreated4.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);


                tileCreated5.GetComponent<Rigidbody>().AddForce(new Vector3(dire, 0, 0) * force, ForceMode.Impulse);
            }
        }


        lastShotAt = Time.time;
    }




    public void Combo()
    {
        GameObject comboObj = comboPrefabA;


        float comboMultiplier = 1;

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
                comboMultiplier = 1.2f;
            }
            else if (comboCur == 2)
            {
                comboObj = comboPrefabC;
                comboMultiplier = 1.4f;
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

        UpdateStamina(staminaLoss * comboMultiplier, true);



        // Get the point along the ray that hits the calculated distance.
        Ray targetPoint = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = Vector3.zero;

        Vector3 pos = targetPoint.GetPoint(0);


        direction = pos - new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - yDis, Camera.main.transform.position.z);

        direction.Normalize();
       // Debug.Log("Dir = " + direction + " true x = " + direction.x + " true y = " + direction.y);
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









    public void playerDash(int dir)
    {
        if (Time.time <= dashTimer && dashTimer != 0 &&  Time.time >= lastDash + impulseTimer)//also add number of dashes can connect
        {
             if (dir == 0)
            {
                // rb.AddForce(new Vector3(-myPlayer.playerSpd - 4, 0,0), ForceMode.Acceleration);
                targetReposition = this.transform.position - (new Vector3(dashDistance, 0, 0));
                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;
                UpdateStamina(staminaLoss * dashDistance, true);
            }
            else if (dir == 1)
            {
                // rb.AddForce(new Vector3(myPlayer.playerSpd + 4, 0, 0), ForceMode.Force);
                targetReposition = this.transform.position + (new Vector3(dashDistance, 0, 0));

                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;

                UpdateStamina(staminaLoss * dashDistance , true);
            }


            sidSpd = 0;
            rb.velocity = Vector3.zero;
            dashes++;
        }
    }




    public void UpdateStamina (float val , bool debug)
    {
        stamina = stamina + val;
        staminaSlider.value = stamina;

        if(debug == true)
        Debug.Log("Stamina change " + val);

        if(stamina > 0)
        {
            canPerformAct = true;
        }
    }









    public void resetStats()
    {

        //lives = 3;
        // points = 0;
        // hp = mhp;



        Debug.Log("Characters are given acces to items and weapons");

        //can fire all weapons in beginning add them

    }


    public void enablePlayer()
    {

        this.enabled = true;
        this.gameObject.gameObject.SetActive(true);
        rb.isKinematic = true;


    }

    public void disablePlayer(bool destroy, int id)
    {
        if (this.isActiveAndEnabled == true)
        {
            this.enabled = false;
            this.gameObject.gameObject.SetActive(false);
            rb.isKinematic = true;
            if (destroy == true)
            {
                Destroy(myCam.gameObject);
                //gameStateManager.player = null;
                Destroy(this.gameObject);
           
            }
        }
        else
            Debug.Log("No disabling diabled players");

    }

    public void UIActions(int action)
    {

        if (action == 0)
        {
            if (Time.time >= lastShotAt + fireRate)
                Shoot(bulletPrefab, bulForce, directionMoving.left);
        }
        if (action == 1)
        {
            if (Time.time >= lastShotAt + fireRate)
                Shoot(bulletPrefab, bulForce, directionMoving.right);
        }
    }


}