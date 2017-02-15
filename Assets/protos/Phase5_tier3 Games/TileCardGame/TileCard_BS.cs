using UnityEngine;
using System.Collections;

public class TileCard_BS : MonoBehaviour {
    public controlledGameStateManager gameStateManager;
    public controlledStageGenerator stageGenerator;
    public controlledUIManager uiManager;


    public enum MatchState { Off, Lobby, Building, Prestart, Ongoing, Wait, Ended, EndScreen };
    public MatchState matchState;

    public float preStartTimer = 5.0f;
    public float matchStartTime, matchTime;

    public float fightStartTime, fightEndTime, totalTurns, nextTurnAt, turnWaitTime, endgamePhase;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        //LOBBY
        if (matchState == MatchState.Lobby)
        {
          

            Debug.Log("Match created");


            //stage.SetActive(false);


        }
        //BUILDING MATCH
        else if (matchState == MatchState.Building)
        {
        
            Debug.Log("Map Created match starting");


            //player falling animation on start

            matchState = MatchState.Prestart;



        }
        //PRESTART
        else if (matchState == MatchState.Prestart)
        {

            if (preStartTimer > 0)
            {
                preStartTimer -= Time.deltaTime;

                if (preStartTimer <= 4)
                {
                    Debug.Log("4");
                  
                }


                if (preStartTimer <=3)
                {
                    Debug.Log("3");
                }

                if (preStartTimer<=2)
                {
                    Debug.Log("2");
                }
                // Debug.Log(preStartTimer);

            }
            else
            {
                Debug.Log("Match Started presStart timer ended");
            
                //temporary set curBattler


                matchState = MatchState.Ongoing;
                matchStartTime = Time.time;
                preStartTimer = 5f;
            }

        }
        //ONGOING
        else if (matchState == MatchState.Ongoing)
        {

            matchTime = (float)System.Math.Round(Time.time - matchStartTime, 0);

            if (matchTime < 60)
                uiManager.matchTime.text = "00:" + Mathf.Floor(matchTime % 60).ToString("00");
            else
            {

                string minutes = Mathf.Floor(matchTime / 60).ToString("00");
                string seconds = Mathf.Floor(matchTime % 60).ToString("00");


                uiManager.matchTime.text = minutes.ToString() + ":" + seconds.ToString();

            }


            //Terrain system

            //

        }
        //Wait period
        else if (matchState == MatchState.Wait)//wait
        {
            if (Time.time >= nextTurnAt)
            {
                matchState = MatchState.Ongoing;
            }

        }



    }

    public void startMatch()
    {


      


    }
}
