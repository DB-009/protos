using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class vonDoom : MonoBehaviour {


    //temp delete this UISystem
    public Text actionText;
    //Boolean variables can be true or false you'd use this to see wheather something is happening or a variable is set.

    public int playerID;

    public bool isGrounded;//is my player colliding with the floor?
    public bool isJumping;//is my player jumping or can he jump?
    public bool onWall;
    public GameObject lastWall;
    //RigidBody is basically a physical object in unitys engine lets us know it takes up space, and  allows it to use gravity

    public Rigidbody rb;//Container variable for my players Rigidbody 
    public float sidSpd;//how fast is my player moving left or right (Left this will be negative, RIght this willbe positive)

    public bool isController;

    //float is basically a number but it can be negative , positive or a decimal number.

    public float moveSpd;//A variable were going to ue later on in the tutorial to increase players speed or decrease it

    public int str, def, intelligence, hp, mhp, level;

    public float jumpForce;//how much force is the player using to jump. gravity is always pushing down on player so he needs X amount of force to jump

    public bool isMorphed, isMoving;
    public GameObject normalMode, morphMode;
    public SphereCollider normalCollider;
    public SphereCollider morphCollider;

    public CameraTracking myCam;
    public GameObject initialSpawnPos;



    public int wepID;
    public int comboCur, hits, kills;
    public float lastComboAt, comboStarted, allowedComboTime, comboLimit;

    //temporary combo system
    public GameObject comboPrefabA, comboPrefabB, comboPrefabC;

    public float yDis;
    //Built in unity function  Use this for initialization any variable that cannot be empty. Like rigidbody for example. this runs once the game loads or the stage is loaded
    void Awake()
    {
        //set the RigidBOdy of the player inside my RB variable.
        rb = GetComponent<Rigidbody>();///GetComponent<> Basically grabs any component attached to the current object. (object this script is on)

    }

    //Built in unity function  Update is called once per frame
    //most math based equations go here. You should not be moving your character or any object in update when working with Physics or rigidbodies 
    void Update()
    {

        MoveObj();


        if (Input.GetKeyDown(KeyCode.Z))//combo / button mash inputs should be  on GetKeyDown
        {
            if (wepID == 1)
            {
                //Debug.Log("in combo");
                Combo("hp");
            }

        }
        else if (Input.GetKeyDown(KeyCode.X))//combo / button mash inputs should be  on GetKeyDown
        {
            if (wepID == 1)
            {
                //Debug.Log("in combo");
                Combo("mp");
            }

        }
        else if (Input.GetKeyDown(KeyCode.C))//combo / button mash inputs should be  on GetKeyDown
        {
            if (wepID == 1)
            {
                //Debug.Log("in combo");
                Combo("lp");
            }

        }



    }





    //Built in unity function  runs on a fixed time scale cam be changed in setting. All physics based movements should go in here
    void FixedUpdate()
    {
        if (isController == true)
        {
            rb.AddForce(new Vector3(sidSpd, 0, 0));//Add a force to my players RigidBody.
                                                   //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
        }
    }


    //custom move function
    void MoveObj()
    {

        if (isController == true)
        {

            sidSpd = Input.GetAxis("Horizontal") * moveSpd;//Input.GetAxis will grab the Axis "Horizontal" in this case.
                                                           // to create axis  hit the Edit tab > Project Setting > Input


            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {//Input.GetKeyDown() waits for the user to press a key once
                if (isGrounded == true && onWall == false)
                {//if my player is grounded do whats in the barckets
                    isJumping = true;
                    rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);//Add a force to my players RigidBody.

                }
                else if (onWall == true)
                {//if my player is grounded do whats in the barckets
                    isJumping = true;
                    rb.isKinematic = false;

                    if (sidSpd < 0)
                        sidSpd = -moveSpd;
                    else if (sidSpd > 0)
                        sidSpd = moveSpd;

                    rb.AddForce(new Vector3(sidSpd, jumpForce, 0), ForceMode.Impulse);//Add a force to my players RigidBody.
                    onWall = false;
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

            if (sidSpd > 0 || sidSpd < 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

        }

        //elaborate here
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
        }
        else if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            if (lastWall != col.gameObject)
            {
                rb.isKinematic = true;
                onWall = true;
                lastWall = col.gameObject;
            }
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


    
    public void Combo(string action)
    {
        GameObject comboObj = comboPrefabA;

        actionText.text = action;
        if(action == "hk")
        {
            Debug.Log("high kick");

        }
        else if (action == "mk")
        {
            Debug.Log("mid kick");


        }
        else if (action == "lk")
        {
            Debug.Log("low kick");

        }

        else if (action == "hp")
        {
            Debug.Log("high punch");

        }
        else if (action == "mp")
        {
            Debug.Log("mid punch");

        }
        else if (action == "lp")
        {
            Debug.Log("low punch");

        }

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






        Quaternion targetRotation = Quaternion.LookRotation(pos - tileCreated.transform.position);

        // Smoothly rotate towards the target point.
        tileCreated.transform.rotation = targetRotation;





    }








    public void resetStats()
    {

        //lives = 3;
        // points = 0;
        // hp = mhp;



        Debug.Log("Characters are given acces to items and weapons");

        //can fire all weapons in beginning add them

    }




}
