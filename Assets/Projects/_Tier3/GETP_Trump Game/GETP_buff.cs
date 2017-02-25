using UnityEngine;
using System.Collections;

public class GETP_buff : MonoBehaviour {


    public bool casual;

    public int buffID;
    public bool isAlive;
    public float createdAt, lifeSpan;
    public bool die;
    // Use this for initialization
    void Awake()
    {
        //myFunctionz = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<myFunctions>();
        createdAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (die == true && Time.time >= createdAt + lifeSpan)
        {
            Destroy(this.gameObject);
            Debug.Log("buff faded remove from list bug");
        }

    }


    public void BuffEffect(GameObject targ)
    {
        Debug.Log("Remove the extra code in this logic on export to seperate project if casual etc add god script to find classes");

        GETP_Controller player = targ.GetComponent<GETP_Controller>();
        
        if (player != null)
        {
            if (buffID == 0)
            {

                player.hp += 25;
                if (player.hp > player.mhp)
                {
                    player.hp = player.mhp;
                }
                player.hpSlider.value = player.hp;

            }
            else if (buffID == 1)
            {
                if (player.curGun == GETP_Controller.gunType.triShot)
                {
                    player.gunDoubleUpgrade = true;
                }
                player.curGun = GETP_Controller.gunType.triShot;
                player.gunUpgradedAt = Time.time;


            }
            else if (buffID == 2)
            {
                player.curGun = GETP_Controller.gunType.normal;
                player.gunUpgradedAt = Time.time;

            }
            else if (buffID == 3)
            {
                player.jetPack = true;

            }



        }
        else
        {
            Debug.Log("temp random fix");
            SlingShotPlayer tempPlayer = targ.GetComponent<SlingShotPlayer>();
            if (buffID == 0)
            {
                tempPlayer.hp += 25;
                if (tempPlayer.hp > tempPlayer.mhp)
                {
                    tempPlayer.hp = tempPlayer.mhp;
                }
                //player.hpSlider.value = player.hp;

            }
            else if (buffID == 1)
            {
                if (tempPlayer.curGun == SlingShotPlayer.gunType.triShot)
                {
                    tempPlayer.gunDoubleUpgrade = true;
                }
                tempPlayer.curGun = SlingShotPlayer.gunType.triShot;
                tempPlayer.gunUpgradedAt = Time.time;


            }
            else if (buffID == 2)
            {
                tempPlayer.curGun = SlingShotPlayer.gunType.normal;
                tempPlayer.gunUpgradedAt = Time.time;

            }
            else if (buffID == 3)
            {
                tempPlayer.jetPack = true;

            }
        }
    }
           
    
}
