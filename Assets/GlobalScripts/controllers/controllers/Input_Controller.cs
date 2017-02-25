using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Controller : MonoBehaviour {

    public LaneShift_TopDown player;

    public float minSwipeDistY;
    public float minSwipeDistX;


    // Use this for initialization
    void Start () {
        player = this.gameObject.GetComponent<LaneShift_TopDown>();
        player.gameStateManager.left.myHero = player;
        player.gameStateManager.right.myHero = player;

        player.gameStateManager.shootLeft.myHero = player;
        player.gameStateManager.shootRight.myHero = player;
    }
	
	// Update is called once per frame
	void Update () {


        if (player.isController == true)
        {
            if (player.canMove != false)
            {


                //inputs
                OnDown();
                OnHold();
                OnUp();

                if (Input.GetKey(KeyCode.G))
                {
                    player.ImFiringMyLaserr(false);
                }
                else if (Input.GetKeyUp(KeyCode.G))
                {
                    player.ImFiringMyLaserr(true);
                }


                if (Input.GetKey(KeyCode.F))
                {
                    player.ImFiringMyLaserr2(false);
                }
                else if (Input.GetKeyUp(KeyCode.F))
                {
                    player.ImFiringMyLaserr2(true);
                }



                if (Input.GetKeyDown(KeyCode.Mouse0))//combo / button mash inputs should be  on GetKeyDown
                {
                    if (player.wepID == 1)
                    {
                        //Debug.Log("in combo");
                        player.Combo();
                    }
                    else
                    {
                        Debug.Log("SHOT DETECT FOR burst beams");
                        player.gunControl.ShotDetect();
                    }

                }

                _touchControls();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.StageProgression();

                }

            }

        }
    }


    public void OnMouseDown()
    {


    }





    public void UIActions(int action)
    {

        if (action == 0)
        {

            if (Time.time >= player.lastShotAt + player.fireRate)
                player.gunControl.CmdShoot(-1);

        }
        else if (action == 1)
        {

            if (Time.time >= player.lastShotAt + player.fireRate)
                player.gunControl.CmdShoot(1);

        }
        else if (action == 2)
        {
            if (player.curMoveType == LaneShift_TopDown.MoveType.survival && player.colHandler.isGrounded == true)
            {
                if (player.sidSpd == player.mvspd *-1)
                {
                    player.sidSpd = 0;
                }
                else
                    player.sidSpd = player.mvspd *-1;
            }
        }
        else if (action == 3)
        {
            if (player.curMoveType == LaneShift_TopDown.MoveType.survival && player.colHandler.isGrounded == true)
            {
                if (player.sidSpd == player.mvspd)
                {

                    player.sidSpd = 0;
                }
                else
                    player.sidSpd = player.mvspd;
            }
        }
    }



    public void OnDown()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            //if (player.curMoveType == LaneShift_TopDown.MoveType.survival && player.jetPack == false)
            // player.Jump();
            player.playerDash(2);

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.playerDash(3);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            player.playerDash(0);


        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            player.playerDash(1);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (player.curMoveType == LaneShift_TopDown.MoveType.survival && player.jetPack == false)
                player.Jump();

        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            player.keybased_combo(0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            player.keybased_combo(1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            player.keybased_combo(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            player.keybased_combo(3);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (player.wepID == 0)
            {
                player.wepID = 1;
            }
            else if (player.wepID == 1)
            {
                player.wepID = 0;
            }
        }

    }



    public void OnHold()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (player.curMoveType == LaneShift_TopDown.MoveType.survival && player.jetPack == true)
            {
                player.JetPack();
            }
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
            if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
            {
                if(player.airWalk == true || player.colHandler.isGrounded == true)
                player.sidSpd = player.mvspd *-1;
            }
            player.dashTimer = Time.time + 1;

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
            {
                if (player.airWalk == true || player.colHandler.isGrounded == true)
                    player.sidSpd = player.mvspd;
                
            }
            player.dashTimer = Time.time + 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";

            if (player.allowZmovement == true)
            {

                if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
                    if (player.airWalk == true || player.colHandler.isGrounded == true)
                        player.fwdSpd = player.mvspd;

            }
            else
            {
                if (player.curMoveType == LaneShift_TopDown.MoveType.survival && player.jetPack == true)
                {
                    player.JetPack();
                }
                else
                    player.dashTimer = Time.time + 1;

            }

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (player.allowZmovement == true)
            {

                if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
                    if (player.airWalk == true || player.colHandler.isGrounded == true)
                        player.fwdSpd = player.mvspd*-1;
            }
            else
                player.dashTimer = Time.time + 1;



        }
    }






    public void OnUp()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";
            if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
                player.sidSpd = 0;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
                player.sidSpd = 0;

        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //  sidSpd = -mvspd;
            //dir = directionMoving.left;
            // dirText.text = "left";

            if (player.allowZmovement == true)
            {

                if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
                    player.fwdSpd = 0;

            }

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            // dir = directionMoving.right;
            // dirText.text = "right";
            //  sidSpd = mvspd;
            if (player.allowZmovement == true)
            {

                if (player.curMoveType == LaneShift_TopDown.MoveType.survival)
                    player.fwdSpd = 0;
            }


        }
    }







    public void laneShift(int dir)
    {

        if (dir == 0 && player.curLanePos != 0)
        {
            this.gameObject.transform.position -= new Vector3(1, 0, 0);
            player.curLanePos -= 1;
        }
        else if (dir == 1 && player.curLanePos != player.curLaneLimit)
        {
            this.gameObject.transform.position += new Vector3(1, 0, 0);
            player.curLanePos += 1;

        }
    }




    void _touchControls()
    {

        //#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];

            switch (touch.phase)

            {

                case TouchPhase.Began:

                    break;

                case TouchPhase.Ended:

                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, player.startPos.y, 0)).magnitude;
                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(player.startPos.x, 0, 0)).magnitude;

                    if (swipeDistVertical > swipeDistHorizontal)

                    {

                        float swipeValue = Mathf.Sign(touch.position.y - player.startPos.y);

                        if (swipeValue < 0)
                        {
                            Debug.Log("DOwn Swype");
                        }
                        else
                        {
                            Debug.Log("Up Swype");
                            if (player.matchStarted == false)
                                player.StageProgression();

                        }




                    }
                    else if (swipeDistHorizontal > swipeDistVertical)

                    {

                        float swipeValue = Mathf.Sign(touch.position.x - player.startPos.x);

                        if (swipeValue < 0)
                        {
                            Debug.Log("left Swype");

                            if (player.curMoveType != LaneShift_TopDown.MoveType.tapHero)
                            {
                                if (player.canMove)
                                {
                                    if (player.curMoveType == LaneShift_TopDown.MoveType.run)
                                        laneShift(0);

                                }

                            }
                        }
                        else
                        {
                            Debug.Log("right Swype");
                            if (player.curMoveType != LaneShift_TopDown.MoveType.tapHero)
                            {
                                if (player.canMove)
                                {
                                    if (player.curMoveType == LaneShift_TopDown.MoveType.run)
                                        laneShift(1);

                                }

                            }

                        }

                        Debug.Log("Touch flank");
                    }

                    break;
            }
        }
    }
}
