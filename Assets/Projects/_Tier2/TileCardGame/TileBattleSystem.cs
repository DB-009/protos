using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TileBattleSystem : MonoBehaviour {

    public tileBattler player1, player2;
    public int playerTurn;

    public enum State { off, on , waiting , lobby, building};
    public State estado;

    public List<tilePiece> player1Tiles, player2Tiles = new List<tilePiece>();


    public List<KeyCode> keyMap = new List<KeyCode>();
    public float turnSwitched;

    // Use this for initialization
    void Start () {


	if(playerTurn == 0)
        {

            player1.myState = tileBattler.State.selecting;

            foreach(tilePiece disPiece in player1Tiles)
            {
                disPiece.isClickable = true;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawCard()
    {
        if (playerTurn == 0)
        {
            Debug.Log("draw on 1" );
        }
        else
        {
            Debug.Log("draw on2 " );
        }

    }

    public void switchTurn()
    {
        if (playerTurn == 0)
        {
            player1.myState = tileBattler.State.waiting;
            player2.myState = tileBattler.State.selecting;
            playerTurn = 1;
            foreach (tilePiece disPiece in player1Tiles)
            {
                disPiece.isClickable = false;
            }

            foreach (tilePiece disPiece in player2Tiles)
            {
                disPiece.isClickable = true;
            }
        }
        else
        {
            playerTurn = 0;
            player2.myState = tileBattler.State.waiting;
            player1.myState = tileBattler.State.selecting;
            foreach (tilePiece disPiece in player2Tiles)
            {
                disPiece.isClickable = false;
            }
            foreach (tilePiece disPiece in player1Tiles)
            {
                disPiece.isClickable = true;
            }
        }




        turnSwitched = Time.time;
    }
}
