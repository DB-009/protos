using UnityEngine;
using System.Collections;

public class buffEffects : MonoBehaviour {

    public int id;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void cast(GameObject targetRacer, int buffID , StateManagerRacing gameStateManager)
    {

        float amount = 0;


        if (gameStateManager.gameMode == StateManagerRacing.Mode.Platformer)
        {
            RacingController targ = targetRacer.GetComponent<RacingController>();
            amount = targ.matchPhaser.upForce * targ.rb.velocity.x;

        }
        else
        {
            _Endless2dController targ = targetRacer.GetComponent<_Endless2dController>();
            amount = targ.matchPhaser.upForce * targ.rb.velocity.x;
        }



        if(buffID == 0)
        {
           targetRacer.GetComponent<Rigidbody>().AddForce(new Vector3( amount, 0, 0), ForceMode.Impulse);//speed player up script

        }
       // else if()==1

    }

}
