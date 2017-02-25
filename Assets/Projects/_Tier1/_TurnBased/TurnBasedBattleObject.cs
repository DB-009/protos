using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnBasedBattleObject : MonoBehaviour {

    public BSys_1 battleSystem1;
    public TurnBasedUIManager uiManager;

    public GameObject battleMenu;

    public float updatePhase;

    public BattleObjType objType;
    public enum BattleObjType
    {
        player, cpu, item, summon, clone
    }

    public TurnPhase turnPhase;
    public enum TurnPhase
    {
        waiting, actionSelect,  dead,  choosingTarget, performingAction , attacking , defending, transforming,skillMenu
    }

    //Player Character stats
    public int str, intel, def, vit, spd, hp, mhp, mp, mmp, lvl, baseHP, baseMp, mvspd,kills,exp,expToLevel;
    public int teamID;

    public bool inBattle;
    //player Game Stats
    public int myID;

    public TurnBasedBattlerClass.PlayerClasses battlerClass;

    public TurnBasedBattleObject myTarget;

    public List<TurnBasedBattleObject> targs = new List<TurnBasedBattleObject>();

    public void Update()
    {

        if(objType == BattleObjType.player && inBattle == true)
        {
            if (objType == BattleObjType.player && battleSystem1.state == BSys_1.BS_States.on)
            {

                PlayerTurn();

            }
        }

    }

    public void PlayerTurn()
    {
        if(battleSystem1.curObjTurn.myID == myID)
        {

            if(turnPhase == TurnPhase.actionSelect && updatePhase == 0 )
            {
                uiManager.TurnBasedBS(this);//Check timers on buffs and stat increases..
                uiManager.updatePhase = 1;
                battleMenu.gameObject.SetActive(true);
                battleMenu.GetComponent<RectTransform>().rect.Set(0 - (uiManager.screenW * .50f), uiManager.screenH * .50f, uiManager.screenW * .50f, uiManager.screenH * .50f);


            }
            else if (turnPhase == TurnPhase.skillMenu && updatePhase == 0)
            {
                battleMenu.gameObject.SetActive(false);
                uiManager.TurnBasedBS(this);//Check timers on buffs and stat increases..
                uiManager.updatePhase = 1;
                


            }
            else if (turnPhase == TurnPhase.choosingTarget && objType == BattleObjType.player)
            {
       
               if(targs.Count == 0)
                {
                    Debug.Log("Choosing targ");
                    Debug.Log("building target list");

                    foreach (TurnBasedBattleObject disBattler in battleSystem1.possibleTargets)
                    {
                        if (disBattler.teamID != teamID)
                        {
                            targs.Add(disBattler);
                            Debug.Log(disBattler.name);
                        }
                    }
                }
               

            }

            //if (Input.GetKeyDown(KeyCode.Return))
            //{
               // if (myID == 0)
                //    LaunchAttack(battleSystem1.possibleTargets[1]);
               /// else if (myID == 1)
               //     LaunchAttack(battleSystem1.possibleTargets[0]);
           // }
        }

    }

    ///Battle System functions
    public void LaunchAttack(TurnBasedBattleObject target)
    {

        if (battlerClass == TurnBasedBattlerClass.PlayerClasses.Fighter)
        {
            turnPhase = TurnBasedBattleObject.TurnPhase.attacking;
            //run attack animation
            int dmgCalc = Random.Range(3, 15);

           
                dmgCalc += str;

            battleSystem1.DealDamage(target, this, dmgCalc);
            battleSystem1.SwitchTurn();
            Debug.Log("Fighter ATTACK");
        }
        else if (battlerClass == TurnBasedBattlerClass.PlayerClasses.Mage)
        {
            turnPhase = TurnBasedBattleObject.TurnPhase.attacking;
            //run attack animation
            int dmgCalc = Random.Range(3, 15);


            if (target.turnPhase == TurnPhase.defending)
            {
                dmgCalc += intel - def;
            }
            else
                dmgCalc += intel;


            battleSystem1.DealDamage(target, this, dmgCalc);
            battleSystem1.SwitchTurn();
            Debug.Log("MAGE ATTACK");
        }
    }

    public void resetStats()
    {

        TurnBasedPlayer myPlayer = this.GetComponent<TurnBasedPlayer>();


        if (objType == BattleObjType.player)
        {
            myPlayer = this.GetComponent<TurnBasedPlayer>();

            myID = myPlayer.myID;

            

            //get player stats from Player Object

       
            exp = myPlayer.exp;
            expToLevel = myPlayer.expToLevel;
            kills = myPlayer.kills;
            str = myPlayer.str;
            intel = myPlayer.intel;
            def = myPlayer.def;
            vit = myPlayer.vit;
            spd = myPlayer.spd;
            hp = myPlayer.hp;

            mp = myPlayer.mp;

            lvl = myPlayer.lvl;
            baseHP = myPlayer.baseHP;
            baseMp = myPlayer.baseMp;



        }

 
    }


    public void OnMouseDown()
    {
        if(battleSystem1.curObjTurn.turnPhase == TurnPhase.choosingTarget)
        {
            foreach(TurnBasedBattleObject disBattler in battleSystem1.curObjTurn.targs)
            {
                if(this == disBattler)
                {
                    Debug.Log("You just technically launched a attack");
                    battleSystem1.curObjTurn.LaunchAttack(disBattler);
                }
            }
        }
    }

}
