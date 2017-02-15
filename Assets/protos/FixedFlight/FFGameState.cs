using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FFGameState : MonoBehaviour {

    public enum State { title, race , oneplayer , settings};
    public State gameState;

    public List<GameObject> lanes = new List<GameObject>();

    public GameObject stage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(gameState == State.title)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("one player");
                gameState = State.oneplayer;
                stage.SetActive(true);
            }
        }

	}
}
