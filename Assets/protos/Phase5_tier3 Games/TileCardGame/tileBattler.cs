using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tileBattler : MonoBehaviour {

    public TileBattleSystem battleSys;

    public int id,cardInPlay,handSize,selectedTile;
    public float lifePoints;

    public GameObject tempCard;

    public List<cardBattler> deck,hand,attackCards = new List<cardBattler>();

    public List<GameObject>  clicked = new List<GameObject>();

    public enum State { draw,selecting , placing, attack, waiting, defend  };
    public State myState;

    public int  curTilePos;


    // Use this for initialization
    void Start () {
        curTilePos = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (battleSys.playerTurn == id && myState == State.selecting)
        {
            if (Input.GetKeyDown(KeyCode.Return) && Time.time >= battleSys.turnSwitched + 1)
            {

                Debug.Log("summoned tile turn swithc");
                myState = State.placing;

            }
        }

        else if (battleSys.playerTurn == id && myState == State.placing && clicked.Count > 0)
        {

            clicked[0].gameObject.SetActive(true);
            cardBattler disCard = clicked[0].GetComponent<cardBattler>();
            if (disCard.atkType == cardBattler.AttackType.splash)//change to current card in play
            {
                disCard.splashAttack.SetActive(true);

                Debug.Log("Cursor select");
                if (battleSys.playerTurn == 0)
                {
                    if (Input.GetKeyDown(KeyCode.A) && curTilePos != 0)
                    {
                        curTilePos -= 1;
                       // Debug.Log(battleSys.player1Tiles[curTilePos].initPos);

                        disCard.transform.Translate(-3, 0, 0);

                        battleSys.player1Tiles[curTilePos + 1].GetComponent<Renderer>().material = battleSys.player1Tiles[curTilePos + 1].mats[0];

                        battleSys.player1Tiles[curTilePos].GetComponent<Renderer>().material = battleSys.player1Tiles[curTilePos].mats[1];


                    }//
                    else if (Input.GetKeyDown(KeyCode.D) && curTilePos != battleSys.player1Tiles.Count - 1)
                    {
                        curTilePos += 1;
                        //Debug.Log(battleSys.player1Tiles[curTilePos].initPos);

                        battleSys.player1Tiles[curTilePos - 1].GetComponent<Renderer>().material = battleSys.player1Tiles[curTilePos - 1].mats[0];
                        disCard.transform.Translate(3, 0, 0);

                        battleSys.player1Tiles[curTilePos].GetComponent<Renderer>().material = battleSys.player1Tiles[curTilePos].mats[1];
                    }

                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.A) && curTilePos != 0)
                    {
                        curTilePos -= 1;
                        //Debug.Log(battleSys.player2Tiles[curTilePos].initPos);



                        disCard.transform.Translate(-3, 0, 0);
                        battleSys.player2Tiles[curTilePos + 1].GetComponent<Renderer>().material = battleSys.player2Tiles[curTilePos + 1].mats[0];


                        battleSys.player2Tiles[curTilePos].GetComponent<Renderer>().material = battleSys.player2Tiles[curTilePos].mats[1];

                    }
                    else if (Input.GetKeyDown(KeyCode.D) && curTilePos != battleSys.player2Tiles.Count - 1)
                    {
                        curTilePos += 1;
                       // Debug.Log(battleSys.player2Tiles[curTilePos].initPos);


                        disCard.transform.Translate(3, 0, 0);
                        battleSys.player2Tiles[curTilePos - 1].GetComponent<Renderer>().material = battleSys.player2Tiles[curTilePos - 1].mats[0];


                        battleSys.player2Tiles[curTilePos].GetComponent<Renderer>().material = battleSys.player2Tiles[curTilePos].mats[1];
                    }
                }


                if (Input.GetKeyDown(KeyCode.Return) && Time.time >= battleSys.turnSwitched + 1)
                {
                    Debug.Log("summoned tile turn swithc run foreach then switch state foreach should be before 1st card check");
                    attackCards.Add(disCard);
                    myState = State.attack;


                }

            }
            else if (disCard.atkType == cardBattler.AttackType.click)
            {

                int temp = 0;

                foreach (KeyCode disKey in battleSys.keyMap)
                {
                    if (Input.GetKeyDown(disKey))
                    {
                        Debug.Log("Clicked this tile " + battleSys.keyMap[temp]);
                        selectedTile = temp;
                    }
                    temp++;
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    attackCards.Add(disCard);

                    Debug.Log("add to areas that will be hit.. save to array");
                    myState = State.attack;
                }


            }
            else if (hand[0].atkType == cardBattler.AttackType.summon)
            {


                disCard.summonSprite.SetActive(true);


                int playerTile = 0;

                if (battleSys.playerTurn == 0)
                {
                    for (int temp = 18; temp < 27; temp++)
                    {

                        if (Input.GetKeyDown(battleSys.keyMap[temp]))
                        {
                            Debug.Log("Clicked this tile " + battleSys.keyMap[temp] + " .. " + playerTile);
                            selectedTile = temp;

                            hand[0].summonSprite.transform.position = new Vector3(battleSys.player1Tiles[playerTile].transform.position.x, battleSys.player1Tiles[playerTile].transform.position.y, -1);


                        }
                        playerTile++;
                    }

                    if (Input.GetKeyDown(KeyCode.Return) && Time.time >= battleSys.turnSwitched + 1)
                    {
                        Debug.Log("summoned tile turn swithc");
                      

                        myState = State.attack;


                    }
                }
                else
                {
                    for (int temp = 0; temp < 9; temp++)
                    {




                        if (Input.GetKeyDown(battleSys.keyMap[temp]))
                        {
                            Debug.Log("Clicked this tile " + battleSys.keyMap[temp] + " .. " + playerTile);
                            selectedTile = temp;


                            disCard.summonSprite.transform.position = new Vector3(battleSys.player2Tiles[playerTile].transform.position.x, battleSys.player2Tiles[playerTile].transform.position.y, -1);


                        }
                        playerTile++;


                    }
                    if (Input.GetKeyDown(KeyCode.Return) && Time.time >= battleSys.turnSwitched + 1)
                    {
                        Debug.Log("summoned tile turn swithc");
                        
                       

                        myState = State.attack;
                    }
                }




            }
        }
        else if (battleSys.playerTurn == id && myState == State.attack)
        {
            Debug.Log("attack Phase");

            int atkCardsCount =0;
            foreach(cardBattler disCard in attackCards)
            {
                atkCardsCount++;
                if(disCard.atkType == cardBattler.AttackType.splash)
                {
                    disCard.SplashAttack();
                }
            }

            for(int temp =0; temp < atkCardsCount; temp++)
            {
                Destroy(hand[0].gameObject);
                clicked.RemoveAt(0);
                hand.RemoveAt(0);
                handSize--;
                attackCards.RemoveAt(0);
                curTilePos = 0;
                
            }


            if (handSize == 0)
            {
                Debug.Log("Temp sysytem implement draw phase instead of giving same card over");
                battleSys.switchTurn();
                GameObject tile = GameObject.Instantiate(tempCard, this.transform.position - new Vector3(-1, 0, 0), Quaternion.identity) as GameObject;
                handSize++;
                hand.Add(tile.GetComponent<cardBattler>());
                hand[0].tileBS = battleSys;
                hand[0].owner = this;
            }



            if (Input.GetKeyDown(KeyCode.Return))
            {
                battleSys.switchTurn();
            }
        }

    }
}
