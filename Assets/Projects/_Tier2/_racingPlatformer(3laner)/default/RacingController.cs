using UnityEngine;
using System.Collections;

public class RacingController : MonoBehaviour
{


    public StateManagerRacing gameStateManager;
    public RaceMatchManager matchPhaser;

    //Boolean variables can be true or false you'd use this to see wheather something is happening or a variable is set.

    public int playerID;

    public bool canMove, isGrounded;//is my player colliding with the floor?
    public bool isJumping;//is my player jumping or can he jump?
    public bool onWall;
    public GameObject lastWall;
    //RigidBody is basically a physical object in unitys engine lets us know it takes up space, and  allows it to use gravity

    public Rigidbody rb;//Container variable for my players Rigidbody 

    public RacerObj racerObj;//Get Custom racer class

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

    public GameObject bulletPrefab;
    public float bulForce, fireRate, lastShotAt, bulletAcceptedDistance;

    public int wepID;
    public int comboCur, hits, kills;
    public float lastComboAt, comboStarted, allowedComboTime, comboLimit;

    //temporary combo system
    public GameObject comboPrefabA, comboPrefabB, comboPrefabC;

    public float yDis;

    public bool inMatch;

    //Built in unity function  Use this for initialization any variable that cannot be empty. Like rigidbody for example. this runs once the game loads or the stage is loaded
    void Awake()
    {
        //set the RigidBOdy of the player inside my RB variable.
        rb = GetComponent<Rigidbody>();///GetComponent<> Basically grabs any component attached to the current object. (object this script is on)
        racerObj = GetComponent<RacerObj>();
    }

    //Built in unity function  Update is called once per frame
    //most math based equations go here. You should not be moving your character or any object in update when working with Physics or rigidbodies 
    void Update()
    {

        if (canMove == true)
            MoveObj();
        else
        {
            if (matchPhaser.matchState == RaceMatchManager.MatchState.Ongoing)
            {
                if (Time.time >= matchPhaser.stunnedAt + matchPhaser.stunTime)
                {
                    canMove = true;
                }
            }

        }

        if (matchPhaser.matchState == RaceMatchManager.MatchState.Ongoing)
        {



            if (Input.GetKeyDown(KeyCode.Mouse0))//combo / button mash inputs should be  on GetKeyDown
            {
                if (wepID == 1)
                {
                    Debug.Log("in combo");
                    Combo();
                }

            }

            if (Input.GetKey(KeyCode.Mouse0))//guns  and rapid fire should be called on Hold mouse fire
            {
                if (wepID == 0)
                {
                    if (Time.time >= lastShotAt + fireRate)
                        Shoot(bulletPrefab, bulForce);
                }

            }

            if (Input.GetKey(KeyCode.Z))
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
        else if (col.collider.tag == "obstacle")//if the object you collided withs tag is ground your player is on the floor
        {
            if (matchPhaser.matchState == RaceMatchManager.MatchState.Ongoing)
            {
                // obstacleEffects efx = col.gameObject.GetComponent<obstacleEffects>();
                matchPhaser.stunnedAt = Time.time;
                canMove = false;
                col.gameObject.SetActive(false);//keep track of this later for laps
                rb.velocity = Vector3.zero;
                sidSpd = 0;
            }
            else
            {
                isGrounded = true;
            }

        }

    }

    //Built in unity function to detect When a collision has ended
    void OnCollisionExit(Collision col)
    {
        if (col.collider.tag == "ground")//if the object you WERE colliding withs tag is ground your player is leaving the floor
        {
            isGrounded = false;//Player is no longer grounded
            //Debug.Log("you left ground ");//basic print message for debugging purposes

        }
        else if (col.collider.tag == "wall" && isGrounded == false)//if the object you collided withs tag is ground your player is on the floor
        {
            rb.isKinematic = false;
            onWall = false;

        }

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "trackArea")//if the object you collided withs tag is ground your player is on the floor
        {
            myCam.canTrack = true;
        }
        else if (col.tag == "GoalZone")//if the object you collided withs tag is ground your player is on the floor
        {
            if (inMatch == true)
                matchPhaser.RacerHitGoal(racerObj);//player reache goal check laps or if hes done
        }
        else if (col.tag == "buffZone")//if the object you collided withs tag is ground your player is on the floor
        {
            //class ref later

            Debug.Log("BuffZone");
            matchPhaser.buffs.cast(this.gameObject, col.GetComponent<buffEffects>().id , gameStateManager);
        }
    }

