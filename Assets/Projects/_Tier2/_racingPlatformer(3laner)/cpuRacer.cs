using UnityEngine;
using System.Collections;

public class cpuRacer : MonoBehaviour {

    public StateManagerRacing gameStateManager;
    public RaceMatchManager matchPhaser;

    public bool canMove, isGrounded;//is my player colliding with the floor?
    public bool isJumping;//is my player jumping or can he jump?
    public bool onWall;
    public GameObject lastWall;
    //RigidBody is basically a physical object in unitys engine lets us know it takes up space, and  allows it to use gravity

    public Rigidbody rb;//Container variable for my players Rigidbody 

    public RacerObj racerObj;//Get Custom racer class

    public float sidSpd;//how fast is my player moving left or right (Left this will be negative, RIght this willbe positive)



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

    public bool inMatch,finishedRace;
    //Built in unity function  Use this for initialization any variable that cannot be empty. Like rigidbody for example. this runs once the game loads or the stage is loaded
    void Awake()
    {
        //set the RigidBOdy of the player inside my RB variable.
        rb = GetComponent<Rigidbody>();///GetComponent<> Basically grabs any component attached to the current object. (object this script is on)
        racerObj = GetComponent<RacerObj>();

    }

    // Update is called once per frame
    void Update () {
        if (canMove == true)
            MoveObj();
        else
        {
            if (matchPhaser.matchState == RaceMatchManager.MatchState.Ongoing && finishedRace == false)
            {
                if (Time.time >= matchPhaser.stunnedAt + matchPhaser.stunTime)
                {
                    canMove = true;
                }
            }

        }
    }



    //Built in unity function  runs on a fixed time scale cam be changed in setting. All physics based movements should go in here
    void FixedUpdate()
    {

            rb.AddForce(new Vector3(sidSpd, 0, 0));//Add a force to my players RigidBody.
                                                   //The force must be a vector 3 for this script so it adds force in 3 Dimensions.


    }

    public void MoveObj()
    {
        sidSpd = 1 * moveSpd;


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

        // obstacleEffects efx = col.gameObject.GetComponent<obstacleEffects>();
        matchPhaser.stunnedAt = Time.time;
        canMove = false;
        col.gameObject.SetActive(false);//keep track of this later for laps
        rb.velocity = Vector3.zero;
        sidSpd = 0;
    }

}

//Built in unity function to detect When a collision has ended
void OnCollisionExit(Collision col)
{
    if (col.collider.tag == "ground")//if the object you WERE colliding withs tag is ground your player is leaving the floor
    {
        isGrounded = false;//Player is no longer grounded
        Debug.Log("you left ground ");//basic print message for debugging purposes

    }
    else if (col.collider.tag == "wall" && isGrounded == false)//if the object you collided withs tag is ground your player is on the floor
    {
        rb.isKinematic = false;
        onWall = false;

    }

}


void OnTriggerEnter(Collider col)
{
     if (col.tag == "GoalZone")//if the object you collided withs tag is ground your player is on the floor
    {
        if (inMatch == true)
            matchPhaser.RacerHitGoal(racerObj);//player reache goal check laps or if hes done
    }
    else if (col.tag == "buffZone")//if the object you collided withs tag is ground your player is on the floor
    {
        //class ref later

        Debug.Log("BuffZone");
        matchPhaser.buffs.cast(this.gameObject, 0 , gameStateManager);
    }
}


}
