using UnityEngine;
using System.Collections;

public class CollinateEvents : MonoBehaviour {

    public CollinateUIManager uiManager;
    public CollinateGameStateManager gameStateManager;
    public playerControls myPlayer;

    public KeyCode keyAwaiting;
    public bool awaitingInput,eventProcessing;
    public int curEvento, curEventPosition;
    public float curEventStartTimer,curEventTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(eventProcessing==true)
        {
            if (awaitingInput == true)
            {
                if(Input.GetKeyDown(keyAwaiting))
                {
                    curEventPosition++;
                    awaitingInput = false;
                }
              
            }
            else
            {
                evento(curEvento, curEventPosition, curEventTime);
            }
        }

	}

    public void evento(int eventNum, int evenPos , float curEventTime)
    {
        eventProcessing = true;
        if(eventNum == 0)
        {
            Intro(gameStateManager.planetFocus);
        }
        else if(eventNum==99)
        {
           // curEventStartTimer = Time.time;
            //getting attacked??
            //if cur event time show this, that, that and this with some of that :P
        }
    }


    public void Intro(Planet startPlanet)
    {
        if(curEventPosition == 0)
        {
            myPlayer.transform.position = new Vector3(startPlanet.transform.position.x, startPlanet.transform.position.y, myPlayer.transform.position.z);

            Mwin("youve been promoted take this ship and conquer that planet!!!!", "System:");
            
            AwaitInput(KeyCode.Return);
        }
        else if (curEventPosition == 1)
        {
           uiManager.mwin.SetActive(false);
            myPlayer.isControlling = true;
            myPlayer.canClick = true;
            eventProcessing = false;
        }
    }

    public void AwaitInput(KeyCode keycode)
    {
        keyAwaiting = keycode;
        awaitingInput = true;
    }

    public void Mwin(string text, string speaker)
    {
        uiManager.mwin.SetActive(true);
        uiManager.mwinSpeaker.text = speaker;
        uiManager.mwinText.text = text;

    }

}
