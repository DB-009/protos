using UnityEngine;
using System.Collections;

public class WB_UIManager : MonoBehaviour {

    public WB_GameStateManager gameStateManger;

    public GameObject titleScreen, battle_UI , battle_Map, worldMap_UI, worldMap_Map, castleUI, innerCityUI, outerCityUI;

    public GameObject playerOwned, enemyOwned;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RefreshUI()
    {
        if(gameStateManger.gameState == WB_GameStateManager.GameState.Title)
        {
            titleScreen.SetActive(true);
            worldMap_Map.SetActive(false);
            battle_Map.SetActive(false);
            worldMap_UI.SetActive(false);
            castleUI.SetActive(false);

        }
        else if (gameStateManger.gameState == WB_GameStateManager.GameState.WorldMap)
        {
            titleScreen.SetActive(false);
            battle_Map.SetActive(false);
            worldMap_Map.SetActive(true);
            worldMap_UI.SetActive(true);
            castleUI.SetActive(false);

        }
        else if (gameStateManger.gameState == WB_GameStateManager.GameState.InMatch)
        {
            titleScreen.SetActive(false);
            battle_Map.SetActive(true);
            worldMap_Map.SetActive(false);
            worldMap_UI.SetActive(false);
            castleUI.SetActive(false);

        }
    }

    public void ActionWindowLoad(int window,GameObject parent)
    {
        if (window == 0)
        {
            playerOwned.SetActive(false);
            enemyOwned.SetActive(false);

            castleUI.SetActive(true);
            if(parent.GetComponent<WB_Castle>().ownerID == 0)
            {
                playerOwned.SetActive(true);
            }
            else
            {
                enemyOwned.SetActive(true);
            }
        }
    }



    public void ActionWindowClose(int window)
    {
        if (window == 0)
        {
            playerOwned.SetActive(false);
            enemyOwned.SetActive(false);

            castleUI.SetActive(false);
          
        }
    }
}
