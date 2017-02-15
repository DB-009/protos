using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class movement : MonoBehaviour {


    public FFGameState gameStateManager;

    public float playerSpd, fwdSpd, sidSpd, jumpHeight;
    public bool isGrounded;


    public Rigidbody rb;
    public int curLane;



    public enum Direction {  left, right, up , down};
    public Direction dirFacing;


    public GameObject bulletPrefab;
    public float bulforce;
    // Use this for initialization
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        transform.position = gameStateManager.lanes[curLane].transform.position;
    }

    // Update is called once per frame
    void Update()
    {


         GetMovementInputs();//movement
         GetInputs();//actions

    }

    void FixedUpdate()
    {


            ApplyMovementInput();
        

    }



    public void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {


        GameObject bul;
        
        if (dirFacing == Direction.left)
        {

            Vector3 spawnPos = new Vector3(this.transform.position.x - 2 + (rb.velocity.x * Time.deltaTime), this.transform.position.y, this.transform.position.z);

             bul = GameObject.Instantiate(bulletPrefab, spawnPos, Quaternion.identity) as GameObject;

            bul.GetComponent<Rigidbody>().AddForce(-bulforce, 0, 0, ForceMode.Impulse);





        }
        else if (dirFacing == Direction.right)
        {
            Vector3 spawnPos = new Vector3(this.transform.position.x + 2 - (rb.velocity.x * Time.deltaTime), this.transform.position.y, this.transform.position.z);

             bul = GameObject.Instantiate(bulletPrefab, spawnPos, Quaternion.identity) as GameObject;
            bul.GetComponent<Rigidbody>().AddForce(bulforce, 0, 0, ForceMode.Impulse);
        }



    }

    public void GetMovementInputs()
    {
        fwdSpd = 0;
        sidSpd = 0;

        //foward and back(z axis)

        if (Input.GetKeyDown(KeyCode.W))
        {
            //fwdSpd += playerSpd;
            if(curLane < gameStateManager.lanes.Count-1)
            {
                curLane++;
                transform.position = new Vector3(this.transform.position.x, gameStateManager.lanes[curLane].transform.position.y, this.transform.position.z);
            }
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            //fwdSpd -= playerSpd;
            if (curLane > 0)
            {
                curLane--;
                transform.position = new Vector3(this.transform.position.x, gameStateManager.lanes[curLane].transform.position.y, this.transform.position.z);
            }
        }


        //horizontal



        if (Input.GetKey(KeyCode.D))
        {
            sidSpd += playerSpd;
            dirFacing = Direction.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            sidSpd -= playerSpd;
            dirFacing = Direction.left;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(0, jumpHeight, 0, ForceMode.Impulse);

        }

    }

    public void ApplyMovementInput()
    {
        rb.AddForce(new Vector3(sidSpd, fwdSpd, 0), ForceMode.Force);
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
        else if (col.gameObject.tag == "obstacle")
        {
            Debug.Log("you Died");
            this.gameObject.SetActive(false);
                
        }

    }

    public void OnTriggerEnter(Collider col)
    {
             if (col.gameObject.tag == "SpeedUp")
        {
            //access the buffZone
            Debug.Log("Speed up");
            rb.AddForce(new Vector3(sidSpd*1.5f, 0, 0), ForceMode.Impulse);

        }
    }
    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = false;
        }
    }

    public void PlayerRespawn()
    {
        //Vector3 initPos = gameStateManager.initSpawnPos;
        //this.transform.position = new Vector3(initPos.x, initPos.y, this.transform.position.z);
    }
}
