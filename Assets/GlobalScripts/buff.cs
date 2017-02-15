using UnityEngine;
using System.Collections;

public class buff : MonoBehaviour {


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


    public void BuffEffect(GameObject targetEffect)
    {
        Debug.Log("Remove the extra code in this logic on export to seperate project if casual etc");
        if (casual == true)
        {
            LaneShift_TopDown player = targetEffect.GetComponent<LaneShift_TopDown>();
            LaneShift_TopDown_NET playerNet = targetEffect.GetComponent<LaneShift_TopDown_NET>();
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
                    if (player.curGun == LaneShift_TopDown.gunType.triShot)
                    {
                        player.gunDoubleUpgrade = true;
                    }
                    player.curGun = LaneShift_TopDown.gunType.triShot;
                    player.gunUpgradedAt = Time.time;


                }
                else if (buffID == 2)
                {
                    player.curGun = LaneShift_TopDown.gunType.normal;
                    player.gunUpgradedAt = Time.time;

                }
                else if (buffID == 3)
                {
                    player.jetPack = true;

                }
            }
            else if (playerNet != null)
            {
                if (buffID == 0)
                {

                    playerNet.hp += 25;
                    if (playerNet.hp > playerNet.mhp)
                    {
                        playerNet.hp = playerNet.mhp;
                    }
                    playerNet.hpSlider.value = playerNet.hp;

                }
                else if (buffID == 1)
                {
                    if (playerNet.curGun == LaneShift_TopDown_NET.gunType.triShot)
                    {
                        playerNet.gunDoubleUpgrade = true;
                    }
                    playerNet.curGun = LaneShift_TopDown_NET.gunType.triShot;
                    playerNet.gunUpgradedAt = Time.time;


                }
                else if (buffID == 2)
                {
                    playerNet.curGun = LaneShift_TopDown_NET.gunType.normal;
                    playerNet.gunUpgradedAt = Time.time;

                }
                else if (buffID == 3)
                {
                    playerNet.jetPack = true;

                }
            }
        }
        else 
        {
            RockToss_Controller player = targetEffect.GetComponent<RockToss_Controller>();
            RockToss_Controller_NET playerNet = targetEffect.GetComponent<RockToss_Controller_NET>();
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
                    if (player.curGun == RockToss_Controller.gunType.triShot)
                    {
                        player.gunDoubleUpgrade = true;
                    }
                    player.curGun = RockToss_Controller.gunType.triShot;
                    player.gunUpgradedAt = Time.time;


                }
                else if (buffID == 2)
                {
                    player.curGun = RockToss_Controller.gunType.normal;
                    player.gunUpgradedAt = Time.time;

                }
                else if (buffID == 3)
                {
                    player.jetPack = true;

                }
            }
            else if (playerNet != null)
            {
                if (buffID == 0)
                {

                    playerNet.hp += 25;
                    if (playerNet.hp > playerNet.mhp)
                    {
                        playerNet.hp = playerNet.mhp;
                    }
                    playerNet.hpSlider.value = playerNet.hp;

                }
                else if (buffID == 1)
                {
                    if (playerNet.curGun == RockToss_Controller_NET.gunType.triShot)
                    {
                        playerNet.gunDoubleUpgrade = true;
                    }
                    playerNet.curGun = RockToss_Controller_NET.gunType.triShot;
                    playerNet.gunUpgradedAt = Time.time;


                }
                else if (buffID == 2)
                {
                    playerNet.curGun = RockToss_Controller_NET.gunType.normal;
                    playerNet.gunUpgradedAt = Time.time;

                }
                else if (buffID == 3)
                {
                    playerNet.jetPack = true;

                }
            }
        }

    }
           
    
}
