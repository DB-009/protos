using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour
{

    public bool charged;

    void OnTriggerEnter(Collider col)
    {
        GM.instance.LoseLife();
    }


    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ball")
        {
            Debug.Log("hey");
            if(charged == true)
            {
                col.gameObject.GetComponent<Ball>().rb.isKinematic = true;
                if (transform.position.x <= col.transform.position.x)
                {
                    Debug.Log("l");
                    col.transform.position = col.transform.position + new Vector3(-.005f,0,0);
                }
                else
                {
                    Debug.Log("r");
                    col.transform.position = col.transform.position + new Vector3(.005f, 0, 0);

                }
            }
        }
    }



    public void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "ball")
        {
            Debug.Log("hey");
            if (charged == true)
            {
                col.gameObject.GetComponent<Ball>().rb.isKinematic = true;
                if (transform.position.x <= col.transform.position.x)
                {
                    Debug.Log("l");

                }
                else
                {
                    Debug.Log("r");

                }
            }
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Yessir");
        charged = true;
    }

    void OnMouseUp()
    {
        Debug.Log("no sir");
        charged = false;
    }
}