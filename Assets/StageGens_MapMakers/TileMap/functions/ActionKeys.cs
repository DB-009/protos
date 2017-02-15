using UnityEngine;
using System.Collections;

public class ActionKeys : MonoBehaviour {

    public controlledStageGenerator stageGen;
    public controlledGameStateManager gameStateManager;
    public controlledMatchPhaser matchPhaser;
    public controlledUIManager uiManager;

    // public KeyCode player1_Left, player1_Right, player1_Up, player1_Down, player1_ActionKey, player1_switchWep;
    public KeyCode[] player1ButtonConfig;//player 1 keycodes

    public KeyCode[] player2ButtonConfig;

  


    public void GetActionKeys(Player disPlayer)
    {
                //Swap Weapon
        if ((Input.GetKeyDown(player1ButtonConfig[5]) && disPlayer.playerID == 0)
            ||
           (Input.GetKeyDown(player2ButtonConfig[5]) && disPlayer.playerID == 1))
          {
            Debug.Log("Swap Wep");
            SwapWeapon(disPlayer);
           }

        //Perform Action KEY
        if ((Input.GetKeyDown(player1ButtonConfig[4]) && disPlayer.playerID == 0)
            ||
           (Input.GetKeyDown(player2ButtonConfig[4]) && disPlayer.playerID == 1))
        {

            if (disPlayer.canFire[disPlayer.equipID] == true)
            {
                if (disPlayer.weaponCount[disPlayer.equipID].wepCount > 0)
                {
                    ///SHOOT
                    if (disPlayer.equipID == 0)
                        Shoot(disPlayer.wepSpawns[disPlayer.equipID], disPlayer.dirFacing, disPlayer.bulForce, disPlayer);

                    //THROW
                    if (disPlayer.equipID == 1)
                        Throw(disPlayer.wepSpawns[disPlayer.equipID], disPlayer.dirFacing, disPlayer);

                    //Reset Weapon Timers / Reduce ammo or life of weapon
                    disPlayer.nextShotAt[disPlayer.equipID] = Time.time + disPlayer.shotTime[disPlayer.equipID];
                    disPlayer.canFire[disPlayer.equipID] = false;
                    disPlayer.weaponCount[disPlayer.equipID].wepCount--;
                }
                else
                {
                    Debug.Log("no ammo to shoot wep #" + disPlayer.equipID);
                }
            }
            else
            {
               
                    Debug.Log("cant fir wep #" + disPlayer.equipID);
            }
        }

        //Tower creation
        if (((Input.GetKeyDown(player1ButtonConfig[6]) && disPlayer.playerID == 0)
            ||
           (Input.GetKeyDown(player2ButtonConfig[6]) && disPlayer.playerID == 1) )&& gameStateManager.gameState != controlledGameStateManager.GameState.MapGen)
        {
            //create a tower..
            Debug.Log("AYE");
             CreateTower(disPlayer);
        }
    }

    //Wep Swap
    public void SwapWeapon(Player disPlayer)
    {
        //ADD OTHER WEAPONS IN HERE ARRAY OF WEP AND CHECK CNT 
        //INTEL HERE

        if (disPlayer.equipID == 0)
            disPlayer.equipID = 1;
        else
            disPlayer.equipID = 0;
    }

    //Shoot
    //

    //Shoot
    //
    public void Shoot(GameObject bulz, Player.PlayerDirection dirFacing, float bulForce, Player owner)
    {
        //change to  get player angle and forward from that





        GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z), Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().myPlayer = owner.myPlayer;
        tileCreated.GetComponent<projectileLife>().playerBullet = true;


        Plane playerPlane = new Plane(Vector3.up, tileCreated.transform.position);

