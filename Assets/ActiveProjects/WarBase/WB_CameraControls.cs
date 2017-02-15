using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WB_CameraControls : MonoBehaviour {


    public int yDis,initYDis, xPos, zPos;
    public int zoomLevel,initZoom;

    public GameObject xInput, zInput;

	// Use this for initialization
	void Start () {
   

    }

    // Update is called once per frame
    void Update () {
	if(Input.GetKeyDown(KeyCode.A))
        {
            CameraPan(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CameraPan(2);

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            CameraPan(4);

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CameraPan(3);

        }
    }


    public void CameraPan(int pos)
    {
        if(pos == 1)
        {
            this.transform.position = this.transform.position+ new Vector3(-1, 0, 0);
        }
        else if (pos == 2)
        {
            this.transform.position = this.transform.position + new Vector3(1, 0, 0);

        }
        else if (pos == 3)
        {
            this.transform.position = this.transform.position + new Vector3(0, 0, -1);

        }
        else if (pos == 4)
        {
            this.transform.position = this.transform.position + new Vector3(0,0, 1);

        }
    }

    public void goToPosition(int x, int z)
    {
        this.transform.position = new Vector3(x, yDis, z);

    }

    public void goToPosition_UI()
    {
        this.transform.position = new Vector3(float.Parse( xInput.GetComponent<InputField>().text), yDis, float.Parse(zInput.GetComponent<InputField>().text));

    }

}
