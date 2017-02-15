using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cardBattler : MonoBehaviour {

    public tileBattler owner;
    public TileBattleSystem tileBS;
     
    public enum AttackType { splash, click , wave , extra , summon };
    public AttackType atkType;



    public Vector3 targPos;
    public int[] attackPattern;
    public List<float> atkVals = new List<float>();


    public GameObject summonSprite,splashAttack;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
 

	}

    public void PlayCard(Vector3 targPos)
    {

        if(atkType == AttackType.splash)
        {
            SplashAttack();
        }
        else if(atkType == AttackType.click)
        {
            Debug.Log("click on least" );
        
        }
   
        

    }



    public void SplashAttack()
    {
        Debug.Log("attacked on " + targPos);
        if (tileBS.playerTurn == 0)
        {

            foreach (float atkVal in atkVals)
            {
              //  Debug.Log("attack " + atkVal);
                tileBS.player2.lifePoints -= atkVal;

                if (tileBS.player2.lifePoints <= 0)
                {
                    Debug.Log("Match over");
                }

            }




        }
        else
        {

            foreach (float atkVal in atkVals)
            {
                Debug.Log("attack " + atkVal);
                tileBS.player1.lifePoints -= atkVal;

                if (tileBS.player1.lifePoints <= 0)
                {
                    Debug.Log("Match over winner is " + tileBS.playerTurn);
                    tileBS.estado = TileBattleSystem.State.off;
                }

            }


        }

    }


    public void OnMouseDown()
    {
        if(owner.id == tileBS.playerTurn)
        {
            bool isClicked = false;
            foreach (GameObject disCard in owner.clicked)
            {

                if (disCard == this.gameObject)
                {
                    isClicked = true;
                }


            }

            if (isClicked == false)
            {
                owner.clicked.Add(this.gameObject);
                Debug.Log("oi you clickd me");
            }
        }

    }
}
