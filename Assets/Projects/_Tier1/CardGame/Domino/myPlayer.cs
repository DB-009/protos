using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myPlayer : MonoBehaviour {

    public int playerID;

    public myGameStateManager gameStateManager;
    public myTurnBasedSystem turnBased;

    public List<GameObject> myObjects,objectsInPlay = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(gameStateManager.myState == myGameStateManager.State.inMatch && turnBased.myState == myTurnBasedSystem.State.ongoing)
        {
            if(turnBased.playerTurn == playerID)
            {
                Debug.Log("Player #" + (playerID+1) + "s turn");
            }
        }

	}
}
