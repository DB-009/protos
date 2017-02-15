using UnityEngine;
using System.Collections;

public class bombLife : MonoBehaviour {

    public Player owner;
    public myFunctions myFunctionz;

    public GameObject explosion;

    public float created, explodeAt, bombLifeSpan;

	// Use this for initialization
	void Start () {
        myFunctionz = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<myFunctions>();

        created = Time.time;
        explodeAt = created + bombLifeSpan;
	}
	
	// Update is called once per frame
	void Update () {
	if(Time.time >= explodeAt )
        {
            Explode();
        }

        
    }


    void Explode()
    {

        GameObject expObj = GameObject.Instantiate(explosion, this.transform.position, Quaternion.identity) as GameObject;

        expObj.GetComponent<explosion>().owner = owner;

        Debug.Log("Boom Explode");

    
         if (Time.time >= explodeAt)
             Destroy(this.gameObject);
    }




    void OnTriggerEnter(Collider col)
    {

        if (col.tag == "projectile")
        {
            Explode();
            Destroy(this.gameObject);
            Destroy(col.gameObject);
        }

         else if (col.tag == "hitObj")
        {
            Explode();
            Destroy(this.gameObject);
        }
    }

}