        // Generate a ray from the cursor position
        Ray ray = owner.myCamera.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("Shoot ammunition:" + owner.weaponCount[0].wepCount);
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
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - tileCreated.transform.position);

            // Smoothly rotate towards the target point.
            tileCreated.transform.rotation = targetRotation;
        }

        tileCreated.GetComponent<Rigidbody>().AddForce(tileCreated.transform.forward * bulForce, ForceMode.Impulse);
    }

    public void OLD_Shoot(GameObject bulz, Player.PlayerDirection dirFacing, float bulForce, Player owner)
    {
        Debug.Log("Shoot");
        if (dirFacing == Player.PlayerDirection.N)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z + 1), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, bulForce), ForceMode.Impulse);
            tileCreated.GetComponent<projectileLife>().myPlayer = owner.myPlayer;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;
        }
        if (dirFacing == Player.PlayerDirection.S)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z - 1), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -bulForce), ForceMode.Impulse);
            tileCreated.GetComponent<projectileLife>().myPlayer = owner.myPlayer;

        }

        if (dirFacing == Player.PlayerDirection.W)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(owner.transform.position.x - 1, owner.transform.position.y, owner.transform.position.z), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(-bulForce, 0, 0), ForceMode.Impulse);
            tileCreated.GetComponent<projectileLife>().myPlayer = owner.myPlayer;

        }
        if (dirFacing == Player.PlayerDirection.E)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(owner.transform.position.x + 1, owner.transform.position.y, owner.transform.position.z), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(bulForce, 0, 0), ForceMode.Impulse);
            tileCreated.GetComponent<projectileLife>().myPlayer = owner.myPlayer;

        }

    }






    //THROW
    //
    public void Throw(GameObject objct, Player.PlayerDirection dirFacing, Player owner)
    {

        Debug.Log(" throw behavior heare  ");
        if (dirFacing == Player.PlayerDirection.N)
        {
            GameObject tileCreated = GameObject.Instantiate(objct, new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z + 1), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<bombLife>().owner = owner;
        }
        if (dirFacing == Player.PlayerDirection.S)
        {
            GameObject tileCreated = GameObject.Instantiate(objct, new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z - 1), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<bombLife>().owner = owner;

        }

        if (dirFacing == Player.PlayerDirection.W)
        {
            GameObject tileCreated = GameObject.Instantiate(objct, new Vector3(owner.transform.position.x - 1, owner.transform.position.y, owner.transform.position.z), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<bombLife>().owner = owner;

        }
        if (dirFacing == Player.PlayerDirection.E)
        {
            GameObject tileCreated = GameObject.Instantiate(objct, new Vector3(owner.transform.position.x + 1, owner.transform.position.y, owner.transform.position.z), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<bombLife>().owner = owner;

        }

    }


    //
//MOVEMENT FUNCTIONS///
//

     //Get Initial Key Presses
    public void inputDown(playerMovementController pmc,Player disPlayer, Rigidbody rb)
    {
        if ((Input.GetKeyDown(player1ButtonConfig[0]) && disPlayer.playerID == 0)
            ||
           (Input.GetKeyDown(player2ButtonConfig[0]) && disPlayer.playerID == 1))
        {
            //shifting positions from south to north
            if (disPlayer.dirFacing == Player.PlayerDirection.S)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, -rb.velocity.z);
                rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, 0);

                pmc.playerJuke(Player.PlayerDirection.N);//juke north
            }
            else if (disPlayer.dirFacing == Player.PlayerDirection.N)
            {
                pmc.playerDash(Player.PlayerDirection.N);//dash north

            }
            else//this is everything else
            {
                pmc.playerDirChange(Player.PlayerDirection.N);//change direction north
            }

            pmc.dashTimer = Time.time + 1;
            pmc.impulseTimer = Time.time + 1;

            pmc.myPlayer.dirFacing = Player.PlayerDirection.N;
            pmc.fwdSpd += disPlayer.playerSpd;
        }


        if ((Input.GetKeyDown(player1ButtonConfig[2]) && disPlayer.playerID == 0)
            ||
           (Input.GetKeyDown(player2ButtonConfig[2]) && disPlayer.playerID == 1))
        {

            if (disPlayer.dirFacing == Player.PlayerDirection.N)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, -rb.velocity.z);
                rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, 0);

                pmc.playerJuke(Player.PlayerDirection.S);


            }
            else if (disPlayer.dirFacing == Player.PlayerDirection.S)
            {
                pmc.playerDash(Player.PlayerDirection.S);

            }
            else
            {
                pmc.playerDirChange(Player.PlayerDirection.S);//change direction south
            }


            pmc.dashTimer = Time.time + 1;
            pmc.impulseTimer = Time.time + 1;
            pmc.myPlayer.dirFacing = Player.PlayerDirection.S;
            pmc.fwdSpd -= disPlayer.playerSpd;
        }


        if ((Input.GetKeyDown(player1ButtonConfig[3]) && disPlayer.playerID == 0)
            ||
           (Input.GetKeyDown(player2ButtonConfig[3]) && disPlayer.playerID == 1))
        {



            if (disPlayer.dirFacing == Player.PlayerDirection.W)
            {
                rb.velocity = new Vector3(-rb.velocity.x, 0, rb.velocity.z);
                rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, 0);

                pmc.playerJuke(Player.PlayerDirection.E);


            }
            else if (disPlayer.dirFacing == Player.PlayerDirection.E)
            {
                pmc.playerDash(Player.PlayerDirection.E);

            }
            else
            {
                pmc.playerDirChange(Player.PlayerDirection.E);//change direction East

            }

            pmc.dashTimer = Time.time + 1;
            pmc.impulseTimer = Time.time + 1;
            pmc.myPlayer.dirFacing = Player.PlayerDirection.E;
            pmc.sidSpd += disPlayer.playerSpd;
        }
        if ((Input.GetKeyDown(player1ButtonConfig[1]) && disPlayer.playerID == 0)
            ||
           (Input.GetKeyDown(player2ButtonConfig[1]) && disPlayer.playerID == 1))
        {


            if (disPlayer.dirFacing == Player.PlayerDirection.E)
            {
                rb.velocity = new Vector3(-rb.velocity.x, 0, rb.velocity.z);
                rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, 0);

                pmc.playerJuke(Player.PlayerDirection.W);


            }
            else if (disPlayer.dirFacing == Player.PlayerDirection.W)
            {
                pmc.playerDash(Player.PlayerDirection.W);


            }
            else
            {
                pmc.playerDirChange(Player.PlayerDirection.W);//change direction west

            }


            pmc.myPlayer.dirFacing = Player.PlayerDirection.W;
            pmc.dashTimer = Time.time + 1;
            pmc.impulseTimer = Time.time + 1;
            pmc.sidSpd -= disPlayer.playerSpd;
        }







    }


    //HOLD KEYS
    public void inputHold(playerMovementController pmc,Player disPlayer, Rigidbody rb)
    {


        if ((Input.GetKey(player1ButtonConfig[0]) && disPlayer.playerID == 0)
            ||
           (Input.GetKey(player2ButtonConfig[0]) && disPlayer.playerID == 1))
        {

            pmc.fwdSpd += disPlayer.playerSpd;
        }
        if ((Input.GetKey(player1ButtonConfig[2]) && disPlayer.playerID == 0)
            ||
           (Input.GetKey(player2ButtonConfig[2]) && disPlayer.playerID == 1))
        {

            pmc.fwdSpd -= disPlayer.playerSpd;
        }


        if ((Input.GetKey(player1ButtonConfig[3]) && disPlayer.playerID == 0)
            ||
           (Input.GetKey(player2ButtonConfig[3]) && disPlayer.playerID == 1))
        {
            pmc.sidSpd += disPlayer.playerSpd;
        }
        if ((Input.GetKey(player1ButtonConfig[1]) && disPlayer.playerID == 0)
            ||
           (Input.GetKey(player2ButtonConfig[1]) && disPlayer.playerID == 1))
        {
            pmc.sidSpd -= disPlayer.playerSpd;
        }



    }



    //UP KEYS
    public void inputUp(Player disPlayer, Rigidbody rb)
    {

        if ((Input.GetKeyUp(player1ButtonConfig[0]) || Input.GetKeyUp(player1ButtonConfig[2]) && disPlayer.playerID == 0)
            ||
            (Input.GetKeyUp(player2ButtonConfig[0]) || Input.GetKeyUp(player2ButtonConfig[2]) && disPlayer.playerID == 1))
        {
            if (disPlayer.dirFacing == Player.PlayerDirection.E || disPlayer.dirFacing == Player.PlayerDirection.W)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, 0);
            }


        }


        if ((Input.GetKeyUp(player1ButtonConfig[3]) || Input.GetKeyUp(player1ButtonConfig[1]) && disPlayer.playerID == 0)
            ||
            (Input.GetKeyUp(player2ButtonConfig[3]) || Input.GetKeyUp(player2ButtonConfig[1]) && disPlayer.playerID == 1))
        {
            if (disPlayer.dirFacing == Player.PlayerDirection.N || disPlayer.dirFacing == Player.PlayerDirection.S)
            {
                rb.velocity = new Vector3(0, 0, rb.velocity.z);
                rb.angularVelocity = new Vector3(0, 0, rb.angularVelocity.z);
            }
        }
    }










    public void CreateTower(Player disPlayer)
    {

        Vector3 newPos = new Vector3(disPlayer.curPos_Tile.x, disPlayer.curPos_Tile.y + (stageGen.tileScale), disPlayer.curPos_Tile.z);
        int vTileID = stageGen.findVtile(newPos);



        Debug.Log(vTileID);

        if (vTileID != -1)
        {
            Debug.Log("found a tile");

            float towerScaleY = disPlayer.towerObj.GetComponent<Renderer>().bounds.size.y - stageGen.tileScale;//get the difference from tower scale to fixedTile Scale (For organized Drawing)
            towerScaleY *= .5f;//multiplying by .5f because object in unity get drawn from the center so half of one


            GameObject tileCreated = GameObject.Instantiate(disPlayer.towerObj, new Vector3(newPos.x, newPos.y + towerScaleY, newPos.z), Quaternion.identity) as GameObject;


            tileCreated.GetComponent<mapTile>().initialTilePos = stageGen.virtualTiles[vTileID].initialTilePos;


            tileCreated.GetComponent<Tower>().owner = disPlayer.gameObject;
            tileCreated.GetComponent<Tower>().towerUI = uiManager.towerUI;
            tileCreated.GetComponent<Tower>().stageGen = disPlayer.stageGen;
            tileCreated.GetComponent<mapTile>().gameStateManager = disPlayer.gameStateManager.gameObject;


            stageGen.fgMapTiles.Add(new mapTile(stageGen.virtualTiles[vTileID].initialTilePos, tileCreated));



            stageGen.virtualTiles.RemoveAt(vTileID);
        }
        else
        {
            Debug.Log("no tile");
        }
    }



    ////BACKWARDS SHOT


    /*
    public void BackwardShot(GameObject bulz, Player.PlayerDirection dirFacing, float bulForce,Player owner)
    {

        if (dirFacing == Player.PlayerDirection.S)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, bulForce), ForceMode.Impulse);
            tileCreated.GetComponent<bulletLife>().myPlayer = owner;

        }
        if (dirFacing == Player.PlayerDirection.N)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -bulForce), ForceMode.Impulse);
            tileCreated.GetComponent<bulletLife>().myPlayer = owner;

        }

        if (dirFacing == Player.PlayerDirection.E)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(-bulForce, 0, 0), ForceMode.Impulse);
            tileCreated.GetComponent<bulletLife>().myPlayer = owner;

        }
        if (dirFacing == Player.PlayerDirection.W)
        {
            GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
            tileCreated.GetComponent<Rigidbody>().AddForce(new Vector3(bulForce, 0, 0), ForceMode.Impulse);
            tileCreated.GetComponent<bulletLife>().myPlayer = owner;

        }
    }
    */
}
