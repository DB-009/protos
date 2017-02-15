using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class itemRelease : MonoBehaviour {

    public List<GameObject> possibleObjects = new List<GameObject>();

    public List<GameObject> actualObjects = new List<GameObject>();

    public int itemsInside;
    // Use this for initialization
    void Start () {
        itemsInside = Random.RandomRange(0,3);

        for(int temp=0;temp<=itemsInside; temp++)
        {
           int randomItem =  Random.RandomRange(1, possibleObjects.Count);
            actualObjects.Add(possibleObjects[randomItem]);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReleaseItems(Vector3 pos, controlledStageGenerator stageGen)
    {
        foreach (GameObject spawnObj in actualObjects)
        {
            Vector3 spawnPos = new Vector3(pos.x, pos.y, pos.z);

            GameObject tileCreated = GameObject.Instantiate(spawnObj, spawnPos, Quaternion.identity) as GameObject;
            //tempMap=grass;
            tileCreated.GetComponent<mapTile>().trackPlacement = false;
            tileCreated.GetComponent<mapTile>().initialTilePos = spawnPos;


            int vTileID = stageGen.findVtile(spawnPos);

            if(vTileID >=0)
            stageGen.virtualTiles.RemoveAt(vTileID);

            stageGen.fgMapTiles.Add(tileCreated.GetComponent<mapTile>());
        }
    }
}
