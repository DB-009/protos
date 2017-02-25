using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FFGenerator : MonoBehaviour {

    public enum ObjType { obstacle, buff , bonus , extra , enemy};
    public ObjType type;
    public List<GameObject> spawnObjs = new List<GameObject>();

    public FFGameState gameStateManager;

    public int amount;//amount can create
    public float xDis;//distance apart
    public bool canSpawn;//on or off switch

    // 0 = obstacle , 1 = speed up , 2 = bonus.

	// Use this for initialization
	void Start () {


        int objID = -1; 
        if(type == ObjType.obstacle)
        {
            objID = 0;
        }
        else if (type == ObjType.buff)
        {
            objID = 1;
        }

        if(objID != -1)
        {

            for(int temp = 0; temp <= amount; temp++)
            {
                Vector3 spawnPos = new Vector3(this.transform.position.x + 2 + (xDis* temp), this.transform.position.y, this.transform.position.z);

                GameObject spawnObj = GameObject.Instantiate(spawnObjs[objID], spawnPos, spawnObjs[objID].transform.rotation) as GameObject;

                int rand = Random.Range(0, gameStateManager.lanes.Count - 1);

                spawnObj.GetComponent<buffZone>().lane = gameStateManager.lanes[rand];
            }




        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
