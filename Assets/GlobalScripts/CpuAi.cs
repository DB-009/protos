using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CpuAi : MonoBehaviour
{
    //thanks to type-21 in reddit-game dev learned about queue

    public GameObject target;
    public float minDistance;
    public int moveSpd, health, maxHealth;
    public bool targetting;
    public Rigidbody rb;

    public bool isController, canMove;

    public objGen eneGen;
    //stats
    public float  jumpVar, closeDistance, landingCooldown;
    //jump fix
    public float lastJumpAt, jumpCooldown;

    //Slingshot player 

    //player variables for movement
    public bool isGrounded, dblJumped, cannonBackward;
    public float jumpHeight;

    //wall jump variables
    public float wallTimeStart, wallTimeLimit, lastWallExitTIme, wallAllowedStoreTime;

    //cannon variables
    public float timeBetweenCanon, LastCannonAt, noGravityTimeLimit;

    //raycast variables
    public float upCheck, horCheck, downCheck;

    public List<GameObject> lastCannon = new List<GameObject>();

    public bool onWall, jumpThrough,canDblJump;
    public GameObject lastWall;

    //Rayvast variable
    public bool isLeft, isRight , isMoving , infront,behind;

    public float xDis, yDis;
    // Use this for initialization

   public enum enemyType { flyer, fighter, shooter};
    public enemyType eneType;


    public GameObject bulletPrefab;
    public float bulForce, fireRate, lastShotAt, bulletAcceptedDistance,chargeForce,minAttackDist,lastAttack,attackTime,attackStart,yAtkShift,xRetreatShift;

    public bool attacking,canAttack,retreating;
    public int chargeSpd;
    public Material normalMat, chargeMat;


    public enum gunType
    {
        normal,
        rapidFire,
        spreadShot,
        triShot,
        hadoken,
    }

    public gunType curGun;

    public float spreadShotAngle;
    public bool gunDoubleUpgrade,canShoot;

    public bool allowZMovement;

    public bool lShot, rShot, playerCol;



    public enum directionMoving { not, left, right, up, down };
    public directionMoving dir;


    public bool trackAtks;
    public int agression;
    public Ene_AtkPatternTracker atkTracker;

    public float nearDis, farDis;

    public void Awake()
    {

        rb = this.GetComponent<Rigidbody>();


        if (allowZMovement == false)
        {

      

            rb.constraints = RigidbodyConstraints.FreezeRotation |  RigidbodyConstraints.FreezePositionZ;

        }

        if (canShoot == true)
        {
            fireRate +=  Random.Range(1,4)    ;
        }

        if(canAttack == true)
        {
            minAttackDist += Random.Range(1, 2);
            attackTime += Random.Range(1, 4);
        }

        upCheck = gameObject.transform.localScale.y / 2 + .2f;
        downCheck = upCheck;



    }

    // Update is called once per frame
    void Update()
    {

       HorColCheck();
        VerColCheck();
        if(allowZMovement)
        ZetaColCheck();
    }

    public void FixedUpdate()
    {
        if (target != null)
        {
            CheckDistance();
        }

    }


    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = true;

        }
        else if (col.gameObject.tag == "bouncePad")
        {
            Debug.Log("UPDATE ME BOUNCE PAD! add if direction facing..jump left or right");
            rb.AddForce(jumpHeight * xDis, jumpHeight * yDis, 0, ForceMode.Impulse);
            dblJumped = false;
        }
        else if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            Debug.Log("Eh enterd;");

            if (lastWall != col.gameObject)
            {
                rb.isKinematic = true;
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
        else if (col.collider.tag == "Player")
        {
            playerCol = true;
        }
        else if (col.collider.tag == "Combo")
        {
            float disChk =  GetDistance();
            if(disChk <= nearDis)
            {
                atkTracker.nearAtklanded(playerAttack.attackType.melee);
            }

        }
    }


    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
             isGrounded = false;
        }

        else if (col.collider.tag == "wall" && isGrounded == false)//if the object you collided withs tag is ground your player is on the floor
        {

            rb.isKinematic = false;

            onWall = false;

            dblJumped = false;


        }
        else if (col.collider.tag == "Player")
        {
            playerCol = false;
        }
    }



    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "SpeedUp")
        {
            //access the buffZone
            Debug.Log("Speed up");
            rb.AddForce(new Vector3(moveSpd * 1.5f, 0, 0), ForceMode.Impulse);

        }
        else if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone

            col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false, this.gameObject);

        }
        else if (col.tag == "edge")
        {
            Debug.Log("IN EDGE Enter");


            EdgeData disEdge = col.GetComponent<EdgeData>();

            int tempVar = 0;
            foreach (GameObject disObj in disEdge.objectsOfInterest)
            {



                Vector3 offset = disObj.transform.position - transform.position;
                float tempDistance = offset.sqrMagnitude;//updating distances list

                //Debug.Log("checking " + tempDistance);

                if (tempDistance < jumpVar + (jumpVar * .5f) && targetting == true)
                {
                    if (isGrounded == true && Time.time >= landingCooldown)
                    {
                        if (target.GetComponent<SlingShotPlayer>().trigger != col.gameObject)
                        {
                            float edgeOffsetX = disObj.transform.position.x - col.transform.position.x;
                            float playerOffsetX = target.transform.position.x - col.transform.position.x;
                            Debug.Log("hmm " + edgeOffsetX + ":" + playerOffsetX);

                            if ((edgeOffsetX < 0 && playerOffsetX < edgeOffsetX) || (edgeOffsetX > 0 && playerOffsetX > edgeOffsetX))
                            {
                                Debug.Log("JUMP1");
                            Jump();

                            }
                            else if (disEdge.canIgnore[tempVar] == false)
                            {
                                Debug.Log("JUMP2");

                                Jump();
                            }
                        }
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


    public void OnTriggerStay(Collider col)
    {

        if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone

            if (col.GetComponent<buffZone>().type != buffZone.Type.speedChange)
            {

                col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false , this.gameObject);

            }

        }
        else if (col.tag == "edge")
        {
            Debug.Log("IN EDGE Stay");


            EdgeData disEdge = col.GetComponent<EdgeData>();

            int tempVar = 0;
            foreach (GameObject disObj in disEdge.objectsOfInterest)
            {



                Vector3 offset = disObj.transform.position - transform.position;
                float tempDistance = offset.sqrMagnitude;//updating distances list

                Debug.Log("checking distance of nearby object ");

                //old if included this --> tempDistance < jumpVar + (jumpVar * .5f) &&
                if ( targetting == true)
                {
                    if (isGrounded == true && Time.time >= landingCooldown)
                    {
                        if(target.GetComponent<SlingShotPlayer>().trigger != col.gameObject)
                        {
                            float edgeOffsetX = disObj.transform.position.x - col.transform.position.x;
                            float playerOffsetX = target.transform.position.x - col.transform.position.x;
                            Debug.Log("hmm " + edgeOffsetX + ":" + playerOffsetX);

                            if ((edgeOffsetX < 0 && playerOffsetX < edgeOffsetX) || (edgeOffsetX > 0 && playerOffsetX > edgeOffsetX))
                            {


                                Jump();
                            }
                            else if (disEdge.canIgnore[tempVar] == false)
                            {
                                Debug.Log("JUMP2");

                                Jump();
                            }
                        }
                      
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


    public float GetDistance()
    {
        float offset = target.transform.position.x - transform.position.x;
        float sqrLen = Mathf.Abs(offset);


        return sqrLen;


    }


    //Check if players close
    public void CheckDistance()
    {
        float xOffset = target.transform.position.x - transform.position.x;
        float yOffset = target.transform.position.y - transform.position.y;
        float zOffset = target.transform.position.z - transform.position.z;

        float sqrLenX = Mathf.Abs(xOffset);
        float sqrLenY = Mathf.Abs(yOffset);
        float sqrLenZ = Mathf.Abs(zOffset);


        if (sqrLenX < minDistance || sqrLenY < minDistance || sqrLenZ < minDistance)
        {



            float x = target.transform.position.x - transform.position.x;
            float y = target.transform.position.y - transform.position.y;
            float z = target.transform.position.z - transform.position.z;

            //print("The target  transform is close to me! ");
            targetting = true;

            if(allowZMovement == true)
            {
                if (x > z && canMove == true)
                {
                    HorBasedtrack(x, y, z);
                }
                else if (z > x && canMove == true)
                {
                    ZetaBasedtrack(x, y, z);

                }
            }
            else
                HorBasedtrack(x, y, z);





            if (Time.time >= attackStart + attackTime && attackStart !=0 && attacking == true)
            {
                attacking = false;
                this.GetComponent<Renderer>().material = normalMat;
                lastAttack = Time.time;
                yAtkShift = 0;
                if (eneType == enemyType.flyer && canShoot == false)
                {
                    retreating = true;
                }
            }
            else if(Time.time >= lastAttack + attackTime && attackStart != 0 && retreating == true)
            {
                retreating = false;
            }

            float yDis = target.transform.position.y;
            //Ycheck for jumping
            if (yDis > transform.position.y+1 && yOffset < minDistance )
            {
                if(isGrounded == true)
                Jump();
            }

            //transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, 0, 0), .7f * Time.deltaTime);
           if(canShoot == true)
            {
                    Shoot(bulletPrefab, bulForce);
                
            } 



        }
        else
        {
            targetting = false;
            isMoving = false;
            //idle behaviour
        }
    }







    public void HorBasedtrack(float horDis, float verDis, float zetaDis)
    {

        //Debug.Log("thes nots");

        if (horDis < 0)
        {

            if (isLeft == false)
            {

                if (attacking == true)
                {
                    yAtkShift = verDis;
                }
                else
                    yAtkShift = 0;

                if (retreating == false)
                {
                    if(allowZMovement == true)
                    {
                        if (zetaDis < 1)
                        {
                            rb.AddForce(new Vector3(-moveSpd, yAtkShift, -moveSpd), ForceMode.Acceleration);
                        }
                        else if (zetaDis > 1)
                        {
                            rb.AddForce(new Vector3(-moveSpd, yAtkShift, moveSpd), ForceMode.Acceleration);
                        }
                    }
                    else
                        rb.AddForce(new Vector3(-moveSpd, yAtkShift, 0), ForceMode.Acceleration);


                }
                else
                    rb.AddForce(new Vector3(moveSpd, yAtkShift, 0), ForceMode.Acceleration);

                isMoving = true;


            }
            else
            {
                isMoving = false;
         

            }


            if (horDis > -minAttackDist)
            {
                if (attacking == false && canAttack == true)
                    Attack();
            }


            


            // else if (isGrounded == true)
            //rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        }
        else if (horDis > 0)
        {

            if (isRight == false)
            {
                if (attacking == true)
                {
                    yAtkShift = verDis;
                }
                else
                    yAtkShift = 0;

                if (retreating == false)
                {
                    if (zetaDis < 1)
                    {
                        rb.AddForce(new Vector3(moveSpd, yAtkShift, -moveSpd), ForceMode.Acceleration);
                    }
                    else if (zetaDis > 1)
                    {
                        rb.AddForce(new Vector3(moveSpd, yAtkShift, moveSpd), ForceMode.Acceleration);
                    }
                }
                else
                    rb.AddForce(new Vector3(-moveSpd, yAtkShift, 0), ForceMode.Acceleration);

                isMoving = true;

            }
            else
            {
                isMoving = false;

            }




            if (horDis < minAttackDist)
            {
                if (attacking == false && canAttack == true)
                    Attack();

            }

            // else if (isGrounded == true)
            // rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);


        }
    }




    public void ZetaBasedtrack(float horDis, float verDis, float zetaDis)
    {
        //Debug.Log("thos nots");

        if (zetaDis < 0)
        {

            if (behind == false)
            {

                if (attacking == true)
                {
                    yAtkShift = verDis;
                }
                else
                    yAtkShift = 0;

                if (retreating == false)
                {
                    if (horDis < 1)
                    {
                        rb.AddForce(new Vector3(-moveSpd, yAtkShift, -moveSpd), ForceMode.Acceleration);
                    }
                    else if (horDis > 1)
                    {
                        rb.AddForce(new Vector3(moveSpd, yAtkShift, -moveSpd), ForceMode.Acceleration);
                    }
                }
                else
                    rb.AddForce(new Vector3(moveSpd, yAtkShift, 0), ForceMode.Acceleration);

                isMoving = true;


            }
            else
            {
                isMoving = false;

            }

            if (zetaDis > -minAttackDist)
            {
                if (attacking == false && canAttack == true)
                    Attack();
            }


            //lShot = true;


            // else if (isGrounded == true)
            //rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        }
        else if (zetaDis > 0)
        {

            if (infront == false)
            {
                if (attacking == true)
                {
                    yAtkShift = verDis;
                }
                else
                    yAtkShift = 0;

                if (retreating == false)
                {
                    if (horDis < 1 && isLeft == false)
                    {
                        rb.AddForce(new Vector3(-moveSpd, yAtkShift, moveSpd), ForceMode.Acceleration);
                    }
                    else if (horDis > 1 && isRight == false)
                    {
                        rb.AddForce(new Vector3(moveSpd, yAtkShift, moveSpd), ForceMode.Acceleration);
                    }
                    else
                    {
                        rb.AddForce(new Vector3(0, yAtkShift, moveSpd), ForceMode.Acceleration);

                    }
                }
                else
                    rb.AddForce(new Vector3(-moveSpd, yAtkShift, 0), ForceMode.Acceleration);

                isMoving = true;

            }
            else
            {
                isMoving = false;

            }


            //rShot = true;

            if (zetaDis < minAttackDist)
            {
                if (attacking == false && canAttack == true)
                    Attack();

            }

            // else if (isGrounded == true)
            // rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);


        }
    }






    // Hor and ver ==  Raycast functions
    private void HorColCheck()
    {
        RaycastHit hitL, hitR;
        isLeft = Physics.Raycast(this.transform.position, -this.transform.right, out hitL, horCheck);
        isRight = Physics.Raycast(this.transform.position, this.transform.right, out hitR, horCheck);


        if (isRight == true)
        {
            if (
                hitR.transform.gameObject.tag == "edge" || 
                hitR.transform.gameObject.tag == "StageController" ||
                hitR.transform.gameObject.tag == "projectile" ||
                 hitR.transform.gameObject.tag == "enemy"
                )
                isRight = false;
            else if(hitR.transform.gameObject.tag == "Player")
            {
                Debug.Log("YEHAW");
            }
            //else
                //Debug.Log("There is something next to me! " + hitR.transform.name);


        }
        else if (isLeft == true)
        {
           
            if (
                hitL.transform.gameObject.tag == "edge" ||
                hitL.transform.gameObject.tag == "StageController" ||
                 hitL.transform.gameObject.tag == "projectile"  
                )
                isLeft = false;
            else if (hitL.transform.gameObject.tag == "enemy")
            {
                Debug.Log("YEHAW2222");
                isLeft = false;
                isMoving = false;
                canMove = false;
            }
            //else
            //Debug.Log("There is something next to me! " +hitL.transform.name);
        }
        else
        {
            canMove = true;
        }

    }





    private void VerColCheck()
    {
        RaycastHit hitUp, hitDown;
        bool isUp = Physics.Raycast(this.transform.position, this.transform.up, out hitUp, upCheck);
        bool isDown = Physics.Raycast(this.transform.position, -this.transform.up, out hitDown, upCheck);
        Debug.DrawRay(this.transform.position, -this.transform.up, Color.green);
        Debug.DrawRay(this.transform.position, this.transform.up, Color.green);

        if (isDown == true)
        {
            // Debug.Log("There is something underneath me!");
            //Check what you are stood on e.g hit.collider.gameObject.layer   etc..

            if (hitDown.transform.gameObject.tag == "ground" || hitDown.transform.gameObject.tag == "passablePlat")
            {
                this.gameObject.GetComponent<BoxCollider>().isTrigger = false;

                isGrounded = true;
                dblJumped = false;
            }
            else if(hitDown.transform.gameObject.tag == "enemy")
            {
                 //Debug.Log("There is something underneath me!");

               
                    this.transform.position = this.transform.position - new Vector3(2,0,0);
          


            }

        }
        else if (isUp == true)
        {
            if (hitUp.transform.gameObject.tag == "passablePlat")
            {
                Debug.Log("There is something Above  me!");
                jumpThrough = true;
                this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            }

        }
        else {
            jumpThrough = false;
            isGrounded = false;
    

        }

    }







    private void ZetaColCheck()
    {
          RaycastHit hitL, hitR;
        behind = Physics.Raycast(this.transform.position, -this.transform.forward, out hitR, horCheck);
        infront = Physics.Raycast(this.transform.position, this.transform.forward, out hitL, horCheck);

        if (behind == true )
        {
            if (
                hitR.transform.gameObject.tag == "edge" ||
                hitR.transform.gameObject.tag == "StageController" ||
                hitR.transform.gameObject.tag == "projectile")
                behind = false;
            else if (playerCol == true)
                behind = false;
            //else
                //Debug.Log("There is something next to me! " + hitR.transform.name);


        }
        else if (infront == true)
        {
           
            if (
                hitL.transform.gameObject.tag == "edge" ||
                hitL.transform.gameObject.tag == "StageController" ||
                 hitL.transform.gameObject.tag == "projectile"
                )
                infront = false;
            else if (playerCol == true)
                infront = false;
            //else
            //Debug.Log("There is something next to me! " +hitL.transform.name);
        }

    }













    public void Shoot(GameObject bulz, float force)
    {

        if(Time.time >= lastShotAt+fireRate)
        {



            float x = target.transform.position.x - transform.position.x;
            float z = target.transform.position.z - transform.position.z;

            float xdire = x;
            float zdire = z;
            Vector3 bulPosition = new Vector3(this.transform.position.x , this.gameObject.transform.position.y, this.transform.position.z);

            

            if (curGun == gunType.normal)
            {
                GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;

                tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated.GetComponent<projectileLife>().playerBullet = false;

                //tileCreated.GetComponent<Rigidbody>().velocity = (new Vector3(xdire, 0, zdire) * bulForce);
                tileCreated.transform.LookAt(target.transform);
                tileCreated.GetComponent<Rigidbody>().velocity = tileCreated.transform.forward * bulForce;
                //tileCreated.GetComponent<Rigidbody>().AddForce(transform.forward*bulForce,ForceMode.Impulse);


            }
            else if (curGun == gunType.spreadShot)
            {
                GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;
                GameObject tileCreated2 = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;
                GameObject tileCreated3 = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;

                tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated.GetComponent<projectileLife>().playerBullet = false;

                tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated2.GetComponent<projectileLife>().playerBullet = false;

                tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated3.GetComponent<projectileLife>().playerBullet = false;


                tileCreated.GetComponent<Rigidbody>().velocity = new Vector3(xdire+ spreadShotAngle, 0, zdire) * bulForce;

                tileCreated2.GetComponent<Rigidbody>().velocity = new Vector3(xdire, 0, zdire) * bulForce;


                tileCreated3.GetComponent<Rigidbody>().velocity = new Vector3(xdire- spreadShotAngle, -0, zdire) * bulForce;

            }
            else if (curGun == gunType.triShot)
            {
                GameObject tileCreated = GameObject.Instantiate(bulz, bulPosition + new Vector3( -.5f,0, 0), Quaternion.identity) as GameObject;
                GameObject tileCreated2 = GameObject.Instantiate(bulz, bulPosition, Quaternion.identity) as GameObject;
                GameObject tileCreated3 = GameObject.Instantiate(bulz, bulPosition - new Vector3( -.5f,0, 0), Quaternion.identity) as GameObject;

                tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated.GetComponent<projectileLife>().playerBullet = false;

                tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated2.GetComponent<projectileLife>().playerBullet = false;

                tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated3.GetComponent<projectileLife>().playerBullet = false;


                tileCreated.GetComponent<Rigidbody>().velocity = new Vector3(xdire, 0, zdire) * bulForce;

                tileCreated2.GetComponent<Rigidbody>().velocity = new Vector3(xdire, 0, zdire) * bulForce;


                tileCreated3.GetComponent<Rigidbody>().velocity = new Vector3(xdire, 0, zdire) * bulForce;

                if (gunDoubleUpgrade == true)
                {
                    GameObject tileCreated4 = GameObject.Instantiate(bulz, bulPosition + new Vector3(1, 0, 0), Quaternion.identity) as GameObject;

                    tileCreated4.GetComponent<projectileLife>().owner = this.gameObject;
                    tileCreated4.GetComponent<projectileLife>().playerBullet = false;

                    GameObject tileCreated5 = GameObject.Instantiate(bulz, bulPosition - new Vector3(1, 0, 0), Quaternion.identity) as GameObject;

                    tileCreated5.GetComponent<projectileLife>().owner = this.gameObject;
                    tileCreated5.GetComponent<projectileLife>().playerBullet = false;


                    tileCreated4.GetComponent<Rigidbody>().velocity = new Vector3(xdire, 0, zdire) * bulForce;


                    tileCreated5.GetComponent<Rigidbody>().velocity = new Vector3(xdire, 0, zdire) * bulForce;
                }
            }


            lastShotAt = Time.time;
        }

    }






    void Jump()
    {
        //JUMP
        if(Time.time >=  lastJumpAt + jumpCooldown)
        {
            rb.AddForce(0, jumpVar, 0, ForceMode.Impulse);
            lastJumpAt = Time.time;

        }


    }


    void Attack()
    {
        if(lastAttack ==0 || Time.time >= lastAttack+attackTime)
        {
                 retreating = false;
                attacking = true;
                moveSpd += chargeSpd;
                this.GetComponent<Renderer>().material = chargeMat;
                attackStart = Time.time;
            
        }

    }


}
