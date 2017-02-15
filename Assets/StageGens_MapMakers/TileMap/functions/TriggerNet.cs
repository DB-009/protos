using UnityEngine;
using System.Collections;

public class TriggerNet : MonoBehaviour {

    public float ySize, yPos;
    public controlledStageGenerator stageGen;
	// Use this for initialization
	void Start () {

        BoxCollider b = this.gameObject.GetComponent<Collider>() as BoxCollider;
        b.size = new Vector3(stageGen.xtiles, ySize, stageGen.ytiles);

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + yPos ,this.transform.position.z);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
