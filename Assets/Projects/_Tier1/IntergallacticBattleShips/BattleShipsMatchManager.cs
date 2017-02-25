using UnityEngine;
using System.Collections;

public class BattleShipsMatchManager : MonoBehaviour {

    public enum MatchState {  off, prestart, ongoing, wait , ended, endScreen}
    public MatchState state;

    public BattleShipsMapObj player1, player2;
    public int playerTurn;



    // Use this for initialization
    void Start () {
        Debug.Log("press enter to start match");
	}
	
	// Update is called once per frame
	void Update () {
	if(state == MatchState.off)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                state = MatchState.prestart;
            }
        }
        else if (state == MatchState.prestart)
        {
 
                state = MatchState.ongoing;
            Debug.Log("match started");

        }

    }


    public void SwitchTurn()
    {
        if (playerTurn == 0)
            playerTurn = 1;
        else
            playerTurn = 0;

    }
}
