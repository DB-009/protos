using UnityEngine;
using System.Collections;

public class HoverBoard : MonoBehaviour
{

    public float speed = 50f;//forward or reverse speed
    public float turnSpeed = 1.65f;//rotational speed
    public float hoverForce = .35f;//strength of hover force
    public float hoverHeight = .95f;//min hover distance

    public float powerInput, turnInput;//i made these 2 public to monitor power input incase you change input method


    public float hoverDamp = 0.3F;//dampening for when on a slope
    private Rigidbody carRigidbody;//rigidbody

    public bool isGrounded;//is grounded is so they cant move while in air

    public GameObject[] raycastPoints;//place object on 4 corners of vehicle

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();//get rigidbody
    }

    void Update()
    {
        
       

        //DELETE THIS
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("made car flip");
            //bounce factor is affected by hover force and hover height
            carRigidbody.AddRelativeForce(0f, 20, 0f, ForceMode.Impulse);//temp function to make car jump so you can test bounce factor
        }


        powerInput = Input.GetAxis("Vertical");//get forward and reverse input

        if (powerInput != 0)//i only let you rotate if youre actually moving
            turnInput = Input.GetAxis("Horizontal");
        else
            turnInput = 0;

        //Key based input remove the top ones if you want to use these (this is for adding certain features when you press keys down , hold, or release)
        // OnDown(); 
        //OnHold();
        OnUp();//i left this one so i can stop rotation when you let go (I kill angular velovity this stops weird rotation
    }

    void FixedUpdate()
    {

        Renderer rend = GetComponent<Renderer>();//get the renderer


        Vector3 center = rend.bounds.center;//get the center this is for future use at this point for making car bounce on rev
        float radius = rend.bounds.extents.magnitude;//get radius basically half the size 

        //This si to see raycasts positions are correct on object
        Debug.DrawRay(raycastPoints[0].transform.position  , -raycastPoints[0].transform.up, Color.green);
        Debug.DrawRay(raycastPoints[1].transform.position, -raycastPoints[1].transform.up, Color.red);
        Debug.DrawRay(raycastPoints[2].transform.position, -raycastPoints[2].transform.up, Color.blue);
        Debug.DrawRay(raycastPoints[3].transform.position, -raycastPoints[3].transform.up, Color.yellow);

        //center  raycast 
        Debug.DrawRay(transform.position, -transform.up, Color.cyan);

        int grnChk = 4;//ground check we need atleast two wheels on floor 

        for(int i = 0; i < raycastPoints.Length; i++)
        {
            Ray ray = new Ray(raycastPoints[i].transform.position, -raycastPoints[i].transform.up);//create the ray  at its given position
            RaycastHit hit;//empty hit var

            if (Physics.Raycast(ray, out hit, hoverHeight))//if raycast hit we shall apply force in here
            {

                if (hit.transform.gameObject.tag == "ground")//I only allow the character to move on certain objects you can alter this to ride on walls etc
                {
                    grnChk--;//one ray has hit floor we take one away to know that one of 4 wheels is on ground
                             //Debug.Log("ray hit #" + i);

                    float proportionalHeight = (hoverHeight - hit.distance);// compression formula

                    Vector3 appliedHoverForce = raycastPoints[i].transform.up * proportionalHeight * hoverForce; // force we shall apply
                    //the transform up is because we are pushing up * compressions * the hover force

                    carRigidbody.AddForce(appliedHoverForce, ForceMode.Impulse);//apply force


                    //this bit is to detect ramps or slopes and tilt vehicle accordingly
                    //for a real hover car effect remove this bit
                    RaycastHit rampDetect;
                    if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out rampDetect, 5))
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, rampDetect.normal), rampDetect.normal), Time.deltaTime * 5.0f);
                    }

                }


            }

        }

        if (grnChk <= 2)//if two wheels are on floor we can move perhaps check if its the wheels that pull for front ot back wheel my cars are 4 wheel drive
            isGrounded = true;
        else
            isGrounded = false;



        if(isGrounded == true)//if grounded he can rev forward or back and turn
        {
            carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed, ForceMode.Force);//move forward or back

            //Wprk in progress drift + traction
            float extraDrift = 0;
         //   if (turnInput < 0)
           //     extraDrift = (-powerInput * (turnInput * turnSpeed)) * Time.fixedDeltaTime;
           // else if (turnInput > 0)
           //     extraDrift = (powerInput * (turnInput * turnSpeed)) * Time.fixedDeltaTime;

            //turn vehicle or in physics terms rotate
            carRigidbody.AddRelativeTorque(0f, turnInput * turnSpeed + (extraDrift), 0f);
        }


    }




    public void OnDown()
    {
        //forwardand reverse inputs current im using get axis but this works also
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Rock Car");
           // powerInput = .5f;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Rock Car");
           // powerInput = -.5f;
        }


        //turn/rotate inputs
        if (Input.GetKeyDown(KeyCode.A) && powerInput != 0)
        {
            turnInput = -.5f;
        }
        else if (Input.GetKeyDown(KeyCode.D) && powerInput != 0)
        {
            turnInput = .5f;

        }






    }

    public void OnHold()
    {


        if (Input.GetKey(KeyCode.W))
        {
            //powerInput = 1;

        }
        else if (Input.GetKey(KeyCode.S))
        {
           // powerInput = -1;

        }
        /* PSUEDO SLOW DOWN SCRIPT removed because im not using keydowns for forward movement using built in drag on rigidbody
        else
        {
            if(powerInput <0)
            {
                if (powerInput == -.5f)
                {
                    powerInput = -.3f;
                }
                else if (powerInput == -.3f)
                {
                    powerInput = 0;
                }
            }
            else if (powerInput > 0)
            {
                if (powerInput == .5f)
                {
                    powerInput = .3f;
                }
                else if (powerInput == .3f)
                {
                    powerInput = 0;
                }
            }
        }
        */

        if (Input.GetKey(KeyCode.A) )
        {
            if(powerInput != 0)//if moving forward allow turn
            turnInput = -1;
            else//kill angular velocity if not moving
            {
                turnInput = 0;
                carRigidbody.angularVelocity = Vector3.zero;

            }
        }
        else if (Input.GetKey(KeyCode.D))//same as above bit
        {
            if (powerInput != 0)
                turnInput = 1;
            else
            {
                turnInput = 0;
                carRigidbody.angularVelocity = Vector3.zero;

            }
        }


    }

    public void OnUp()
    {


        if (Input.GetKeyUp(KeyCode.W))
        {
          //  powerInput = 0;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {

          //  powerInput = 0;
        }

        //Kill Anglar velocity of vehicle this is more for ground , planes/hover you wouldnt use this
        if (Input.GetKeyUp(KeyCode.A))
        {
            turnInput = 0;
            carRigidbody.angularVelocity = Vector3.zero;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {

            turnInput = 0;
            carRigidbody.angularVelocity = Vector3.zero;
        }
    }




}