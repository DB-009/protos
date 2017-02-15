using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{



    public int eneID;

    public GameObject target;
    public float eneHp;

    public enum EnemyAction { walking, idle, attacking, defending, fleeing, dead, casting, parry };
    public EnemyAction curAction;
    public float closeDistance;

    public bool targeting;
    public float timeCreated;//time it was spawned at
    public float sleepTime;//time to hav enemies sleep on spawn so they dont immediately chase player 

    public bool isAwake;

    void Start()
    {
        timeCreated = Time.time;
        targeting = false;

    }

    void Update()
    {


        if (eneHp <= 0)//check if enemy is alive
        {
            Destroy(gameObject);//destroy this enemy if 0  or less hp
        }


        float timeAlive = Time.time - timeCreated;//update time alive


        if (isAwake == true)
        {
            if (targeting == true)
            {
               // if (target.GetComponent<PlatformControls>().hp <= 0)
              //  {
              //      target = null;
               //     targeting = false;
              //  }
            }

            if (timeAlive >= sleepTime)
            {
                if (targeting == false)
                {

                    GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject disTarget in possibleTargets)
                    {
                        if (targeting == false)
                        {
                            Vector3 offset = disTarget.transform.position - transform.position;
                            float sqrLen = offset.sqrMagnitude;
                            if (sqrLen < closeDistance * closeDistance)
                            {
                                //if (disTarget.GetComponent<PlatformControls>().hp >= 1)
                               // {
                                //    targeting = true;
                                //    target = disTarget;
                              //  }
                            }
                        }
                    }

                    if (targeting == false)
                    {
                        target = null;
                    }
                }
            }

            if (target)
            {
                Vector3 offset = target.transform.position - transform.position;
                float sqrLen = offset.sqrMagnitude;
                if (sqrLen < closeDistance * closeDistance)
                {
                    print("The other transform is close to me!");

                    transform.position = Vector3.Lerp(transform.position, target.transform.position, .7f * Time.deltaTime);
                }
                else
                {
                    targeting = false;
                    //idle behaviour
                }
            }
        }
    }


    //Built in unity function to detect When a collision has occured
    void OnCollisionEnter(Collision col)
    {

        if (col.collider.tag == "Player")
        {


        }


    }


}