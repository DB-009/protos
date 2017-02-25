using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class projectileLife : MonoBehaviour
{

    public GameObject owner;
    public myFunctions myFunctionz;

    public Player myPlayer;


    public bool playerBullet;
    public float createdAt,lifeSpan;
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
       



        if (die==true && Time.time >= createdAt+lifeSpan)
        {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter(Collider col)
    {
        

        if (col.gameObject.tag == "enemy")
        {
            if(playerBullet == true)
            {

                CpuAi disEne = col.gameObject.GetComponent<CpuAi>();

                if(disEne.trackAtks == true)
                {
                   

                    float disChk = disEne.GetDistance();
                    Debug.Log(disChk);
                    if (disChk <= disEne.nearDis)
                    {
                        disEne.atkTracker.nearAtklanded(playerAttack.attackType.projectile);
                    }
                    else
                        disEne.atkTracker.farAtklanded(playerAttack.attackType.projectile);

                }

                int damage = 25;

                disEne.health -= damage;
                //Debug.Log("Did " + damage + "  Dmg but popup is off myfunnktions script");
                //myFunctionz.CreateDamagePopup(damage, col.transform, myFunctionz.textPrefab);


                if (disEne.health <= 0)
                {

                    // myPlayer.kills++;
                    // myPlayer.stageGen.enesCount--;
                    if(owner.GetComponent<LaneShift_TopDown>() != null)
                    owner.GetComponent<LaneShift_TopDown>().kills++;
                    else if (owner.GetComponent<RockToss_Controller>() != null)
                        owner.GetComponent<RockToss_Controller>().kills++;

                    //Debug.Log("Killed enemy add in tile map systems if needed");
                    // myPlayer.stageGen.DestroyTile(col.GetComponent<mapTile>());

                    Destroy(col.gameObject);

                }

                Destroy(this.gameObject);

            }
            else if (col.gameObject.tag == "obstacle")
            {
                // col.GetComponent<buffZone>().invisbleExploder.SetActive(true);
                Destroy(this.gameObject);
            }

        }

        if (col.gameObject.tag == "Player")
        {
            if (playerBullet == false)
            {
                int damage = 25;

                LaneShift_TopDown lanePlayer = col.gameObject.GetComponent<LaneShift_TopDown>();
                GETP_Controller getp_player = col.gameObject.GetComponent<GETP_Controller>();


                if (lanePlayer != null)
                {
                    lanePlayer.hp -= damage;
                    lanePlayer.hpSlider.value -= damage;
                }
                else if (getp_player != null)
                {
                    getp_player.hp -= damage;
                    getp_player.hpSlider.value -= damage;
                }



                Destroy(this.gameObject);

            }
          

        }
  else if (col.gameObject.tag == "obstacle")
            {
                // col.GetComponent<buffZone>().invisbleExploder.SetActive(true);
                Destroy(this.gameObject);
            }


    }

    void OnCollisionEnter(Collision col)
    {

        Destroy(this.gameObject);


    }
}
