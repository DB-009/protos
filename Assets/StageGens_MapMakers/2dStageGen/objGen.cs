using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class objGen : MonoBehaviour
{

    public enum ObjType { obstacle, buff, bonus, endlessPlatforms, enemy, bg, runway, cannon, npc };
    public ObjType type;
    public GameObject targetPlayer;

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


    void Awake()
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
                    disObj.GetComponent<CpuAi>().target = targetPlayer;
                    Debug.Log("Set target");
                }
            }
            else if (type != ObjType.cannon && type != ObjType.endlessPlatforms && type != ObjType.buff)
            {
                ObjGeneration();
               
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

        //parallax
        if (type == ObjType.bg)
        {
            if (right == true)
                this.transform.position += new Vector3((gameStateManager.player1.GetComponent<Rigidbody>().velocity.x * drag) * Time.deltaTime, 0, 0);
            else
                this.transform.position -= new Vector3((gameStateManager.player1.GetComponent<Rigidbody>().velocity.x * drag) * Time.deltaTime, 0, 0);

        }


    }


    public void ObjGeneration()
    {

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


    public void TimedGen()
    {
        bool isCasual = false;
        //Debug.Log("remove the excess checks in here for other games laneShift and rocktoss");
        if(createdObjs.Count != amount && targetPlayer!= null)
        {
            int spawnNum = 0;
            if (spawnObjs.Count > 1)
            {
                LaneShift_TopDown casualPlayer = targetPlayer.GetComponent<LaneShift_TopDown>();
                LaneShift_TopDown_NET casualPlayer_NET = targetPlayer.GetComponent<LaneShift_TopDown_NET>();
                //Debug.Log("remove excess checks of other componenet laneshift and rocktoss");
                RockToss_Controller rockToss_Player = targetPlayer.GetComponent<RockToss_Controller>();
                RockToss_Controller_NET rockToss_Player_NET = targetPlayer.GetComponent<RockToss_Controller_NET>();

                GETP_Controller getp_player = targetPlayer.GetComponent<GETP_Controller>();


                if (casualPlayer != null)
                {
                    spawnNum = casualSpawnOffline(casualPlayer);
                    isCasual = true;
                }
                else if (casualPlayer_NET != null)
                {
                    spawnNum = casualSpawnOnline(casualPlayer_NET);
                    isCasual = true;

                }
                else if (rockToss_Player != null)
                {
                    spawnNum = RockToss_SpawnOffline(rockToss_Player);

                }
                else if (rockToss_Player_NET != null)
                {
                    spawnNum = RockToss_SpawnOnline(rockToss_Player_NET);

                }
                else if (getp_player != null)
                {
                    spawnNum = SpawnOffline(getp_player);

                }
                else
                {
                    Debug.Log("Random spawns sould be controlled add god script in here yo");
                    spawnNum = Random.RandomRange(0, spawnObjs.Count);
                }

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

            if(isCasual==true && type == ObjType.buff)
            {
                Debug.Log("Remove is casual check ");
                spawnObj.GetComponent<buff>().casual = true;
            }


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
            else if (type == ObjType.enemy)
            {
                Debug.Log("GOD VARIABLE IN HERE YO");

                CpuAi disCpu = spawnObj.GetComponent<CpuAi>();
                disCpu.target = targetPlayer;
                disCpu.eneGen = this;


                /*
                CpuAi disCpu = spawnObj.GetComponent<CpuAi>();
                 disCpu.target = targetPlayer;
                //.GetComponent<CpuAi>().target = gameStateManager.player1.gameObject;
               
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

                 */

            }

            ////








            xReturn += createdObjs[createdObjs.Count - 1].transform.position.x - this.transform.position.x + padding;
            yReturn += createdObjs[createdObjs.Count - 1].transform.position.y - this.transform.position.y + padding;
            //Debug.Log(xReturn + "xretrurn" + createdObjs[createdObjs.Count - 1].name);


        }




    }





    public int SpawnOffline(GETP_Controller player)
    {
        //Debug.Log("Remove excess code casual span and rocktoss spawn logic");
        int spawnNum = 0;

        if (type == ObjType.buff)
        {
            if (player.hp >= player.mhp - 30)
            {
                //Debug.Log(" to much hp for hp buff ");
                spawnNum = Random.RandomRange(1, spawnObjs.Count);

            }
            else if (player.hp <= player.mhp / 2)
            {
                spawnNum = 0;
            }
            else
                spawnNum = Random.RandomRange(0, spawnObjs.Count);


            while (spawnNum == 2 && player.jetPack == true)
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






    public int casualSpawnOffline(LaneShift_TopDown player)
    {
        //Debug.Log("Remove excess code casual span and rocktoss spawn logic");
        int spawnNum = 0;

            if (type == ObjType.buff)
            {
                if (player.hp >= player.mhp - 30)
                {
                    //Debug.Log(" to much hp for hp buff ");
                    spawnNum = Random.RandomRange(1, spawnObjs.Count);

                }
                else if (player.hp <= player.mhp / 2)
                {
                    spawnNum = 0;
                }
                else
                    spawnNum = Random.RandomRange(0, spawnObjs.Count);


                while (spawnNum == 2 && player.jetPack == true)
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

    public int casualSpawnOnline(LaneShift_TopDown_NET player)
    {
        Debug.Log("Remove excess code casual span and rocktoss spawn logic");

        int spawnNum = 0;

        if (type == ObjType.buff)
        {
            if (player.hp >= player.mhp - 30)
            {
                //Debug.Log(" to much hp for hp buff ");
                spawnNum = Random.RandomRange(1, spawnObjs.Count);

            }
            else if (player.hp <= player.mhp / 2)
            {
                spawnNum = 0;
            }
            else
                spawnNum = Random.RandomRange(0, spawnObjs.Count);


            while (spawnNum == 2 && player.jetPack == true)
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













    public int RockToss_SpawnOffline(RockToss_Controller player)
    {
        Debug.Log("Remove excess code casual span and rocktoss spawn logic");
        int spawnNum = 0;

        if (type == ObjType.buff)
        {
            if (player.hp >= player.mhp - 30)
            {
                //Debug.Log(" to much hp for hp buff ");
                spawnNum = Random.RandomRange(1, spawnObjs.Count);

            }
            else if (player.hp <= player.mhp / 2)
            {
                spawnNum = 0;
            }
            else
                spawnNum = Random.RandomRange(0, spawnObjs.Count);


            while (spawnNum == 2 && player.jetPack == true)
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

    public int RockToss_SpawnOnline(RockToss_Controller_NET player)
    {
        Debug.Log("Remove excess code casual span and rocktoss spawn logic");

        int spawnNum = 0;

        if (type == ObjType.buff)
        {
            if (player.hp >= player.mhp - 30)
            {
                //Debug.Log(" to much hp for hp buff ");
                spawnNum = Random.RandomRange(1, spawnObjs.Count);

            }
            else if (player.hp <= player.mhp / 2)
            {
                spawnNum = 0;
            }
            else
                spawnNum = Random.RandomRange(0, spawnObjs.Count);


            while (spawnNum == 2 && player.jetPack == true)
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

}
