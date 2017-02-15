using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myTurnBasedSystem : MonoBehaviour {

    public myGameStateManager gameStateManager;


    public enum State { off, ongoing, wait, ended , endscreen , lobby }
    public State myState;

    public GameObject[] players;

    public List<GameObject> summons = new List<GameObject>();
    public int playerTurn,curToken;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(myState == State.lobby)
        {
            Debug.Log("Hijacked the State Machine Logic, Deal Cards From here");
            List<GameObject> nonUsedSummon = summons;


            for(int temp=0; temp <5; temp++)
            {
                int objID = Random.Range(0, nonUsedSummon.Count - 1);

            



                Transform trans = players[0].transform.GetChild(0).transform;
                Vector3 disPos = new Vector3((trans.position.x - (trans.localScale.x/2)) +  (trans.position.x + ( 4 * temp)) , trans.position.y, trans.position.z);

                GameObject summon = GameObject.Instantiate(nonUsedSummon[objID], disPos, Quaternion.identity) as GameObject;

                if (summon.GetComponent<numberToken>().tokenID == 1)
                    playerTurn = 0;

                nonUsedSummon.RemoveAt(objID);

                summon.SetActive(true);

                players[0].GetComponent<myPlayer>().myObjects.Add(summon);

            }



            for (int temp = 0; temp < 5; temp++)
            {
                int objID = Random.Range(0, nonUsedSummon.Count - 1);



                Transform trans = players[1].transform.GetChild(0).transform;
                Vector3 disPos = new Vector3((trans.position.x - (trans.localScale.x / 2)) + (trans.position.x + (4 * temp)), trans.position.y, trans.position.z);

                GameObject summon = GameObject.Instantiate(nonUsedSummon[objID], disPos, Quaternion.identity) as GameObject;

                summon.GetComponent<numberToken>().gameStateManager = gameStateManager;
                summon.GetComponent<numberToken>().turnBased = this;

                if (summon.GetComponent<numberToken>().tokenID == 1)
                    playerTurn = 1;

                nonUsedSummon.RemoveAt(objID);

                summon.SetActive(true);

                players[1].GetComponent<myPlayer>().myObjects.Add(summon);

            }


            //turn on players
            foreach (GameObject disObj in players[playerTurn].GetComponent<myPlayer>().myObjects)
            {
                disObj.GetComponent<numberToken>().clickable = true;
            }


            curToken = 0;


            myState = State.ongoing;

        }
	}

    public void SwitchTurn()
    {
        //turn off previous players cards
        foreach (GameObject disObj in players[playerTurn].GetComponent<myPlayer>().myObjects)
        {
            disObj.GetComponent<numberToken>().clickable = false;
        }

        if (playerTurn == 0)
        {
            playerTurn = 1;

        }
        else
        {
            playerTurn = 0;
        }

        //turn on players
        foreach (GameObject disObj in players[playerTurn].GetComponent<myPlayer>().myObjects)
        {
            disObj.GetComponent<numberToken>().clickable = true;
        }


    }


    public void tokenPlay()
    {



        for (int temp =0; temp < players[playerTurn].GetComponent<myPlayer>().myObjects.Count ; temp++)
        {
            if (players[playerTurn].GetComponent<myPlayer>().myObjects[temp].GetComponent<numberToken>().tokenID == curToken + 1)
            {
                players[playerTurn].GetComponent<myPlayer>().myObjects.RemoveAt(temp);
            }
        
        }

        curToken += 1;

        bool changeturn = true;

        foreach (GameObject disObj in players[playerTurn].GetComponent<myPlayer>().myObjects)
        {
            if (disObj.GetComponent<numberToken>().tokenID == curToken + 1)
            {
                changeturn = false;
            }
        }

        if (changeturn == true)
        {
            if (players[playerTurn].GetComponent<myPlayer>().myObjects.Count > 0)
                SwitchTurn();
            else
                Debug.Log("PLAYER" +(playerTurn + 1) + " won");
        }
    }
}
