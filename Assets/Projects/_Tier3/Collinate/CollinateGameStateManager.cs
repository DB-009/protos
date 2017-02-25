using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollinateGameStateManager : MonoBehaviour
{

    //   

    public enum GameState { Off, TitleScreen, OnePlayer };
    public GameState gameState;
    public CollinateUIManager uiManager;
    public Vector3 initSpawnPos;
    public playerControls myPlayer;
    public CollinateEvents eventSys;

    public Planet planetFocus;


    //public Player[] players;
    public GameObject Stage;


    // Use this for initialization
    void Start()
    {
        goToTitleScreen();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameState + " Press 1 to switch state");

        if (gameState == GameState.TitleScreen)
        {

            TitleScreenInputs();

        }

        else if (gameState == GameState.OnePlayer)
        {

            OnePlayerInputs();

        }

    }

    public void goToTitleScreen()
    {
        gameState = GameState.TitleScreen;

        //  DestroyAllPlayers();

        Stage.SetActive(false);
        uiManager.UIFix();
    }

    public void goToOnePlayer()
    {
        gameState = GameState.OnePlayer;
        Stage.SetActive(true);
        uiManager.UIFix();


        eventSys.evento(0,0,Time.time);
        

        //setting camera target to player 1
        //Camera.main.GetComponent<CameraTracking>().myTarget = players[0].transform;


    }

    //Title Screen User Input fucntions
    public void TitleScreenInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            goToOnePlayer();
        }
    }

    public void OnePlayerInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            goToTitleScreen();

        }
    }

  

}
