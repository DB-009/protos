using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceMatchManager : MonoBehaviour {

    public StateManagerRacing gameStateManager;

    public controlledUIManager uiManager;


    public enum MatchState { Off,Lobby, Building , Prestart , Ongoing, Ended, EndScreen };
    public MatchState matchState;

    public bool inMatch,multiplayer;

    public List<RacerObj> racers = new List<RacerObj>();

    public float preStartTimer = 5.0f;
    public float matchStartTime,matchTime;

    public bool playerSpawned;

    public GameObject raceStartPos,raceEndPos;

    public int racersFinished,lapsInTrack;

    public buffEffects buffs;

    public float stunnedAt, stunTime;

    public int upForce;
    // Use this for initialization
    void Awake()
    {
        buffs = this.GetComponent<buffEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (matchState == MatchState.Lobby)
        {
            gameStateManager.gameState = StateManagerRacing.GameState.InMatch;
           // uiManager.titleScreen.SetActive(false);
        //this gets set by menu
                matchState = MatchState.Building;
                 
            Debug.Log("Match created");

            //stage.SetActive(false);


        }
        else if (matchState == MatchState.Building)
        {
            Debug.Log("Matc Created match starting - setting players");



            //player falling animation on start
      foreach(RacerObj disRacer in racers)
            {
                if(disRacer.type == RacerObj.RacerType.Player)
                {

                    if (gameStateManager.gameMode == StateManagerRacing.Mode.Endless)
                    {

                        _Endless2dController disPlayer = disRacer.GetComponent<_Endless2dController>();
                        disPlayer.GetComponent<Rigidbody>().isKinematic = false;
                        disPlayer.transform.position = raceStartPos.transform.position;

                        disPlayer.canMove = false;//player can no longer move
                        disPlayer.rb.velocity = Vector3.zero;//stop object movement
                        disPlayer.sidSpd = 0;
                        disPlayer.inMatch = true;
                        disPlayer.racerObj.timeStarted = Time.time;
                    }
                    else if (gameStateManager.gameMode == StateManagerRacing.Mode.Platformer)
                    {
                        RacingController disPlayer = disRacer.GetComponent<RacingController>();
                        disPlayer.GetComponent<Rigidbody>().isKinematic = false;
                        disPlayer.transform.position = raceStartPos.transform.position;
                        disPlayer.canMove = false;//player can no longer move
                        disPlayer.rb.velocity = Vector3.zero;//stop object movement
                        disPlayer.sidSpd = 0;
                        disPlayer.inMatch = true;
                        disPlayer.racerObj.timeStarted = Time.time;
                    }





                }
            }




            matchState = MatchState.Prestart;


        }
        else if (matchState == MatchState.Prestart)
        {
            if (preStartTimer > 0)
            {
                preStartTimer -= Time.deltaTime;

              
                 //Debug.Log(preStartTimer);

            }
            else
            {
                matchState = MatchState.Ongoing;
                matchStartTime = Time.time;
                preStartTimer = 5f;

                foreach (RacerObj disRacer in racers)
                {
                    if (disRacer.type == RacerObj.RacerType.Player)
                    {


                        if (gameStateManager.gameMode == StateManagerRacing.Mode.Endless)
                        {
                            _Endless2dController disPlayer = disRacer.GetComponent<_Endless2dController>();
                            disPlayer.canMove = true;
                        }
                        else if (gameStateManager.gameMode == StateManagerRacing.Mode.Platformer)
                        {
                            RacingController disPlayer = disRacer.GetComponent<RacingController>();
                            disPlayer.canMove = true;
                        }



                    }
                }

            }

        }
        else if (matchState == MatchState.Ongoing)
        {

            matchTime = (float)System.Math.Round( Time.time - matchStartTime,0);

            if (matchTime < 60)
            {
                //uiManager.matchTime.text = "00:"+ Mathf.Floor(matchTime % 60).ToString("00");

            }
            else
            {
                
                string minutes = Mathf.Floor(matchTime / 60).ToString("00");
                string seconds = Mathf.Floor(matchTime % 60).ToString("00");


               // uiManager.matchTime.text = minutes.ToString()+":"+ seconds.ToString();

            }


        }
     }

    public void startMatch()
    {
        gameStateManager.gameState = StateManagerRacing.GameState.InMatch;
        matchState = MatchState.Lobby;

        
  
    }

    public void endMatch()
    {
       

      



        gameStateManager.gameState = StateManagerRacing.GameState.Title;


        matchState = RaceMatchManager.MatchState.Off;


        racers.Clear();

       


        //uiManager.titleScreen.SetActive(true);
    }





    public void RacerHitGoal(RacerObj disObj)
    {
        if (disObj.type == RacerObj.RacerType.Player)
        {




            if (gameStateManager.gameMode == StateManagerRacing.Mode.Endless)
            {


                _Endless2dController disPlayer = disObj.GetComponent<_Endless2dController>();
                if (lapsInTrack == 0)
                {
                    //check laps to start return mode / change depth trigger etc
                    disObj.finishedRace = true;//complted race (later change for laps check)
                    disObj.timeFinished = Time.time;//set the time Racer finished based off  this/server match Manager

                    //Kill movement
                    disPlayer.canMove = false;//player can no longer move
                    disPlayer.rb.velocity = Vector3.zero;//stop object movement
                    disPlayer.sidSpd = 0;
                    racersFinished++;
                }
                else
                {

                }

            }
            else if (gameStateManager.gameMode == StateManagerRacing.Mode.Platformer)
            {


                RacingController disPlayer = disObj.GetComponent<RacingController>();
                if (lapsInTrack == 0)
                {
                    //check laps to start return mode / change depth trigger etc
                    disObj.finishedRace = true;//complted race (later change for laps check)
                    disObj.timeFinished = Time.time;//set the time Racer finished based off  this/server match Manager

                    //Kill movement
                    disPlayer.canMove = false;//player can no longer move
                    disPlayer.rb.velocity = Vector3.zero;//stop object movement
                    disPlayer.sidSpd = 0;
                    racersFinished++;
                }
                else
                {

                }


            }


        }
        else if (disObj.type == RacerObj.RacerType.CPU)
        {

            cpuRacer disPlayer = disObj.GetComponent<cpuRacer>();
            disPlayer.canMove = false;//player can no longer move
            disPlayer.rb.velocity = Vector3.zero;//stop object movement
            disPlayer.sidSpd = 0;
            disPlayer.finishedRace = true;
        }
            checkState();
    }

    public void checkState()
    {
        if(racersFinished == racers.Count)
        {
            matchState = MatchState.Ended;
            Debug.Log("Match is over calculate winner here and show on EndScreen");
        }
    }
}
