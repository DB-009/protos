using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LaneShift_TopDown : MonoBehaviour
{


    public CasualGameManager gameStateManager;

 
    public Transform myTrans;


    public float minSwipeDistY;
    public float minSwipeDistX;
    private Vector2 startPos;

    public Slider hpSlider;
  
    public controlledStageGenerator stageGen;
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

    public enum MoveType { run, tapHero , survival };
    public MoveType curMoveType;
    public int curLanePos,curLaneLimit;

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


    ///COMBO VARIABLES
    public int wepID;
    public int comboCur, hits;
    public float lastComboAt, comboStarted, allowedComboTime, comboLimit;

    //temporary combo system
    public GameObject comboPrefabA, comboPrefabB, comboPrefabC;





    public float screen_half = Screen.width / 2;


    public float laserFiredAt,laserRate;
    public bool firingLaser;
    public float laserFiredAt2;
    public bool firingLaser2;
    public GameObject laserEnd, laserCenter, laserEnd2, laserCenter2;



    public void Awake()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.myTrans = gameObject.transform;
        this.hp = this.vit * this.lvl + this.baseHP;
        this.mp = this.intel * this.lvl + this.baseMp;
        this.mhp = this.hp;
        this.mmp = this.mp;


        this.initPos = gameObject.transform.position;
        this.gameStateManager = gameStateManager;
        this.hpSlider = this.gameStateManager.hpSlider1;


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




        this.hpSlider.maxValue = this.mhp;
        this.hpSlider.value = this.hp;
    }





    public void Update()
    {

        //reset input remove this for accelartion based movement if remove add limit
        //fwdSpd = 0;
        //sidSpd = 0;

  
            if (hp <= 0)
            {
                Debug.Log("you died;");
                this.gameObject.SetActive(false);
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




                if (Input.GetKey(KeyCode.G))
                {
                    ImFiringMyLaserr(false);
                }
                else if (Input.GetKeyUp(KeyCode.G))
                {
                    ImFiringMyLaserr(true);
                }




                if (Input.GetKey(KeyCode.F))
                {
                    ImFiringMyLaserr2(false);
                }
                else if (Input.GetKeyUp(KeyCode.F))
                {
                    ImFiringMyLaserr2(true);
                }



                if (Input.GetKeyDown(KeyCode.Mouse0))//combo / button mash inputs should be  on GetKeyDown
                {
                    if (wepID == 1)
                    {
                        //Debug.Log("in combo");
                        Combo();
                    }
                    else
                    {
                        Debug.Log("SHOT DETECT FOR burst beams");
                        ShotDetect();
                    }

                }
              


                SpeedUpates();





            }

  
                    _touchControls();

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        StageProgression();

                    }



                HorColCheck();
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




    public void ImFiringMyLaserr(bool onUp)
        {

        if(onUp == false)
        {
            if (firingLaser == false)
            {
                firingLaser = true;
                laserFiredAt = Time.time;
                laserEnd.SetActive(true);
                laserCenter.SetActive(true);
                laserEnd.transform.position = this.transform.right;

            }
            else
            {
                laserEnd.transform.position += new Vector3(laserRate, 0, 0) * Time.deltaTime;


                laserCenter.transform.position = new Vector3( ((this.transform.position.x+1) +  laserEnd.transform.position.x) / 2, laserCenter.transform.position.y, laserCenter.transform.position.z);
                laserCenter.transform.localScale = new Vector3((this.transform.position.x+1) - laserEnd.transform.position.x, laserCenter.transform.localScale.y, laserCenter.transform.localScale.z);
            }
        }
        else
        {
            firingLaser = false;
            laserFiredAt = 0;
            //Debug.Log("cooldown");
            laserEnd.SetActive(false);
            laserCenter.SetActive(false);
            laserCenter.transform.localScale = new Vector3(.3f, laserCenter.transform.localScale.y, laserCenter.transform.localScale.z);


            laserEnd.transform.position = this.transform.position;
            laserCenter.transform.position = this.transform.position;


        }



    }



    public void ImFiringMyLaserr2(bool onUp)
    {
        Debug.Log("make this one function during tutorial");
        if (onUp == false)
        {
            if (firingLaser2 == false)
            {
                firingLaser2 = true;
                laserFiredAt2 = Time.time;
                laserEnd2.SetActive(true);
                laserCenter2.SetActive(true);
                laserEnd2.transform.position = this.transform.position;

            }
            else
            {
                laserEnd2.transform.position -= new Vector3(laserRate, 0, 0) * Time.deltaTime;


                laserCenter2.transform.position = new Vector3( ((this.transform.position.x-1) + laserEnd2.transform.position.x) / 2, laserCenter2.transform.position.y, laserCenter2.transform.position.z);
                laserCenter2.transform.localScale = new Vector3((this.transform.position.x-1) - laserEnd2.transform.position.x, laserCenter2.transform.localScale.y, laserCenter2.transform.localScale.z);
            }
        }
        else
        {
            firingLaser2 = false;
            laserFiredAt2 = 0;
            //Debug.Log("cooldown");
            laserEnd2.SetActive(false);
            laserCenter2.SetActive(false);
            laserCenter2.transform.localScale = new Vector3(.3f, laserCenter2.transform.localScale.y, laserCenter2.transform.localScale.z);


            laserEnd2.transform.position = this.transform.position;
            laserCenter2.transform.position = this.transform.position;


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




    public void ShotDetect()
    {
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


            Debug.Log("Left");



        }
        else if (direction.x > 0)
        {


            Debug.Log("Right");


        }








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

        Vector3 objPos = new Vector3(gameObject.transform.position.x + tempXdis, gameObject.transform.position.y, gameObject.transform.position.z);


        if (canMove == true)
        {
            if (sidSpd < 0 && direction.x < 0)
            {
                objPos = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd < 0 && direction.x > 0)
            {
                objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x > 0)
            {
                objPos = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x < 0)
            {
                objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        }
        else
        {
            objPos = new Vector3(gameObject.transform.position.x + tempXdis, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        GameObject tileCreated = GameObject.Instantiate(comboObj, objPos, Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;




        Quaternion targetRotation = Quaternion.LookRotation(pos - tileCreated.transform.position);

        // Smoothly rotate towards the target point.
        tileCreated.transform.rotation = targetRotation;





    }






    public void keybased_combo(int dir)
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
        Vector3 objPos =  Vector3.zero;

        if(dir == 0)
        {
            objPos = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, gameObject.transform.position.z);

        }
        else if (dir == 1)
        {
            objPos = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z);

        }
        else if (dir == 2)
        {
            objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y +1, gameObject.transform.position.z);

        }
        else if (dir == 3)
        {
            objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y -1, gameObject.transform.position.z);

        }



        if (canMove == true)
        {
            if (sidSpd < 0 && dir == 0)
            {
                objPos = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd < 0 && dir == 1)
            {
                objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd > 0 && dir == 0)
            {
                objPos = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd > 0 && dir == 1)
            {
                objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        }
        else
        {
            objPos = new Vector3(gameObject.transform.position.x , gameObject.transform.position.y, gameObject.transform.position.z);
        }

        GameObject tileCreated = GameObject.Instantiate(comboObj, objPos, Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;





    }







    public void CmdShoot(int dire)
    {






        float force = bulForce;

        GameObject bulz = bulletPrefab;



        if (curGun == gunType.normal)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;

        }
        else if (curGun == gunType.spreadShot)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);
            var tileCreated3 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity *= bulForce * dire;

            tileCreated2.GetComponent<Rigidbody>().velocity *= bulForce * dire;


            tileCreated3.GetComponent<Rigidbody>().velocity *= bulForce * dire;

        }
        else if (curGun == gunType.triShot)
        {
            var tileCreated = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z) + new Vector3(0, -.5f, 0), Quaternion.identity) as GameObject;
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z), myTrans.rotation);
            var tileCreated3 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z) - new Vector3(0, -.5f, 0), Quaternion.identity) as GameObject;

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
                var tileCreated4 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z) + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated4.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated4.GetComponent<projectileLife>().playerBullet = true;

                var tileCreated5 = GameObject.Instantiate(bulz, new Vector3(myTrans.position.x + dire, myTrans.position.y, myTrans.position.z) - new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated5.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated5.GetComponent<projectileLife>().playerBullet = true;


                tileCreated4.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;


                tileCreated5.GetComponent<Rigidbody>().velocity = (myTrans.right * dire) * bulForce;



            }


        }
        else if (curGun == gunType.hadoken)
        {

        }

        lastShotAt = Time.time;
    }


























    private void HorColCheck()
    {
        RaycastHit hitL, hitR;
        bool isLeft = Physics.Raycast(gameObject.transform.position, -gameObject.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(gameObject.transform.position, gameObject.transform.right, out hitR, horCheck);
        Debug.DrawRay(gameObject.transform.position, -gameObject.transform.right, Color.green);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right, Color.green);

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
        bool isLeft = Physics.Raycast(gameObject.transform.position, -gameObject.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(gameObject.transform.position, gameObject.transform.right, out hitR, horCheck);


        if (isRight == true)
        {
            Debug.Log("There is something next to me!");
            if (hitR.transform.gameObject.tag == "Player" || hitR.transform.gameObject.tag == "enemy" || hitR.transform.gameObject.tag == "border")
                isRight = false;
            else
            {
                if (sidSpd > 0)
                    sidSpd = 0;
            }



        }
        else if (isLeft == true)
        {
            Debug.Log("There is something next to me!");
            if (hitL.transform.gameObject.tag == "Player" || hitL.transform.gameObject.tag == "enemy" || hitL.transform.gameObject.tag == "border")
                isLeft = false;
            else
            {
                if (sidSpd < 0)
                    sidSpd = 0;
            }


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
            col.gameObject.GetComponent<buff>().BuffEffect(this.gameObject);
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
                if (curMoveType == MoveType.survival)
                {
                    if (sidSpd == -mvspd)
                    {

                        sidSpd = 0;
                    }
                    else
                        sidSpd = -mvspd;
                }
            }
            else if (action == 3)
            {
                if (curMoveType == MoveType.survival)
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












    public void OnDown()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //  fwdSpd = mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.N;
            //dir = directionMoving.up;
            // dirText.text = "jump";
            //  Jump();
            if (curMoveType == MoveType.survival && jetPack == false)
                Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //  fwdSpd = -mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.S;

            // dir = directionMoving.down;
            // dirText.text = "spike";

            // Spike();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {


            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
            if (curMoveType == MoveType.run)
                laneShift(0);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            // dir = directionMoving.right;
            // dirText.text = "right";

            //  sidSpd = mvspd;
            if (curMoveType == MoveType.run)
                laneShift(1);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            // dir = directionMoving.right;
            // dirText.text = "right";
            if (curMoveType == MoveType.survival && jetPack == false)
                Jump();

        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            keybased_combo(0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            keybased_combo(1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            keybased_combo(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            keybased_combo(3);
        }


        if (Input.GetKeyDown(KeyCode.Z))
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






    public void OnHold()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (curMoveType == MoveType.survival && jetPack == true)
            {
                JetPack();
            }
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
            if (curMoveType == MoveType.survival && isGrounded == true)
                sidSpd = -mvspd;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (curMoveType == MoveType.survival && isGrounded == true)
                sidSpd = mvspd;

        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";

            if (allowZmovement == true)
            {

                if (curMoveType == MoveType.survival && isGrounded == true)
                    fwdSpd = mvspd;

            }
            else
            {
                if (curMoveType == MoveType.survival && jetPack == true)
                {
                    JetPack();
                }
            }

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (allowZmovement == true)
            {

                if (curMoveType == MoveType.survival && isGrounded == true)
                    fwdSpd = -mvspd;
            }


        }
    }






    public void OnUp()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
            if (curMoveType == MoveType.survival)
                sidSpd = 0;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (curMoveType == MoveType.survival)
                sidSpd = 0;

        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";

            if (allowZmovement == true)
            {

                if (curMoveType == MoveType.survival)
                    fwdSpd = 0;

            }

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (allowZmovement == true)
            {

                if (curMoveType == MoveType.survival)
                    fwdSpd = 0;
            }


        }
    }







    public void laneShift(int dir)
    {

        if (dir == 0 && curLanePos != 0)
        {
            this.gameObject.transform.position -= new Vector3(1, 0, 0);
            curLanePos -= 1;
        }
        else if (dir == 1 && curLanePos != curLaneLimit)
        {
            this.gameObject.transform.position += new Vector3(1, 0, 0);
            curLanePos += 1;

        }
    }

    public void StageProgression()
    {
        if (matchStarted == false)
        { 
            matchStarted = true;
            Debug.Log("match started");

            curMoveType = MoveType.survival;
            //gameObject.transform.position = pathListObjs[0].transform.position+new Vector3(0,1,0);
            cam.transform.position = gameObject.transform.position + new Vector3(0, cam.yPos, 0);

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
        if (jetPack_fuel <= 0)
        {
            rb.useGravity = true;
            jetPack = false;
            jetPack_fuel = 100;
        }
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
                            if (matchStarted == false)
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

                            if (curMoveType != MoveType.tapHero)
                            {
                                if (canMove)
                                {
                                    if (curMoveType == MoveType.run)
                                        laneShift(0);

                                }

                            }                            //  sidSpd = -mvspd;
                                                         // dir = directionMoving.left;
                                                         // dirText.text = "left";
                        }
                        else
                        {
                            Debug.Log("right Swype");
                            if (curMoveType != MoveType.tapHero)
                            {
                                if (canMove)
                                {
                                    if (curMoveType == MoveType.run)
                                        laneShift(1);

                                }

                            }
                            // dir = directionMoving.right;
                            // dirText.text = "right";


                        }

                        Debug.Log("Touch flank");



                    }


                    break;
            }
        }
    }









}


