using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

    public float ballInitialVelocity = 600f;


    public Rigidbody rb;
    public Vector3 initPos;
    public bool ballInPlay;

    void Awake()
    {

        rb = GetComponent<Rigidbody>();
        initPos = this.transform.position;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && ballInPlay == false)
        {
            transform.parent = null;
            ballInPlay = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector3(ballInitialVelocity, ballInitialVelocity, 0));
        }
    }




}