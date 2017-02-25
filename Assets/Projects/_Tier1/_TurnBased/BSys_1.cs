using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BSys_1 : MonoBehaviour
{
    public TurnBasedUIManager uiManager;

    public TurnBasedPlayer player1,player2, player3, player4, ene1, ene2, ene3, ene4, winner;
    

    public BSys_1.BS_States state;
    public float[] abs;
    public bool _keyPressed;

    public float playerTurn,fightStartTime,fightEndTime,totalTurns,nextTurnAt,turnWaitTime,endgamePhase;

    public List<TurnBasedBattleObject> team1,team2,possibleTargets = new List<TurnBasedBattleObject>();



    public TurnBasedBattleObject curObjTurn;



    public enum BS_States
    {
        off,
        on,
        wait,
        prestart,
        endgame,
        lobby
    }

    public void Start()
    {

        state = BS_States.off;
        Debug.Log("F to fight");
    }

    public void Update()
    {
        //state management
      if(state == BS_States.off)//off
        { 

            if(Input.GetKeyDown(KeyCode.F))
            {
                state = BS_States.prestart;
                Debug.Log("Entered Battle we should load teams here for now its manual");

                foreach(TurnBasedBattleObject disPlayer in team1)
                {

                    disPlayer.inBattle = true;
                    disPlayer.GetComponent<TurnBasedPlayer>().inBattle = true;

                    disPlayer.uiManager = uiManager;

                    disPlayer.battleSystem1 = this;
                    disPlayer.battleMenu = uiManager.battleMenu;

                    disPlayer.resetStats();
                    possibleTargets.Add(disPlayer);
                }


                foreach (TurnBasedBattleObject disPlayer in team2)
                {

                    disPlayer.inBattle = true;
                    disPlayer.GetComponent<TurnBasedPlayer>().inBattle = true;

                    disPlayer.uiManager = uiManager;

                    disPlayer.battleSystem1 = this;
                    disPlayer.battleMenu = uiManager.battleMenu;

                    disPlayer.resetStats();
                    possibleTargets.Add(disPlayer);

                }

                Debug.Log("Temp choose first player");
                playerTurn = 0;
                curObjTurn = possibleTargets[0];


                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                uiManager.TurnBasedBS(curObjTurn);
            }
        }
        else if(state == BS_States.prestart)//prestart
        {
            Debug.Log("Prestart");
            state = BS_States.on;
        }
        else if (state == BS_States.wait)//wait
        {
           if(Time.time>= nextTurnAt)
            {
                state = BS_States.on;
            }
           
        }
        else if (state == BS_States.endgame)//wait
        {
            if (Time.time >= endgamePhase)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    state = BS_States.off;
                    player1.gameObject.SetActive(true);
                    player2.gameObject.SetActive(true);

                    player1.hp = player1.mhp;
                    player2.hp = player2.mhp;
                    player3.hp = player3.mhp;
                    player4.hp = player4.mhp;
                    playerTurn = 0;
                }
            }
            else
            {
                Debug.Log("Le calc ");
            }
        }
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10f, 10f, 400f, 20f), 
            "1.Player1 " + player1.battlerClass +
            " " + player1.hp + "/" + player1.mhp
            );
        GUI.Label(new Rect(10f, 25f, 400f, 20f), 
            "2.Player2 " + player2.battlerClass +
            " " + player2.hp + "/" + player2.mhp
            );

        GUI.Label(new Rect(10f, 40f, 400f, 20f),
    "2.Player2 " + player3.battlerClass +
    " " + player3.hp + "/" + player3.mhp
    );

        GUI.Label(new Rect(10f, 55f, 400f, 20f),
    "2.Player2 " + player4.battlerClass +
    " " + player4.hp + "/" + player4.mhp
    );


    }


    public void DealDamage(TurnBasedBattleObject target, TurnBasedBattleObject attacker, int dmg)
    {

        
        if (target.objType == TurnBasedBattleObject.BattleObjType.player)
        {

            target.GetComponent<TurnBasedPlayer>().hp -= dmg;
            if (target.GetComponent<TurnBasedPlayer>().hp <= 0)
            {
                target.gameObject.SetActive(false);

                uiManager.battleMenu.SetActive(false);

                Debug.Log("Player number " + attacker.GetComponent<TurnBasedPlayer>().myID + "Won ");
                winner = attacker.GetComponent<TurnBasedPlayer>();
                endgamePhase = Time.time + turnWaitTime * 2;
                state = BS_States.endgame;
            }
        }
    }



    public void SwitchTurn()
    {
        Debug.Log("Turn Switch sould follow a turnOrder List, need CurrentTurn int and TurnOrder List for who goes");

        if (state == BS_States.on)
        {
            if (playerTurn == 0)
            {
                playerTurn = 1;
                curObjTurn = possibleTargets[1];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }
            else if(playerTurn == 1)
            {
                playerTurn = 2;
                curObjTurn = possibleTargets[2];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }
            else if (playerTurn == 2)
            {
                playerTurn = 3;
                curObjTurn = possibleTargets[3];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }
            else if (playerTurn == 3)
            {
                playerTurn = 4;
                curObjTurn = possibleTargets[4];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }
            else if (playerTurn == 4)
            {
                playerTurn = 5;
                curObjTurn = possibleTargets[5];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }
            else if (playerTurn == 5)
            {
                playerTurn = 6;
                curObjTurn = possibleTargets[6];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }
            else if (playerTurn == 6)
            {
                playerTurn = 7;
                curObjTurn = possibleTargets[7];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }
            else if (playerTurn == 7)
            {
                playerTurn = 0;
                curObjTurn = possibleTargets[0];
                curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.actionSelect;
                curObjTurn.updatePhase = 0;
            }

            uiManager.battleMenu.SetActive(false);
            state = BS_States.wait;
            totalTurns += 1;
            nextTurnAt = Time.time + turnWaitTime;
        }
    }

}
