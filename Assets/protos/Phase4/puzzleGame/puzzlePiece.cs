using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
public class puzzlePiece : MonoBehaviour {

    public bool clickable;

    public int id;
    public puzzleMatch matchManager;
    public puzzleSpawner puzzleGen;

    public enum Type { piece, buff, extra};
    public Type type;

    public float initXPos, initYPos, xPos, yPos;
    public Vector3 initPos, curPos;

    public string extraString;
    public int extraInt,dropped;

    public List<puzzlePiece> clusterPieces = new List<puzzlePiece>();//list of cluster pieces resets on destroy.


    public puzzlePiece[] dropZones;//build a list of coordinates to drop tile from to drop after destroy
    public Vector3 targPos;

    public List<puzzlePiece> tempPieces = new List<puzzlePiece>();
    public List<puzzlePiece> sorted = new List<puzzlePiece>();
    // Use this for initialization
    void Awake()
    {

        if (type == Type.piece)
        {
            if (extraInt == 0)
            {
                extraString = "red";
            }
            else if (extraInt == 1)
            {
                extraString = "blue";
            }
            else if (extraInt == 2)
            {
                extraString = "green";
            }
            else if (extraInt == 3)
            {
                extraString = "grey";
            }
        }







    }

    // Update is called once per frame
    void Update () {
	
	}


