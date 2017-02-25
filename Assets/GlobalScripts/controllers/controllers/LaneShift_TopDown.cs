using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LaneShift_TopDown : MonoBehaviour
{


    public CasualGameManager gameStateManager;
    public CollisionHandler   colHandler;
    public Input_Controller inputController;
    public GunControl gunControl;
 
    public Transform myTrans;

    public Vector2 startPos;

    public Slider hpSlider;
  
    public controlledStageGenerator stageGen;
    public PlayerClasses playerClass;

    public CameraTracking cam;

    public Rigidbody rb;

    //player variables for movement
    public bool isController;

    public float jumpHeight;

    public bool isJumping, canDblJump, dblJumped;
    public bool doubleJump = false;
    public bool isDoubleJumping = false;
    public float doubleJumpedAt;

    public float jumpForce = 700f;

    //raycast variables
    public float upCheck, horCheck, downCheck;

    public bool canMove, matchStarted,allowZmovement;
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


 
    public float bulForce, fireRate, lastShotAt, bulletAcceptedDistance;
    public int kills;
    public objGen eneGen1, eneGen2;

    public float spreadShotAngle,jetPack_fuel,jetPack_rate, jetPack_force,gunUpgradedAt,gunUpgradeTime, dashDistance;
    public bool jetPack,gunDoubleUpgrade, isDashing;


    ///COMBO VARIABLES
    public int wepID;
    public int comboCur, hits;
    public float lastComboAt, comboStarted, allowedComboTime, comboLimit;

    //temporary combo system
    public GameObject comboPrefabA, comboPrefabB, comboPrefabC;

    public GunControl.gunType curGun;

    public float screen_half = Screen.width / 2;

    public float laserFiredAt,laserRate;
    public bool firingLaser;
    public float laserFiredAt2;
    public bool firingLaser2;
    public GameObject laserEnd, laserCenter, laserEnd2, laserCenter2;

    public float walkTimer, impulseTimer, dashTimer, lastDash, lastImpulse;
    public Vector3 targetReposition;
    public int dashes,maxImpulses,impulses;

    public bool isAndroid, airWalk;


    public void Awake()
    {

        colHandler = this.gameObject.GetComponent<CollisionHandler>();
        inputController = this.gameObject.GetComponent<Input_Controller>();
        gunControl = this.gameObject.GetComponent<GunControl>();

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

        foreach (GameObject disGen in this.gameStateManager.gens)
        {
            disGen.GetComponent<objGen>().targetPlayer = this.gameObject;
        }

        this.hpSlider.maxValue = this.mhp;
        this.hpSlider.value = this.hp;
    }

    public void Update()
    {
        if (Time.time >= lastDash + 2)
        {
            targetReposition = Vector3.zero;
            isDashing = false;
            rb.isKinematic = false;

            if (dashes== maxImpulses && Time.time >= lastDash + 3)
            {
                dashes = 0;
            }
        }


        if (hp <= 0)
            {
                Debug.Log("you died;");
                this.gameObject.SetActive(false);
            }

            if (isController == true)
            {
                if (canMove != false)
                {

                SpeedUpates();
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

        if (isDashing == true)
        {
            //this.transform.position = targetReposition;
            // rb.MovePosition(targetReposition);
            rb.isKinematic = true;


            rb.MovePosition(transform.position + new Vector3(targetReposition.x, targetReposition.y, 0) * (mvspd * dashDistance) * Time.fixedDeltaTime);

            
            if (Time.time >= lastDash + 1)
            {
                isDashing = false;
                targetReposition = Vector3.zero;
                rb.isKinematic = false;

            }

        }
        else
            this.gameObject.transform.Translate(sidSpd * Time.fixedDeltaTime, 0, fwdSpd * Time.fixedDeltaTime);

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
                laserEnd2.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);

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
            laserEnd2.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
            laserCenter2.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
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
                Debug.Log("1");

                objPos = new Vector3(gameObject.transform.position.x - tempXdis - 1, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd < 0 && direction.x > 0)
            {
                Debug.Log("2");

                objPos = new Vector3(gameObject.transform.position.x - tempXdis, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x > 0)
            {
                Debug.Log("3");

                objPos = new Vector3(gameObject.transform.position.x + tempXdis + 1, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x < 0)
            {
                Debug.Log("4");
                objPos = new Vector3(gameObject.transform.position.x - tempXdis, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        }
        else
        {
            objPos = new Vector3(gameObject.transform.position.x + tempXdis, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        GameObject tileCreated = GameObject.Instantiate(comboObj, objPos, Quaternion.identity) as GameObject;

        //tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        //tileCreated.GetComponent<projectileLife>().playerBullet = true;

        Quaternion targetRotation = Quaternion.LookRotation(pos - tileCreated.transform.position);

        // Smoothly rotate towards the target point.
        tileCreated.transform.rotation = targetRotation;

    }

    public void Impulse(int dir)
    {
        if (impulses != maxImpulses)//also add number of dashes can connect
        {
            if(dir == 0)
            {
                transform.position =  transform.position - new Vector3(dashDistance, 0, 0) ;
            }
            else if (dir == 1)
            {
                transform.position = transform.position + new Vector3(dashDistance, 0, 0);
            }
            else if (dir == 2)
            {
                transform.position = transform.position + new Vector3(0, dashDistance, 0);

            }
            else if (dir == 3)
            {
                transform.position = transform.position - new Vector3(0, dashDistance, 0);

            }
        }
    }

    public void playerDash(int dir)
    {
        if (Time.time <= dashTimer && dashTimer != 0 && Time.time >= lastDash + impulseTimer && dashes != maxImpulses)//also add number of dashes can connect
        {

            if (dir == 0)
            {
                // rb.AddForce(new Vector3(-myPlayer.playerSpd - 4, 0,0), ForceMode.Acceleration);
                targetReposition = new Vector3(-dashDistance, 0, 0);
                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;
           
            }
            else if (dir == 1)
            {
                // rb.AddForce(new Vector3(myPlayer.playerSpd + 4, 0, 0), ForceMode.Force);
                targetReposition = new Vector3(dashDistance, 0, 0);

                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;

    
            }               
            else if (dir == 2)
            {
                // rb.AddForce(new Vector3(myPlayer.playerSpd + 4, 0, 0), ForceMode.Force);
                targetReposition = new Vector3(0, dashDistance, 0);

                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;

  
            }
            else if (dir == 3)
            {
                // rb.AddForce(new Vector3(myPlayer.playerSpd + 4, 0, 0), ForceMode.Force);
                targetReposition = new Vector3(0, -dashDistance, 0);


                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;

    
            }
            sidSpd = 0;
            rb.velocity = Vector3.zero;
            dashes++;
        }
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
        Vector3 objPos = Vector3.zero;

   
        if (dir == 0)
        {
            if (sidSpd < 0)//
            {
                objPos = new Vector3(gameObject.transform.position.x - (1.3f), gameObject.transform.position.y, gameObject.transform.position.z);
               // Debug.Log("spd1 norm" + sidSpd);

            }
            else if (sidSpd > 0)
            {
                objPos = new Vector3(gameObject.transform.position.x - .7f, gameObject.transform.position.y, gameObject.transform.position.z);
                //Debug.Log("spd1 else" + sidSpd);

            }
            else
            {
                objPos = new Vector3(gameObject.transform.position.x -1, gameObject.transform.position.y, gameObject.transform.position.z);
            }


        }
        else if (dir == 1)
        {
            if (sidSpd > 0)
            {
                objPos = new Vector3(gameObject.transform.position.x + (1.3f), gameObject.transform.position.y, gameObject.transform.position.z);
                //Debug.Log("spd2 norm" + sidSpd);
            }
            else if (sidSpd < 0)///
            {
                objPos = new Vector3(gameObject.transform.position.x + .7f, gameObject.transform.position.y, gameObject.transform.position.z);
               // Debug.Log("spd2 else" + sidSpd);

            }
            else
            {
                objPos = new Vector3(gameObject.transform.position.x+1, gameObject.transform.position.y, gameObject.transform.position.z);
            }

        }
        else if (dir == 2)
        {
            if (sidSpd > 0)
                objPos = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y + 1, gameObject.transform.position.z);
            else if (sidSpd < 0)
                objPos = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y + 1, gameObject.transform.position.z);
            else
                objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);

        }
        else if (dir == 3)
        {
            if (sidSpd > 0)
                objPos = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y - 1, gameObject.transform.position.z);
            else if (sidSpd < 0)
                objPos = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y - 1, gameObject.transform.position.z);
            else
                objPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, gameObject.transform.position.z);

        }
    

        GameObject tileCreated = GameObject.Instantiate(comboObj, objPos, Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;

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



        if (!doubleJump && !colHandler.isGrounded && jetPack == false)//double jump
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
        else if (colHandler.isGrounded && !doubleJump && jetPack == false)//normal jump
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
}


