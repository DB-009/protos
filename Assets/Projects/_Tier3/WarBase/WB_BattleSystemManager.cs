using UnityEngine;
using System.Collections;

public class WB_BattleSystemManager : MonoBehaviour {

    public float  fightStartTime, fightEndTime,  endgamePhase;


    public enum BS_States
    {
        off,
        on,
        wait,
        prestart,
        endgame,
        lobby
    }

    public WB_BattleSystemManager.BS_States state;

    // Use this for initialization
    public void Start()
    {

        state = BS_States.off;
        Debug.Log("F to fight");
    }
 
	
	// Update is called once per frame
	void Update () {
	
	}
}
