using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class puzzleMatch : MonoBehaviour {

    public enum State { off, ongoing, wait, ended, endscreen, lobby }
    public State myState;

    public enum GameType { onePlayer, TwoPlayer, Puzzle, Survival, Platform }
    public GameType gamType;

    public GameObject spawn1, spawn2;

    public List<GameObject> puzzlePieces, player1Pieces, player2Pieces = new List<GameObject>();

    public float liftIntrerval;

    public float xDis, yDis;
    public float tileSizeX, tileSizeY;
    public int tilesX,startAmnt;

    // Use this for initialization
    void Start () {
        Debug.Log("Press Enter to start match");

    }

    // Update is called once per frame
    void Update () {
        if(myState == State.off)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                myState = State.lobby;
            }
        }
	else if(myState == State.lobby)
        {
            //MatchStart
            spawn1.GetComponent<puzzleSpawner>().PuzzleGenerate(startAmnt);


            myState = State.ongoing;
        }
	}
}
