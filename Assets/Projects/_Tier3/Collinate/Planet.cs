using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public CollinateUIManager uiManager;
    public CollinateGameStateManager gameStateManager;
    public playerControls myPlayer;
    

    public bool inhabited;//has it been touched by the world? 
    public float ownerID;//0 is universe,1 is server; 2 is first test / admin etc
    public int empire, faction, bandits,natural;//max is 100 between all 4




    public float planetSize ,enemies, cities, technology, magic, wildlife, sizeFactor, ruins, quests , nature , mines ;

    public float ships, people, troops, upkeep , robots ;

    // Use this for initialization
    void Awake () {

        upkeep = (people * 1) + (troops * 2);
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<CollinateUIManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseDown()
    {
        if(gameStateManager.myPlayer.canClick == true)
        {
            if (uiManager.planetaryScreen.active == false)
            {
                uiManager.PlanetaryUI(this);
                myPlayer.isControlling = false;
            }
            else
            {
                uiManager.CloseUI(uiManager.planetaryScreen);
                myPlayer.isControlling = true;
            }
        }

    }
}
