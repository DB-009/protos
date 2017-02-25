using UnityEngine;
using System.Collections;

public class Ene : MonoBehaviour {

    public bool isController,targetting,canMove;

    //target Tracking
    public vonDoom target;

    //stats
    public float spd, jumpVar,closeDistance,landingCooldown;



    //movement system
    public Rigidbody rb;
    public float fwdSpd, sidSpd;
    public bool isGrounded;







    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {


            // Jump();

            TargettingSys();

            if (target != null)
            {
                Vector3 offset = target.transform.position - transform.position;
                float sqrLen = offset.sqrMagnitude;
                if (sqrLen < closeDistance * closeDistance)
                {
                    print("The other transform is close to me!");
                    if (canMove == true)
                        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, 0, 0), .7f * Time.deltaTime);
                    else
                        Debug.Log("CPU ATTACK LAUNCH");
                }
                else
                {
                    targetting = false;
                    //idle behaviour
                }
            }
        



    }


    void FixedUpdate()
    {
        if (isController == true)
        {

            // rb.AddForce(new Vector3(sidSpd, fwdSpd, 0),ForceMode.Force);//Add a force to my players RigidBody.
           // this.gameObject.transform.Translate(sidSpd, 0, 0);
            //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
        }
    }


    void Jump()
    {
        //JUMP
        Debug.Log("JUMP");
           
                rb.AddForce(0, jumpVar, 0, ForceMode.Impulse);
            
        

    }


    void TargettingSys()
    {

        if (targetting == false)
        {

            GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject disTarget in possibleTargets)
            {
                if (targetting == false)
                {
                    Vector3 offset = disTarget.transform.position - transform.position;
                    float sqrLen = offset.sqrMagnitude;
                    if (sqrLen < closeDistance * closeDistance)
                    {
                        targetting = true;
                        target = disTarget.GetComponent<vonDoom>();

                    }
                }
            }

            if (targetting == false)
            {
                target = null;
            }
        }

    }




    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground" )
        {
            isGrounded = true;
        }
    }





    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "edge")
        {
            Debug.Log("IN EDGE Enter");

            
            EdgeData disEdge =  col.GetComponent<EdgeData>();

            int tempVar = 0;
            foreach (GameObject disObj in disEdge.objectsOfInterest)
            {



                Vector3 offset = disObj.transform.position - transform.position;
                float tempDistance = offset.sqrMagnitude;//updating distances list

                Debug.Log("checking " + tempDistance);


                if (tempDistance < jumpVar * 4 && targetting == true)
                {
                    if (isGrounded == true && Time.time >= landingCooldown)
                    {
                        Jump();
                        landingCooldown = Time.time + 1;
                    }
                    //DOUBLE JUMP LOGIC OFF RIP
                }

                tempVar++;
            }

            if (tempVar == 0)
                canMove = false;

        }
    }


    void OnTriggerStay(Collider col)
    {
        if (col.tag == "edge")
        {
            Debug.Log("IN EDGE Stay");


            EdgeData disEdge = col.GetComponent<EdgeData>();

            int tempVar = 0;
            foreach (GameObject disObj in disEdge.objectsOfInterest)
            {



                Vector3 offset = disObj.transform.position - transform.position;
                float tempDistance = offset.sqrMagnitude;//updating distances list

                Debug.Log("checking distance of nearby object" );


                if (tempDistance < jumpVar + (jumpVar * .5f) && targetting == true)
                {
                    if (isGrounded == true && Time.time >= landingCooldown)
                    {
                        Jump();
                        landingCooldown = Time.time + 1;
                    }
                    //DOUBLE JUMP LOGIC OFF RIP
                }

                tempVar++;
            }



        }
    }






    void OnTriggerExit(Collider col)
    {
            if (col.tag == "edge")
            {
                Debug.Log("Left EDGE");
                canMove = true;
            }

      }

    










}