    void OnTriggerExit(Collider col)
    {

        if (col.tag == "trackArea")//if the object you collided withs tag is ground your player is on the floor
        {
            myCam.canTrack = false;
        }


    }



    public void Shoot(GameObject bulz, float force)
    {

        // Get the point along the ray that hits the calculated distance.
        Ray targetPoint = gameStateManager.racingCam.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = Vector3.zero;

        Vector3 pos = targetPoint.GetPoint(0);

        

        direction = pos - new Vector3(gameStateManager.racingCam.transform.position.x, gameStateManager.racingCam.transform.position.y - yDis, gameStateManager.racingCam.transform.position.z);

        direction.Normalize();
        //Debug.Log("Dir = " + direction + " true x = " + direction.x + " true y = " + direction.y);

        if (direction.x < 0 && direction.x > -bulletAcceptedDistance)
        {
            if (direction.x > -bulletAcceptedDistance)
            {

                if (direction.y < 0 && direction.y > -bulletAcceptedDistance)
                {

                    Debug.Log("1 left side - bottom y - slow shot");

                }
                else if (direction.y > 0 && direction.y < bulletAcceptedDistance)
                {

                    Debug.Log("2 left side - top y - slow Shot");
                }
                else
                {
                    Debug.Log("quick shot; doesnt matter");

                }
            }
            else
            {
                Debug.Log("quick shot; doesnt matter");

            }

        }
        else if (direction.x > 0 && direction.x < bulletAcceptedDistance)
        {

            if (direction.x < bulletAcceptedDistance)
            {
                if (direction.y < 0 && direction.y > -bulletAcceptedDistance)
                {

                    Debug.Log("3 right side - bottom y - slow shot");

                }
                else if (direction.y > 0 && direction.y < bulletAcceptedDistance)
                {

                    Debug.Log("4 right side - top y - slow Shot");
                }
                else
                {
                    Debug.Log("quick shot; doesnt matter");

                }
            }


        }
        else
        {
            Debug.Log("quick shot; doesnt matter");
        }

        Vector3 bulPosition = Vector3.zero;
        if (isMoving == true)
        {
            if (sidSpd < 0 && direction.x < 0)
            {
                bulPosition = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd < 0 && direction.x > 0)
            {
                bulPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x > 0)
            {
                bulPosition = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
            }
            else if (sidSpd > 0 && direction.x < 0)
            {
                bulPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            }
        }
        else
        {
            bulPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }

        GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;




        Quaternion targetRotation = Quaternion.LookRotation(pos - tileCreated.transform.position);

        // Smoothly rotate towards the target point.
        tileCreated.transform.rotation = targetRotation;




        tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, direction.y, 0) * force, ForceMode.Impulse);
        lastShotAt = Time.time;
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
        Ray targetPoint = gameStateManager.racingCam.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = Vector3.zero;

        Vector3 pos = targetPoint.GetPoint(0);


        direction = pos - new Vector3(gameStateManager.racingCam.transform.position.x, gameStateManager.racingCam.transform.position.y - yDis, gameStateManager.racingCam.transform.position.z);

        direction.Normalize();
        Debug.Log("Dir = " + direction + " true x = " + direction.x + " true y = " + direction.y);
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

    public void resetStats()
    {

        //lives = 3;
        // points = 0;
        // hp = mhp;
        canMove = false;


        //Debug.Log("Characters are given acces to items and weapons");

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
                // Destroy(myCam.gameObject);
                gameStateManager.player = null;
                Destroy(this.gameObject);

            }
        }
        else
            Debug.Log("No disabling diabled players");

    }

}