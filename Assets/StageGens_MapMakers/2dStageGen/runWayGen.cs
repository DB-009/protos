using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class runWayGen : MonoBehaviour {

    public enum ObjType { obstacle, buff , bonus , endlessPlatforms , enemy, bg , runway };
    public ObjType type;
    public List<GameObject> spawnObjs = new List<GameObject>();



    public List<GameObject> createdObjs = new List<GameObject>();


    public GameManager gameStateManager;



    public int amount;//amount can create
    public float xDis;//distance apart
    public bool canSpawn;//on or off switch

    public float yJitterMin, yJitterMax;

    public float padding,drag;

    public bool right;
    public bool invertedSpawn;
    // 0 = obstacle , 1 = speed up , 2 = bonus.

	// Use this for initialization
	void Start () {



            for(int temp = 0; temp <= amount; temp++)
            {

                float yDis = Random.Range(yJitterMin, yJitterMax);

              int objID = Random.Range(0, spawnObjs.Count-1);

               xDis = spawnObjs[objID].transform.localScale.x;

                Vector3 spawnPos = Vector3.zero; 


                if (invertedSpawn == false)
                {
                     spawnPos = new Vector3(this.transform.position.x + (padding * temp) + (xDis * temp), this.transform.position.y + yDis, this.transform.position.z);

                }
                else
                {
                     spawnPos = new Vector3(this.transform.position.x - (padding * temp) - (xDis * temp), this.transform.position.y + yDis, this.transform.position.z);

                }


                GameObject spawnObj = GameObject.Instantiate(spawnObjs[objID], spawnPos, spawnObjs[objID].transform.rotation) as GameObject;

                spawnObj.transform.SetParent(this.transform);

                createdObjs.Add(spawnObj);//floor object
          

            }

            BGCreation();

            
        

    }
	
	// Update is called once per frame
	void Update () {

        //parallax

    }

    public void BGCreation()
    {

    }

}
