using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class I_PE_Mwin_Follow : MonoBehaviour {

    public bool hidden, playerMwin;

    public Camera activeCam;
    public Transform player, target;

    public Vector3 upDis, rightDis, leftDis;
    public Vector3 chosenDis;

    public enum chosenArea { left, right, up,none};
    public chosenArea displayArea;
 
	// Use this for initialization
	void Awake () {

        hidden = true;

        float width = Mathf.Abs(this.GetComponent<RectTransform>().rect.width)/2;
        float height = Mathf.Abs(this.GetComponent<RectTransform>().rect.height)/2;

        upDis = new Vector3(0,height,0);
        leftDis = new Vector3( -width, 0, 0);
        rightDis = new Vector3(width, 0, 0);
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
       
        if(!hidden)
        {
            Vector3 screenPos = Vector3.zero;
            if (playerMwin)
            {
                screenPos = activeCam.WorldToScreenPoint(player.position);
            }
            else
            {
                screenPos = activeCam.WorldToScreenPoint(target.position);
            }


                this.transform.position = screenPos + chosenDis;
        }
        else
        {

            int rand = Random.Range(0, 3);

            if(rand == 0)
            {
                chosenDis = leftDis;
                displayArea = chosenArea.left;
            }
            else if(rand == 1)
            {
                chosenDis = rightDis;
                displayArea = chosenArea.right;

            }
            else if (rand == 2)
            {
                chosenDis = upDis;
                displayArea = chosenArea.up;

            }

            this.transform.GetChild(0).gameObject.SetActive(true);
            hidden = false;
        }


    }
}
