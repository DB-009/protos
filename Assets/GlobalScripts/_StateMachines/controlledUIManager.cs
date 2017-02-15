using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class controlledUIManager : MonoBehaviour {


    public Text matchTime;


    public GameObject titleScreen, towerUI,tilecardUI, optionsScreen, camMenu_MapGen;
    public Tower curTowerSelected;


    public Player player1, player2, player0;

    public bool multiplayer;

    public controlledGameStateManager gameStateManager;
    public controlledStageGenerator stageGen;



    public Text player1_Kills, player1_coins, player1_lives;
    public Text player1_buff1, player1_buls, player1_bombs;
    public Text player1_exp, player1_spd, player1_lvl;
    public Text player1_hp;

    public GameObject player1_Menu, player2_Menu;

    public Text player2_Kills, player2_coins, player2_lives;
    public Text player2_buff1, player2_buls, player2_bombs;
    public Text player2_exp, player2_spd, player2_lvl;
    public Text player2_hp;


    public GameObject xTiles, yTiles;


    public GameObject battleMenu;
    public List<Button> battleButtons = new List<Button>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //send to fucntion
        if (gameStateManager.gameState == controlledGameStateManager.GameState.InMatch)
        {
            player1 = gameStateManager.players[0];
            player1_Menu.SetActive(true);

            player1_Kills.text = player1.kills.ToString();
            player1_coins.text = player1.coins.ToString();
            player1_lives.text = player1.lives.ToString();

            player1_buls.text = player1.weaponCount[0].wepCount.ToString();
            player1_bombs.text = player1.weaponCount[1].wepCount.ToString();

            player1_buff1.text = player1.itemBuffCount[0].itemCount.ToString();
            player1_exp.text = player1.exp.ToString();
            player1_spd.text = player1.playerSpd.ToString();
            player1_lvl.text = player1.expToLevel.ToString();
            player1_hp.text = player1.hp.ToString() + "/" + player1.mhp;

            //player 2 stats
            if (multiplayer == true)
            {
                player2_Menu.SetActive(true);
                player2 = gameStateManager.players[1];


                player2_Kills.text = player2.kills.ToString();
                player2_coins.text = player2.coins.ToString();
                player2_lives.text = player2.lives.ToString();

                player2_bombs.text = "0";
                player2_buls.text = player2.weaponCount[0].wepCount.ToString();
                player2_buff1.text = player2.itemBuffCount[0].itemCount.ToString();
                player2_exp.text = player2.exp.ToString();
                player2_spd.text = player2.playerSpd.ToString();
                player2_lvl.text = player2.expToLevel.ToString();
                player2_hp.text = player2.hp.ToString() + "/" + player2.mhp;
            }
        }

        if (gameStateManager.gameState == controlledGameStateManager.GameState.MapGen)
        {

        }



    }


    public void ChangeVerticalSplitScreen()
    {
        if (gameStateManager.verticalSplitScreen == true)
            gameStateManager.verticalSplitScreen = false;
        else
            gameStateManager.verticalSplitScreen = true;
    }


    public void goToOptionsMenu()
    {
        titleScreen.SetActive(false);
        optionsScreen.SetActive(true);

        camMenu_MapGen.SetActive(false);

    }

    public void goToTitleScreen()
    {
        tilecardUI.SetActive(false);

        optionsScreen.SetActive(false);
        titleScreen.SetActive(true);
        camMenu_MapGen.SetActive(false);
        player2_Menu.SetActive(false);
        player1_Menu.SetActive(false);
        //battleMenu.SetActive(false);
    }

    public void goToMapGen()
    {
        camMenu_MapGen.SetActive(true);
        titleScreen.SetActive(false);
        optionsScreen.SetActive(false);

    }


    public void UITroopCreate()
    {
        curTowerSelected.CreateTroop();
    }

    public void TileCardTroopCreate()
    {
        Debug.Log("player enter summon mode, grab clicked map tile in player/maptile functionality//irganize project later.. look for work");
        gameStateManager.players[0].player = Player.PlayerMode.summon;
        gameStateManager.troopCreateClickAt = Time.time;
            }

    public void hideTowerUI()
    {
        towerUI.SetActive(false);
    }


    public void hideTileCardUI()
    {
        tilecardUI.SetActive(false);
    }
}
