using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class tilePiece : MonoBehaviour {


    public Vector3 initPos;
    public TileBattleSystem tileBS;
    public bool isClickable;

    public List<Material> mats = new List<Material>();

    // Use this for initialization
    void Start () {
        mats[0] = this.GetComponent<Renderer>().material;
        initPos = transform.position;
	}

	
	// Update is called once per frame
	void Update () {
	
	}


    void OnMouseDown()
    {

        //Debug.Log("clicked a piece " +  initPos);

        if (tileBS.playerTurn == 0 && tileBS.player1.handSize > 0 && tileBS.estado != TileBattleSystem.State.off)
        {

            if (Input.GetMouseButton(0) && isClickable == true && tileBS.player1.hand[0].atkType == cardBattler.AttackType.click)
            {
               tileBS.player1.hand[0].PlayCard(initPos);
                
            }

        }
        //Debug.Log("clicked a piece " +  initPos);

        if (tileBS.playerTurn == 1 && tileBS.player2.handSize > 0 && tileBS.estado != TileBattleSystem.State.off)
        {

            if (Input.GetMouseButton(0) && isClickable == true && tileBS.player2.hand[0].atkType == cardBattler.AttackType.click)
            {
                tileBS.player2.hand[0].PlayCard(initPos);

            }

        }

    }




}
