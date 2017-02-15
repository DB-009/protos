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
        detectMove();

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
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //jump
            if (isGrounded == true && onWall == false)
                rb.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
            else if(dblJumped == false && onWall == false)
            {
                rb.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
                dblJumped = true;
            }
            else if (onWall == true)
            {//if my player is grounded do whats in the barckets
              
                rb.isKinematic = false;

                if (sidSpd < 0)
                    sidSpd = -mvspd;
                else if (sidSpd > 0)
                    sidSpd = mvspd;

                onWall = false;
                lastWallExitTIme = Time.time;
                rb.AddForce(new Vector3(sidSpd, jumpHeight, 0), ForceMode.Impulse);//Add a force to my players RigidBody.
                
            }


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
            //isGrounded = true;

        }
        else if (col.gameObject.tag == "bouncePad")
        {
            rb.AddForce(jumpHeight * xDis, jumpHeight * yDis, 0, ForceMode.Impulse);
            dblJumped = false;
        }
        else if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            Debug.Log("Eh enterd;");

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







