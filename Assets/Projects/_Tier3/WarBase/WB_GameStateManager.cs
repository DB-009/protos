using UnityEngine;
using System.Collections;

public class WB_GameStateManager : MonoBehaviour {

    public enum GameState { Title, WorldMap, Castle, City, ResourceField, ActionWindow, LegacyScreen ,InMatch, Settings };
    public GameState gameState;

    public WB_UIManager uiManager;
    public controlledStageGenerator stageGenerator;

    public WB_CameraControls cam;

    public GameObject castle1;

    // Use this for initialization
    void Awake () {
        uiManager.RefreshUI();

    }

    // Update is called once per frame
    void Update () {
	if(gameState == GameState.Title)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                gameState = GameState.WorldMap;
                loadWorldMap();
                loadCastles();
                uiManager.RefreshUI();
            }
        }
       else if (gameState == GameState.WorldMap)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                gameState = GameState.Title;
                uiManager.RefreshUI();

            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                gameState = GameState.InMatch;
                uiManager.RefreshUI();

            }
        }
        else if (gameState == GameState.InMatch)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                gameState = GameState.WorldMap;
                uiManager.RefreshUI();

            }
    
        }
    }


    public void loadWorldMap()
    {
        stageGenerator.deleteMap();
        stageGenerator.updateFreeSpacesList();


        stageGenerator.GenerateBGMap(stageGenerator.possibleTiles[1]);//creates the imaginary background layer..

        //stageGenerator.GenerateBorder(stageGenerator.possibleTiles[0]);//1st top layer item
                                                 // stageGenerator.GenBattleZone(stageObj);

        // stageGenerator.GenerateMiddleArena(randomObj, true);

        stageGenerator.updateFreeSpacesList();
        cam.goToPosition(12, 12);
    }



    public void loadCastles()
    {

        CreateCastle(0, 0,1);
        CreateCastle(12, 12,0);
        CreateCastle(3, 3,1);

    }



    public void CreateCastle(int x, int z , int id)
    {

        Vector3 newPos = new Vector3(x, 0 + (stageGenerator.tileScale), z);
        int vTileID = stageGenerator.findVtile(newPos);

        

        Debug.Log(vTileID);

        if (vTileID != -1)
        {
            Debug.Log("found a tile");

            float towerScaleY = castle1.GetComponent<Renderer>().bounds.size.y - stageGenerator.tileScale;//get the difference from tower scale to fixedTile Scale (For organized Drawing)
            towerScaleY *= .5f;//multiplying by .5f because object in unity get drawn from the center so half of one


            GameObject tileCreated = GameObject.Instantiate(castle1, new Vector3(newPos.x, newPos.y + towerScaleY, newPos.z), Quaternion.identity) as GameObject;


            tileCreated.GetComponent<mapTile>().initialTilePos = stageGenerator.virtualTiles[vTileID].initialTilePos;

            tileCreated.GetComponent<WB_Castle>().ownerID = id;


            tileCreated.GetComponent<mapTile>().disType = mapTile.TileType.castle;


            tileCreated.GetComponent<mapTile>().gameStateManager = this.gameObject;


            stageGenerator.fgMapTiles.Add(new mapTile(stageGenerator.virtualTiles[vTileID].initialTilePos, tileCreated));



            stageGenerator.virtualTiles.RemoveAt(vTileID);
        }
        else
        {
            Debug.Log("no tile");
        }
    }



}
