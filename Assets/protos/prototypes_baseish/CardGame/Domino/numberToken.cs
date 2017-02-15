using UnityEngine;
using System.Collections;

public class numberToken : MonoBehaviour {

    public myGameStateManager gameStateManager;
    public myTurnBasedSystem turnBased;
    public int tokenID;

    public bool clickable;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if(clickable == true)
        {
            if(tokenID == turnBased.curToken+1)
            {
                Debug.Log("Good JOb");
                turnBased.tokenPlay();
                gameObject.SetActive(false);

            }
            else
            {
                Debug.Log("bad JOb");
            }
        }
    }
}
