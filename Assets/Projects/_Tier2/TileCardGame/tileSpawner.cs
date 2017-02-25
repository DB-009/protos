using UnityEngine;
using System.Collections;

public class tileSpawner : MonoBehaviour {


    public GameObject ObjPiece,extraPiece;
    public int objScale,tileCount,tilesY;
    public TileBattleSystem tileBS;

    // Use this for initialization
    void Start () {


            float xDis = 0, yDis = 0;

            for (int temp = 0; temp < tileCount; temp++)
            {

                if (temp != 0 && temp % tilesY == 0)
                {


                yDis += objScale;
                    xDis = 0;


                }

                if(temp == 0)
                {
                GeneratePiece(new Vector3(xDis- objScale, yDis, 0), extraPiece);
                GeneratePiece(new Vector3(xDis- (objScale*2), yDis, 0), extraPiece);

                GeneratePiece(new Vector3(xDis + (tilesY*objScale) , yDis, 0), extraPiece);
                GeneratePiece(new Vector3(xDis +(tilesY*objScale) , yDis, 0), extraPiece);
            }




                GeneratePiece(new Vector3( xDis,   yDis, 0), ObjPiece);
                xDis += objScale;
            }

        GeneratePiece(new Vector3(xDis + objScale, yDis, 0), extraPiece);
        GeneratePiece(new Vector3(xDis + (objScale * 2), yDis, 0), extraPiece);

        GeneratePiece(new Vector3(xDis - (tilesY * objScale), yDis, 0), extraPiece);
        GeneratePiece(new Vector3(xDis - (tilesY * objScale), yDis, 0), extraPiece);
    }
	
	// Update is called once per frame
	void Update () {
	





	}


    public void GeneratePiece(Vector3 spawnPos, GameObject gameObj)
    {

        int rand = Random.RandomRange(0, tileCount);

        GameObject tile = GameObject.Instantiate(gameObj, spawnPos, Quaternion.identity) as GameObject;

        tilePiece disPiece = tile.GetComponent<tilePiece>();

        disPiece.initPos= spawnPos;

        disPiece.tileBS = tileBS;
        disPiece.isClickable = true;

    }
}
