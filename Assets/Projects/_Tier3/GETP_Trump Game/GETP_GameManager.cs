﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GETP_GameManager : MonoBehaviour {



    public GETP_Controller player1;

    public GameObject titleScreen;
    public Slider hpSlider1;
    public int spawn;



    public enum GameState { Title, WorldMap,  City, BattleArena, Travelling, LegacyScreen, InMatch, Settings };
    public GameState gameState;



    public CameraTracking myCam;
    public bool fixedCam,invertedCam,camPanning;
    public float camXDisMax,camXDisMin;
    public float camPanSpeed,camPanMaxSpeed,panStartTime,panSpeedUpTime;
    public Vector3 camInitPos;
  

    public GETP_button left, right, shootLeft, shootRight;

    public List<GameObject> gens = new List<GameObject>();

    public Transform spawnPos1, spawnPos2;
    // Use this for initialization
    void Awake () {
        Debug.Log("enteretd match change this i gues");
        gameState = GameState.InMatch;
        player1.initPos = player1.transform.position;

    }

    // Update is called once per frame
    void Update () {
        if (gameState == GameState.Title)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gameState = GameState.InMatch;


            }
        }
        else if(gameState == GameState.InMatch)
        {
            if(player1.isALive == false && Time.time >= player1.diedAT + player1.respawnTime)
            {
                Respawn();
            }
        }

    }


    public void Respawn()
    {
        Debug.Log("you died;");

        player1.gameObject.SetActive(true);

        //respawn
        player1.transform.position = player1.initPos;
        player1.hp = player1.mhp;
        player1.isALive = true;
        player1.canMove = true;



    }

    public void CameraPanInput()
    {
        float tempPanSpeed = camPanSpeed;
        if(Input.GetKey(KeyCode.LeftArrow))
        {

            //Camera HotSpot Logic (so player doesnt move to much to left)

            if(myCam.myTarget.transform.position.x > camInitPos.x)
            {
                if (panStartTime == 0)
                    panStartTime = Time.time;
                else if (Time.time >= panStartTime + panSpeedUpTime)
                {
                    tempPanSpeed = camPanMaxSpeed;
                }

                if (myCam.transform.position.x >= camInitPos.x)
                {
                    myCam.transform.position = new Vector3(myCam.transform.position.x - tempPanSpeed * Time.fixedDeltaTime, myCam.transform.position.y, myCam.transform.position.z);

                }
            }


        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (panStartTime == 0)
                panStartTime = Time.time;
            else if (Time.time >= panStartTime + panSpeedUpTime)
            {
                tempPanSpeed = camPanMaxSpeed;
            }

            if (myCam.transform.position.x < camXDisMax)
            {
                myCam.transform.position = new Vector3(myCam.transform.position.x + tempPanSpeed * Time.fixedDeltaTime, myCam.transform.position.y, myCam.transform.position.z);

            }

        }

        if(panStartTime !=0)
        {
            myCam.canTrack = false;
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                panStartTime = 0;
                myCam.canTrack = true;
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                panStartTime = 0;
                myCam.canTrack = true;

            }
        }
       

    }

    public void FixCamPos()
    {

        if (myCam.transform.position.x < camInitPos.x)
        {
            myCam.transform.position = camInitPos;
        }
        else if (myCam.transform.position.x > camXDisMax)
        {
            myCam.transform.position = new Vector3(camXDisMax, myCam.transform.position.y, myCam.transform.position.z);
        }

        //player cam hotSpot
        if (panStartTime == 0 && camPanning == true)
        {
            if (myCam.myTarget.transform.position.x <= camInitPos.x)
            {
                myCam.canTrack = false;
                myCam.transform.position = camInitPos;

            }
            else if (myCam.myTarget.transform.position.x >= camXDisMax)
            {
                myCam.canTrack = false;
                myCam.transform.position = new Vector3(camXDisMax, myCam.transform.position.y, myCam.transform.position.z);

            }
            else
            {
                myCam.canTrack = true;

            }

        }


    }

    public void CreatePlayer(GameObject player)
    {
        GETP_Controller disPlayer = player.GetComponent<GETP_Controller>();
        Debug.Log("you either need to instantiate player or change gfx and set stats delete player1 and player 2 vars ");
        disPlayer.rb = player.GetComponent<Rigidbody>();
        disPlayer.myTrans = disPlayer.transform;
        disPlayer.hp = disPlayer.vit * disPlayer.lvl + disPlayer.baseHP;
        disPlayer.mp = disPlayer.intel * disPlayer.lvl + disPlayer.baseMp;
        disPlayer.mhp = disPlayer.hp;
        disPlayer.mmp = disPlayer.mp;


        disPlayer.initPos = player.transform.position;
        disPlayer.gameStateManager = this;
        disPlayer.hpSlider = disPlayer.gameStateManager.hpSlider1;


        if (disPlayer.cam == null)
            disPlayer.cam = Camera.main.GetComponent<CameraTracking>();

        disPlayer.gameStateManager.left.myHero = disPlayer;
        disPlayer.gameStateManager.right.myHero = disPlayer;

        disPlayer.gameStateManager.shootLeft.myHero = disPlayer;
        disPlayer.gameStateManager.shootRight.myHero = disPlayer;

        foreach (GameObject disGen in disPlayer.gameStateManager.gens)
        {
            disGen.GetComponent<objGen>().targetPlayer = disPlayer.gameObject;
        }




        disPlayer.hpSlider.maxValue = disPlayer.mhp;
        disPlayer.hpSlider.value = disPlayer.hp;
    }


}
