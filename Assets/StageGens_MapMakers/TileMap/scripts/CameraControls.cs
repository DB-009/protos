using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{


    public controlledGameStateManager gameStateManager;
    public controlledStageGenerator stageGen;

    public Vector3 camReposition;
    public Vector3 initialPosition;//update Initial Position with functions?

    public float xPos, yPos, zPos;
    public float panSpd, panH, panV,panZ;
    public float timeSinceLastMove,allowedStopTime;

    public Camera disCam;

    public bool canControl;


    void Start()
    {
        initialPosition = this.transform.position;
         disCam = this.GetComponent<Camera>();
    }

    void Update()
    {
        if (canControl == true)
        {
            panH = 0;
            panZ = 0;
            panV = 0;

            float allowedLeft = -3;
            float allowedRight = stageGen.xtiles * stageGen.tileScale;

            float allowedTop = stageGen.ytiles * stageGen.tileScale;
            float allowedBot = -3;

                if (Input.GetKey(KeyCode.A))
                {
                    if (disCam.transform.position.x > allowedLeft)
                         panH -= panSpd * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    if (disCam.transform.position.x < allowedRight)
                        panH += panSpd * Time.deltaTime;

                }
            





                if (Input.GetKey(KeyCode.W))
                {
                    if ( disCam.transform.position.z < allowedTop)
                        panV += panSpd * Time.deltaTime;//topDown camera change for Y axis

                }
                else if (Input.GetKey(KeyCode.S))//topDown camera change for Y axis
                {
                    if (disCam.transform.position.z > allowedBot)
                          panV -= panSpd * Time.deltaTime;

                }
            


            if (panH == 0 && panV == 0 && panZ == 0)
            {
                timeSinceLastMove += Time.deltaTime;
            }
            else
            {
                timeSinceLastMove = 0;
            }

        }
       


    }

    void FixedUpdate()
    {
        if (canControl == true)
        {
            if(panH != 0 || panV !=0 )
            {
                camReposition = new Vector3(panH, panV, panZ);
               disCam.transform.Translate(camReposition);

            }
            if (timeSinceLastMove >= allowedStopTime)
            {
                disCam.transform.position = initialPosition;
            }
        }

     }


}