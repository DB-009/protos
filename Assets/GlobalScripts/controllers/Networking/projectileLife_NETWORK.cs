using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class projectileLife_NETWORK : NetworkBehaviour
{
    [SyncVar]
    public GameObject owner;
    public myFunctions myFunctionz;

    public Player myPlayer;

    [SyncVar]
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
        if (!isServer)
        {
            return;
        }

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

                int damage = 25;

                disEne.health -= damage;
                //Debug.Log("Did " + damage + "  Dmg but popup is off myfunnktions script");
                //myFunctionz.CreateDamagePopup(damage, col.transform, myFunctionz.textPrefab);


                if (disEne.health <= 0)
                {

                    // myPlayer.kills++;
                    // myPlayer.stageGen.enesCount--;
                    owner.GetComponent<LaneShift_TopDown_NET>().kills++;
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
            if (owner != col.gameObject)
            {

                LaneShift_TopDown_NET disPlay = col.gameObject.GetComponent<LaneShift_TopDown_NET>();

                disPlay.TakeDamage(25, disPlay.gameObject);
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
        if (!isServer)
        {
            return;
        }

        Destroy(this.gameObject);


    }
}
