using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//_DB  reality jam contest 
//projest started: 10:45 am 12/31/15

    public class controlledStageGenerator : MonoBehaviour
{
    public GameObject gameStateManager;

    public enum DrawMode { Two_D, Three_D };
    public DrawMode drawMode;

    public float xtiles, ytiles, tileSizeW,tileSizeH,tileScale;
    public int floors;

    public List<mapTile> bgMapTiles, fgMapTiles, virtualTiles = new List<mapTile>();


    public int vTilesCount,coinsCount,buffsCount,wepsCount,itemsCount,enesCount;

    public itemRelease itemReleaseScript;

    public List<GameObject> possibleTiles = new List<GameObject>();

    public GameObject parentMapObj;

    void Start()
    {
        Debug.Log("If error its because parentmap obj isnt set");
    }
	
	// Update is called once per frame
	void Update () {
	
     

    }



    public void GenerateBGMap(GameObject spawnObj)
    {
        //background generation
        float xPos = 0, yPos = 0, zPos = 0;
        float zAxis, yAxis;
        for (int x = 0; x < (xtiles * ytiles) * floors; x++)
        {

            if (xPos % xtiles == 0 && x != 0)
            {

                xPos = 0;
                yPos += 1;



                if (yPos == ytiles)
                {
                    yPos = 0;
                    zPos++;
                }
            }


            if(drawMode == DrawMode.Three_D)
            {
                yAxis = zPos;
                zAxis = yPos;
            }
            else
            {
                yAxis = yPos;
                zAxis = zPos;
            }


            Vector3 spawnPos = new Vector3(xPos * tileSizeW, tileScale * yAxis, zAxis * tileSizeH);

            GameObject tileCreated = GameObject.Instantiate(spawnObj, spawnPos, Quaternion.identity) as GameObject;

            tileCreated.transform.SetParent(parentMapObj.transform);

            tileCreated.GetComponent<mapTile>().stageGen = this;
            tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

            //tempMap=grass;
            tileCreated.GetComponent<mapTile>().initialTilePos = spawnPos;
            fgMapTiles.Add(tileCreated.GetComponent<mapTile>());

            //int vTileID = findVtile(spawnPos);

            //virtualTiles.RemoveAt(vTileID);

            xPos++;
        }


    }


    public void GenerateBorder(GameObject spawnObj)
    {

        float xPos = -1, yPos = -1, zPos = 0;
        float zAxis, yAxis;
        //border creation

        for (int x = 0; x < (xtiles+2) *( ytiles+2)*(floors+1); x++)
        {


            if (xPos == xtiles+1 && x != -1)
            {

                xPos = -1;
                yPos += 1;




                if (yPos == ytiles+1)
                {
                    yPos = -1;
                    zPos++;
                }
            }

            if (drawMode == DrawMode.Three_D)
            {
                yAxis = zPos;
                zAxis = yPos;
            }
            else
            {
                yAxis = yPos;
                zAxis = zPos;
            }

            //These if must be seperated for the way borders are drawn
            if ((xPos == -1 || xPos == xtiles ) )
            {
                //Debug.Log(xPos + " xPos " + yPos + "Y POS" + x + " X");
                GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(xPos * tileSizeW, tileScale * yAxis, zAxis * tileSizeH), Quaternion.identity) as GameObject;
                //tempMap=grass;
                tileCreated.transform.SetParent(parentMapObj.transform);

                tileCreated.GetComponent<mapTile>().initialTilePos = new Vector3(xPos * tileSizeW, tileScale * zPos, yPos * tileSizeH);
                tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

                fgMapTiles.Add(tileCreated.GetComponent<mapTile>());

            }

            else if (yPos == -1 || yPos == ytiles)
            {
               // Debug.Log(xPos + " xPos " + yPos + "Y POS" + x + " X");
                GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(xPos * tileSizeW, tileScale * yAxis, zAxis * tileSizeH), Quaternion.identity) as GameObject;
                //tempMap=grass;
                tileCreated.transform.SetParent(parentMapObj.transform);
                tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

                tileCreated.GetComponent<mapTile>().initialTilePos = new Vector3(xPos * tileSizeW, tileScale * yAxis, zAxis * tileSizeH);
                fgMapTiles.Add(tileCreated.GetComponent<mapTile>());

            }


