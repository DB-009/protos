using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGenManager : MonoBehaviour {

     public GameManager gameManager;

    public int maxStages;

    public float padding;

    public List<StageDesignClass> ScriptedStage = new List<StageDesignClass>();

    public List<GameObject> possibleStageDesigns = new List<GameObject>();


    public Vector3 fwdHotSpot, bkwdHotSpot;
	// Use this for initialization
	void Start () {

        float xDis = 2;
        float yDis = 0;
        for(int temp=0; temp <= maxStages-1; temp++)
        {


            


            int rand = Random.Range(0, possibleStageDesigns.Count);

            if (possibleStageDesigns[rand].GetComponent<StageDesignClass>().classType == StageDesignClass.type.stage)
            {
                xDis += possibleStageDesigns[rand].GetComponent<StageDesignClass>().width/2;
            }
            else if (possibleStageDesigns[rand].GetComponent<StageDesignClass>().classType == StageDesignClass.type.platforms)
            {
                xDis += possibleStageDesigns[rand].GetComponent<objGen>().xDis;
            }

            Vector3 spawnPos = new Vector3(this.transform.position.x + xDis, this.transform.position.y + yDis , this.transform.position.z);

          
            GameObject spawnObj = GameObject.Instantiate(possibleStageDesigns[rand], spawnPos, Quaternion.identity) as GameObject;

            StageDesignClass disStage = spawnObj.GetComponent<StageDesignClass>();
            ScriptedStage.Add(disStage);



                objGen disGen = disStage.GetComponent<objGen>();
            if (disGen != null)
            {
                disGen.gameStateManager = gameManager;
                Debug.Log("Called Object Gen function from StageGenManager");
                disGen.ObjGeneration();
            }

            if (disStage.classType == StageDesignClass.type.stage)
            {
                xDis += disStage.width/2;
                xDis += padding;

                if(disStage.hasYExit == true)
                {
                    yDis += disStage.height + 4;
                }

            }
            else if (disStage.classType == StageDesignClass.type.platforms)
            {


                xDis += disStage.GetComponent<objGen>().amount * (disStage.GetComponent<objGen>().xDis + (disStage.GetComponent<objGen>().padding* disStage.GetComponent<objGen>().amount/3));
                xDis += padding;
            }
            else if (disStage.classType == StageDesignClass.type.stack)
            {

                disStage.GetComponent<StackStageGen>().GenerateStackStage();

                Debug.Log("ehh increased " + disStage.GetComponent<StackStageGen>().xIncrease);
                xDis += disStage.GetComponent<StackStageGen>().xIncrease;
                yDis += disStage.GetComponent<StackStageGen>().yIncrease;
                Debug.Log("Stack");
            }
            else if (disStage.classType == StageDesignClass.type.cannon)
            {
                if (temp !=0 &&  ScriptedStage[temp - 1].classType == StageDesignClass.type.cannon && disStage.classType == StageDesignClass.type.cannon)
                {
                    Debug.Log("connecting cannons");

                    GameObject previousStage = ScriptedStage[temp - 1].gameObject;

                    buffZone previousCannon = previousStage.GetComponent<objGen>().createdObjs[previousStage.GetComponent<objGen>().createdObjs.Count - 1].transform.GetChild(0).GetComponent<buffZone>();

                    Debug.Log("prev cannon " + previousCannon.name + " : " + previousStage.name);

                    previousCannon.fwdTarget = ScriptedStage[temp].GetComponent<objGen>().createdObjs[0];

                    disGen.createdObjs[0].transform.GetChild(0).GetComponent<buffZone>().bkwrdTarget = previousCannon.gameObject;

                }
                xDis += disGen.xReturn;
                yDis += disGen.yReturn;
                    //track second cannon position
            }
        }



    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
