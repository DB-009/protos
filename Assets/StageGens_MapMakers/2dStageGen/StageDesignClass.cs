using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageDesignClass : MonoBehaviour {


    public enum type { platforms, stage, maze ,  stack, runway , cannon};

    public type classType;

    public float width,height;

    public bool hasYExit,xExit,isGenerator;

    public GameObject invertedObj;

    public List<Transform> bgLocations = new List<Transform>();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
