using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour {

    public LaneShift_TopDown player;

    public enum gunType
    {
        normal,
        rapidFire,
        spreadShot,
        triShot,
        hadoken,
    }

    public GameObject bulletPrefab;

    // Use this for initialization
    void Start () {
        player = this.gameObject.GetComponent<LaneShift_TopDown>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ShotDetect()
    {
        // Get the point along the ray that hits the calculated distance.
        Ray targetPoint = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = Vector3.zero;

        Vector3 pos = targetPoint.GetPoint(0);


        direction = pos - new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - player.yDis, Camera.main.transform.position.z);

        direction.Normalize();
        // Debug.Log("Dir = " + direction + " true x = " + direction.x + " true y = " + direction.y);
        int tempXdis = 0;
        if (direction.x < 0)
        {

            Debug.Log("Left");

        }
        else if (direction.x > 0)
        {


            Debug.Log("Right");


        }

    }




    public void CmdShoot(int dire)
    {


        float force = player.bulForce;

        GameObject bulz = bulletPrefab;



        if (player.curGun == gunType.normal)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z), player.myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (player.myTrans.right * dire) * player.bulForce;

        }
        else if (player.curGun == gunType.spreadShot)
        {
            var tileCreated = (GameObject)Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z), player.myTrans.rotation);
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z), player.myTrans.rotation);
            var tileCreated3 = (GameObject)Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z), player.myTrans.rotation);

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity *= player.bulForce * dire;

            tileCreated2.GetComponent<Rigidbody>().velocity *= player.bulForce * dire;


            tileCreated3.GetComponent<Rigidbody>().velocity *= player.bulForce * dire;

        }
        else if (player.curGun == gunType.triShot)
        {
            var tileCreated = GameObject.Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z) + new Vector3(0, -.5f, 0), Quaternion.identity) as GameObject;
            var tileCreated2 = (GameObject)Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z), player.myTrans.rotation);
            var tileCreated3 = GameObject.Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z) - new Vector3(0, -.5f, 0), Quaternion.identity) as GameObject;

            tileCreated.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated.GetComponent<projectileLife>().playerBullet = true;

            tileCreated2.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated2.GetComponent<projectileLife>().playerBullet = true;

            tileCreated3.GetComponent<projectileLife>().owner = this.gameObject;
            tileCreated3.GetComponent<projectileLife>().playerBullet = true;


            tileCreated.GetComponent<Rigidbody>().velocity = (player.myTrans.right * dire) * player.bulForce;

            tileCreated2.GetComponent<Rigidbody>().velocity = (player.myTrans.right * dire) * player.bulForce;


            tileCreated3.GetComponent<Rigidbody>().velocity = (player.myTrans.right * dire) * player.bulForce;


            if (player.gunDoubleUpgrade == true)
            {
                var tileCreated4 = GameObject.Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z) + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated4.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated4.GetComponent<projectileLife>().playerBullet = true;

                var tileCreated5 = GameObject.Instantiate(bulz, new Vector3(player.myTrans.position.x + dire, player.myTrans.position.y, player.myTrans.position.z) - new Vector3(0, 1, 0), Quaternion.identity) as GameObject;

                tileCreated5.GetComponent<projectileLife>().owner = this.gameObject;
                tileCreated5.GetComponent<projectileLife>().playerBullet = true;


                tileCreated4.GetComponent<Rigidbody>().velocity = (player.myTrans.right * dire) * player.bulForce;


                tileCreated5.GetComponent<Rigidbody>().velocity = (player.myTrans.right * dire) * player.bulForce;



            }


        }
        else if (player.curGun == gunType.hadoken)
        {

        }

        player.lastShotAt = Time.time;
    }




}
