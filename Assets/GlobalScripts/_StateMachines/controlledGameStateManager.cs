using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class controlledGameStateManager : MonoBehaviour
{



    public List<Player> players = new List<Player>();

    public GameObject playerPrefab, cameraPrefab;//child object camera prefab later ??
    public ActionKeys actionKeys;

    public List<GameObject> spawnPos = new List<GameObject>();

    public enum GameState { Title, OnePlayer, TwoPlayer,TileCard, InMatch, Settings, MapGen };
    public GameState gameState;

    public GameObject tileObj, stageObj, randomObj, borderObj;

    public controlledStageGenerator stageGenerator;
    public controlledMatchPhaser matchPhaser;
    public controlledUIManager uiManager;

    public bool multiplayer;

    public bool sleepingEnemies;

    public bool verticalSplitScreen;



    public GameObject troop, troop2;
    public List<Troop> createdTroops = new List<Troop>();
    public float troopCreateClickAt,createdelay;
    void Start()
    {


        gameState = GameState.Title;
        //myvar = 0;



        Debug.Log("At title Screen awaiting input (press1)");



    }

    void Update()
    {


        if (gameState == GameState.Title)//0
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // startMatch();
                startStory();
            }


            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                multiplayer = true;

                startMatchTwo();
            }



            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GenerateMap();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {

                mapLoadMode();
            }



            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                uiManager.goToOptionsMenu();
            }


        }
        else if (gameState == GameState.OnePlayer)
        {
            //
            if (Input.GetKeyDown(KeyCode.M))
            {
                stageGenerator.floors = 1;
                startMatch();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {

                stageGenerator.deleteMap();


                Debug.Log("quitting");
                resetGame();


            }
        }
        else if (gameState == GameState.TwoPlayer)
        {
            //
            if (Input.GetKeyDown(KeyCode.M))
            {
                stageGenerator.floors = 1;
                startMatchTwo();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {

                stageGenerator.deleteMap();


                Debug.Log("quitting");
                resetGame();


            }
        }
        else if (gameState == GameState.MapGen)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                players[0].myCamera.GetComponent<CameraTracking>().camMode = CameraTracking.CamType.topdown;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                players[0].myCamera.GetComponent<CameraTracking>().camMode = CameraTracking.CamType.thirdPerson;
            }


            if (Input.GetKeyDown(KeyCode.R))
            {
                stageGenerator.deleteMap();
                GenerateMap();
            }


            if (Input.GetKeyDown(KeyCode.Q))
            {

                stageGenerator.deleteMap();


                Debug.Log("quitting");
                resetGame();


            }

        }

        else if (gameState == GameState.TileCard)
        {
          

            if (Input.GetKeyDown(KeyCode.Q))
            {

                stageGenerator.deleteMap();


                Debug.Log("quitting");
                resetGame();


            }

        }

        else if (gameState == GameState.InMatch)
        {
            /*
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                stageGenerator.deleteMap();
                matchPhaser.GenerateBattleArena_small();
            }


            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                stageGenerator.GenerateObjects(matchPhaser.tempAmount, matchPhaser.coinObj, false, "bottom",0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                stageGenerator.GenerateObjects(matchPhaser.tempAmount, matchPhaser.coinObj, false, "top",0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                stageGenerator.GenerateObjects(matchPhaser.tempAmount, matchPhaser.coinObj, true, "top",0);
            }


            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                stageGenerator.updateFreeSpacesList();
            }
            */

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (sleepingEnemies == true)//wake enemies up
                {
                    GameObject[] enes = GameObject.FindGameObjectsWithTag("enemy");
                    foreach (GameObject disEnemy in enes)
                    {
                        disEnemy.GetComponent<Enemy>().isAwake = true;
                        sleepingEnemies = false;
                    }
                }
                else//put enemies to sleep
                {

                    GameObject[] enes = GameObject.FindGameObjectsWithTag("enemy");
                    foreach (GameObject disEnemy in enes)
                    {
                        disEnemy.GetComponent<Enemy>().isAwake = false;
                        sleepingEnemies = true;
                    }

                }
            }


            if (Input.GetKeyDown(KeyCode.Q))
            {



                matchPhaser.endMatch();
                resetGame();
            }
        }



    }

    void startStory()
    {

        // matchPhaser.matchState = controlledMatchPhaser.MatchState.Lobby;
        gameState = GameState.OnePlayer;
        multiplayer = false;
        Debug.Log("Entered Story");

        //gamestart
        uiManager.titleScreen.SetActive(false);

        //create player
        CreatePlayer();
        //createPacman();
       


        uiManager.player1_Menu.active = true;


       // matchPhaser.multiplayer = false;
        uiManager.multiplayer = false;

        stageGenerator.deleteMap();
        stageGenerator.floors = 2;
        //matchPhaser.GenerateBattleArena_small();

        stageGenerator.GenerateBorder(borderObj);
        stageGenerator.GenerateBGMap(stageGenerator.possibleTiles[1]);
  

        stageGenerator.updateFreeSpacesList();

        //Start Story 

        Debug.Log("It Works");


    }

    void mapLoadMode()
    {

        // matchPhaser.matchState = controlledMatchPhaser.MatchState.Lobby;
        gameState = GameState.TileCard;
        multiplayer = false;
        Debug.Log("Entered Story");

        //gamestart
        uiManager.titleScreen.SetActive(false);
        uiManager.tilecardUI.SetActive(true);

        //create player
        CreatePlayer();
        players[0].enablePlayer();


        uiManager.player1_Menu.active = true;


        //matchPhaser.multiplayer = false;
        uiManager.multiplayer = false;



        //matchPhaser.GenerateBattleArena_small();

        //List<int> preMadeMap, float columns, float rows , int levels , DrawMode mode
        List<int> preMadeMap = new List<int>();




        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);

        preMadeMap.Add(0);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(0);


        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);

        ///


        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);


        preMadeMap.Add(1);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);

        //


        preMadeMap.Add(0);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);


        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(0);
        //

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);


        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        //

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);


        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);


        //

        preMadeMap.Add(0);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);


        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(0);

        //



        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);


        preMadeMap.Add(1);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);


        //



        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);

        preMadeMap.Add(0);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);

        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(1);
        preMadeMap.Add(0);


        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);
        preMadeMap.Add(0);



        //
        stageGenerator.deleteMap();
        stageGenerator.LoadMap(preMadeMap, 16, 8, 1, controlledStageGenerator.DrawMode.Three_D);
        stageGenerator.updateFreeSpacesList();

        //Start Story 

        Debug.Log("It Works");


    }

    void startMatch()
    {
        if (players[0] == null)
            CreatePlayer();

        players[0].enablePlayer();
        players[0].resetStats();

        uiManager.player1_Menu.active = true;
        uiManager.player2_Menu.active = false;


        matchPhaser.multiplayer = false;
        uiManager.multiplayer = false;


        matchPhaser.startMatch();


    }

    void startMatchTwo()
    {
        //gamestart
        uiManager.titleScreen.SetActive(false);

        if (players.Count == 0)
        {
            CreatePlayer();//create player1\
            CreatePlayer();
        }

        else if (players.Count <= 1)//create playe2
            CreatePlayer();

        players[0].enablePlayer();
        players[0].resetStats();

        players[1].enablePlayer();
        players[1].resetStats();

        uiManager.player1_Menu.active = true;
        uiManager.player2_Menu.active = true;


       // matchPhaser.multiplayer = true;
        uiManager.multiplayer = true;



       // matchPhaser.startMatch();

    }



    public void resetGame()
    {
        int playerCount = 0;
        foreach (Player disPlayer in players)
        {
            disPlayer.disablePlayer(false, disPlayer.playerID);
            playerCount++;
        }

        for (int temp = playerCount; temp > 0; temp--)
        {
            Destroy(players[0].gameObject);

            Destroy(players[0].myCamera.gameObject);
            players.RemoveAt(0);
        }

        uiManager.tilecardUI.SetActive(false);
        uiManager.player1_Menu.active = false;
        uiManager.player2_Menu.active = false;
        uiManager.titleScreen.SetActive(true);
        uiManager.camMenu_MapGen.SetActive(false);

        gameState = GameState.Title;

    }





    public void GenerateMap()///MAP GEN DEMO MODE
    {


        if (players.Count == 0 || players.Count == null)
            CreatePlayer();


        gameState = GameState.MapGen;

        uiManager.goToMapGen();


        stageGenerator.deleteMap();


        stageGenerator.GenerateBGMap(tileObj);//creates the imaginary background layer..

        stageGenerator.GenerateBorder(borderObj);//1st top layer item
                                                 // stageGenerator.GenBattleZone(stageObj);

        // stageGenerator.GenerateMiddleArena(randomObj, true);

        stageGenerator.updateFreeSpacesList();


        //once map is created player can start moving
        players[0].enablePlayer();
        players[0].resetStats();
        players[0].rb.isKinematic = false;
        players[0].myCamera.GetComponent<CameraTracking>().staticX = false;


    }


    void loadMapArray()
    {

        // matchPhaser.matchState = controlledMatchPhaser.MatchState.Lobby;
        gameState = GameState.OnePlayer;
        multiplayer = false;
        Debug.Log("Entered Story");

        //gamestart
        uiManager.titleScreen.SetActive(false);

        //create player
        CreatePlayer();
        players[0].enablePlayer();


        //matchPhaser.multiplayer = false;
        uiManager.multiplayer = false;



        //matchPhaser.GenerateBattleArena_small();

        //List<int> preMadeMap, float columns, float rows , int levels , DrawMode mode
        List<int> preMadeMap = new List<int>();


        //completely random block in space
        /*
        for(int x= 0; x< 64;x++)
         {
            preMadeMap.Add(Random.Range(0, stageGenerator.possibleTiles.Count));
         }
         */

        //simple border and same floor no pattern

        for (int x = 0; x < 5; x++)
        {
            preMadeMap.Add(0);
            preMadeMap.Add(0);
            preMadeMap.Add(0);
            preMadeMap.Add(0);

            preMadeMap.Add(0);
            preMadeMap.Add(1);
            preMadeMap.Add(1);
            preMadeMap.Add(0);

            preMadeMap.Add(0);
            preMadeMap.Add(1);
            preMadeMap.Add(1);
            preMadeMap.Add(0);


            preMadeMap.Add(0);
            preMadeMap.Add(0);
            preMadeMap.Add(0);
            preMadeMap.Add(0);

        }


        stageGenerator.deleteMap();
        stageGenerator.LoadMap(preMadeMap, 4, 4, 4, controlledStageGenerator.DrawMode.Three_D);
        stageGenerator.updateFreeSpacesList();

        //Start Story 

        Debug.Log("It Works");


    }




















    public void CreatePlayer()
    {



        GameObject playerCreated = GameObject.Instantiate(playerPrefab, spawnPos[0].transform.position, Quaternion.identity) as GameObject;
        Player disPlayer = playerCreated.GetComponent<Player>();


        //give player access to all our managers (within reason)
        disPlayer.stageGen = stageGenerator;
        disPlayer.gameStateManager = this.GetComponent<controlledGameStateManager>();
        //disPlayer.matchPhaser = matchPhaser;
        Debug.Log(" player created commented out matchPhaser ");
        disPlayer.uiManager = uiManager;

        //give player access to Action keys
        disPlayer.actionKeys = actionKeys;
        disPlayer.GetComponent<playerMovementController>().actionKeys = actionKeys;
        disPlayer.GetComponent<playerMovementController>().myPlayer = disPlayer;


        disPlayer.initialSpawnPos = spawnPos[0];


        //set player ID
        disPlayer.playerID = players.Count; // set it before adding 1 to list (arrays starts at 0)

        players.Add(disPlayer);
        disPlayer.resetStats();


        GameObject camCreated = GameObject.Instantiate(cameraPrefab, spawnPos[0].transform.position, Quaternion.identity) as GameObject;
        CameraTracking disCam = camCreated.GetComponent<CameraTracking>();
        disCam.myTarget = disPlayer.transform;

        disCam.camMode = CameraTracking.CamType.topdown;

        disPlayer.myCamera = camCreated.GetComponent<Camera>();

        if (disPlayer.playerID == 0)
        {
            if (multiplayer == true)
            {
                if (verticalSplitScreen == false)
                    disPlayer.myCamera.rect = new Rect(0, 0, 0.5f, 1);
                else
                    disPlayer.myCamera.rect = new Rect(0, .5f, 1, .5f);

            }

        }
        else if (disPlayer.playerID == 1)
        {
            if (verticalSplitScreen == false)
                disPlayer.myCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
            else
                disPlayer.myCamera.rect = new Rect(0, 0, 1, .5f);
        }






    }





    public void CreatePacman()
    {



        GameObject playerCreated = GameObject.Instantiate(playerPrefab, spawnPos[0].transform.position, Quaternion.identity) as GameObject;
        pacmanGame disPlayer = playerCreated.GetComponent<pacmanGame>();


        //give player access to all our managers (within reason)
        disPlayer.gameStateManager = this.GetComponent<controlledGameStateManager>();
        //disPlayer.matchPhaser = matchPhaser;
        Debug.Log(" player created commented out matchPhaser ");
        //disPlayer.uiManager = uiManager;

        //give player access to Action keys
       




        GameObject camCreated = GameObject.Instantiate(cameraPrefab, spawnPos[0].transform.position, Quaternion.identity) as GameObject;
        CameraTracking disCam = camCreated.GetComponent<CameraTracking>();
        disCam.myTarget = disPlayer.transform;

        disCam.camMode = CameraTracking.CamType.topdown;

       






    }









    public void CreateTroop(GameObject owner, GameObject troop, GameObject troop2, mapTile disTile)
    {
       
            Debug.Log("SUMOOONNNNN!");
            int rand = Random.Range(0, 4);
            GameObject spawnObj = troop;

            if (rand > 2)
            {
                spawnObj = troop2;
            }


            Vector3 newPos = new Vector3(disTile.initialTilePos.x, disTile.initialTilePos.y,  disTile.initialTilePos.z );

            Vector3 tacticalPos = Vector3.zero;
            bool posSelected = false;
            foreach (mapTile disTilee in stageGenerator.fgMapTiles)
            {
                if (disTilee.initialTilePos == newPos)
                {
                    Debug.Log("found tile " + newPos);
                    foreach (tacticalTile disTTile in disTilee.tacticalTiles)
                    {
                        Debug.Log("found a tile here should go tactical tile placement");

                        if (disTTile.occupied == false && posSelected == false)
                        {
                            tacticalPos = disTTile.transform.position;
                            posSelected = true;
                            disTTile.occupied = true;

                            float objScaleY = spawnObj.GetComponent<Renderer>().bounds.size.y - stageGenerator.tileScale;//get the difference from tower scale to fixedTile Scale (For organized Drawing)
                            objScaleY *= .5f;//multiplying by .5f because object in unity get drawn from the center so half of one

                            GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(tacticalPos.x, tacticalPos.y + objScaleY + 3, tacticalPos.z), Quaternion.identity) as GameObject;

                            createdTroops.Add(tileCreated.GetComponent<Troop>());

                            tileCreated.GetComponent<mapTile>().initialTilePos = tacticalPos;
                            tileCreated.GetComponent<Troop>().owner = owner;
                        }
                    }
                }

            
        }
    }
}