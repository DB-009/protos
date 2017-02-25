using UnityEngine;
using System.Collections;

public class RacerObj : MonoBehaviour {


    public enum RacerType { Player, CPU };
    public RacerType type;

    public bool finishedRace,isAlive;
    public int place,lapsCompleted,kills;
    public float timeStarted, timeFinished;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
