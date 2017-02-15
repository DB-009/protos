using UnityEngine;
using System.Collections;

public class Troop : MonoBehaviour {

    public GameObject owner;

    public int eneID;

    public GameObject target;
    public float hp,maxHp;

    public float closeDistance;

    public bool targeting;
    public float timeCreated;//time it was spawned at
    public float sleepTime;//time to hav enemies sleep on spawn so they dont immediately chase player 

    public bool isAwake;

    public bool canMove, moveable;//can move if frozen etc, moveable capable of movement (canons etc))

    public enum AttackType {  melee, ranged , none };

    public AttackType disAtk;

    public enum TroopCommand   { walking, idle, attacking, defending, fleeing, dead, casting, parry };

    public TroopCommand command;

    public GameObject attackObject;
    public float atkForce;


    void Start()
    {
        timeCreated = Time.time;
        targeting = false;

    }

    void Update()
    {


        if (hp <= 0)//check if enemy is alive
        {
            Destroy(gameObject);//destroy this enemy if 0  or less hp
        }


        float timeAlive = Time.time - timeCreated;//update time alive

        if(command == TroopCommand.attacking && target != null)
        {
            if(disAtk == AttackType.ranged)
            {
                Shoot(attackObject, atkForce, this.gameObject);
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


    public void OnMouseDown()
    {
        owner.GetComponent<Player>().TroopHighlight(this.gameObject);
    }


    public void Shoot(GameObject bulz, float bulForce, GameObject owner)
    {
        //change to  get player angle and forward from that





        GameObject tileCreated = GameObject.Instantiate(bulz, new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z), Quaternion.identity) as GameObject;

        tileCreated.GetComponent<projectileLife>().owner = owner;
        tileCreated.GetComponent<projectileLife>().playerBullet = false;

        this.transform.LookAt(target.transform);
        tileCreated.transform.LookAt(target.transform);


        tileCreated.GetComponent<Rigidbody>().AddForce(tileCreated.transform.forward * bulForce, ForceMode.Impulse);

        

        // Smoothly rotate towards the target point.


    }

}
