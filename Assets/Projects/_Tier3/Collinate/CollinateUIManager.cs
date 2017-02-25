using UnityEngine;
using System;
using UnityEngine.UI;


public class CollinateUIManager : MonoBehaviour {

    public CollinateGameStateManager gameStateManager;

    playerControls myPlayer;
    //TITLESCREEN
    public GameObject titleScreen;


    //PLANETARY UI OBJECTS
    public GameObject planetaryScreen,mwin;
    public Text ships, people, troops, upkeep,mwinText,mwinSpeaker;




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {



    }

    public void UIFix()
    {

        if (gameStateManager.gameState != CollinateGameStateManager.GameState.TitleScreen)
        {
            titleScreen.SetActive(false);
        }

        if (gameStateManager.gameState == CollinateGameStateManager.GameState.TitleScreen)
        {
            titleScreen.SetActive(true);
        }

    }

    public void PlanetaryUI(Planet planet)
    {


        ships.text = planet.ships.ToString();

        people.text = planet.people.ToString();

        troops.text = planet.troops.ToString();

        upkeep.text = planet.upkeep.ToString();

        planetaryScreen.SetActive(true);

    }

    public void CloseUI(GameObject ui)
    {
        ui.SetActive(false);
    }
    
}