    void OnMouseDown()
    {
        Debug.Log("if in puzzle match change to lotteryGame");
        if(clickable == true)
        {
            clusterPieces.Clear();
            if (type == Type.piece)
            {
                Debug.Log("clicked " + extraString + " piece");

                clusterPieces.Add(this);
                string searchColor = extraString;

                for (int temp = 0; temp <= clusterPieces.Count - 1; temp++)
                {



                    float tempx = clusterPieces[temp].initXPos, tempy = clusterPieces[temp].initYPos;


                    Vector3 clickedPos = clusterPieces[temp].initPos;
                    //Cluster check

                    bool foundMatching = false;


                    foreach (GameObject disPiece in puzzleGen.createdPieces)
                    {
                        foundMatching = false;
                        puzzlePiece piece = disPiece.GetComponent<puzzlePiece>();


                        //check left and right
                        if (piece.initPos == new Vector3(clickedPos.x - matchManager.tileSizeX, clickedPos.y, clickedPos.z))
                        {
                            if (piece.extraString == searchColor)
                            {
                                Debug.Log("Found left Piece type is " + piece.extraString);
                                foundMatching = true;
                            }
                        }
                        else if (piece.initPos == new Vector3(clickedPos.x + matchManager.tileSizeX, clickedPos.y, clickedPos.z))
                        {
                            if (piece.extraString == searchColor)
                            {
                                Debug.Log("Found right Piece type is " + piece.extraString);
                                foundMatching = true;

                            }


                        }

                        //up and down
                        if (piece.initPos == new Vector3(clickedPos.x, clickedPos.y - matchManager.tileSizeY, clickedPos.z))
                        {
                            if (piece.extraString == searchColor)
                            {
                                Debug.Log("Found down Piece type is " + piece.extraString);
                                foundMatching = true;

                            }


                        }
                        else if (piece.initPos == new Vector3(clickedPos.x, clickedPos.y + matchManager.tileSizeY, clickedPos.z))
                        {
                            if (piece.extraString == searchColor)
                            {

                                foundMatching = true;

                            }


                        }

                        //Is this piece in cluster already (Cluster check)
                        if (foundMatching == true)
                        {
                            bool inCluster = false;

                            //is this piece already in cluster
                            foreach (puzzlePiece clusterPiece in clusterPieces)
                            {
                                if (clusterPiece.initPos == piece.initPos)
                                {
                                    inCluster = true;
                                    Debug.Log("piece was in cluster");
                                }
                            }

                            if (inCluster != true)
                            {

                                clusterPieces.Add(piece);
                                Debug.Log("piece added to cluster " + clusterPieces.Count);
                            }
                        }



                    }
            }














                //Remove the cluster pieces from the puzzle because they will get destroeyd pay attention where this is
                foreach(puzzlePiece disPiece in clusterPieces)
                {
                    //Remove at is better because object pooling will cause errors
                    puzzleGen.createdPieces.Remove(disPiece.gameObject);
                }



                //build a list of coordinates to drop tile from to drop after destroy

                dropZones = new puzzlePiece[clusterPieces.Count];

                float[] unsortedIDs = new float[clusterPieces.Count];


                for (int temp = 0; temp <= clusterPieces.Count - 1; temp++)
                {
                    dropZones[temp] = clusterPieces[temp];
                    unsortedIDs[temp] = clusterPieces[temp].initPos.y;
                }

                //LINQ sort array by puzzle piece init pos . x values
                //dropZones.OrderBy(puzzlePiece => puzzlePiece.initPos.y).ToArray();


                
                Array.Sort(unsortedIDs);

               // unsortedIDs.Reverse();//Reverse order to get from greatest to least

                for (int i = unsortedIDs.Length-1; i >= 0 ; i--)
                {
                    Debug.Log(unsortedIDs[i] + "sorted");
                }

                //built a new list based on coordinates I saved
               
                for (int temp = unsortedIDs.Length-1 ; temp >= 0 ; temp--)
                {

                    for (int tem = 0; tem <= clusterPieces.Count - 1; tem++)
                    {
                        if(clusterPieces[tem].initPos.y == unsortedIDs[temp])
                        {
                            bool isClone = false;
                            foreach (puzzlePiece tempPiece in sorted)
                            {
                                if (tempPiece == clusterPieces[tem])
                                {
                                    isClone = true;
                                }
                            }

                            if (isClone == false)
                            {
                                sorted.Add(clusterPieces[tem]);
                            }
                        }
                      
                        
                    }

                }


                //Destroy list of pieces
                for (int temp = 0; temp <= clusterPieces.Count - 1; temp++)
                {
                    clusterPieces[temp].gameObject.SetActive(false);
                }

                //Vector3[] sortedVectors = unsortedVectors.OrderBy(v => v.x).ToArray();

                foreach (puzzlePiece disPiece in sorted)
                {
                     targPos = disPiece.initPos;
                    Debug.Log("Target Pos = " + targPos);
                    foreach (GameObject obj in puzzleGen.createdPieces)
                    {
                        puzzlePiece puzPiece = obj.GetComponent<puzzlePiece>();
                        if (puzPiece.initPos.x == targPos.x)
                        {
                            Debug.Log("checked  piece " + puzPiece.id);
                            if (puzPiece.initPos.y >= targPos.y)
                            {
                                puzPiece.transform.position = new Vector3(puzPiece.initPos.x, puzPiece.initPos.y - matchManager.tileSizeY , puzPiece.initPos.z);
                                puzPiece.dropped += 1;
                                Debug.Log("shifted  piece " + puzPiece.id + "  from " + puzPiece.initPos + " to : " + puzPiece.transform.position + " drops are " + puzPiece.dropped);
                                puzPiece.initPos = puzPiece.transform.position;
                              
                            }

                        }

                    }
                }





                if(clusterPieces.Count >= 4)
                {

                    ///temppp
                    ///


                    //Destroy list of 

                    puzzleSpawner puzzle2 = matchManager.spawn2.GetComponent<puzzleSpawner>();

                    foreach (GameObject disObj in puzzle2.createdPieces)
                    {
                        Destroy(disObj);
                        Debug.Log("object destroyed");
                    }

                    matchManager.spawn2.GetComponent<puzzleSpawner>().createdPieces.Clear();
                    Debug.Log("list destroyed");


                    ///////


                    for (int temp = 0; temp <= clusterPieces.Count - 1; temp++)
                    {

                        if (temp >= 3)
                        {

                            if (this.puzzleGen.gameObject == matchManager.spawn1)//if this object is connected to player 1s spawn generator send piece to player2
                            {
                                matchManager.spawn2.GetComponent<puzzleSpawner>().GenerateRandomPiece(new Vector3(1 + temp, 4, 0));
                            }

                        }
                        // Destroy(clusterPieces[temp].gameObject);
                    }
                }
              
            }






        }
      
    }

    


}
