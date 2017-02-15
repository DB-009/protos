using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StateManagerRacing : MonoBehaviour {


    public GameObject player;
    public Camera racingCam,endlessCam;


    public GameObject playerPrefab,endlessPlayer;//child object camera prefab later ??

    // Use this for initialization

    public List<GameObject> spawnPos = new List<GameObject>();

    public enum Mode { Platformer, Endless, Extra , None};
    public Mode gameMode;

    public enum GameState { Title, OnePlayer, InMatch, Settings };
    public GameState gameState;

    //map spawning
   
    public RaceMatchManager matchPhaser;
    public controlledUIManager uiManager;


    public StateManagerRacing control;

    //Send to Ui Manager
    public GameObject titleScreen, inGamePanel;

    public bool sleepingEnemies;


    public GameObject stage1, stage2, walls;

    void Awake()
    {

        GameObject target = GameObject.FindGameObjectWithTag("GameStateManager");
        if (target != null)
        {
            Destroy(target);
        }
        else
        {
            //leaderBoardStart.ConnectPlayer();
            Debug.Log("Ads are turned off here edit me");
        }

        if (control == null)
        {
            gameState = GameState.Title;
            titleScreen.SetActive(true);
            gameMode = Mode.None;
            this.gameObject.tag = "GameStateManager";
            DontDestroyOnLoad(gameObject);
            control = this;

        }
        else if (control != null)
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {

        if (gameState == GameState.Title)//0
        {

            //Debug.Log("Await input title screen");
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //startMatch();
          
                gameMode = Mode.Platformer;

                startStory();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //startMatch();
            
                gameMode = Mode.Endless;
                startStory();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                uiManager.goToOptionsMenu();
            }

        }
       else if (gameState == GameState.OnePlayer)//0
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
               
                startMatch();
            }
        }
        else if (gameState == GameState.InMatch)
        {
            //send to UiManager
            string playerStatus = "";
            if (player.GetComponent<RacerObj>().finishedRace == false)
                playerStatus = "Racing";
            else
                playerStatus = "Completed";

            inGamePanel.transform.GetChild(0).GetComponent<Text>().text = 
                " GameMode : " + gameMode + "\n"
                + "GameTime : " + Time.time + "\n"
                + "MatchTime : "  + matchPhaser.matchTime + "\n" 
                + "PlayerStatus : " + playerStatus
                ;

            //sleeping enemies
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
            //Quit function
            else if (Input.GetKeyDown(KeyCode.Escape))
            {



                matchPhaser.endMatch();
                resetGame();
            }
        }


    }




    void startMatch()
    {
   
        if(player == null)
        {
            if (gameMode == Mode.Endless)
            {
                CreatePlayer(playerPrefab);
            }
            else if (gameMode == Mode.Platformer)
            {
                CreatePlayer(endlessPlayer);
            }
        
        }

        if(gameMode == Mode.Endless)
        {
            player.GetComponent<_Endless2dController>().enablePlayer();
            player.GetComponent<_Endless2dController>().resetStats();
        }
        else if(gameMode == Mode.Platformer)
        {
            player.GetComponent<RacingController>().enablePlayer();
            player.GetComponent<RacingController>().resetStats();
        }


       
        matchPhaser.inMatch = true;
        matchPhaser.multiplayer = false;
        //uiManager.multiplayer = false;
        //

        matchPhaser.startMatch();


    }



    void startStory()
    {

        //send to UiManager
        titleScreen.SetActive(false);
        inGamePanel.transform.GetChild(0).GetComponent<Text>().text = " GameMode: " + gameMode;
        inGamePanel.SetActive(true);

        if (player == null)
        {
            if (gameMode == Mode.Endless)
            {
                CreatePlayer(endlessPlayer);
            }
            else if (gameMode == Mode.Platformer)
            {
                CreatePlayer(playerPrefab);
            }

        }

        if (gameMode == Mode.Endless)
        {
            walls.SetActive(true);
            stage2.SetActive(true);
            player.GetComponent<_Endless2dController>().enablePlayer();
            player.GetComponent<_Endless2dController>().resetStats();
            player.GetComponent<_Endless2dController>().rb.isKinematic = false;
            player.GetComponent<_Endless2dController>().canMove = true;
        }
        else if (gameMode == Mode.Platformer)
        {
            // stage2.SetActive(false);
            // walls.SetActive(true);

            walls.SetActive(true);
            stage2.SetActive(true);


            player.GetComponent<RacingController>().enablePlayer();
            player.GetComponent<RacingController>().resetStats();
            player.GetComponent<RacingController>().rb.isKinematic = false;
            player.GetComponent<RacingController>().canMove = true;
        }


        matchPhaser.multiplayer = false;
        //uiManager.multiplayer = false;
        //

        gameState = GameState.OnePlayer;


        matchPhaser.racers.Clear();

                if (gameMode == Mode.Endless)
        {
           
                     matchPhaser.racers.Add(player.GetComponent<_Endless2dController>().racerObj);
        }
        else if (gameMode == Mode.Platformer)
        {
   
                     matchPhaser.racers.Add(player.GetComponent<RacingController>().racerObj);
        }

       
        

    }




    public void resetGame()
    {


        if (gameMode == Mode.Endless)
        {

      
            player.GetComponent<_Endless2dController>().disablePlayer(true, player.GetComponent<_Endless2dController>().playerID);
        }
        else if (gameMode == Mode.Platformer)
        {

            player.GetComponent<RacingController>().disablePlayer(true, player.GetComponent<RacingController>().playerID);
        }



        gameState = GameState.Title;
        //uiManager.goToTitleScreen();


    }




    public void CreatePlayer(GameObject prefabPlayer)
    {



        GameObject playerCreated = GameObject.Instantiate(prefabPlayer, spawnPos[0].transform.position, Quaternion.identity) as GameObject;

        if (gameMode == Mode.Endless)
        {
            _Endless2dController disPlayer = playerCreated.GetComponent<_Endless2dController>();

            //disPlayer.myPlayer = disPlayer;
            //give player access to all our managers (within reason)

            disPlayer.gameStateManager = this;
            disPlayer.matchPhaser = matchPhaser;
            //disPlayer.uiManager = uiManager;



            disPlayer.initialSpawnPos = spawnPos[0];


            //set player ID
            disPlayer.playerID = 0; // set it before adding 1 to list (arrays starts at 0)

            player = playerCreated;
            disPlayer.resetStats();



            //  GameObject camCreated = GameObject.Instantiate(cameraPrefab, spawnPos[0].transform.position, Quaternion.identity) as GameObject;
            //  CameraTracking disCam = camCreated.GetComponent<CameraTracking>();

            endlessCam.gameObject.SetActive(true);
            racingCam.gameObject.SetActive(false);

            CameraTracking disCam =endlessCam.GetComponent<CameraTracking>();

            disCam.myTarget = disPlayer.transform;

            //disCam.camMode = CameraTracking.CamType.topdown;

            disPlayer.myCam = disCam;

        }
        else if (gameMode == Mode.Platformer)
        {
            RacingController disPlayer = playerCreated.GetComponent<RacingController>();

            //disPlayer.myPlayer = disPlayer;
            //give player access to all our managers (within reason)

            disPlayer.gameStateManager = this;
            disPlayer.matchPhaser = matchPhaser;
            //disPlayer.uiManager = uiManager;



            disPlayer.initialSpawnPos = spawnPos[0];


            //set player ID
            disPlayer.playerID = 0; // set it before adding 1 to list (arrays starts at 0)

            player = playerCreated;
            disPlayer.resetStats();


            //  GameObject camCreated = GameObject.Instantiate(cameraPrefab, spawnPos[0].transform.position, Quaternion.identity) as GameObject;
            //  CameraTracking disCam = camCreated.GetComponent<CameraTracking>();

            //endlessCam.gameObject.SetActive(false);
            racingCam.gameObject.SetActive(true);

            CameraTracking disCam = racingCam.GetComponent<CameraTracking>();

            disCam.myTarget = disPlayer.transform;

            //disCam.camMode = CameraTracking.CamType.topdown;

            disPlayer.myCam = disCam;
            disPlayer.yDis = disCam.plat_yPos;

        }





    }





}
