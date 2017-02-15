using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class mf_player : MonoBehaviour
{
    public GameObject target;
    public bool waveOn;


    public bool inCannon;
    public GameObject curCannon;

    public mf_GameManager gameStateManager;

    public PlayerClasses playerClass;

    public CameraTracking cam;

    public Rigidbody2D rb;

    //player variables for movement
    public bool isController,isGrounded, dblJumped, cannonBackward;
    public float jumpHeight;

    //wall jump variables
    public float wallTimeStart, wallTimeLimit, lastWallExitTIme, wallAllowedStoreTime;

    //cannon variables
    public float timeBetweenCanon, LastCannonAt, noGravityTimeLimit;

    //raycast variables
    public float upCheck,horCheck,downCheck;

    public List<GameObject> lastCannon = new List<GameObject>();

    public bool onWall , jumpThrough;
    public GameObject lastWall;

    public GameObject curAtmosphere, summonObj;
    public List<GameObject> summons = new List<GameObject>();

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


    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        this.hp = this.vit * lvl + baseHP;
        this.mp = this.intel * lvl + baseMp;
        this.mhp = this.hp;
        this.mmp = this.mp;


        initPos = this.transform.position;
        if(this.gameObject != gameStateManager.player1.gameObject)
        {
            target = gameStateManager.player1.gameObject;
        }
    }

    public void Update()
    {

        //reset input remove this for accelartion based movement if remove add limit
        fwdSpd = 0;
        sidSpd = 0;


        //Wall jump functions
        //if player has been holding wall for x time make em fall
        if (onWall == true)
        {
            if (Time.time >= wallTimeStart + wallTimeLimit)
            {
                Debug.Log("free fall");
                onWall = false;
                rb.isKinematic = false;
                rb.gravityScale = 1;
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



        if (isController == true)
        {



           
            if (inCannon == true)//change to canmove
            {
                rb.velocity = Vector2.zero;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    inCannon = false;
                    curCannon.GetComponent<buffZone>().RunBuffPragma(curCannon.GetComponent<buffZone>().type, true, this.gameObject);

                }

            }
            else if(LastCannonAt == 0 )
            {
                detectMove();
            }


        }
        //HorColCheck();

        if(this.gameObject != gameStateManager.player1.gameObject)
        {
            if(waveOn == true)
            {
                float playerX = gameStateManager.player1.gameObject.transform.position.x;
               if(this.gameObject.transform.position.x < playerX)
                {
                    sidSpd = mvspd;
                }
               else
                {
                    sidSpd = -mvspd;
                }
            }
        }


    }

    void FixedUpdate()
    {

                if (onWall == false)//its this players turn in battle system
                {

                    // rb.AddForce(new Vector3(sidSpd, fwdSpd, 0),ForceMode.Force);//Add a force to my players Rigidbody2D.
                    //this.gameObject.transform.Translate(sidSpd, 0, fwdSpd);
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce( new Vector2( sidSpd, fwdSpd), ForceMode2D.Force);
                    //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
                }

    }








    public void detectMove()
    {


        if (Input.GetKey(KeyCode.W))
        {
            //fwdSpd = mvspd * Time.deltaTime;
            //myPlayer.dirFacing = Player.PlayerDirection.N;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //  fwdSpd = -mvspd * Time.deltaTime;
            //myPlayer.dirFacing = Player.PlayerDirection.S;


        }
        if (Input.GetKey(KeyCode.A))
        {


            sidSpd = -mvspd;



        }
        if (Input.GetKey(KeyCode.D))
        {


            sidSpd = mvspd;


        }
        if (Input.GetKeyDown(KeyCode.F))
        {


         
            summonSpawn();

        }

        if (Input.GetKeyDown(KeyCode.X))
        {



            SendAttackWave();

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //jump
            if (isGrounded == true && onWall == false && inCannon == false)
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            else if(dblJumped == false && onWall == false && inCannon == false)
            {
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                dblJumped = true;
            }
            else if (onWall == true)
            {//if my player is grounded do whats in the barckets
              
                rb.isKinematic = false;
                rb.gravityScale = 1;
                if (sidSpd < 0)
                    sidSpd = -mvspd;
                else if (sidSpd > 0)
                    sidSpd = mvspd;
                dblJumped = false;
                onWall = false;
                lastWallExitTIme = Time.time;
                rb.AddForce(new Vector2(sidSpd, jumpHeight), ForceMode2D.Impulse);//Add a force to my players Rigidbody2D.
                
            }


        }



    }











    public void summonSpawn()
    {
         GameObject spawnObj = GameObject.Instantiate(summonObj, this.gameObject.transform.position+new Vector3(2+summons.Count,0,0), Quaternion.identity) as GameObject;
        summons.Add(spawnObj);


        spawnObj.gameObject.SetActive(true);
    }


    public void SendAttackWave()
    {
        if(waveOn == false)
        {
            foreach(GameObject disSum in summons)
            {
                disSum.GetComponent<mf_player>().waveOn = true;
            }
            waveOn = true;
        }
        else
        {
            foreach (GameObject disSum in summons)
            {
                disSum.GetComponent<mf_player>().waveOn = false;
            }
            waveOn = false;
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



    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = true;
            dblJumped = false;
        }
        else if (col.gameObject.tag == "bouncePad")
        {
            rb.AddForce(new Vector2(jumpHeight * xDis, jumpHeight * yDis), ForceMode2D.Impulse);
            dblJumped = false;
        }
        else if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            Debug.Log("Eh enterd;");

            if (lastWall != col.gameObject)
            {
    
                Debug.Log("Eh enterd2;");
                rb.isKinematic = true;
                rb.gravityScale = 0;
                wallTimeStart = Time.time;
                onWall = true;
                lastWall = col.gameObject;

            }
        }
    }

    public void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            Debug.Log("Eh colsaty;");
            if (lastWall != col.gameObject)
            {
        
                Debug.Log("Eh enterd3;");

                rb.isKinematic = true;
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
             isGrounded = false;
            //dblJumped = false;
        }

        else if (col.collider.tag == "wall" && isGrounded == false)//if the object you collided withs tag is ground your player is on the floor
        {
           // rb.isKinematic = false;
           // rb.gravityScale = 1;
            Debug.Log("Eh enterd4;");

           // onWall = false;
           // dblJumped = false;
        }
    }



    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "SpeedUp")
        {
            //access the buffZone
            Debug.Log("Speed up");
            rb.AddForce(new Vector2(sidSpd * 1.5f, 0), ForceMode2D.Impulse);

        }
        if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone

            col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, true, this.gameObject);

        }
        if (col.gameObject.tag == "edge")
        {
            //access the buffZone

            trigger = col.gameObject;

        }
    }


    public void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone

            if (col.GetComponent<buffZone>().type == buffZone.Type.cannon)
            {
                //Debug.Log("another yo0");
                if(rb.isKinematic == false)
                {
                    col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, true, this.gameObject);

                }
                else
                {
                    inCannon = true;
                    curCannon = col.gameObject;
                }

                
            }

        }
        if (col.gameObject.tag == "atmosphere" && this.gameObject != gameStateManager.player1)
        {
            if (curAtmosphere != null)
            {
                if (col.gameObject != curAtmosphere)
                {
                    if (col.gameObject.GetComponent<_Atmosphere>().energyVal < curAtmosphere.GetComponent<_Atmosphere>().energyVal)
                    {
                        Debug.Log("in atmos thats weaker ");
                    }
                    else
                    {
                        Debug.Log("in atmos thats greater switch ");
                        curAtmosphere = col.gameObject;
                    }
                }
                else
                {
                    //Debug.Log("in cur atmos ");

                }
            }
            else
            {
                curAtmosphere = col.gameObject;
            }
        }

    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "edge")
        {
            //access the buffZone

            trigger = null;

        }
        else if (col.gameObject.tag == "atmosphere" && this.gameObject != gameStateManager.player1)
        {
            if (curAtmosphere != null)
            {
                if (col.gameObject == curAtmosphere)
                {

                    Debug.Log("left cur atmos ");
                    curAtmosphere = null;

                    gameStateManager.objectControlling = gameStateManager.player1.gameObject;

                    gameStateManager.objectControlling.GetComponent<mf_player>().isController = true;

                    this.isController = false;
                    cam.myTarget = gameStateManager.player1.transform;

                }
                else
                {
                    Debug.Log("exited the weaker atmos ");

                }
            }

        }
    }






















    public void OnMouseDown()
    {
        Debug.Log("Clickd me " + this.gameObject.name);
        if(this.gameObject != gameStateManager.objectControlling && this.gameObject.tag != "atmosphere")
        {
            Debug.Log("Clickd me2 " + this.gameObject.name);
            if (this.gameObject != gameStateManager.player1.gameObject)
            {
                if(curAtmosphere != null)
                {

                    if(curAtmosphere.GetComponent<_Atmosphere>().owner == gameStateManager.player1.gameObject)
                    {
                        gameStateManager.objectControlling.GetComponent<mf_player>().isController = false;
                        gameStateManager.objectControlling = this.gameObject;
                        waveOn = false;
                        this.isController = true;
                        cam.myTarget = this.transform;
                    }
                }
            }
            else if(this.gameObject == gameStateManager.player1.gameObject)
            {
                Debug.Log("Clickd me3 " + this.gameObject.name);

                gameStateManager.objectControlling.GetComponent<mf_player>().isController = false;
                gameStateManager.objectControlling = this.gameObject;

                this.isController = true;
                cam.myTarget = this.transform;
            }
         

        }

    }
}







