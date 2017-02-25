using UnityEngine;
using System.Collections;

public class pacmanGame : MonoBehaviour
{

    public controlledGameStateManager gameStateManager;
    public controlledStageGenerator stageGen;
    public PlayerClasses playerClass;

    public CameraTracking cam;

    public Rigidbody rb;

    //player variables for movement
    public bool isController, isGrounded, dblJumped, cannonBackward;
    public float jumpHeight;

    //wall jump variables

    //raycast variables
    public float upCheck, horCheck, downCheck;

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


        if (isController == true)
            detectMove();

        HorColCheck();




    }

    void FixedUpdate()
    {


        // rb.AddForce(new Vector3(sidSpd, fwdSpd, 0),ForceMode.Force);//Add a force to my players RigidBody.
        //this.gameObject.transform.Translate(sidSpd, 0, fwdSpd);
        this.gameObject.transform.Translate(sidSpd*Time.fixedDeltaTime, 0, fwdSpd * Time.fixedDeltaTime);
        //The force must be a vector 3 for this script so it adds force in 3 Dimensions.


    }








    public void detectMove()
    {


        if (Input.GetKey(KeyCode.W))
        {
            //  fwdSpd = mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.N;
            dir = directionMoving.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //  fwdSpd = -mvspd;
            //myPlayer.dirFacing = Player.PlayerDirection.S;

            dir = directionMoving.down;

        }
        if (Input.GetKey(KeyCode.A))
        {


            //  sidSpd = -mvspd;
            dir = directionMoving.left;



        }
        if (Input.GetKey(KeyCode.D))
        {

            dir = directionMoving.right;

            //  sidSpd = mvspd;


        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //jump
            if (isGrounded == true)
                rb.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
            else if (dblJumped == false)
            {
                rb.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
                dblJumped = true;
            }



        }

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
            Debug.Log("There is something next to me!");
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
            Debug.Log("There is something next to me!");
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
            //isGrounded = true;

        }
        else if (col.gameObject.tag == "bouncePad")
        {
            rb.AddForce(jumpHeight * xDis, jumpHeight * yDis, 0, ForceMode.Impulse);
            dblJumped = false;
        }
     
    }

    public void OnCollisionStay(Collision col)
    {
        
    }


    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            // isGrounded = false;
            //dblJumped = false;
        }

      
    }



    public void OnTriggerEnter(Collider col)
    {
     
    }


    public void OnTriggerStay(Collider col)
    {

     
    }

    public void OnTriggerExit(Collider col)
    {
    
    }

    public void OnMouseDown()
    {


    }
}


