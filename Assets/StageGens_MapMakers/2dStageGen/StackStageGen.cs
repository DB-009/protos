using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StackStageGen : MonoBehaviour {



    public int maxStages,rows;

    public float padding;

    public bool inverted;

    public List<StageDesignClass> ScriptedStage = new List<StageDesignClass>();

    public List<GameObject> possibleStageDesigns = new List<GameObject>();

    public float xIncrease, yIncrease;

    public Vector3 fwdHotSpot, bkwdHotSpot;



    public void GenerateStackStage()
    {
        float xDis = 2;
        float yDis = 0;
        int row = 1;
        int draws = 0, endDraw = maxStages - 1;

        for (int temp = 0; temp < maxStages * rows; temp++)
        {

            //Debug.Log("temp is " + temp);

            int rand = Random.Range(0, possibleStageDesigns.Count);

            if (inverted == true && draws == endDraw)
            {
                rand = 2;//our wall category
                inverted = false;
                xDis -= possibleStageDesigns[rand].GetComponent<StageDesignClass>().width;
                //Debug.Log("reverted at" + temp);
                draws = -1;
            }
            else if (draws == endDraw)//awlays create a lift/stair at end of stack so 2 is wall
            {
                rand = 2;//our wall category
                inverted = true;


                //Debug.Log("inverted at" + temp + " end invert at " + (endDraw + temp));
                xDis += possibleStageDesigns[rand].GetComponent<StageDesignClass>().width;
                row++;
                draws = -1;
            }



            if (possibleStageDesigns[rand].GetComponent<StageDesignClass>().classType == StageDesignClass.type.stage)
            {

                if (inverted == true)
                {
                    xDis -= possibleStageDesigns[rand].GetComponent<StageDesignClass>().width / 2;

                }
                else
                {
                    xDis += possibleStageDesigns[rand].GetComponent<StageDesignClass>().width / 2;

                }

            }
            else if (possibleStageDesigns[rand].GetComponent<StageDesignClass>().classType == StageDesignClass.type.platforms)
            {


                if (inverted == true)
                {
                    xDis -= possibleStageDesigns[rand].GetComponent<objGen>().xDis;

                }
                else
                {
                    xDis += possibleStageDesigns[rand].GetComponent<objGen>().xDis;

                }
            }

            Vector3 spawnPos = new Vector3(this.transform.position.x + xDis, this.transform.position.y + yDis, this.transform.position.z);



             GameObject prefab = possibleStageDesigns[rand];

            if (rand == 2 && inverted == true)
                prefab = possibleStageDesigns[rand].GetComponent<StageDesignClass>().invertedObj;

                GameObject spawnObj = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity) as GameObject;

          

            StageDesignClass disStage = spawnObj.GetComponent<StageDesignClass>();

            objGen disGen = null;
            if (disStage.isGenerator == true)
            {
                 disGen =  disStage.GetComponent<objGen>();

                if (inverted == true)
                    disGen.invertedSpawn = true;

                disGen.ObjGeneration();

                
             }

            spawnObj.transform.SetParent(this.transform);

            if (disStage.classType == StageDesignClass.type.stage)
            {

                if (inverted == true)
                {
                    xDis -= disStage.width / 2;
                    xDis -= padding;

                    if (disStage.hasYExit == true)
                    {
                        yDis += disStage.height + 4;
                    }
                }
                else
                {
                    xDis += disStage.width / 2;
                    xDis += padding;

                    if (disStage.hasYExit == true)
                    {
                        yDis += disStage.height + 4;
                    }
                }


            }
            else if (disStage.classType == StageDesignClass.type.platforms)
            {



                if (inverted == true)
                {
                   
                    xDis -= disGen.amount * (disStage.GetComponent<objGen>().xDis + (disGen.padding * disGen.amount / 3));
                    xDis -= padding;
                }
                else
                {
                    xDis += disGen.amount * (disStage.GetComponent<objGen>().xDis + (disGen.padding * disGen.amount / 3));
                    xDis += padding;
                }

            }

            draws++;



        }

        xIncrease = xDis;
        yIncrease = yDis;

    }
}

