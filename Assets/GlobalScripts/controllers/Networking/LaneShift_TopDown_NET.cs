using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LaneShift_TopDown_NET :  NetworkBehaviour
{


    public CasualGameManager gameStateManager;

    [SyncVar]
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
    [SyncVar]
    public bool isDead;
    [SyncVar]
    public float hp;
    [SyncVar]
    public float mhp, mp, mmp, lvl, baseHP, baseMp, mvspd;
    public float str, intel, def, vit, spd;

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
    [SyncVar]
    public float bulForce, fireRate, bulletAcceptedDistance, lastShotAt;
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


















    void Awake()
    {
        gameStateManager = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<CasualGameManager>();
        myTrans = this.transform;
        rb = this.GetComponent<Rigidbody>();

        gameStateManager.myCanvas.SetActive(true);

        myTrans = this.transform;
        initPos = this.transform.position;

    }





    //netowrking

    public override void OnStartLocalPlayer()
    {

        cam = Camera.main.GetComponent<CameraTracking>();

            CmdPlayerSpawn();
        




        gameStateManager.left.myNetHero = this;
        gameStateManager.right.myNetHero = this;

        gameStateManager.shootLeft.myNetHero = this;
        gameStateManager.shootRight.myNetHero = this;



        GetComponent<MeshRenderer>().material.color = Color.blue;




    }


    [Command]
    void CmdShoot(int dir)
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            this.transform.position,
            this.transform.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.right * dir * 6;

        bullet.GetComponent<projectileLife_NETWORK>().owner = this.gameObject;
        bullet.GetComponent<projectileLife_NETWORK>().playerBullet = true;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);



    }












    [Command]
    public void CmdPlayerSpawn()
    {

        if (gameStateManager.spawn == 0)//servers stats
        {
            Debug.Log("server spawned  server ");

            hpSlider = gameStateManager.hpSlider1;
            gameStateManager.server = this;

            gameStateManager.server.hp = gameStateManager.server.baseHP;
            gameStateManager.server.mp = gameStateManager.server.baseMp;
            gameStateManager.server.mhp = gameStateManager.server.hp;
            gameStateManager.server.mmp = gameStateManager.server.mp;

            //cam = Camera.main.GetComponent<CameraTracking>();
            hpSlider.maxValue = gameStateManager.server.mhp;
            hpSlider.value = gameStateManager.server.hp;

          

            if (isServer)
            {
                foreach (GameObject disGen in gameStateManager.gens)
                {
                    disGen.GetComponent<objGen_Net>().targetPlayers.Add(this.gameObject);
                }
                    RpcStatsSet(0,this.gameObject);
            }

        }
        else //server getting players stats
        {
            Debug.Log("server player spaned");

            hpSlider = gameStateManager.hpSlider2;
            gameStateManager.client1 = this;

            gameStateManager.client1.hp = gameStateManager.client1.baseHP;
            gameStateManager.client1.mp = gameStateManager.client1.baseMp;
            gameStateManager.client1.mhp = gameStateManager.client1.hp;
            gameStateManager.client1.mmp = gameStateManager.client1.mp;


            //cam = Camera.main.GetComponent<CameraTracking>();
            gameStateManager.hpSlider2.maxValue = gameStateManager.client1.mhp;
            gameStateManager.hpSlider2.value = gameStateManager.client1.hp;

            if(isServer)
            {
                foreach (GameObject disGen in gameStateManager.gens)
                {
                    disGen.GetComponent<objGen_Net>().targetPlayers.Add(this.gameObject);
                }
                RpcStatsSet(1,this.gameObject);
                RpcStatsSet(0, gameStateManager.server.gameObject);

            }
        } 
        gameStateManager.spawn++;
    }





    [ClientRpc]
    public void RpcStatsSet(int id, GameObject player)
    {

        if (id == 0)
        {
            Debug.Log("client   stats set on server");

            hpSlider = gameStateManager.hpSlider1;
            gameStateManager.server =  player.GetComponent<LaneShift_TopDown_NET>(); 

            gameStateManager.server.hp =  gameStateManager.server.baseHP;
            gameStateManager.server.mp =   gameStateManager.server.baseMp;
            gameStateManager.server.mhp = gameStateManager.server.hp;
            gameStateManager.server.mmp = gameStateManager.server.mp;


            //cam = Camera.main.GetComponent<CameraTracking>();
                hpSlider.maxValue = gameStateManager.server.mhp;
                hpSlider.value = gameStateManager.server.hp;


            //teleport to spawn location
            gameStateManager.server.transform.position = gameStateManager.spawnPos1.position;

        }
        else if (id == 1) 
        {


            Debug.Log("client  stats set on client");



            hpSlider = gameStateManager.hpSlider2;
            gameStateManager.client1 = this;

            gameStateManager.client1.hp = gameStateManager.client1.baseHP;
            gameStateManager.client1.mp = gameStateManager.client1.baseMp;
            gameStateManager.client1.mhp = gameStateManager.client1.hp;
            gameStateManager.client1.mmp = gameStateManager.client1.mp;


            //cam = Camera.main.GetComponent<CameraTracking>();
            gameStateManager.hpSlider2.maxValue = gameStateManager.client1.mhp;
            gameStateManager.hpSlider2.value = gameStateManager.client1.hp;

            //teleport to spawn location
            gameStateManager.client1.transform.position = gameStateManager.spawnPos2.position;

        }





        // foreach (objGen disGen in gameStateManager.gens)
        // {
        //    disGen.targetPlayer = this.gameObject;
        // }

    }










    [ClientRpc]
    public void RpcUpdateHPBars(GameObject targetPlayer,float amount)
    {

        if(targetPlayer == gameStateManager.server.gameObject)
        {
           // Debug.Log("servers hp bars updated means server took dmg");
            gameStateManager.server.hp -= amount;
            gameStateManager.hpSlider1.maxValue = gameStateManager.server.mhp;
            gameStateManager.hpSlider1.value = gameStateManager.server.hp;
            if (gameStateManager.server.hp <= 0 && gameStateManager.server.isDead == false)
            {
                Debug.Log("server died;");
                gameStateManager.server.isDead = true;

                gameStateManager.server.gameObject.SetActive(false);

                if (isServer)
                {
                    RpcUpdateHPBars(targetPlayer,0);
                    RpcRespawn();
                }




            }
        }
      


        if (gameStateManager.client1 != null)
        {


            if (targetPlayer == gameStateManager.client1.gameObject)
            {
                Debug.Log("clients hp bars updated means client took dmg");
                gameStateManager.client1.hp -= amount;
                gameStateManager.hpSlider2.maxValue = gameStateManager.client1.mhp;
                gameStateManager.hpSlider2.value = gameStateManager.client1.hp;

                if (gameStateManager.client1.hp <= 0 && gameStateManager.client1.isDead == false)
                {
                    Debug.Log("client died;");
                    gameStateManager.client1.isDead = true;
                    gameStateManager.client1.gameObject.SetActive(false);

                    if (isServer)
                    {
                        RpcUpdateHPBars(targetPlayer,0);
                        RpcRespawn();
                    }


                }
            }

          
        }

    }


    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            Debug.Log("Respawn sys");
            
        }
    }








    public void TakeDamage(int amount,GameObject player)
    {
        if (isServer)
        {
            if (player == gameStateManager.server.gameObject)
            {
                
                Debug.Log("server took damage " + amount);

            }
            else if (player == gameStateManager.client1.gameObject)
            {
                Debug.Log("client took damage " + amount);

            }

            RpcUpdateHPBars(player,amount);

        }





    }



















    public void Update()
    {

        //reset input remove this for accelartion based movement if remove add limit
        //fwdSpd = 0;
        //sidSpd = 0;
        if (!isLocalPlayer)
        {
            return;
        }
        else if (isLocalPlayer)
        {
            if(cam.myTarget == null)
            cam.myTarget = this.transform;
        }



        //Wall jump functions


        if (isController == true)
        { 
            if(canMove != false)
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
         


 
      }

        HorColCheck();




    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        if (jetPack)
        {
            rb.AddForce(Physics.gravity * rb.mass);
            rb.useGravity = false;
        }

            // rb.AddForce(new Vector3(sidSpd, fwdSpd, 0),ForceMode.Force);//Add a force to my players RigidBody.
            //this.gameObject.transform.Translate(sidSpd, 0, fwdSpd);
            this.gameObject.transform.Translate(sidSpd*Time.fixedDeltaTime, 0, fwdSpd * Time.fixedDeltaTime);
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

                            if (curMoveType != MoveType.tapHero)
                            {
                                if(canMove)
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
                TakeDamage(25, this.gameObject);
                
               
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
                TakeDamage(10,this.gameObject);
               

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


    public void laneShift(int dir)
    {

        if(dir==0 && curLanePos != 0)
        {
            this.gameObject.transform.position -= new Vector3(1, 0, 0);
            curLanePos -= 1;
        }
        else if(dir==1 && curLanePos != curLaneLimit)
        {
            this.gameObject.transform.position += new Vector3(1, 0, 0);
            curLanePos += 1;

        }
    }

    public void StageProgression()
    {
        if (matchStarted == true)
        {

            Debug.Log("check object type to see how we continue movement");
            Debug.Log("lane + guitar hero have same position system");
            Debug.Log("icy tower + timeed jumper + pinball have exact positioning");


            curPath++;
            //this.transform.position = pathListObjs[curPath].transform.position;
            canMove = true;
        }
        else
        {
            matchStarted = true;
            Debug.Log("match started");

            curMoveType = MoveType.survival;
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
        if (!isLocalPlayer)
        {
            return;
        }

        if (action == 0)
            {

            if (Time.time >= lastShotAt + fireRate)
            {
               // CmdFire(-1);
                CmdShoot(-1);
                lastShotAt = Time.time;
            }



        }
            else if (action == 1)
            {


            if (Time.time >= lastShotAt + fireRate)
            {
               // CmdFire(1);
                CmdShoot(1);
                lastShotAt = Time.time;

            }


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
            if (curMoveType == MoveType.run)
                laneShift(0);

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

            // dir = directionMoving.right;
            // dirText.text = "right";

            //  sidSpd = mvspd;
            if (curMoveType == MoveType.run)
                laneShift(1);

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {

            // dir = directionMoving.right;
            // dirText.text = "right";
            if (curMoveType == MoveType.survival && jetPack == false)
                Jump();

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


        if (Input.GetKey(KeyCode.A))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
            if (curMoveType == MoveType.survival)
                sidSpd = -mvspd;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (curMoveType == MoveType.survival)
                sidSpd = mvspd;

        }

        if (Input.GetKey(KeyCode.W))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";

            if (allowZmovement == true)
            {

                if (curMoveType == MoveType.survival)
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

                if (curMoveType == MoveType.survival)
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
            if (curMoveType == MoveType.survival)
                sidSpd = 0;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (curMoveType == MoveType.survival)
                sidSpd = 0;

        }

        if (Input.GetKeyUp(KeyCode.W))
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
        else if (Input.GetKeyUp(KeyCode.S))
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





















   



}


