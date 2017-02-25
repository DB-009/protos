using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {

    public LaneShift_TopDown player;
    public bool isGrounded,  cannonBackward;



    //wall jump variables
    // Use this for initialization
    void Awake () {
        player = this.gameObject.GetComponent<LaneShift_TopDown>();
	}



    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {


            isGrounded = true;

           player.isJumping = false;
            player.isDoubleJumping = false;
           player.doubleJump = false;


        


        }


        if (col.gameObject.tag == "obstacle")
        {

            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "bouncePad")
        {
            player.rb.AddForce(player.jumpHeight * player.xDis, player.jumpHeight * player.yDis, 0, ForceMode.Impulse);
            player.dblJumped = false;
        }
        else if (col.gameObject.tag == "buff")
        {
            col.gameObject.GetComponent<buff>().BuffEffect(this.gameObject);
            Destroy(col.gameObject);
            Debug.Log("buff grabbed by player remove from list bug");

        }
        else if (col.gameObject.tag == "enemy")
        {

            isGrounded = true;

            player.isJumping = false;
            player.isDoubleJumping = false;
            player.doubleJump = false;

            CpuAi disCpu = col.gameObject.GetComponent<CpuAi>();
            if (disCpu.attacking == true)
            {
                Debug.Log("you got hit by chargin ene;");
                player.hp -= 25;
                player.hpSlider.value = player.hp;

                disCpu.attacking = false;
                disCpu.lastAttack = Time.time;
                disCpu.GetComponent<Renderer>().material = disCpu.normalMat;

                if (disCpu.eneType == CpuAi.enemyType.flyer)
                {
                    disCpu.retreating = true;
                }
                else
                {
                    player.rb.AddForce(new Vector3(0, disCpu.chargeForce, 0), ForceMode.Impulse);
                }
            }
            else
            {
                Debug.Log("you got hit by enemy;");
                player.hp -= 10;
                player.hpSlider.value = player.hp;

            }

        }

    }

    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = false;

            player.isDoubleJumping = false;
        }
    }





    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "QuadrantController")
        {
            if (col.GetComponent<QuadrantController>().cntrlType == QuadrantController.controlType.quadControl)
            {
                Debug.Log("press space movement haulted");
                player.canMove = false;
            }
            else if (col.GetComponent<QuadrantController>().cntrlType == QuadrantController.controlType.goal)
                this.gameObject.SetActive(false);

        }



    }


    public void OnTriggerStay(Collider col)
    {

        if (col.tag == "StageController")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //if(kills == eneGen1.amount + eneGen2.amount)
                // {
                //    Debug.Log("space ship takeoff");

                // }
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {

    }

}
