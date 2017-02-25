using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PathGen_Bug : MonoBehaviour {

    public Vector3 initPos;
    public int moveDis;
    public bool left, right, up, down;

    public int pathsLimit,pathsDrawn;
    public bool randomPaths,drawDelay;
    public float drawDelayTime;
    public List<int> pathToFollow = new List<int>();


    public enum GenMode { path, room, etc};//controls objects for future usage
    public GenMode genMode;

    public enum LastDir { left, right, down, up , none };//last direction bug moved
    public LastDir lastDir;

    public bool generateRooms,wasRoomCreated;

    public List<GameObject> pathObjs,pathPoints =  new List<GameObject>();

    public bool canMove, createPath;
    public GameObject updateTarg;
    public float upCheck, horCheck, downCheck;
    // Use this for initialization
    void Awake () {
        lastDir = LastDir.none;
        initPos = this.transform.position;
        if(drawDelay == true)
        {
            InvokeRepeating("ControlledMove", .5f, drawDelayTime);

        }
        else
        {
            InvokeRepeating("ControlledMove", .5f, .1f);

        }

        /* infinity symbol (for infinite loop test XD)

        ControlledMove(1);//left
        ControlledMove(2);//right (checking movement info) cant go left to right this is debug error check
        ControlledMove(3);//up
        ControlledMove(2);//rght
        ControlledMove(4);//down
        ControlledMove(4);//down
        ControlledMove(2);//rght
        ControlledMove(3);//up 
        ControlledMove(1);//left
        */


    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Move()
    {



        bool moved = false;
        while(moved == false)
        {
            int dir = Random.Range(1, 5);

            if (wasRoomCreated == true)
            {
                Debug.Log("moved character to where room exit should be made because this is where path is exitting from, ideally we should increase y but exit can be placed anywhere ? randomRange");
                if (dir == 1 && left == true)
                {
                    //transform.position = pathPoints[pathPoints.Count-1].transform.position - new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);
                }
                else if (dir == 2 && right == true)
                {
                   // transform.position = pathPoints[pathPoints.Count - 1].transform.position + new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);
                }
                else if (dir == 3 && up == true)
                {
                   // transform.position = pathPoints[pathPoints.Count - 1].transform.position + new Vector3(0,(pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);
                }
                else if (dir == 4 && down == true)
                {
                   // transform.position = pathPoints[pathPoints.Count - 1].transform.position - new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);
                }
            }




            if (dir==1 && left == true)
            {
                HorColCheck(dir,false,this.gameObject);
                if (canMove == true)
                {
                    
                  for(int n = 0; n < moveDis;n++)
                    {
                        transform.position = this.transform.position + new Vector3(-moveDis, 0, 0);

                        GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(+0, 0, 0), Quaternion.identity) as GameObject;

                        pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;


                        if (lastDir != LastDir.none && lastDir != LastDir.left && n==0)
                        {
                            if (lastDir == LastDir.up)
                            {
                                pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upLTurn;

                            }
                            else if (lastDir == LastDir.down)
                            {
                                pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnLTurn;
                            }
                        }

                        pathPoints.Add(pathy);
                    }

                    moved = true;
                    // GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(+0, 0, 0), Quaternion.identity) as GameObject;
                    //GameObject pathy2 = Instantiate(pathObjs[0], transform.position + new Vector3(+1, 0, 0), Quaternion.identity) as GameObject;
                    // GameObject pathy3 = Instantiate(pathObjs[0], transform.position + new Vector3(+2, 0, 0), Quaternion.identity) as GameObject;
                    // GameObject pathy4 = Instantiate(pathObjs[0], transform.position + new Vector3(+3, 0, 0), Quaternion.identity) as GameObject;
                    //  GameObject pathy5 = Instantiate(pathObjs[0], transform.position + new Vector3(+4, 0, 0), Quaternion.identity) as GameObject;



                    //  pathy2.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                    //  pathy3.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                    //  pathy4.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                    //  pathy5.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;

                    // pathPoints.Add(pathy5);
                    // pathPoints.Add(pathy4);
                    // pathPoints.Add(pathy3);
                    // pathPoints.Add(pathy2);
                    // pathPoints.Add(pathy);



                    lastDir = LastDir.left;


                    // VerColCheck(dir, true,pathy5);
                    // VerColCheck(dir, true, pathy);

                }

                else
                {
                    Debug.Log("cant move");
                }


            }
            else if (dir == 2 && right == true)
            {
                HorColCheck(dir,false,this.gameObject);
                if (canMove == true)
                {
                    moved = true;
                    transform.position = this.transform.position + new Vector3(+moveDis, 0, 0);
                    GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(-0, 0, 0), Quaternion.identity) as GameObject;
                    GameObject pathy2 = Instantiate(pathObjs[0], transform.position + new Vector3(-1, 0, 0), Quaternion.identity) as GameObject;
                    GameObject pathy3 = Instantiate(pathObjs[0], transform.position + new Vector3(-2, 0, 0), Quaternion.identity) as GameObject;
                    GameObject pathy4 = Instantiate(pathObjs[0], transform.position + new Vector3(-3, 0, 0), Quaternion.identity) as GameObject;
                    GameObject pathy5 = Instantiate(pathObjs[0], transform.position + new Vector3(-4, 0, 0), Quaternion.identity) as GameObject;

                    pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                    pathy2.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                    pathy3.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                    pathy4.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                    pathy5.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;


                    pathPoints.Add(pathy5);
                    pathPoints.Add(pathy4);
                    pathPoints.Add(pathy3);
                    pathPoints.Add(pathy2);
                    pathPoints.Add(pathy);

                    if(lastDir != LastDir.none && lastDir != LastDir.right)
                    {
                        if(lastDir == LastDir.up)
                        {
                            pathy.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upRTurn;

                        }
                        else if(lastDir == LastDir.down)
                        {
                            pathy.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnRTurn;
                        }
                    }

                    lastDir = LastDir.right;


                   // VerColCheck(dir, true,pathy5);
                   // VerColCheck(dir, true, pathy);


                }
                else
                {
                    Debug.Log("cant move");
                }
               

            }
            else if (dir == 3 && up == true)
            {
                VerColCheck(dir,false,this.gameObject);
                if (canMove == true)
                {
                    moved = true;
                    transform.position = this.transform.position + new Vector3(0, +moveDis, 0);
                    GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(0, -0, 0), Quaternion.identity) as GameObject;
                    GameObject pathy2 = Instantiate(pathObjs[0], transform.position + new Vector3(0, -1, 0), Quaternion.identity) as GameObject;
                    GameObject pathy3 = Instantiate(pathObjs[0], transform.position + new Vector3(0, -2, 0), Quaternion.identity) as GameObject;
                    GameObject pathy4 = Instantiate(pathObjs[0], transform.position + new Vector3(0, -3, 0), Quaternion.identity) as GameObject;
                    GameObject pathy5 = Instantiate(pathObjs[0], transform.position + new Vector3(0, -4, 0), Quaternion.identity) as GameObject;


                    pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy2.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy3.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy4.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy5.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;


                    if (lastDir != LastDir.none && lastDir != LastDir.up)
                    {
                        if (lastDir == LastDir.left)
                        {
                            pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upLTurn;

                        }
                        else if (lastDir == LastDir.right)
                        {
                            pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upRTurn;
                        }
                    }

                    pathPoints.Add(pathy5);
                    pathPoints.Add(pathy4);
                    pathPoints.Add(pathy3);
                    pathPoints.Add(pathy2);
                    pathPoints.Add(pathy);

                    lastDir = LastDir.up;


                    //HorColCheck(dir, true,pathy5);
                    // HorColCheck(dir, true, pathy);


                }
                else
                {
                    Debug.Log("cant move");
                }



            }
            else if (dir == 4 && down == true)
            {

                VerColCheck(dir,false,this.gameObject);
                if (canMove == true)
                {
                    moved = true;
                    transform.position = this.transform.position + new Vector3(0, -moveDis, 0);


                    GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(0, +0, 0), Quaternion.identity) as GameObject;
                    GameObject pathy2 = Instantiate(pathObjs[0], transform.position + new Vector3(0, +1, 0), Quaternion.identity) as GameObject;
                    GameObject pathy3 = Instantiate(pathObjs[0], transform.position + new Vector3(0, +2, 0), Quaternion.identity) as GameObject;
                    GameObject pathy4 = Instantiate(pathObjs[0], transform.position + new Vector3(0, +3, 0), Quaternion.identity) as GameObject;
                    GameObject pathy5 = Instantiate(pathObjs[0], transform.position + new Vector3(0, +4, 0), Quaternion.identity) as GameObject;

                    pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy2.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy3.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy4.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                    pathy5.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;


                    pathPoints.Add(pathy5);
                    pathPoints.Add(pathy4);
                    pathPoints.Add(pathy3);
                    pathPoints.Add(pathy2);
                    pathPoints.Add(pathy);


                    if (lastDir != LastDir.none && lastDir != LastDir.down)
                    {
                        if (lastDir == LastDir.left)
                        {
                            pathy.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnLTurn;

                        }
                        else if (lastDir == LastDir.right)
                        {
                            pathy.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnRTurn;
                        }
                    }

                    lastDir = LastDir.down;


                    // HorColCheck(dir, true,pathy5);
                    // HorColCheck(dir, true, pathy);


                }
                else
                {
                    Debug.Log("cant move");
                }
              
            }

            //generate a room
            if (generateRooms == true && moved== true)
            {
                Vector3 spawnPos = pathPoints[pathPoints.Count-1].transform.position;

                if (dir == 1)
                {
                    spawnPos = spawnPos - new Vector3((pathObjs[0].GetComponent<Renderer>().bounds.size.x*.5f), 0, 0);//deduct half of path created  cus left creation
                    spawnPos = spawnPos - new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);//deduct half of room created  cus left creation
                }
                else if (dir == 2)
                {
                    spawnPos = spawnPos + new Vector3((pathObjs[0].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);//add half of path created  cus right creation
                    spawnPos = spawnPos + new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);//add half of room created  cus right creation
                }
                else if (dir == 3)
                {
                    spawnPos = spawnPos + new Vector3(0, (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);//add half of path created  cus up creation
                    spawnPos = spawnPos + new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f), 0);//add half of room created  cus up creation
                }
                else if (dir == 4)
                {
                    spawnPos = spawnPos - new Vector3(0,(pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);//deduct half of path created  cus down creation
                    spawnPos = spawnPos - new Vector3(0,(pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f), 0);//deduct half of room created  cus down creation
                }

                GameObject room = Instantiate(pathObjs[1], spawnPos , Quaternion.identity) as GameObject;
                room.GetComponent<pathPoint>().type = pathPoint.ObjType.room;
                wasRoomCreated = true;
              
            }

        }


    }




    //Raycast target checks

    private void HorColCheck(int dir, bool move, GameObject targ)
    {
        canMove = true;
        RaycastHit hitL, hitR;
        bool isLeft = Physics.Raycast(targ.transform.position, -targ.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(targ.transform.position, targ.transform.right, out hitR, horCheck);
        Debug.DrawRay(targ.transform.position, -targ.transform.right, Color.green);
        Debug.DrawRay(targ.transform.position, targ.transform.right, Color.green);

        if (isRight == true)
        {
            if (dir == 2)//moving right and hit detected
            {
              //  Debug.Log("There is something right to me! " + hitR.transform.gameObject.name);
                if (hitR.transform.gameObject.GetComponent<pathPoint>().type == pathPoint.ObjType.path)
                {
                    if (hitR.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.hor)
                    {
                        canMove = false;
                       Debug.Log("Cant move " + this.gameObject.name +" bug path is blocking owner is" + hitR.transform.gameObject.GetComponent<pathPoint>().owner.name);
                    }
                    else if (hitR.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.ver)
                    {
                        if (pathsDrawn < pathsLimit - 1)
                        {
                            Debug.Log("ran into special path4 " + hitR.transform.gameObject.GetComponent<pathPoint>().dir);
                            hitR.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                            Debug.Log("made a 4 way");
                        }
                        else
                        {
                            canMove = false;
                            hitR.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                            Debug.Log("made a 3 way on last move");
                        }

                    }
                    else if (hitR.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor && hitR.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                    {
                        Debug.Log("ran into special path1 " + hitR.transform.gameObject.GetComponent<pathPoint>().dir);
                        createPath = false;
                        hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                        Debug.Log("made a 4 way");
                    }
                }
                
            }
            else if (dir == 3 || dir == 4)
            {
                if (hitR.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.hor)
                {
                    pathPoints[pathPoints.Count - 1].transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                    Debug.Log("i hit a hor tile while moving ver, 3 way made");
                }
            }


        }

        if (isLeft == true)
        {
            Debug.Log("is left");
            if (dir == 1)//moving left
            {
               // Debug.Log("There is something left to me!" + hitL.transform.gameObject.name);
                if (hitL.transform.gameObject.GetComponent<pathPoint>().type == pathPoint.ObjType.path)
                {
                    if (hitL.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.hor)
                    {
                        canMove = false;
                       Debug.Log("Cant move " + this.gameObject.name +" bug path is blocking owner is" + hitL.transform.gameObject.GetComponent<pathPoint>().owner.name);
                    }
                    else if(hitL.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.ver)
                    {
                        createPath = false;
                         if (pathsDrawn < pathsLimit - 1)
                        {
                            Debug.Log("ran into special path4 " + hitL.transform.gameObject.GetComponent<pathPoint>().dir);
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                            Debug.Log("made a 4 way");
                        }
                        else
                        {
                            canMove = false;
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                            Debug.Log("made a 3 way on last move");
                        }

                    }
                    else if(hitL.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor && hitL.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                    {
                        createPath = false;

                        if (pathsDrawn < pathsLimit - 1)
                        {
                            Debug.Log("ran into special path4 " + hitL.transform.gameObject.GetComponent<pathPoint>().dir);
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                            Debug.Log("made a 4 way");
                        }
                        else
                        {
                            canMove = false;
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                            Debug.Log("made a 3 way on last move");
                        }
                    }

                }
            }
            else if (dir == 3 || dir == 4)
            {
                if (hitL.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.hor)
                {
                    pathPoints[pathPoints.Count - 1].transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                    Debug.Log("i hit a hor tile while moving ver, 3 way made");
                }
            }


        }

    }





    private void VerColCheck(int dir,bool move, GameObject targ)
    {
        canMove = true;
        RaycastHit hitL, hitR;
        bool isDown = Physics.Raycast(targ.transform.position, -targ.transform.up, out hitL, horCheck);
        bool isUp = Physics.Raycast(targ.transform.position, targ.transform.up, out hitR, horCheck);
        Debug.DrawRay(targ.transform.position, -targ.transform.up, Color.green);
        Debug.DrawRay(targ.transform.position, targ.transform.up, Color.green);

        if (isUp == true )
        {
            if(dir==3)
            {
               // Debug.Log("There is something above to me! " + hitR.transform.gameObject.name);
                if (hitR.transform.gameObject.GetComponent<pathPoint>().type == pathPoint.ObjType.path)
                {
                    if (hitR.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.ver)
                    {
                        canMove = false;
                        Debug.Log("Cant move " + this.gameObject.name +" bug path is blocking owner is" + hitR.transform.gameObject.GetComponent<pathPoint>().owner.name);  
                    }
                    else if (hitR.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.hor)
                    {
                        createPath = false;
                        if (pathsDrawn < pathsLimit - 1)
                        {
                            Debug.Log("ran into special path4 " + hitR.transform.gameObject.GetComponent<pathPoint>().dir);
                            hitR.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                            Debug.Log("made a 4 way");
                        }
                        else
                        {
                            canMove = false;
                            hitR.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                            Debug.Log("made a 3 way on last move");
                        }


                    }
                    else if (hitR.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor && hitR.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                    {
                        createPath = false;

                        if (pathsDrawn < pathsLimit - 1)
                        {
                            Debug.Log("ran into special path4 " + hitR.transform.gameObject.GetComponent<pathPoint>().dir);
                            hitR.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                            Debug.Log("made a 4 way");
                        }
                        else
                        {
                            canMove = false;
                            hitR.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                            Debug.Log("made a 3 way on last move");
                        }
                    }
                }

            }
            else if(dir == 1 || dir == 2)
            {
                if (hitR.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.ver)
                {
                    pathPoints[pathPoints.Count-1].transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                    Debug.Log("i hit a ver tile while moving hor, 3 way made");
                }
            }

        }

         if (isDown == true )
        {
            if (dir == 4)
            {
               // Debug.Log("There is something below to me!" + hitL.transform.gameObject.name);
                if (hitL.transform.gameObject.GetComponent<pathPoint>().type == pathPoint.ObjType.path)
                {
                    if (hitL.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.ver)
                    {
                        canMove = false;
                        Debug.Log("Cant move " + this.gameObject.name + " bug path is blocking owner is" + hitL.transform.gameObject.GetComponent<pathPoint>().owner.name);
                    }
                    else if (hitL.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.hor)
                    {
                        createPath = false;
                     

                        if (pathsDrawn < pathsLimit - 1)
                        {
                            Debug.Log("ran into special path4 " + hitL.transform.gameObject.GetComponent<pathPoint>().dir);
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                            Debug.Log("made a 4 way");
                        }
                        else
                        {
                            canMove = false;
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                            Debug.Log("made a 3 way on last move");
                        }
                    }
                    else if (hitL.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor && hitL.transform.gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                    { 
                        createPath = false;
                        if (pathsDrawn < pathsLimit - 1)
                        {
                            Debug.Log("ran into special path4 " + hitL.transform.gameObject.GetComponent<pathPoint>().dir);
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.fourWay;
                            Debug.Log("made a 4 way");
                        }
                        else
                        {
                            canMove = false;
                            hitL.transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                            Debug.Log("made a 3 way on last move");
                        }
              


                    }
                }
            }
            else if (dir == 1 || dir == 2)
            {
                if (hitL.transform.gameObject.GetComponent<pathPoint>().dir == pathPoint.PathDir.ver)
                {
                    pathPoints[pathPoints.Count - 1].transform.gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.threeWay;

                    Debug.Log("i hit a ver tile while moving hor, 3 way made");
                }
            }

        }


    }





    //Controlled movement via single integers update to a list of int's


    public void ControlledMove()
    {
        int dir = 0;//initialize 0 = error
        createPath = true;//we should create a path always unless raycast check says otherwise
        if(pathsDrawn != pathsLimit)
        {
            if (randomPaths == true)
            {
                dir = Random.Range(1, 5);

            }
            else
            {
                dir = pathToFollow[pathsDrawn];
            }
        }
        else
        {
            CancelInvoke("ControlledMove");
            Debug.Log("movement ended");
        }


        bool moved = false;

            if (wasRoomCreated == true)
            {
                Debug.Log("moved character to where room exit should be made because this is where path is exitting from, ideally we should increase y but exit can be placed anywhere ? randomRange");
                if (dir == 1 && left == true)
                {
                if(lastDir != LastDir.left)
                {
                    //initial movement to continue path 
                    transform.position = this.transform.position - new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f) - (pathObjs[0].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);
                   if (lastDir == LastDir.up)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position + new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);

                    }
                    else if (lastDir == LastDir.down)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position - new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);

                    }
                }
                else//if moving in same direction as last time move full size of room
                    transform.position = this.transform.position - new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);
            }
            else if (dir == 2 && right == true)
                {
                if (lastDir != LastDir.right)
                {
                    //initial movement to continue path 
                    transform.position = this.transform.position + new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f) - (pathObjs[0].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);
                    if (lastDir == LastDir.up)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position + new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);

                    }
                    else if (lastDir == LastDir.down)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position - new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);

                    }
                }
                else
                    transform.position = this.transform.position + new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x) , 0, 0);
            }
            else if (dir == 3 && up == true)
                {
                if (lastDir != LastDir.up)
                {
                    //initial movement to continue path 
                    transform.position = this.transform.position + new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) - (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);
                    if (lastDir == LastDir.left)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position - new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0, 0);

                    }
                    else if ( lastDir == LastDir.right)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position + new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f)+ (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0, 0);

                    }
                }
                else
                    transform.position = this.transform.position + new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y) , 0);

            }
            else if (dir == 4 && down == true)
                {
                if (lastDir != LastDir.down)
                {
                    //initial movement to continue path 
                    transform.position = this.transform.position - new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f) - (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);
                    if (lastDir == LastDir.left)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position - new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0, 0);

                    }
                    else if (lastDir == LastDir.right)//if shifting from vertical to horizontal path move player to center of room
                    {
                        transform.position = this.transform.position + new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f) + (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0, 0);

                    }
                }
                else
                    transform.position = this.transform.position - new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y) , 0);

            }

            wasRoomCreated = false;
            }



       if(dir !=0)
        {
            if (dir == 1 && left == true)
            {
                HorColCheck(dir, false, this.gameObject);
                if (canMove == true)
                {


                    for (int n = 0; n < moveDis; n++)
                    {

                        if (n != 0)
                            HorColCheck(dir, false, this.gameObject);

                        if (canMove == true)
                        {

                            if(createPath == true)
                            {
                                transform.position = this.transform.position + new Vector3(-1, 0, 0);

                                GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(+0, 0, 0), Quaternion.identity) as GameObject;

                                pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;

                                pathy.GetComponent<pathPoint>().owner = this.gameObject;//set owner bug to path


                                if (lastDir != LastDir.none && lastDir != LastDir.left && n == 0)
                                {
                                    if (lastDir == LastDir.up)
                                    {
                                       
                                        if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                             pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upLTurn;
                                        Debug.Log("moving left up l turn");
                                        }

                                    }
                                    else if (lastDir == LastDir.down)
                                    {

                                        if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                             pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnLTurn;
                                        Debug.Log("moving left down  l turn");
                                        }

                                       

                                    }
                                }

                                pathPoints.Add(pathy);
                            }
                            else
                            {
                             
                                transform.position = this.transform.position + new Vector3(-1, 0, 0);

                            }
                        }



                    }


                    lastDir = LastDir.left;
                    pathsDrawn++;
                    moved = true;

                     VerColCheck(dir, true,pathPoints[pathPoints.Count-1]);
                    // VerColCheck(dir, true, pathy);

                }


            }
            else if (dir == 2 && right == true)
            {
                HorColCheck(dir, false, this.gameObject);
                if (canMove == true)
                {

                    for (int n = 0; n < moveDis; n++)
                    {
                        if (n != 0)
                            HorColCheck(dir, false, this.gameObject);

                        if (canMove == true)
                        {

                            if (createPath == true)
                            {

                            transform.position = this.transform.position + new Vector3(+1, 0, 0);
                            GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(+0, 0, 0), Quaternion.identity) as GameObject;

                            pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.hor;
                            pathy.GetComponent<pathPoint>().owner = this.gameObject;//set owner bug to path

                            if (lastDir != LastDir.none && lastDir != LastDir.right && n == 0)
                            {
                                if (lastDir == LastDir.up)
                                {

                                        if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                          
                                        pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upRTurn;
                                    Debug.Log("moving right up R turn");

                                        }

                                        

                                }
                                else if (lastDir == LastDir.down)
                                {

                                        if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                           
                                        pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnRTurn;
                                    Debug.Log("moving right  down r turn");

                                        }

                                        

                                }

                            }


                            pathPoints.Add(pathy);
                            }
                            else
                            {
                         
                                transform.position = this.transform.position + new Vector3(+1, 0, 0);

                            }


                        }

                    }




                    lastDir = LastDir.right;
                    pathsDrawn++;
                    moved = true;


                    VerColCheck(dir, true, pathPoints[pathPoints.Count - 1]);
                    // VerColCheck(dir, true, pathy);


                }


            }
            else if (dir == 3 && up == true)
            {
                VerColCheck(dir, false, this.gameObject);
                if (canMove == true)
                {
                    for (int n = 0; n < moveDis; n++)
                    {

                        if (n != 0)
                            VerColCheck(dir, false, this.gameObject);

                        if (canMove == true)
                        {

                            if (createPath == true)
                            {

                            transform.position = this.transform.position + new Vector3(0, +1, 0);

                            GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(+0, 0, 0), Quaternion.identity) as GameObject;

                            pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                            pathy.GetComponent<pathPoint>().owner = this.gameObject;//set owner bug to path

                            if (lastDir != LastDir.none && lastDir != LastDir.up && n == 0)
                            {
                                if (lastDir == LastDir.left)
                                {
                                        if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                           
                                            pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upLTurn;
                                    Debug.Log("moving  up , up left turn");
                                        }

                                    

                                }
                                else if (lastDir == LastDir.right)
                                {
                                        if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                         
                                            pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.upRTurn;
                                    Debug.Log("moving up, up r turn");
                                        }
                                        
                                }
                            }



                            pathPoints.Add(pathy);
                            }
                            else
                            {
                            
                                transform.position = this.transform.position + new Vector3(0, +1, 0);

                            }


                        }


                    }
                    HorColCheck(dir, true, pathPoints[pathPoints.Count - 1]);

                    lastDir = LastDir.up;
                    pathsDrawn++;
                    moved = true;

                }


            }
            else if (dir == 4 && down == true)
            {

                VerColCheck(dir, false, this.gameObject);
                if (canMove == true)
                {
                    for (int n = 0; n < moveDis; n++)
                    {

                        if (n != 0)
                            VerColCheck(dir, false, this.gameObject);

                        if (canMove == true)
                        {

                            if (createPath == true)
                            {
                            transform.position = this.transform.position + new Vector3(0, -1, 0);

                            GameObject pathy = Instantiate(pathObjs[0], transform.position + new Vector3(+0, 0, 0), Quaternion.identity) as GameObject;

                            pathy.GetComponent<pathPoint>().dir = pathPoint.PathDir.ver;
                            pathy.GetComponent<pathPoint>().owner = this.gameObject;//set owner bug to path


                            if (lastDir != LastDir.none && lastDir != LastDir.down && n == 0)
                            {
                                if (lastDir == LastDir.left)
                                {
                                   if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                            pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnLTurn;
                                            Debug.Log("moving down , down l turn");
                                        }
                          

                                }
                                else if (lastDir == LastDir.right)
                                {
                                        if (pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.hor || pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir != pathPoint.PathDir.ver)
                                        {
                                            pathPoints[pathPoints.Count - 1].gameObject.GetComponent<pathPoint>().dir = pathPoint.PathDir.dwnRTurn;
                                    Debug.Log("moving down, down r turn");
                                        }
                                  

                                }


                            }


                            pathPoints.Add(pathy);
                            }
                            else
                            {

                                transform.position = this.transform.position + new Vector3(0, -1, 0);

                            }


                        }


                    }
                    HorColCheck(dir, true, pathPoints[pathPoints.Count - 1]);

                    lastDir = LastDir.down;
                    pathsDrawn++;
                    moved = true;


                }


            }

        }

        //generate a room
        if (generateRooms == true && moved == true)
            {
                Vector3 spawnPos = this.transform.position;

                if (dir == 1)
                {
                    spawnPos = spawnPos - new Vector3((pathObjs[0].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);//deduct half of path created  cus left creation
                    spawnPos = spawnPos - new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);//deduct half of room created  cus left creation
                }
                else if (dir == 2)
                {
                    spawnPos = spawnPos + new Vector3((pathObjs[0].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);//add half of path created  cus right creation
                    spawnPos = spawnPos + new Vector3((pathObjs[1].GetComponent<Renderer>().bounds.size.x * .5f), 0, 0);//add half of room created  cus right creation
                }
                else if (dir == 3)
                {
                    spawnPos = spawnPos + new Vector3(0, (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);//add half of path created  cus up creation
                    spawnPos = spawnPos + new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f), 0);//add half of room created  cus up creation
                }
                else if (dir == 4)
                {
                    spawnPos = spawnPos - new Vector3(0, (pathObjs[0].GetComponent<Renderer>().bounds.size.y * .5f), 0);//deduct half of path created  cus down creation
                    spawnPos = spawnPos - new Vector3(0, (pathObjs[1].GetComponent<Renderer>().bounds.size.y * .5f), 0);//deduct half of room created  cus down creation
                }

                GameObject room = Instantiate(pathObjs[1], spawnPos, Quaternion.identity) as GameObject;
                room.GetComponent<pathPoint>().type = pathPoint.ObjType.room;
                wasRoomCreated = true;

            }

        


    }

}
