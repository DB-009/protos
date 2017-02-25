using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GETP_ENEMY : MonoBehaviour {


    public Rigidbody rb;
    public GameObject target;

    public objGen eneGen;
    public float alignmentWeight, cohesionWeight, separationWeight,distanceChk;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 alignment = computeAlignment(this.gameObject);
        Vector3 cohesion = computeCohesion(this.gameObject);
        Vector3 separation = computeSeparation(this.gameObject);


        rb.velocity += new Vector3(alignment.x + cohesion.x + separation.x, 0, alignment.z + cohesion.z + separation.z) * Time.deltaTime;

        rb.velocity.Normalize();

        float xMath = alignment.x * alignmentWeight + cohesion.x * cohesionWeight + separation.x * separationWeight;
        float zMath = alignment.z * alignmentWeight + cohesion.z * cohesionWeight + separation.z * separationWeight;
        rb.velocity += new Vector3(xMath, 0, zMath)*Time.deltaTime;
    }


    public Vector3 computeAlignment(GameObject disEnemy)
        {

             Vector3 computedVec = Vector3.zero;

            int neighborCount = 0;
            

            foreach(GameObject disEne in eneGen.createdObjs)
            {
                if (disEne != this.gameObject)
                {
                    Vector3 offset = disEne.transform.position - transform.position;
                    float sqrLen = offset.sqrMagnitude;

                    if (sqrLen < distanceChk)
                    {

                        computedVec += new Vector3(disEne.GetComponent<Rigidbody>().velocity.x, 0, disEne.GetComponent<Rigidbody>().velocity.z);


                        neighborCount++;
                    }

                }

            }

        if (neighborCount == 0)
            return computedVec;


        computedVec = new Vector3(computedVec.x / neighborCount, 0 / neighborCount);

        computedVec.Normalize();


        return computedVec;
    }







    public Vector3 computeCohesion(GameObject disEnemy)
    {




        Vector3 computedVec = Vector3.zero;

        int neighborCount = 0;
      

        foreach (GameObject disEne in eneGen.createdObjs)
        {
            if (disEne != this.gameObject)
            {
                Vector3 offset = disEne.transform.position - transform.position;
                float sqrLen = offset.sqrMagnitude;

                if (sqrLen < distanceChk)
                {

                    computedVec += new Vector3(disEne.transform.position.x, 0, disEne.transform.position.z);


                    neighborCount++;
                }

            }

        }

        if (neighborCount == 0)
            return computedVec;


        computedVec = new Vector3(computedVec.x / neighborCount, 0, computedVec.z / neighborCount);

        computedVec = new Vector3(computedVec.x - target.transform.position.x, 0, computedVec.z - target.transform.position.z);

        computedVec.Normalize();
        return computedVec;
    }





    public Vector3 computeSeparation(GameObject disEnemy)
    {




        Vector3 computedVec = Vector3.zero;

        int neighborCount = 0;
      

        foreach (GameObject disEne in eneGen.createdObjs)
        {
            if (disEne != this.gameObject)
            {
                Vector3 offset = disEne.transform.position - transform.position;
                float sqrLen = offset.sqrMagnitude;

                if (sqrLen < distanceChk)
                {

                    computedVec += new Vector3(disEne.transform.position.x - target.transform.position.x, 0, disEne.transform.position.z - target.transform.position.z);
             

                    neighborCount++;
                }

            }

        }

        if (neighborCount == 0)
            return computedVec;


        computedVec = new Vector3(computedVec.x / neighborCount, 0, computedVec.z / neighborCount);

        computedVec = new Vector3(computedVec.x - target.transform.position.x, 0, computedVec.z - target.transform.position.z);

        computedVec *= -1;
        computedVec.Normalize();
        return computedVec;
    }




}
