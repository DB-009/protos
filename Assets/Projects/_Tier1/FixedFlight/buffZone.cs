using UnityEngine;
using System.Collections;

public class buffZone : MonoBehaviour {

    public GameObject lane;
    public GameObject invisbleExploder;
    public GameObject fwdTarget,bkwrdTarget;
    public GameObject realTarget = null;
    public enum Type {  cannon , speedChange , obstacle , portal , depthChange };
    public Type type;
    public GameObject objGen;

    public float valChange1, valChange2;
    public bool inverted;

    //type = speed up, slow down, posion, stun

	// Use this for initialization
	void Awake () {
        if(lane)
        transform.position = new Vector3(this.transform.position.x, lane.transform.position.y, this.transform.position.z);


    }

    public void RunBuffPragma(Type disType, bool is2D, GameObject disObj)
    {


        Rigidbody obj = null;
        Rigidbody2D obj2 = null;

        if (is2D == false)
        {
            obj = disObj.GetComponent<Rigidbody>();


            if (disType == Type.speedChange)
            {
                obj.AddForce(new Vector3(valChange1 * 1.5f, 0, 0), ForceMode.Impulse);

            }
            else if (disType == Type.cannon)
            {
                SlingShotPlayer player = obj.GetComponent<SlingShotPlayer>();
                if (player != null)
                {
                    if (obj.isKinematic == false && Time.time >= player.LastCannonAt + player.timeBetweenCanon)
                    {


                        obj.useGravity = false;
                        obj.isKinematic = true;
                        Debug.Log("here1");
                        obj.transform.position = this.transform.position;
                    }
                    else if (obj.isKinematic == true)
                    {

                        bool isLAstCannon = false;


                        if ( Time.time >= player.LastCannonAt + player.timeBetweenCanon)
                        {
                            // obj.transform.position = targetObj.transform.position;
                            player.LastCannonAt = Time.time;
                            Debug.Log("here2");
                            //if last cannon == target objet shoot player to go back object



                            if (bkwrdTarget == null)//if in first cannon bkwrd target is false so go forward
                            {
                                Debug.Log("1");

                                realTarget = fwdTarget;
                                player.cannonBackward = false;
                            }
                            else if (player.lastCannon[1] == this.gameObject.transform.parent.gameObject)
                            {

                                if (player.cannonBackward == true)
                                {
                                    realTarget = fwdTarget;
                                    player.cannonBackward = false;
                                }
                                else
                                {
                                    realTarget = bkwrdTarget;
                                    player.cannonBackward = true;
                                }


                            }
                            else if (player.lastCannon[1] == fwdTarget)//if going backwards go backwards til end
                            {

                                realTarget = bkwrdTarget;
                                player.cannonBackward = true;

                            }
                            else if (fwdTarget != null)//if going forward keep going forward til end
                            {
                                realTarget = fwdTarget;
                                player.cannonBackward = false;
                            }
                            else if (fwdTarget == null)//if in last connon go backward to beginning
                            {
                                // realTarget = bkwrdTarget;
                                Debug.Log("2");
                                isLAstCannon = true;
                                player.cannonBackward = false;
                            }//change of direction

                            if (isLAstCannon == false)
                            {
                                Ray targetPoint = Camera.main.ScreenPointToRay(realTarget.transform.position);

                                Vector3 direction = Vector3.zero;

                                Vector3 pos = targetPoint.GetPoint(0);


                                direction = realTarget.transform.position - this.transform.position;

                                float sqrLen = direction.sqrMagnitude;


                                Quaternion targetRotation = Quaternion.LookRotation(pos - realTarget.transform.position);

                                // Smoothly rotate towards the target point.
                                this.transform.rotation = targetRotation;

                                obj.isKinematic = false;

                                player.lastCannon[0] = player.lastCannon[1];
                                player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                                obj.AddForce(new Vector3(direction.x, direction.y + 2, 0) * valChange1, ForceMode.Impulse);
                                player.dblJumped = false;
                            }
                            else
                            {
                                obj.isKinematic = false;
                                obj.useGravity = false;
                                float xDis = 0;
                                if (inverted)
                                    xDis = -valChange2;
                                else
                                    xDis = valChange2;

                                obj.AddForce(0, xDis, 0, ForceMode.Impulse);
                                player.lastCannon[0] = player.lastCannon[1];
                                player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                                player.dblJumped = false;
                                player.LastCannonAt = Time.time;
                            }


                        }
                        else if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            obj.isKinematic = false;
                            obj.useGravity = false;
                            float xDis = 0;
                            if (inverted)
                                xDis = -valChange2;
                            else
                                xDis = valChange2;

                            obj.AddForce(0, xDis, 0, ForceMode.Impulse);

                            player.LastCannonAt = Time.time;
                            player.lastCannon[0] = player.lastCannon[1];
                            player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                        }


                    }
                }
                else
                {
                    Debug.Log("seems like a computer landed in a cannon what sould I do?!");
                }
            }
        }
        else
        {
            obj2 = disObj.GetComponent<Rigidbody2D>();


            if (disType == Type.speedChange)
            {
                obj2.AddForce(new Vector2(valChange1 * 1.5f, 0), ForceMode2D.Impulse);

            }
            else if (disType == Type.cannon)
            {
                mf_player player = obj2.GetComponent<mf_player>();
                if (player != null)
                {
                    if (obj2.isKinematic == false && Time.time >= player.LastCannonAt + player.timeBetweenCanon)
                    {


                       // obj2.gravityScale = 0;
                        obj2.isKinematic = true;
                        obj2.velocity = Vector2.zero;
                        Debug.Log("here1");
                        obj2.transform.position = this.transform.position;
                    }
                    else if (obj2.isKinematic == true)
                    {

                        bool isLAstCannon = false;
                        obj2.velocity = Vector2.zero;


                        if ( Time.time >= player.LastCannonAt + player.timeBetweenCanon)
                        {
                            // obj2.transform.position = targetobj2.transform.position;
                            player.LastCannonAt = Time.time;
                            Debug.Log("here2");
                            //if last cannon == target objet shoot player to go back object



                            if (bkwrdTarget == null)//if in first cannon bkwrd target is false so go forward
                            {
                                Debug.Log("1");

                                realTarget = fwdTarget;
                                player.cannonBackward = false;
                            }
                            else if (player.lastCannon[1] == this.gameObject.transform.parent.gameObject)
                            {

                                if (player.cannonBackward == true)
                                {
                                    realTarget = fwdTarget;
                                    player.cannonBackward = false;
                                }
                                else
                                {
                                    realTarget = bkwrdTarget;
                                    player.cannonBackward = true;
                                }


                            }
                            else if (player.lastCannon[1] == fwdTarget)//if going backwards go backwards til end
                            {

                                realTarget = bkwrdTarget;
                                player.cannonBackward = true;

                            }
                            else if (fwdTarget != null)//if going forward keep going forward til end
                            {
                                realTarget = fwdTarget;
                                player.cannonBackward = false;
                            }
                            else if (fwdTarget == null)//if in last connon go backward to beginning
                            {
                                // realTarget = bkwrdTarget;
                                Debug.Log("2");
                                isLAstCannon = true;
                                player.cannonBackward = false;
                            }//change of direction

                            if (isLAstCannon == false)
                            {
                                Ray targetPoint = Camera.main.ScreenPointToRay(realTarget.transform.position);

                                Vector3 direction = Vector3.zero;

                                Vector3 pos = targetPoint.GetPoint(0);


                                direction = realTarget.transform.position - this.transform.position;

                                float sqrLen = direction.sqrMagnitude;


                                Quaternion targetRotation = Quaternion.LookRotation(pos - realTarget.transform.position);

                                // Smoothly rotate towards the target point.
                               // this.transform.rotation = targetRotation;

                                obj2.isKinematic = false;

                                player.lastCannon[0] = player.lastCannon[1];
                                player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                                obj2.AddForce(new Vector2(direction.x, direction.y + 4) * valChange1, ForceMode2D.Impulse);
                                player.dblJumped = true;
                            }
                            else
                            {
                                Debug.Log("Yoo");
                                obj2.isKinematic = false;
                               // obj2.gravityScale = 0;
                                float xDis = 0;
                                if (inverted)
                                    xDis = -valChange2;
                                else
                                    xDis = valChange2;

                                obj2.AddForce(new Vector2(0, xDis), ForceMode2D.Impulse);
                                player.lastCannon[0] = player.lastCannon[1];
                                player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                                player.dblJumped = false;
                                player.LastCannonAt = Time.time;
                                player.dblJumped = false;
                            }


                        }
                        else if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            Debug.Log("bug");
                            obj2.isKinematic = false;
                            //obj2.gravityScale = 0;
                            float xDis = 0;
                            if (inverted)
                                xDis = -valChange2;
                            else
                                xDis = valChange2;

                            obj2.AddForce(new Vector2(0, xDis), ForceMode2D.Impulse);

                            player.LastCannonAt = Time.time;
                            player.lastCannon[0] = player.lastCannon[1];
                            player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                        }


                    }
                }
                else
                {
                    Debug.Log("seems like a computer landed in a cannon what sould I do?!");
                }
            }
        }
    }
}
