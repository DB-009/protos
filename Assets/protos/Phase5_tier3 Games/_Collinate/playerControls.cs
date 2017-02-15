using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerControls : MonoBehaviour {


    public bool isControlling,canClick;
    public float zoomOut;
    public Vector3 curPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(isControlling == true)
        {
            Pan();
        }
	}



    public void Pan()
    {
        if (Input.GetKey(KeyCode.W))
        {
            curPos = this.transform.position;

            Camera.main.transform.position = new Vector3(curPos.x,curPos.y+1*Time.deltaTime, curPos.z);
            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            curPos = this.transform.position;

            Camera.main.transform.position = new Vector3(curPos.x, curPos.y -1*Time.deltaTime, curPos.z);

        }


        if (Input.GetKey(KeyCode.A))
        {
            curPos = this.transform.position;
            Camera.main.transform.position = new Vector3(curPos.x-1*Time.deltaTime, curPos.y, curPos.z);
          

        }
        else if (Input.GetKey(KeyCode.D))
        {
            curPos = this.transform.position;
            Camera.main.transform.position = new Vector3(curPos.x+1*Time.deltaTime, curPos.y, curPos.z);
 

        }
    }



}
