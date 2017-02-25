using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleShipsMatchObj : MonoBehaviour {

    public List<BattleShipsMapObj> myShips = new List<BattleShipsMapObj>();
    public int playerID;
    public BattleShipsMatchManager MatchManager;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if(MatchManager.playerTurn == playerID)
        {
            Debug.Log("Player " + playerID + " TUrn");
            if(Input.GetKeyDown(KeyCode.Return))
            {

                foreach(BattleShipsMapObj disObj in myShips)
                {
                    Debug.Log("ship " + disObj.name);
                }

                MatchManager.SwitchTurn();
            }
        }

    }
}
