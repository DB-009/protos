using UnityEngine;
using System.Collections;

public class playerShipController : MonoBehaviour
{



    public float verSpd;
    public float horSpd;
    public float flightSpd;


    public Rigidbody rb;

    public float extraForce;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {




        horSpd = Input.GetAxis("Horizontal") * flightSpd;

        verSpd = Input.GetAxis("Vertical") * flightSpd;



    }

    //Built in unity function  runs on a fixed time scale cam be changed in setting. All physics based movements should go in here
    void FixedUpdate()
    {

        rb.AddForce(horSpd , 0, verSpd, ForceMode.Force);

    }


    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Enemy")
        {
            Destroy(gameObject);
        }

        if (col.transform.tag == "wall")
        {
            Destroy(gameObject);
        }
    }

}