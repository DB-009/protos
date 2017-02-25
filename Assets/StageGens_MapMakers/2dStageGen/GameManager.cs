using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public SlingShotPlayer player1, player2, winner;
    public GameObject burningSun,objectControlling;

    public GameManager.BS_States state;
    public float[] abs;
    public bool _keyPressed;

    public float fightStartTime, fightEndTime, totalTurns, nextTurnAt, turnWaitTime, endgamePhase;
    public int playerTurn;
    public SlingShotPlayer[] possibleTargets;


    public GameObject enemy;
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
        playerTurn = 0;
        state = BS_States.off;
        Debug.Log("F to fight");
        objectControlling = player1.gameObject;
    }

    public void Update()
    {
        //state management
        if (state == BS_States.off)//off
        {

         
                state = BS_States.prestart;
                Debug.Log("Entered STage AutoMAtically");

                foreach (SlingShotPlayer disPlayer in possibleTargets)
                {
                    disPlayer.inBattle = true;
                    disPlayer.cam.myTarget = disPlayer.transform;
                }

            
        }
        else if (state == BS_States.prestart)//prestart
        {
            Debug.Log("Prestart");
            state = BS_States.on;

           


        }
        else if (state == BS_States.wait)//wait
        {
            if (Time.time >= nextTurnAt)
            {
                state = BS_States.on;
            }

        }
        else if (state == BS_States.endgame)//wait
        {
            if (Time.time >= endgamePhase)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = BS_States.off;
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
        if (state == BS_States.on)
        {

            //player1.turnPhase = SlingShotPlayer.TurnPhase.rockToss;
            Debug.Log("change player phase check commented");
            state = BS_States.wait;

            totalTurns += 1;
            nextTurnAt = Time.time + turnWaitTime;
        }
    }

}
