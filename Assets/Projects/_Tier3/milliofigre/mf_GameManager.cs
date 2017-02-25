using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mf_GameManager : MonoBehaviour
{
    public mf_player player1, player2, winner;
    public GameObject burningSun,objectControlling;

    public BS_State state;
    public float[] abs;
    public bool _keyPressed;

    public float fightStartTime, fightEndTime, totalTurns, nextTurnAt, turnWaitTime, endgamePhase;
    public int playerTurn;
    public mf_player[] possibleTargets;


    public GameObject enemy;
    public enum BS_State
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
        playerTurn = 0;
        state = BS_State.off;
        //Debug.Log("F to fight");
        objectControlling = player1.gameObject;
    }

    public void Update()
    {
        //state management
        if (state == BS_State.off)//off
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
                state = BS_State.prestart;
               // Debug.Log("Entered Battle");

                foreach (mf_player disPlayer in possibleTargets)
                {
                    disPlayer.inBattle = true;
                    disPlayer.cam.myTarget = disPlayer.transform;
                    disPlayer.isController = true;
                }

            }
        }
        else if (state == BS_State.prestart)//prestart
        {
            Debug.Log("Prestart");
            state = BS_State.on;

           


        }
        else if (state == BS_State.wait)//wait
        {
            if (Time.time >= nextTurnAt)
            {
                state = BS_State.on;
            }

        }
        else if (state == BS_State.endgame)//wait
        {
            if (Time.time >= endgamePhase)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = BS_State.off;
                    player1.gameObject.SetActive(true);

                    player1.hp = player1.mhp;

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


    }


    public void DealDamage()
    {



    }



    public void SwitchTurn()
    {
        if (state == BS_State.on)
        {

            //player1.turnPhase = mf_player.TurnPhase.rockToss;
            Debug.Log("change player phase check commented");
            state = BS_State.wait;

            totalTurns += 1;
            nextTurnAt = Time.time + turnWaitTime;
        }
    }

}
