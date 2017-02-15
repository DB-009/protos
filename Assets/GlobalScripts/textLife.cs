using UnityEngine;
using System.Collections;

public class textLife : MonoBehaviour
{


    public float created, explodeAt, lifeSpan;

    // Use this for initialization
    void Start()
    {

        created = Time.time;
        explodeAt = created + lifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= explodeAt)
        {

            Destroy(this.gameObject);
        }
        else
            this.transform.Translate(0, Time.deltaTime * 5, 0);
      

    }
}
