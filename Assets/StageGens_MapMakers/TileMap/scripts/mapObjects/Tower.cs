using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{

    public Tower disTower;
    public GameObject towerUI;
    public controlledStageGenerator stageGen;

    public int units;

    public GameObject owner;
    public mapTile disTile;
    public Transform spawnPos;

    public float hp, mhp, height;

    public GameObject troop,troop2;
    public List<Troop> createdTroops = new List<Troop>();
    // Use this for initialization
    void Awake()
    {
        height = this.gameObject.GetComponent<Renderer>().bounds.size.y;
        hp = 50;
        mhp = hp;

        disTile = this.GetComponent<mapTile>();
        disTower = this.GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        Debug.Log("You clicked me");
        if(towerUI.activeSelf == false)
         {
            owner.GetComponent<Player>().uiManager.curTowerSelected = disTower;
           towerUI.SetActive(true);
          }

    }


    public void CreateTroop()
    {

        Debug.Log("Change this to UI baseed summons");

        int rand = Random.Range(0,4);
         GameObject spawnObj = troop;

        if(rand >2)
        {
            spawnObj = troop2;
        }
  

        Vector3 newPos = new Vector3(this.disTile.initialTilePos.x, this.disTile.initialTilePos.y - stageGen.tileScale, this.disTile.initialTilePos.z - stageGen.tileScale);

        Vector3 tacticalPos = Vector3.zero;
        bool posSelected = false;
        foreach (mapTile disTile in stageGen.fgMapTiles)
        {
            if(disTile.initialTilePos == newPos)
            {
                Debug.Log("found tile " + newPos);
                foreach (tacticalTile disTTile in disTile.tacticalTiles)
                {
                    Debug.Log("found a tile here should go tactical tile placement");

                    if (disTTile.occupied == false && posSelected == false)
                    {
                        tacticalPos = disTTile.transform.position;
                        posSelected = true;
                        disTTile.occupied = true;

                        float objScaleY = spawnObj.GetComponent<Renderer>().bounds.size.y - stageGen.tileScale;//get the difference from tower scale to fixedTile Scale (For organized Drawing)
                        objScaleY *= .5f;//multiplying by .5f because object in unity get drawn from the center so half of one

                        GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(tacticalPos.x, tacticalPos.y + objScaleY + 3, tacticalPos.z), Quaternion.identity) as GameObject;

                        createdTroops.Add(tileCreated.GetComponent<Troop>());

                        tileCreated.GetComponent<mapTile>().initialTilePos = tacticalPos;
                        tileCreated.GetComponent<Troop>().owner = owner;
                    }
                }
             }

            }




    }
}
