using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class objGen_Net : NetworkBehaviour
{

    public enum ObjType { obstacle, buff, bonus, endlessPlatforms, enemy, bg, runway, cannon, npc };
    public ObjType type;
   
    public List<GameObject> targetPlayers = new List<GameObject>();
    public List<GameObject> spawnObjs = new List<GameObject>();

    public List<GameObject> createdObjs = new List<GameObject>();

    public GameManager gameStateManager;

    public int amount;//amount can create
    public float xDis;//distance apart
    public bool canSpawn;//on or off switch

    public bool allowJitter;
    public float yJitterMin, yJitterMax, xJitterMin, xJitterMax;

    public float padding, drag;

    public bool right;
    public bool invertedSpawn;

    public float xReturn, yReturn;

    public bool timedGen;

    public float timeBetween;
    public int groundTroops, GroundTroops_gun, flyers, flyers_gun,allowedFlyers,allowedShooters;
    // 0 = obstacle , 1 = speed up , 2 = bonus.


    public override void OnStartServer()
    {


        if (timedGen == true)
        {
            InvokeRepeating("TimedGen", timeBetween, timeBetween);
           
        }
        else
        {
            if (type == ObjType.enemy)
            {
                ObjGeneration();

                foreach (GameObject disObj in createdObjs)
                {
                    //.GetComponent<CpuAi>().target = gameStateManager.player1.gameObject;


                    int clientTarget = Random.Range(0, targetPlayers.Count);

                    disObj.GetComponent<CpuAi>().target = targetPlayers[clientTarget];

                    Debug.Log("rand target Set based on clients");
                }
            }
            else if (type != ObjType.cannon && type != ObjType.endlessPlatforms && type != ObjType.buff)
            {
                ObjGeneration();
               
            }

        }

    }




    public void TimedGen()
    {

        if(createdObjs.Count != amount)
        {
            int spawnNum = 0;
            if (spawnObjs.Count > 1)
            {

                    spawnNum = casualSpawnOnline();


            }
            float yDis = Random.Range(yJitterMin, yJitterMax);

            float xDis = Random.Range(xJitterMin, xJitterMax);

            if (allowJitter == false && spawnNum != 1)
                xDis = spawnObjs[spawnNum].transform.localScale.x;



            Vector3 spawnPos = Vector3.zero;

            int temp = createdObjs.Count;


            if (invertedSpawn == false)
            {
                if (allowJitter == false || spawnNum != 1)
                    spawnPos = new Vector3(this.transform.position.x + (padding) + (xDis), this.transform.position.y , this.transform.position.z);
                else if (spawnNum == 1)
                    spawnPos = new Vector3(this.transform.position.x + xDis, this.transform.position.y + yDis, this.transform.position.z);

            }
            else
            {
                if (allowJitter == false || spawnNum != 1)
                    spawnPos = new Vector3(this.transform.position.x - (padding) - (xDis), this.transform.position.y , this.transform.position.z);
                else if(spawnNum == 1)
                    spawnPos = new Vector3(this.transform.position.x  - (xDis), this.transform.position.y + yDis, this.transform.position.z);

            }



            GameObject spawnObj = GameObject.Instantiate(spawnObjs[spawnNum], spawnPos, spawnObjs[spawnNum].transform.rotation) as GameObject;

            if (type != ObjType.cannon)//normal creation
            {
                spawnObj.transform.SetParent(this.transform);

            }
            else
                spawnObj.transform.GetChild(0).GetComponent<buffZone>().objGen = this.gameObject;




            ///Set Stats onto Object depending on type

            if (type == ObjType.npc)
            {
                spawnObj.GetComponent<mf_player>().isController = false;
                spawnObj.SetActive(true);
            }
            else if (type == ObjType.enemy)
            {

                CpuAi disCpu = spawnObj.GetComponent<CpuAi>();

                  int clientTarget = Random.Range(0, targetPlayers.Count);

                disCpu.target = targetPlayers[clientTarget];

                //Debug.Log("rand target Set based on clients");

               
                if (disCpu.eneType == CpuAi.enemyType.flyer)
                {
                    disCpu.rb.useGravity = false;
                    if (disCpu.canShoot == true)
                    {
                        flyers_gun++;
                    }
                    else
                        flyers++;
                }
                else if(disCpu.eneType == CpuAi.enemyType.fighter)
                {
                    groundTroops++;
                }
                else if(disCpu.eneType == CpuAi.enemyType.shooter)
                {
                    GroundTroops_gun++;
                }


            }

            ////


           
            createdObjs.Add(spawnObj);

            NetworkServer.Spawn(spawnObj);

            xReturn += createdObjs[createdObjs.Count - 1].transform.position.x - this.transform.position.x + padding;
            yReturn += createdObjs[createdObjs.Count - 1].transform.position.y - this.transform.position.y + padding;
            //Debug.Log(xReturn + "xretrurn" + createdObjs[createdObjs.Count - 1].name);



        }




    }



    public int casualSpawnOnline()
    {
        int spawnNum = 0;

        if (type == ObjType.buff)
        {
            bool needHpSpawn = false;
            bool needJetPack = false;

            foreach(GameObject disPlayer in targetPlayers)
            {

                LaneShift_TopDown_NET player = disPlayer.GetComponent<LaneShift_TopDown_NET>();

                if (player.hp <= player.mhp / 2)
                {
                    needHpSpawn = true;
                   
                }

                if (player.jetPack == false)
                {
                    needJetPack = true;
                    
                }
            }

                      
            if(needHpSpawn != true)
                spawnNum = Random.RandomRange(1, spawnObjs.Count);





            while (spawnNum == 2 && needJetPack == false)
            {
                spawnNum = Random.RandomRange(1, spawnObjs.Count);
                //Debug.Log("Tried spawning a jet pack while you had one");
            }

        }
        else if (type == ObjType.enemy)
        {
            spawnNum = Random.RandomRange(0, spawnObjs.Count);

            CpuAi disCpu = spawnObjs[spawnNum].GetComponent<CpuAi>();

            if ((disCpu.eneType == CpuAi.enemyType.flyer) && flyers >= allowedFlyers)
            {
                while ((disCpu.eneType == CpuAi.enemyType.flyer))
                {
                    spawnNum = Random.RandomRange(0, spawnObjs.Count);
                    disCpu = spawnObjs[spawnNum].GetComponent<CpuAi>();
                    //.Log("tried summoning a flyer when limit was reached");
                }
            }
            else if ((disCpu.eneType == CpuAi.enemyType.shooter) && GroundTroops_gun >= allowedShooters)
            {
                while (disCpu.eneType == CpuAi.enemyType.shooter)
                {
                    spawnNum = Random.RandomRange(0, spawnObjs.Count);
                    // Debug.Log("tried summoning a shooter when limit was reached");
                    disCpu = spawnObjs[spawnNum].GetComponent<CpuAi>();
                }
            }
        }

        return spawnNum;

    }




    /// <summary>
    /// DEFAULT OBJ GEN NOT NETWORK READY THIS STUFF BELOW IS FOR STAGE GEN
    /// 
    /// 
    /// </summary>











    //not NETWORK ready stage gen stuff
    public void ObjGeneration()
    {
        Debug.Log("this function isnt network ready");
        int objID = 0;


        if (objID != -1)
        {

            for (int temp = 0; temp < amount; temp++)
            {
                float yDis = Random.Range(yJitterMin, yJitterMax);

                float xDis = Random.Range(xJitterMin, xJitterMax);

                if (type == ObjType.cannon)//cannon placement 1st should be 0,0,0 else leave as is
                {

                    if (temp == 0)
                    {
                        Debug.Log("started cann");

                        yDis = 0;
                        xDis = 0;
                    }
                }
                else//if creating other object keep y jitter
                {
                    xDis = spawnObjs[objID].transform.localScale.x;
                }


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

                if (type != ObjType.cannon)//normal creation
                {
                    spawnObj.transform.SetParent(this.transform);

                }
                else
                    spawnObj.transform.GetChild(0).GetComponent<buffZone>().objGen = this.gameObject;

                createdObjs.Add(spawnObj);

                ///RANDOM FIXES

                if (type == ObjType.npc)
                {
                    spawnObj.GetComponent<mf_player>().isController = false;
                    spawnObj.SetActive(true);
                }

                ////



            }

            SecondaryCreation();


            xReturn += createdObjs[createdObjs.Count - 1].transform.position.x - this.transform.position.x + padding;
            yReturn += createdObjs[createdObjs.Count - 1].transform.position.y - this.transform.position.y + padding;
            //Debug.Log(xReturn + "xretrurn" + createdObjs[createdObjs.Count - 1].name);

            if (type == ObjType.cannon)
            {
                Debug.Log("enter forloop cann");

                int x = 1;
                foreach (GameObject disCannon in createdObjs)
                {
                    if (x < createdObjs.Count)//if x is less than crated objects count (x starts at 1 not 0) we do this to control last cannons target
                    {
                        disCannon.transform.GetChild(0).GetComponent<buffZone>().fwdTarget = createdObjs[x];

                        if (x != 1)
                            disCannon.transform.GetChild(0).GetComponent<buffZone>().bkwrdTarget = createdObjs[x - 2];
                        else
                            disCannon.transform.GetChild(0).GetComponent<buffZone>().bkwrdTarget = null;
                    }
                    else//set fwrd target of last cannon
                    {
                        disCannon.transform.GetChild(0).GetComponent<buffZone>().fwdTarget = null;
                        disCannon.transform.GetChild(0).GetComponent<buffZone>().bkwrdTarget = createdObjs[x - 2];


                    }

                    x++;
                }





            }


        }

    }

    public void SecondaryCreation()
    {
        //secondary creation
        if (type == ObjType.endlessPlatforms)
        {
            int created = 0;
            foreach (GameObject disFloor in createdObjs)
            {
                Vector3 spawnPos = new Vector3(disFloor.transform.position.x, disFloor.transform.position.y + 2, disFloor.transform.position.z);

                GameObject spawnObj = GameObject.Instantiate(spawnObjs[1], spawnPos, spawnObjs[1].transform.rotation) as GameObject;
                spawnObj.transform.SetParent(this.transform);
                created++;

                if (created % 2 == 0)
                {
                    disFloor.GetComponent<movingPlats>().animTic = 2;//start anition phase at 2 to move down
                    disFloor.GetComponent<movingPlats>().inverted = true;//this tile is inverted

                }
                else
                    disFloor.GetComponent<movingPlats>().animTic = 3;//start anition phase at 2 to move up

            }



            int tempp = 0;
            foreach (GameObject disFloor in createdObjs)
            {
                if (tempp % 2 == 0)
                {
                    Vector3 spawnPos = new Vector3(disFloor.transform.position.x - 2, disFloor.transform.position.y + 2, disFloor.transform.position.z);

                    GameObject spawnObj = GameObject.Instantiate(spawnObjs[2], spawnPos, spawnObjs[2].transform.rotation) as GameObject;

                    spawnObj.transform.SetParent(this.transform);
                }

                tempp++;
            }




        }




    }





}
