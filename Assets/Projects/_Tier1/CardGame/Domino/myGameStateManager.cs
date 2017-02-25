using UnityEngine;
using System.Collections;

public class myGameStateManager : MonoBehaviour {

    public myTurnBasedSystem turnBased;

    public enum State { titlescreen, onePlayer, fourPlayer, inMatch }
    public State myState;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    

        if(myState == State.titlescreen)
        {

            Debug.Log("TItile Screen");

            if (Input.GetKeyDown(KeyCode.Return))
            {
                myState = State.inMatch;
                turnBased.myState = myTurnBasedSystem.State.lobby;
            }
        }
	}
}
