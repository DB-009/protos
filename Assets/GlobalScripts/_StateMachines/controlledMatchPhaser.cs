using UnityEngine;
using System.Collections;

public class controlledMatchPhaser : MonoBehaviour {

    public controlledGameStateManager gameStateManager;
    public controlledStageGenerator stageGenerator;
    public controlledUIManager uiManager;


    public enum MatchState { Off,Lobby, Building , Prestart , Ongoing, Ended, EndScreen };
    public MatchState matchState;

    public enum MatchType { Survival, TowerDef, extra};
    public MatchType matchType;

    public bool inMatch,multiplayer;


    //map spawning
    public GameObject tileObj, stageObj, randomObj,borderObj;

    public bool genMidground;

    public float preStartTimer = 5.0f;
    public float matchStartTime,matchTime;

    public bool playerSpawned, coinsSpawned, buffsSpawned, enemiesSpawned;


    public GameObject stage;

    //coin spawning
    public int tempAmount;//delete change to algorithm for coins, item/buff & enemy spawn calculation

    public GameObject coinObj;//change to array of possible object & object pooling

    //Object arrays for multiple spawn possibilities buffs,weps,items,enemus

    public GameObject[] wepSpawns,buffSpawns,eneSpawns;


    public Transform spawnPos;

    public int maxSpawns, curSpawns;
 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (matchState == MatchState.Lobby)
        {
            gameStateManager.gameState = controlledGameStateManager.GameState.InMatch;
            uiManager.titleScreen.SetActive(false);
        //this gets set by menu
                matchState = MatchState.Building;
                Debug.Log("Match created");

            //stage.SetActive(false);


        }
        else if (matchState == MatchState.Building)
        {
            GenerateBattleArena_small();//this will be changed from setting selected in lobby
            Debug.Log("Map Created match starting");

            //player falling animation on start
            gameStateManager.players[0].GetComponent<Rigidbody>().isKinematic = false;
            matchState = MatchState.Prestart;


        }
        else if (matchState == MatchState.Prestart)
        {
            if (preStartTimer > 0)
            {
                preStartTimer -= Time.deltaTime;


                if (matchType != MatchType.TowerDef)
                {

                    if (preStartTimer <= 4 && coinsSpawned == false)
                    {
                        Debug.Log("coins generated");
                        float calc = (stageGenerator.vTilesCount) * .30f;
                        int genAmount = (int)calc;
                        stageGenerator.GenerateObjects(genAmount, coinObj, true, "top", 0);
                        coinsSpawned = true;
                    }


                    if (preStartTimer <= 3 && buffsSpawned == false)
                    {
                        Debug.Log("buffs generated");
                        float calc = (stageGenerator.vTilesCount) * .30f;
                        int genAmount = (int)calc;
                        foreach (GameObject disObj in buffSpawns)
                        {
                            stageGenerator.GenerateObjects(genAmount / buffSpawns.Length, disObj, true, "top", 1);

                        }
                        buffsSpawned = true;
                    }

                    if (preStartTimer <= 2 && enemiesSpawned == false)
                    {
                        Debug.Log("enemies generated");
                        float calc = (stageGenerator.vTilesCount) * .30f;
                        int genAmount = (int)calc;


                        //WEAPON SPAWN REMOVE
                        foreach (GameObject disObj in wepSpawns)
                        {
                            stageGenerator.GenerateObjects(genAmount / wepSpawns.Length, disObj, true, "top", 2);

                        }

                        //
                        foreach (GameObject disObj in eneSpawns)
                        {
                            stageGenerator.GenerateObjects(genAmount / eneSpawns.Length, disObj, true, "top", 3);

                        }

                        enemiesSpawned = true;
                    }
                    // Debug.Log(preStartTimer);
                }
            }
            else
            {
                matchState = MatchState.Ongoing;
                matchStartTime = Time.time;
                preStartTimer = 5f;
            }

        }
        else if (matchState == MatchState.Ongoing)
        {

            matchTime = (float)System.Math.Round( Time.time - matchStartTime,0);

            if (matchTime < 60)
                uiManager.matchTime.text = "00:"+ Mathf.Floor(matchTime % 60).ToString("00");
            else
            {
                
                string minutes = Mathf.Floor(matchTime / 60).ToString("00");
                string seconds = Mathf.Floor(matchTime % 60).ToString("00");


                uiManager.matchTime.text = minutes.ToString()+":"+ seconds.ToString();

            }
            //while the match is ongoing maintain coins, buffs and enemy spawns
            if(matchType != MatchType.TowerDef)
            {
                coinRespawn();
                eneRespawn();
                buffRespawn();
            }



        }
     }

    public void startMatch()
    {
        gameStateManager.gameState = controlledGameStateManager.GameState.InMatch;
        matchState = MatchState.Lobby;

        
  
    }

    public void endMatch()
    {
       

      

        stageGenerator.deleteMap();

        gameStateManager.players[0].disablePlayer(true,0);

        uiManager.player1_Menu.active = false;

        if (multiplayer == true)
        {
            gameStateManager.players[0].disablePlayer(true,0);

            uiManager.player2_Menu.active = false;

        }
        gameStateManager.gameState = controlledGameStateManager.GameState.Title;

        enemiesSpawned = false;
        coinsSpawned = false;
        buffsSpawned = false;

        matchState = controlledMatchPhaser.MatchState.Off;

        
    }

    //Generate SMALL DEATH MACTH///

    public void GenerateBattleArena_small()
    {
    

        stageGenerator.deleteMap();
        

        stageGenerator.GenerateBGMap(tileObj);
        //stageGenerator.GenerateBGMap2(tileObj);
        stageGenerator.GenerateBorder(borderObj);
       // stageGenerator.GenBattleZone(stageObj);
        if (genMidground == true)
            stageGenerator.GenerateMiddleArena(randomObj,true);

        stageGenerator.updateFreeSpacesList();


    }



    void coinRespawn()
    {
        if (stageGenerator.coinsCount < 10)
        {
            Debug.Log("coins generated");
            float calc = (stageGenerator.vTilesCount) * .30f;
            int genAmount = (int)calc;
            stageGenerator.GenerateObjects(genAmount, coinObj, true, "top",0);
        }
    }

    void eneRespawn()
    {
        if (stageGenerator.enesCount < 4)
        {
            Debug.Log("enes generated");
            float calc = (stageGenerator.vTilesCount) * .20f;
            int genAmount = (int)calc;
            foreach (GameObject disObj in eneSpawns)
            {
                stageGenerator.GenerateObjects(genAmount / eneSpawns.Length, disObj, true, "top", 3);

            }
        }
    }

    void buffRespawn()
    {
        if (stageGenerator.buffsCount < 5)
        {
            Debug.Log("buffs generated");
            float calc = (stageGenerator.vTilesCount) * .30f;
            int genAmount = (int)calc;
            foreach (GameObject disObj in buffSpawns)
            {
                stageGenerator.GenerateObjects(genAmount / buffSpawns.Length, disObj, true, "top", 1);

            }

        }
    }





}
