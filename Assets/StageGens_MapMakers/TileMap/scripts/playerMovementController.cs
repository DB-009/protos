using UnityEngine;
using System.Collections;

public class playerMovementController : MonoBehaviour {


    public ActionKeys actionKeys;

    public float sidSpd, fwdSpd;
    public Rigidbody rb;
    public bool isController, isMoving, isGrounded, isDashing, isImpulse, isSliding;

    public Player myPlayer;


    //Timers
    public float walkTimer, impulseTimer, dashTimer, lastDash, lastImpulse;

    //walking 
    public float walkTime, startWalkTime, allowedWalkingTime;

    public int impulses, dashes;

    public Vector3 targetReposition;




    // Use this for initialization
    void Start()
    {


        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {



        fwdSpd = 0;
        sidSpd = 0;

        if (isController == true)
        {


                actionKeys.inputDown(this.GetComponent<playerMovementController>(), myPlayer, rb);

                actionKeys.inputHold(this.GetComponent<playerMovementController>(), myPlayer, rb);

                actionKeys.inputUp(myPlayer, rb);


            // Extra Space for smoothing/slowdown

            //motion check

            if ((fwdSpd != 0 || sidSpd != 0))//in motion
            {
                if (startWalkTime == 0)//if no startWalk time then this is new motion
                {
                    startWalkTime = Time.time;
                    // Debug.Log("new motion");
                }
                else//must be an old motion
                {
                    //Debug.Log("old motion ");
                    walkTime = Time.time - startWalkTime;
                }

                walkTimer = 0;
            }
            else if (fwdSpd == 0 && sidSpd == 0)//not in motion
            {

                if (walkTimer == 0 && startWalkTime != 0)//if motion was started and you just stopped moving
                {
                    walkTimer = Time.time + allowedWalkingTime;



                    //Debug.Log("motion timer started");
                }
                else if (Time.time >= walkTimer && walkTimer != 0)//if its been a second since you stopped moving
                {
                    startWalkTime = 0;
                    walkTime = 0;
                    walkTimer = 0;

                    rb.velocity = Vector3.zero;//stopping all motion
                    rb.angularVelocity = Vector3.zero;//stopping all motion
                                                      // Debug.Log("motion timer ended");
                }


                //Debug.Log("not in motion");
            }


            if (Time.time >= lastDash + 1)
            {
                isDashing = false;

            }
            if (Time.time >= lastImpulse + 1)
            {
                isImpulse = false;

            }
        }




    }


    void FixedUpdate()
    {



        if (isController == true)
        {

            if (isDashing == true)
            {
                //this.transform.position = targetReposition;
                // rb.MovePosition(targetReposition);

                Vector3 direction = (targetReposition - transform.position).normalized;
                rb.MovePosition(transform.position + direction * (myPlayer.playerSpd * myPlayer.dashDistance) * Time.fixedDeltaTime);


                float temp = (targetReposition - transform.position).magnitude;
                if (temp <= .6f)
                {
                    isDashing = false;

                }

            }
            if (isImpulse == true)
            {
                //this.transform.position = targetReposition;
                // rb.MovePosition(targetReposition);

                Vector3 direction = (targetReposition - transform.position).normalized;
                rb.MovePosition(transform.position + direction * (myPlayer.playerSpd * 2) * Time.fixedDeltaTime);

                Debug.Log("in impulse");

                float temp = (targetReposition - transform.position).magnitude;
                if (temp <= .6f)
                {
                    isImpulse = false;

                }
            }
            else
            {
                rb.AddForce(new Vector3(sidSpd, 0, fwdSpd), ForceMode.Force);//Add a force to my players RigidBody.
            }

            // this.gameObject.transform.Translate(sidSpd,0, fwdSpd);
            //The force must be a vector 3 for this script so it adds force in 3 Dimensions.
        }
    }


















    /// 
    ///   MOVEMENT FUNCTION FOR SKILLS
    ///
    ///



    public void playerDash(Player.PlayerDirection dir)
    {
        if (Time.time <= dashTimer && dashTimer != 0)//also add number of dashes can connect
        {
            if (dir == Player.PlayerDirection.N)
            {
                // rb.AddForce(new Vector3(0, 0, myPlayer.playerSpd + 4), ForceMode.VelocityChange);
                targetReposition = this.transform.position + (new Vector3(0, 0, myPlayer.dashDistance));

                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;
            }
            else if (dir == Player.PlayerDirection.S)
            {
                //rb.AddForce(new Vector3(0, 0, -myPlayer.playerSpd - 4), ForceMode.Impulse);
                targetReposition = this.transform.position - (new Vector3(0, 0, myPlayer.dashDistance));

                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;

            }
            else if (dir == Player.PlayerDirection.W)
            {
                // rb.AddForce(new Vector3(-myPlayer.playerSpd - 4, 0,0), ForceMode.Acceleration);
                targetReposition = this.transform.position - (new Vector3(myPlayer.dashDistance, 0, 0));
                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;


            }
            else if (dir == Player.PlayerDirection.E)
            {
                // rb.AddForce(new Vector3(myPlayer.playerSpd + 4, 0, 0), ForceMode.Force);
                targetReposition = this.transform.position + (new Vector3(myPlayer.dashDistance, 0, 0));

                isDashing = true;
                lastDash = Time.time;
                dashTimer = 0;
            }

            dashes++;
        }
    }

    public void playerJuke(Player.PlayerDirection dir)
    {
        if (Time.time <= impulseTimer && impulseTimer != 0)//also add number of dashes can connect
        {
            if (dir == Player.PlayerDirection.N)
            {
                // rb.AddForce(new Vector3(0, 0, myPlayer.playerSpd + 4), ForceMode.VelocityChange);
                targetReposition = this.transform.position + (new Vector3(0, 0, myPlayer.dashDistance));

                isImpulse = true;
                lastImpulse = Time.time;
                // impulseTimer = 0;
            }
            else if (dir == Player.PlayerDirection.S)
            {
                //rb.AddForce(new Vector3(0, 0, -myPlayer.playerSpd - 4), ForceMode.Impulse);
                targetReposition = this.transform.position - (new Vector3(0, 0, myPlayer.dashDistance));

                isImpulse = true;
                lastImpulse = Time.time;
                //impulseTimer = 0;

            }
            else if (dir == Player.PlayerDirection.W)
            {
                // rb.AddForce(new Vector3(-myPlayer.playerSpd - 4, 0,0), ForceMode.Acceleration);
                targetReposition = this.transform.position - (new Vector3(myPlayer.dashDistance, 0, 0));
                isImpulse = true;
                lastImpulse = Time.time;
                // impulseTimer = 0;


            }
            else if (dir == Player.PlayerDirection.E)
            {
                // rb.AddForce(new Vector3(myPlayer.playerSpd + 4, 0, 0), ForceMode.Force);
                targetReposition = this.transform.position + (new Vector3(myPlayer.dashDistance, 0, 0));

                isImpulse = true;
                lastImpulse = Time.time;
                //impulseTimer = 0;
            }

            impulses++;

        }
    }

    public void playerDirChange(Player.PlayerDirection dir)
    {
        if (startWalkTime != 0)
        {
            // Debug.Log("dir change");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (dir == Player.PlayerDirection.N)
            {
                rb.AddForce(new Vector3(0, 0, myPlayer.playerSpd / 2), ForceMode.Impulse);
            }
            else if (dir == Player.PlayerDirection.S)
            {
                rb.AddForce(new Vector3(0, 0, -myPlayer.playerSpd / 2), ForceMode.Impulse);
            }
            else if (dir == Player.PlayerDirection.W)
            {
                rb.AddForce(new Vector3(-myPlayer.playerSpd / 2, 0, 0), ForceMode.Impulse);
            }
            else if (dir == Player.PlayerDirection.E)
            {
                rb.AddForce(new Vector3(myPlayer.playerSpd / 2, 0, 0), ForceMode.Impulse);
            }
        }
    }
}
