using UnityEngine;
using System.Collections;

public class movingPlats : MonoBehaviour {
    public bool canMove,inverted;
    public float yDir,xDir,moveRate,waitTime;

    public Vector3 initPos;
    public int animTic;
    public float animStartTime;

	// Use this for initialization
	void Awake ()
    {
        initPos = this.transform.position;
        
    }


    //Anim Start Floats
    //0 default position moving down
    //1 moving up
    //2 wait period move up after
    //3 wait period move down after


    // Update is called once per frame
    void FixedUpdate()
    {

        if (canMove == true)
        {
            if (animTic == 0)
            {
                this.transform.Translate(0, moveRate * Time.fixedDeltaTime, 0);


                Vector3 targPos = Vector3.zero;
                if (inverted == false)
                {
                    targPos = initPos + new Vector3(0, yDir, 0);
                }
                else
                {
                    targPos = initPos;
                }

                if (transform.position.y >= targPos.y)
                {
                    animTic = 2;
                    transform.position = targPos;
                    animStartTime = Time.time;
                }
            }

            else if (animTic == 1)
            {
                this.transform.Translate(0, -(moveRate * Time.fixedDeltaTime), 0);

                Vector3 targPos = Vector3.zero;
                if (inverted == true)
                {
                    targPos = initPos + new Vector3(0, -yDir, 0);
                }
                else
                {
                    targPos = initPos;
                }

                if (transform.position.y <= targPos.y)
                {
                    animTic = 3;
                    transform.position = targPos;
                    animStartTime = Time.time;
                }

            }


            //check if waitTime is up to begin movement
            if (animTic == 2 && Time.time >= animStartTime + waitTime)
            {
                animTic = 1;
            }
            else if (animTic == 3 && Time.time >= animStartTime + waitTime)
            {
                animTic = 0;
            }
        }
    }
}
