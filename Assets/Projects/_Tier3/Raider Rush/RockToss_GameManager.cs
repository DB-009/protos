using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RockToss_GameManager : MonoBehaviour {

    public GameObject MatchManager;



    public GameObject titleScreen;
    public Slider hpSlider1,hpSlider2;
    public RockToss_Controller activePlayer;
    public int spawn;



    public enum GameState { Title, WorldMap,  City, BattleArena, Travelling, LegacyScreen, InMatch, Settings };
    public GameState gameState;



    public CameraTracking myCam;
    public bool fixedCam,invertedCam,camPanning;
    public float camXDisMax,camXDisMin;
    public float camPanSpeed,camPanMaxSpeed,panStartTime,panSpeedUpTime;
    public Vector3 camInitPos;
  

    public RockToss_button left, right, shootLeft, shootRight;

    public List<GameObject> gens = new List<GameObject>();

    public Transform spawnPos1, spawnPos2;
    // Use this for initialization
    void Awake () {

      invertedCam = true;
       camPanning = true;
       fixedCam = false;
        camInitPos = myCam.transform.position;
     
    }

    // Update is called once per frame
    void Update () {
        if (gameState == GameState.Title)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gameState = GameState.InMatch;


            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gameState = GameState.Travelling;
                MatchManager.GetComponent<RockToss_MatchManager>().challengeStage.SetActive(true);
                titleScreen.SetActive(false);
            }
        }


            if (camPanning == true)
                CameraPanInput();

            if (fixedCam == false)
                FixCamPos();
        

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
            myCam.transform.position = new Vector3(camXDisMin, myCam.transform.position.y, myCam.transform.position.z);
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

   

}
