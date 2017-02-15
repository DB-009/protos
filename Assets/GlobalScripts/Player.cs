using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    public controlledStageGenerator stageGen;
    public controlledGameStateManager gameStateManager;
    public controlledMatchPhaser matchPhaser;
    public controlledUIManager uiManager;

    public Camera myCamera;

    public Player myPlayer;
    public Rigidbody rb;
    public ActionKeys actionKeys;

    public float playerSpd, maxPSpd, dashDistance;
    public int coins, points, exp, expToLevel, hp, mhp, lives, kills;

    public int wins, losses, draws;

    public int rWep, lWep;

    public float bulForce;

    public int playerID, equipID;
    public enum PlayerDirection { N, S, W, E };
    public PlayerDirection dirFacing;

    public enum PlayerMode {attack, idle, dead , cast,summon, turnSequence};
    public PlayerMode player;

    public Vector3 curPos_Tile;//Current position of the tile youre on

    public float speed;

    public GameObject towerObj;

    public GameObject initialSpawnPos;

    public GameObject towerUI;
    //Arrays for multiple weapons
    public List<bool> canFire = new List<bool>();
    public List<float> reloadTime, nextShotAt, shotTime = new List<float>();

    //arrays of objects to spawn
    public GameObject[] wepSpawns;



    public List<Weapon> weaponCount = new List<Weapon>();
    //weapons list
    //gun1 (ammo count)
    //bombs(bomb count)
    //sword1(strikes counts) 1 sword=10;
    //ninja star (star count)
    //traps (trap count)

    public List<itemBuff> itemBuffCount = new List<itemBuff>();
    //item list
    //speed up 
    //double coins 
    // stronger bullets
    // shieled
    // star 
    // regen health


    public List<GameObject> clickedTroops = new List<GameObject>();

    public mapTile clickedSumTIle;

    void Awake()
    {

        rb = this.GetComponent<Rigidbody>();
        myPlayer = this;

    }

    void Update()
    {

        if (gameStateManager.gameState != controlledGameStateManager.GameState.Title && gameStateManager.gameState != controlledGameStateManager.GameState.Settings)
        {
            //Update NExt shot at for each weapon
            int temp = 0;
            foreach (bool disCanFire in canFire)
            {
                if (disCanFire == false && Time.time >= nextShotAt[temp])
                {
                    canFire[temp] = true;
                    Debug.Log("Weapon " + temp + "can now fire");
                }
                temp++;
            }

            //Check if use is pressing any action keys this frame
            actionKeys.GetActionKeys(this);



            //boundary check
            float boundX = stageGen.xtiles * stageGen.tileSizeW;
            float boundY = stageGen.ytiles * stageGen.tileSizeH;


        }
    }

    void FixedUpdate()
    {
        if (gameStateManager.gameState != controlledGameStateManager.GameState.Title && gameStateManager.gameState != controlledGameStateManager.GameState.Settings)
        {
            // Generate a plane that intersects the transform's position with an upwards normal.
            Plane playerPlane = new Plane(Vector3.up, transform.position);

            // Generate a ray from the cursor position
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

            // Determine the point where the cursor ray intersects the plane.
            // This will be the point that the object must look towards to be looking at the mouse.
            // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
            //   then find the point along that ray that meets that distance.  This will be the point
            //   to look at.
            float hitdist = 0.0f;
            // If the ray is parallel to the plane, Raycast will return false.
            if (playerPlane.Raycast(ray, out hitdist))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 targetPoint = ray.GetPoint(hitdist);

                // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }

        }
    }

    void PerformAction(string act)
    {

        GameObject spawnObj = wepSpawns[equipID];
        if (act == "shoot")
        {
            actionKeys.Shoot(spawnObj, dirFacing, bulForce, myPlayer);
        }
        if (act == "throw")
        {
            actionKeys.Throw(spawnObj, dirFacing, myPlayer);
        }
        if (act == "tower")
        {
            //actionKeys.Tower();
            actionKeys.CreateTower(myPlayer);
        }


    }





    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "enemy")
        {
            hp -= 10;

            if (hp <= 0)
            {
                //Destroy(this.gameObject);
                lives--;
                if (lives <= 0)
                {
                    //Destroy(this.gameObject);
                    if (gameStateManager.gameState == controlledGameStateManager.GameState.InMatch)
                    {
                        if (matchPhaser.multiplayer != true)
                            matchPhaser.endMatch();//temporary let battle system know we died
                        else if (playerID == 1)
                            matchPhaser.endMatch();
                        else
                            disablePlayer(false, playerID);



                    }
                }
                else
                {
                    hp = mhp;
                    this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.position = new Vector3(initialSpawnPos.transform.position.x, (stageGen.floors * stageGen.tileScale) + stageGen.tileScale, initialSpawnPos.transform.position.z);
                }
            }
        }

    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "border")
        {
            Debug.Log("hit border");

            myPlayer.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        if (col.gameObject.tag == "mapTile")
        {
            mapTile disTile = col.gameObject.GetComponent<mapTile>();
            if (disTile.disType == mapTile.TileType.floor)
            {
                //if it floor get the initial pos of this tile because we are creating tower here

                curPos_Tile = new Vector3(disTile.initialTilePos.x, disTile.initialTilePos.y, disTile.initialTilePos.z);

            }

        }


    }

    void OnTriggerEnter(Collider col)
    {

        if (col.tag == "triggerNet")
        {
            transform.position = new Vector3(initialSpawnPos.transform.position.x, (stageGen.floors * stageGen.tileScale) + stageGen.tileScale, initialSpawnPos.transform.position.z);
        }
        else if (col.tag == "mapItem")
        {
            int id = col.GetComponent<mapItem>().itemID;//get id of map item
            int type = col.GetComponent<mapItem>().itemType;//get typeid of map item 0coin,1=buff,2=wep,3=item,



            //increase item count ,run give item function

            if (type == 0)//collectible
            {
                coins++;
                stageGen.coinsCount--;
            }
            if (type == 1)//item buff
            {
                itemBuffEffect(id);
                stageGen.buffsCount--;
            }
            if (type == 2)//weapon
            {
                wepPickUp(id);
                stageGen.wepsCount--;
            }


            stageGen.DestroyTile(col.GetComponent<mapTile>());




        }

    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "triggerNet")
        {
            // transform.position = new Vector3(initialSpawnPos.transform.position.x, (stageGen.floors * stageGen.tileScale) + stageGen.tileScale, initialSpawnPos.transform.position.z);
            Debug.Log("In TriggerNet");
            transform.position = new Vector3(initialSpawnPos.transform.position.x, (stageGen.floors * stageGen.tileScale) + stageGen.tileScale, initialSpawnPos.transform.position.z);

        }

    }

    void itemBuffEffect(int id)
    {
        itemBuffCount[id].itemCount++;

        if (id == 0)//speed
        {
            if (playerSpd < maxPSpd)
            {

                playerSpd += 1;
                // Debug.Log(itemBuffCount[0].itemCount + " speed buffs");
            }

        }
        if (id == 1)//life
        {

            lives += 1;
            // Debug.Log(itemBuffCount[1].itemCount + " life buffs");


        }
    }


    void wepPickUp(int id)
    {
        if (id == 0)//gun bullets
        {
            weaponCount[0].wepCount += 100;
            //Debug.Log(weaponCount[0].wepCount + " : " + weaponCount[0].wepType);

        }
        if (id == 1)//bomb
        {
            weaponCount[1].wepCount += 1;
            //  Debug.Log(weaponCount[1].wepCount + " : " + weaponCount[1].wepType);



        }
    }


    public void resetStats()
    {

        lives = 3;
        points = 0;
        hp = mhp;

        weaponCount.Clear();
        itemBuffCount.Clear();


        weaponCount.Add(new Weapon(1000, 1));//gun1
        weaponCount.Add(new Weapon(5, 1));//bomb

        itemBuffCount.Add(new itemBuff(0, 0, 0));//speed up infinite
        itemBuffCount.Add(new itemBuff(1, 0, 0));//speed up infinite

        Debug.Log("Characters are given acces to items and weapons");

        //can fire all weapons in beginning add them
        canFire.Add(true);
        canFire.Add(true);

        //set shot time for each wep
        shotTime.Add(.5f);
        shotTime.Add(1.5f);

        //set next shot at to now so they can fire.
        nextShotAt.Add(Time.time);
        nextShotAt.Add(Time.time);
    }


    public void enablePlayer()
    {

        this.enabled = true;
        this.gameObject.gameObject.SetActive(true);
        rb.isKinematic = false;


    }

    public void disablePlayer(bool destroy, int id)
    {
        if (this.isActiveAndEnabled == true)
        {
            this.enabled = false;
            this.gameObject.gameObject.SetActive(false);
            rb.isKinematic = true;
            if (destroy == true)
            {
    
                gameStateManager.players.RemoveAt(id);
                Destroy(myCamera.gameObject);

                Destroy(this.gameObject);
            }
        }
        else
            Debug.Log("No disabling diabled players");

    }




    public void OnMouseDown()
    {
        TroopHighlight(this.gameObject);
    }




    public void TroopHighlight(GameObject disTroop)
    {
        bool add = true;
        foreach(GameObject troopCheck in clickedTroops)
        {
            if(disTroop == troopCheck)
            {
                add = false;
            }
        }


        if (add == true)
        {
            clickedTroops.Add(disTroop);
            Debug.Log("Added troop");
        }

    }



}
