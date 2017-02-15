using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapTile : MonoBehaviour {


    //Tile Type for future usage
    // public enum TileType { grass, forest, mountain, water, sand, lava, ice, deepWater, border };

    //public GameObject gameObj;

    //public TileType tileType;
    public controlledStageGenerator stageGen;
    public GameObject gameStateManager;
  
    public Vector3 initialTilePos;
    public GameObject disTile;

    public bool destructible,trackPlacement;
    public float hp, mhp;
    public int vTileID;

    public itemRelease itemReleaseScript;
    

    public enum TileType { floor, border, mapItem , enemy , obstacle, tower  ,castle};
    public TileType disType;

    public bool isTactical;
    public List<tacticalTile> tacticalTiles = new List<tacticalTile>(); 

    public mapTile( Vector3 initPos , GameObject leTile )
    {
      
        initialTilePos = initPos;
        disTile = leTile;
     

       // disTile.GetComponent<mapTile>().initialTilePos = initPos;
        //disTile.GetComponent<mapTile>().initialTilePos = initPos;

    }

    public float mouseDownStart;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "projectile")
        {
            if (destructible == true)
            {
                itemReleaseScript.ReleaseItems(initialTilePos, col.GetComponent<projectileLife>().myPlayer.stageGen);
                col.GetComponent<projectileLife>().myPlayer.stageGen.DestroyTile(this);
                Destroy(col.gameObject);
            }
        }

        else if (col.tag == "hitObj")
        {
            Destroy(this.gameObject);
        }
    }

    void OnMouseDown()
    {
        if(disType == TileType.floor)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                mouseDownStart = Time.time;
            }
        }
        else if(disType == TileType.castle)
        {
            gameStateManager.GetComponent<WB_GameStateManager>().uiManager.ActionWindowLoad(0, this.gameObject);
        }
    }

    void OnMouseOver()
    {
        if (gameStateManager.GetComponent<controlledGameStateManager>() != null)
        {
            controlledGameStateManager gs = gameStateManager.GetComponent<controlledGameStateManager>();
            if (disType == TileType.floor && mouseDownStart != 0)
            {
                if (gs.gameState != controlledGameStateManager.GameState.TileCard && gs.gameState != controlledGameStateManager.GameState.OnePlayer)
            {
                if (gs.players[0].clickedTroops.Count > 0)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        Debug.Log("tile click animation");

                        if (Time.time >= mouseDownStart + 2 && mouseDownStart != 0)
                        {
                            Debug.Log("MOve enemies here");

                            Vector3 tacticalPos = Vector3.zero;
                            bool posSelected = false;

                            int temp = 0;
                            foreach (GameObject disTroop in gs.players[0].clickedTroops)
                            {

                                Debug.Log("found a tile here should go tactical tile placement");
                                if (disTroop != gs.players[0].gameObject)
                                {
                                    if (tacticalTiles[temp].occupied == false && disTroop.GetComponent<Troop>().moveable == true)
                                    {
                                        tacticalPos = tacticalTiles[temp].transform.position;

                                        tacticalTiles[temp].occupied = true;


                                        float objScaleY = disTroop.GetComponent<Renderer>().bounds.size.y - stageGen.tileScale;//get the difference from tower scale to fixedTile Scale (For organized Drawing)
                                        objScaleY *= .5f;//multiplying by .5f because object in unity get drawn from the center so half of one


                                        disTroop.transform.position = new Vector3(tacticalPos.x, tacticalPos.y + objScaleY + 3, tacticalPos.z);


                                    }
                                }
                                else
                                {
                                    Debug.Log("player object delete move him");

                                    //
                                    if (tacticalTiles[temp].occupied == false)
                                    {
                                        tacticalPos = tacticalTiles[temp].transform.position;

                                        tacticalTiles[temp].occupied = true;


                                        float objScaleY = disTroop.GetComponent<Renderer>().bounds.size.y - stageGen.tileScale;//get the difference from tower scale to fixedTile Scale (For organized Drawing)
                                        objScaleY *= .5f;//multiplying by .5f because object in unity get drawn from the center so half of one


                                        disTroop.transform.position = new Vector3(tacticalPos.x, tacticalPos.y + objScaleY + 3, tacticalPos.z);


                                    }
                                    //
                                }
                                temp++;
                            }



                            mouseDownStart = 0;
                        }

                    }
                }
                else
                    Debug.Log("click a troop");






            }
            else if (gs.gameState == controlledGameStateManager.GameState.TileCard)
            {
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {

                        if (gs.players[0].player == Player.PlayerMode.summon)
                        {
                                gs.CreateTroop(gs.players[0].gameObject, gs.troop, gs.troop2, this);
                                gs.players[0].player = Player.PlayerMode.turnSequence;

                        }
                    }
                }

            }
        }
        }


        
      
    }

    void OnMouseExit()
    {
        if (disType == TileType.floor)
        {


            mouseDownStart = 0;

        }
    }

}
