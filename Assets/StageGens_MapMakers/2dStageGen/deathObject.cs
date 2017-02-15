using UnityEngine;
using System.Collections;

public class deathObject : MonoBehaviour {

    public GameManager gameManager;
    public GameObject player;
    public float pushRate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        if (gameManager.state == GameManager.BS_States.on && player.activeSelf == true)
        {
            if (this.transform.position.x >= player.transform.position.x)
            {
                Debug.Log("player should be dead");
                player.SetActive(false);
            }
            else
            {
                this.transform.Translate(pushRate * Time.deltaTime, 0, 0);
            }
        }
        else if(player.activeSelf == false)
        {
            Debug.Log("player  is  dead");

        }


    }
}
