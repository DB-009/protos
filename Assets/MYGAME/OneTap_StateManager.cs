using UnityEngine;
using System.Collections;

using System.Collections.Generic;
public class OneTap_StateManager : MonoBehaviour {


    public enum GameState {  TitleScreen, OnePlayer, TwoPlayer, Lobby , Settings , Credits , LegacyScreen };
    public GameState gameState;
    bool gameStarted;

    public GameObject titleScreen;


    public GameObject obj1, obj2;


    public List<GameObject> myInteractbleObjects = new List<GameObject>();

    public List<GameObject> myObjectPooler = new List<GameObject>();

    public List<Transform> spawnPositions = new List<Transform>();


    public int clicks;

	// Use this for initialization
	void Awake () {
        gameState = GameState.TitleScreen;
        ObjSpawn();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(gameState == GameState.TitleScreen)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Game Started");
                gameState = GameState.OnePlayer;
                titleScreen.SetActive(false);
            }
        }
        else if(gameState == GameState.OnePlayer && gameStarted == false)
        {
            Debug.Log("One Player");
            gameStarted = true;

        }

	}

    public void ObjClicked()
    {
        Debug.Log("Object clicked");

        if(myInteractbleObjects.Count == 0)
        {
            foreach (GameObject disInteractableObj in myObjectPooler)
            {

                myInteractbleObjects.Add(disInteractableObj);
                disInteractableObj.SetActive(true);


            }

            myObjectPooler.Clear();


        }

    }


    public void ObjSpawn()
    {
      

        for(int i=0; i < 6; i++)
        {
            GameObject disSpawn = GameObject.Instantiate(obj1, spawnPositions[i].position, Quaternion.identity) as GameObject;
            myInteractbleObjects.Add(disSpawn);
            disSpawn.GetComponent<interactableObject>().gameStateManager = this;
        }




    }

}
