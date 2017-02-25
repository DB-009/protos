using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopDownController : MonoBehaviour
{

    //line 357 gameSTateManager ref in RemovePlayer to take destroy player then recreate on game start


    //public controlledGameStateManager gameStateManager;
    //public controlledMatchPhaser matchPhaser;
   // public controlledUIManager uiManager;


    public Camera myCamera;

    public Rigidbody rb;
    public float sidSpd,fwdSpd;//how fast is my player moving left or right (Left this will be negative, RIght this willbe positive)

    public bool isController,isGrounded,isJumping,isMoving;
    public float playerSpd, maxPSpd, dashDistance,jmpForce;
    // speed is the rate at which the object will rotate
    public float speed;
 
    public int wins, losses, draws;



    public float bulForce;

    public int playerID, equipID;
    public enum PlayerDirection { N, S, W, E };
    public PlayerDirection dirFacing;


   

    public GameObject initialSpawnPos;

   
    //Arrays for multiple weapons
    public List<bool> canFire = new List<bool>();
    public List<float> reloadTime, nextShotAt, shotTime = new List<float>();

    //arrays of objects to spawn
    public GameObject[] wepSpawns;

    public List<Weapon> weaponCount = new List<Weapon>();
    //weapons list
    //gun1 (ammo count)
    //bombs(bomb count)
    //sword1(strikes counts) 1 sword=10;
    //ninja star (star count)
    //traps (trap count)

    public GameObject bulPrefab;

    void Awake()
    {

        rb = this.GetComponent<Rigidbody>();
        resetStats();

    }

    void Update()
    {

        MoveObj();//get key inputs for movement of player

        if(Input.GetKeyDown(KeyCode.H))
        {

            if (equipID == 0)
                equipID = 1;
            else
                equipID = 0;
        }


        //Update NExt shot at for each weapon
        int temp = 0;
            foreach (bool disCanFire in canFire)
            {
                if (disCanFire == false && Time.time >= nextShotAt[temp])
                {
                    canFire[temp] = true;
                    Debug.Log("Weapon " + temp + "can now fire");
                }
                temp++;
            }


        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (canFire[equipID] == true)
            {
                if (weaponCount[equipID].wepCount > 0)
                {
                    ///SHOOT
                    if (equipID == 0)
                    {
                        Shoot();
                    }
                    else if(equipID == 1)
                    {
                        //bomb throw script
                        Shoot();
                    }
                }
                else
                {

                    Debug.Log("cant fir wep #" + equipID);
                }
            }
        }
    }



    void FixedUpdate()
    {

            // Generate a plane that intersects the transform's position with an upwards normal.
            Plane playerPlane = new Plane(Vector3.up, transform.position);

            // Generate a ray from the cursor position
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

            // Determine the point where the cursor ray intersects the plane.
            // This will be the point that the object must look towards to be looking at the mouse.
            // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
            //   then find the point along that ray that meets that distance.  This will be the point
            //   to look at.
            float hitdist = 0.0f;
            // If the ray is parallel to the plane, Raycast will return false.
            if (playerPlane.Raycast(ray, out hitdist))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 targetPoint = ray.GetPoint(hitdist);

                // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }

        if (isController == true)
        {
            rb.AddForce(new Vector3(sidSpd, 0, fwdSpd));//Add a force to my players RigidBody.
                                                   //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
        }

    }





    void OnCollisionEnter(Collision col)
    {
        Debug.Log("you hit something " + col.collider.tag);//basic print message for debugging purposes
        if (col.collider.tag == "ground")//if the object you collided withs tag is ground your player is on the floor
        {
            isGrounded = true;///so grounded must be true because Player has hit the floor.
          
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
    }


        void OnTriggerEnter(Collider col)
    {

     

    }

    void OnTriggerStay(Collider col)
    {


    }


    //custom move function
    void MoveObj()
    {

        if (isController == true)
        {

            sidSpd = Input.GetAxis("Horizontal") * speed;//Input.GetAxis will grab the Axis "Horizontal" in this case.
                                                         // to create axis  hit the Edit tab > Project Setting > Input
            fwdSpd = Input.GetAxis("Vertical") * speed;//Input.GetAxis will grab the Axis "Horizontal" in this case.

            if (Input.GetKeyDown(KeyCode.Space))
            {//Input.GetKeyDown() waits for the user to press a key once
                if (isGrounded == true)
                {//if my player is grounded do whats in the barckets
                    isJumping = true;
                    rb.AddForce(new Vector3(0, jmpForce, 0), ForceMode.Impulse);//Add a force to my players RigidBody.

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


    public void Shoot()
    {
        GameObject tileCreated = GameObject.Instantiate(bulPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;


        Plane playerPlane = new Plane(Vector3.up, tileCreated.transform.position);

        // Generate a ray from the cursor position
        Ray ray = this.myCamera.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("Shoot ammunition:" + this.weaponCount[0].wepCount);
        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - tileCreated.transform.position);

            // Smoothly rotate towards the target point.
            tileCreated.transform.rotation = targetRotation;
        }

        tileCreated.GetComponent<Rigidbody>().AddForce(tileCreated.transform.forward * bulForce, ForceMode.Impulse);



        //Reset Weapon Timers / Reduce ammo or life of weapon
        nextShotAt[equipID] = Time.time + shotTime[equipID];
        canFire[equipID] = false;
        weaponCount[equipID].wepCount--;
    }

    void wepPickUp(int id)
    {
        if (id == 0)//gun bullets
        {
            weaponCount[0].wepCount += 100;
            //Debug.Log(weaponCount[0].wepCount + " : " + weaponCount[0].wepType);

        }
        if (id == 1)//bomb
        {
            weaponCount[1].wepCount += 1;
            //  Debug.Log(weaponCount[1].wepCount + " : " + weaponCount[1].wepType);



        }
    }


    public void resetStats()
    {

        //lives = 3;
       // points = 0;
       // hp = mhp;

        weaponCount.Clear();
        canFire.Clear();
        nextShotAt.Clear();
        shotTime.Clear();
        // itemBuffCount.Clear();

        // myPlayer = this;

        weaponCount.Add(new Weapon(1000, 1));//gun1
        weaponCount.Add(new Weapon(5, 1));//bomb

        Debug.Log("Characters are given acces to items and weapons");

        //can fire all weapons in beginning add them
        canFire.Add(true);
        canFire.Add(true);

        //set shot time for each wep
        shotTime.Add(.2f);
        shotTime.Add(1.5f);

        //set next shot at to now so they can fire.
        nextShotAt.Add(Time.time);
        nextShotAt.Add(Time.time);
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
                Destroy(myCamera.gameObject);

                Destroy(this.gameObject);
               // gameStateManager.players.RemoveAt(id);
            }
        }
        else
            Debug.Log("No disabling diabled players");

    }




}
