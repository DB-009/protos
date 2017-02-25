using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TurnBasedPlayer : MonoBehaviour
{




    public TurnBasedBattleObject myBattleObj;
    public bool inBattle;

    //Player Character stats
    public int str, intel, def, vit, spd, hp, mhp, mp, mmp, lvl, baseHP, baseMp, mvspd,kills, exp, expToLevel;

    //player Game Stats
    public int myID;
    public int teamID;

    public TurnBasedBattlerClass.PlayerClasses battlerClass;




    public List<TurnBasedEquip> equips = new List<TurnBasedEquip>();


    


    public void Awake()
    {



        this.hp = this.vit * lvl + baseHP;
        this.mp = this.intel * lvl + baseMp;
        this.mhp = this.hp;
        this.mmp = this.mp;



        myBattleObj = this.GetComponent<TurnBasedBattleObject>();

    }

    public void Update()
    {


    }

    void FixedUpdate()
    {

    }







}