            xPos++;
        }

    }


    public void GenerateMiddleArena(GameObject spawnObj, bool midSpawn)
    {
        //background generation
        float xPos = 0, yPos = 0, zPos = floors;
        float zAxis, yAxis;
        for (int x = 0; x < (xtiles * ytiles) ; x++)
        {

            if (xPos % xtiles == 0 && x != 0)
            {

                xPos = 0;
                yPos += 1;



                if (yPos == ytiles)
                {
                    yPos = 0;
                    zPos++;
                }
            }


            
            if (drawMode == DrawMode.Three_D)
            {
                yAxis = zPos;
                zAxis = yPos;
            }
            else
            {
                yAxis = yPos;
                zAxis = zPos;
            }

            if (xPos % 2 == 0 && yPos % 2 == 0)
            {
                GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(xPos * tileSizeW, tileScale*yAxis, zAxis * tileSizeH), Quaternion.identity) as GameObject;
                //tempMap=grass;
                tileCreated.transform.SetParent(parentMapObj.transform);

                tileCreated.GetComponent<mapTile>().initialTilePos = new Vector3(xPos * tileSizeW, tileScale*yAxis, zAxis * tileSizeH);
                fgMapTiles.Add(tileCreated.GetComponent<mapTile>());

                                tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;


            }
            else if (xPos % 2 != 0 && yPos % 2 != 0)
            {
                GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(xPos * tileSizeW, tileScale * yAxis, zAxis * tileSizeH), Quaternion.identity) as GameObject;
                //tempMap=grass;
                tileCreated.transform.SetParent(parentMapObj.transform);

                tileCreated.GetComponent<mapTile>().initialTilePos = new Vector3(xPos * tileSizeW, tileScale * yAxis, zAxis * tileSizeH);
                tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

                fgMapTiles.Add(tileCreated.GetComponent<mapTile>());

            }


            //int vTileID = findVtile(spawnPos);

            //virtualTiles.RemoveAt(vTileID);

            xPos++;
        }


    }




    //ID for keeping track of coins, buffs and weapons on stage can  erase a few or swap places stage effects
    //0=coins,1=buff,2=weapons,3=item/collectible ,4=enemy
    public void GenerateObjects(int amount, GameObject spawnObj, bool rand, string spawnPos, int itemTypeID)
    {

        int tempVTileCount = virtualTiles.Count;

        if (amount > tempVTileCount)
        {
            Debug.Log("ehh not enough tiles");
            amount = tempVTileCount;
        }

       // Debug.Log(tempVTileCount + "free tiles : " + amount);

        if (rand == false)
        {

            if (spawnPos == "bottom")
            {
                {
                    for (int x = 0; x < amount; x++)
                    {

                        int indexOf = (amount - 1) - x;
                        //Debug.Log(indexOf + "indexOF");
                        GameObject tileCreated = GameObject.Instantiate(spawnObj, virtualTiles[indexOf].initialTilePos, Quaternion.identity) as GameObject;
                        tileCreated.transform.SetParent(parentMapObj.transform);
                        tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

                        tileCreated.transform.Translate(0, -1, 0);
                        tileCreated.GetComponent<mapTile>().initialTilePos = virtualTiles[indexOf].initialTilePos;

                        fgMapTiles.Add(tileCreated.GetComponent<mapTile>());
                        virtualTiles.RemoveAt(indexOf);

                    }
                }
            }
            else if (spawnPos == "top")
            {
                for (int x = 0; x < amount; x++)
                {
                    //get the virtual tile in backwards order
                    // virtualTiles.Count -1 because arrays start at 0; ex if vtile.count is 4 we need 3
                    //  subtract x from that amount we just got each for update to get next value in pattern
                    int indexOf = (tempVTileCount - 1) - x;
                   // Debug.Log(indexOf + "indexOF");
                    GameObject tileCreated = GameObject.Instantiate(spawnObj, virtualTiles[indexOf].initialTilePos, Quaternion.identity) as GameObject;
                    tileCreated.transform.SetParent(parentMapObj.transform);
                    tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

                    tileCreated.transform.Translate(0, -1, 0);
                    tileCreated.GetComponent<mapTile>().initialTilePos = virtualTiles[indexOf].initialTilePos;
                    fgMapTiles.Add(tileCreated.GetComponent<mapTile>());
                    virtualTiles.RemoveAt(indexOf);

                }
            }

        }
        else
        {

            for (int x = 0; x < amount; x++)
            {
                int indexOf = Random.Range(0, virtualTiles.Count - 1);

                GameObject tileCreated = GameObject.Instantiate(spawnObj, virtualTiles[indexOf].initialTilePos, Quaternion.identity) as GameObject;
                tileCreated.transform.SetParent(parentMapObj.transform);
                tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

                tileCreated.transform.Translate(0, -1, 0);
                tileCreated.GetComponent<mapTile>().initialTilePos = virtualTiles[indexOf].initialTilePos;
                fgMapTiles.Add(tileCreated.GetComponent<mapTile>());
                virtualTiles.RemoveAt(indexOf);

            }
        }


        if(itemTypeID == 0)
        {
            coinsCount += amount;//add amount to coins
        }
        else if (itemTypeID == 1)
        {
            buffsCount += amount;//add amount to buffs
        }
        else if(itemTypeID == 2)
        {
            wepsCount += amount;//add amount to weps
        }

        else if(itemTypeID == 3)
        {
            enesCount += amount;//add amount to enemies
        }
        else if(itemTypeID == 4)
        {
            itemsCount += amount;//add amount to items
        }
    }












    public void deleteMap()
    {
        Debug.Log("map deleted");
        GameObject[] gos = GameObject.FindGameObjectsWithTag("mapTile");
        foreach (Transform thisTile in parentMapObj.transform)
        {
            Debug.Log("map tile deleted");

            Destroy(thisTile.gameObject);
        }

    

        fgMapTiles.Clear();
        virtualTiles.Clear();
        bgMapTiles.Clear();




    }

    public void updateFreeSpacesList()
    {
        //same setup as background tiles but include scale for virtual map
        float xPos = 0, yPos = 0, zPos = 0;
        float yAxis, zAxis;
        for (int x = 0; x < (xtiles * ytiles)*(floors+1); x++)
        {

            if (xPos % xtiles == 0 && x != 0)
            {

                xPos = 0;
                yPos += 1;

                if (yPos == ytiles)
                {
                    yPos = 0;
                    zPos++; 
                }
            }

            if (drawMode == DrawMode.Three_D)
            {
                yAxis = zPos;
                zAxis = yPos;
            }
            else
            {
                yAxis = yPos;
                zAxis = zPos;
            }


            Vector3 virtualPos = new Vector3(xPos * tileSizeW,  tileScale* yAxis, zAxis * tileSizeH);//Y and Z fliped for 2d game

            bool occupied = false;
            foreach(mapTile disTile in fgMapTiles)
            {
                if(disTile.initialTilePos == virtualPos)
                {
                    occupied = true;
                    Debug.Log("true");
                }
            }

            if (occupied == false)
            {
                virtualTiles.Add(new mapTile(virtualPos,null));
                virtualTiles[virtualTiles.Count - 1].initialTilePos = virtualPos;
               // GameObject tileCreated = GameObject.Instantiate(randomObj,virtualPos, Quaternion.identity) as GameObject;

            }
            xPos++;

        
        }

        vTilesCount = virtualTiles.Count;
    }

   
    public int findVtile(Vector3 initPos)
    {
        bool cleared=false;
        //Debug.Log("eh");
       
        int tempCount = 0;
        foreach(mapTile disTile in virtualTiles)
        {
            Debug.Log(disTile.initialTilePos + " : " + initPos);
            if (disTile.initialTilePos == initPos)
            {
                Debug.Log("found a tile");
                cleared = true;
                return tempCount;
            }
            tempCount++;
        }


        if (cleared == false)
        {
            return -1;
        }
        else
            return -2;

    }


    public void DestroyTile(mapTile disTile)
    {

        if (disTile.trackPlacement == true)
        {
            Vector3 tempPos = disTile.initialTilePos;
            virtualTiles.Add(new mapTile(tempPos, disTile.gameObject));
            Destroy(disTile.gameObject);

        }
        else
        {
            Destroy(disTile.gameObject);
        }

    }




    //Load a Premade Map Array
    public void LoadMap(List<int> preMadeMap, float columns, float rows , int levels , DrawMode mode)
    {
        //background generation
        float xPos = 0, yPos = 0, zPos = 0;
        float zAxis, yAxis;
        for (int x = 0; x < preMadeMap.Count; x++)
        {

            if (xPos % columns == 0 && x != 0)
            {

                xPos = 0;
                yPos += 1;



                if (yPos == rows)
                {
                    yPos = 0;
                    zPos++;
                }
            }


            if (mode == DrawMode.Three_D)
            {
                yAxis = zPos;
                zAxis = yPos;
            }
            else
            {
                yAxis = yPos;
                zAxis = zPos;
            }
            Vector3 spawnPos = new Vector3(xPos * tileSizeW, tileScale * yAxis, zAxis * tileSizeH);

            GameObject tileCreated = GameObject.Instantiate(possibleTiles[preMadeMap[x]], spawnPos, Quaternion.identity) as GameObject;
            //tempMap=grass;
            tileCreated.GetComponent<mapTile>().initialTilePos = spawnPos;
            fgMapTiles.Add(tileCreated.GetComponent<mapTile>());


            tileCreated.GetComponent<mapTile>().stageGen = this;
            tileCreated.GetComponent<mapTile>().gameStateManager = gameStateManager;

        
            //virtualTiles.RemoveAt(vTileID);

            xPos++;
        }


    }
































































    public void GenBattleZone(GameObject spawnObj)
    {

        float xPos = -1, yPos = -1, zPos = 0;

        //border creation

        for (int x = 0; x < (xtiles + 2) * (ytiles + 2) * floors; x++)
        {


            if (xPos == xtiles + 1 && x != -1)
            {

                xPos = -1;
                yPos += 1;

                if (yPos == ytiles + 1)//if reached limit on Z axis reset to 0 (flipped z and Y for 2d)
                { 
                yPos = -1;
                    Debug.Log("Youve accessed battleZone");
                }

                if (yPos == ytiles)
                {
                    yPos = 0;
                    zPos++;
                }
            }



            //These if must be seperated for the way borders are drawn
            if ((xPos == -1 || xPos == xtiles))
            {
                //Debug.Log(xPos + " xPos " + yPos + "Y POS" + x + " X");
                GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(xPos * tileSizeW, tileScale * zPos, yPos * tileSizeH), Quaternion.identity) as GameObject;
                //tempMap=grass;
                tileCreated.GetComponent<mapTile>().initialTilePos = new Vector3(xPos * tileSizeW, tileScale * zPos, yPos * tileSizeH);
                fgMapTiles.Add(new mapTile(new Vector3(xPos * tileSizeW, tileScale * zPos, yPos * tileSizeH), tileCreated));

            }

            else if (yPos == -1 || yPos == ytiles)
            {
                // Debug.Log(xPos + " xPos " + yPos + "Y POS" + x + " X");
                GameObject tileCreated = GameObject.Instantiate(spawnObj, new Vector3(xPos * tileSizeW, tileScale * zPos, yPos * tileSizeH), Quaternion.identity) as GameObject;
                //tempMap=grass;
                tileCreated.GetComponent<mapTile>().initialTilePos = new Vector3(xPos * tileSizeW, tileScale * zPos, yPos * tileSizeH);
                fgMapTiles.Add(new mapTile(new Vector3(xPos * tileSizeW, tileScale * zPos, yPos * tileSizeH), tileCreated));

            }


            xPos++;
        }

    }

}