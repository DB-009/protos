using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class puzzleSpawner : MonoBehaviour {

    public puzzleMatch matchManager;

    public List<GameObject> createdPieces;
 


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PuzzleGenerate(int pieces)
    {

        float xDis=0, yDis = 0;

        for(int temp = 0; temp < pieces; temp++)
        {

            if(temp != 0 && temp % matchManager.tilesX == 0)
            {
                yDis += matchManager.tileSizeY;
                xDis = 0;
            }


            GenerateRandomPiece(new Vector3(matchManager.xDis + xDis, matchManager.yDis - yDis, 0));
            xDis += matchManager.tileSizeX;
        }

        


    }


    public void LotteryGame(int pieces)
    {

        float xDis = 0, yDis = 0;

        for (int temp = 0; temp < pieces; temp++)
        {

            if (temp != 0 && temp % matchManager.tilesX == 0)
            {
                yDis += matchManager.tileSizeY;
                xDis = 0;
            }


            GeneratePiece(matchManager.puzzlePieces[0],new Vector3(matchManager.xDis + xDis, matchManager.yDis - yDis, 0));
            xDis += matchManager.tileSizeX;
        }




    }

    public void GenerateRandomPiece(Vector3 spawnPos)
    {
        int rand = Random.RandomRange(0, matchManager.puzzlePieces.Count);
        GameObject puzzlePiece = GameObject.Instantiate(matchManager.puzzlePieces[rand],  spawnPos, Quaternion.identity) as GameObject;


        puzzlePiece disPiece = puzzlePiece.GetComponent<puzzlePiece>();

        disPiece.initXPos = spawnPos.x;
        disPiece.initYPos = spawnPos.y;

        disPiece.initPos = spawnPos;

        disPiece.puzzleGen = this;
        disPiece.matchManager = matchManager;

        createdPieces.Add(puzzlePiece);

        disPiece.id = createdPieces.Count - 1;
    }





    public void GeneratePiece(GameObject spawnObj, Vector3 spawnPos)
    {
       
        GameObject puzzlePiece = GameObject.Instantiate(spawnObj, spawnPos, Quaternion.identity) as GameObject;


        puzzlePiece disPiece = puzzlePiece.GetComponent<puzzlePiece>();

        disPiece.initXPos = spawnPos.x;
        disPiece.initYPos = spawnPos.y;

        disPiece.initPos = spawnPos;

        disPiece.puzzleGen = this;
        disPiece.matchManager = matchManager;

        createdPieces.Add(puzzlePiece);

        disPiece.id = createdPieces.Count - 1;
    }
}
