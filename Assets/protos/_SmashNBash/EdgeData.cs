using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Use this for initialization

public class EdgeData : MonoBehaviour {

    public List<GameObject> objectsOfInterest = new List<GameObject>();

    public List<float> distances = new List<float>();
    public List<bool> canIgnore = new List<bool>();
    void Start()
    {
      
        foreach(GameObject disObj in objectsOfInterest)
        {

            float offsetX = disObj.transform.position.x - transform.position.x;
            distances.Add(offsetX);//create distances list
       
        }

    }

    void Update()
    {
        int tempVar = 0;
        foreach (GameObject disObj in objectsOfInterest)
        {

            float offsetX = disObj.transform.position.x - transform.position.x;
            distances[tempVar]  = offsetX;//updating distances list
            tempVar++;
        }
    }

}
