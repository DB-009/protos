using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour
{



    public Transform myTarget;
    public Vector3 camReposition;

    public GameObject activePlayer;
    

    public float xPos, yPos, zPos, angle;
    public float plat_xPos, plat_yPos, plat_zPos, plat_angle;
    public float topdown_xPos, topdown_yPos, topdown_zPos, topdown_angle;


    public bool canTrack;
    public enum CamType { topdown, firstPerson, platformer, thirdPerson };
    public CamType camMode;

    public bool staticX,staticY;

    void Start()
    {

    }


    void FixedUpdate()
    {




        if (camMode == CamType.thirdPerson)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            xPos = 0;
            yPos = 3;
            zPos = -8.5f;
        }
        else if (camMode == CamType.topdown)
        {
            transform.localEulerAngles = new Vector3(topdown_angle, 0, 0);
            /// yPos = 20;
            xPos = topdown_xPos;
            yPos = topdown_yPos;
             zPos = topdown_zPos;

        }
        else if (camMode == CamType.platformer)
        {
            transform.localEulerAngles = new Vector3(plat_angle, 0, 0);
            /// yPos = 20;
            xPos = plat_xPos;
            yPos = plat_yPos;
            zPos = plat_zPos;

        }

        //if targetting enemy
        if (myTarget != null && canTrack == true)
        {

            if (staticX == false)
            {
                  if (staticY == false)
                    camReposition = new Vector3(myTarget.position.x + xPos, myTarget.position.y + yPos, myTarget.position.z + zPos);
                  else
                camReposition = new Vector3(myTarget.position.x + xPos , this.transform.position.y , myTarget.position.z + zPos);

            }
          
            else
                camReposition = new Vector3(this.transform.position.x, this.transform.position.y, myTarget.position.z + zPos);



            transform.position = Vector3.Lerp(transform.position, camReposition, Time.deltaTime * 8);

        }


        ///////elaborate here


    }



    public void ZoomIn()
    {
        _touch player = activePlayer.GetComponent<_touch>();
        if (player.zoomLvl > 0)
        {
            player.zoomLvl--;


            player.curZ -= player.JumpZoomIncrementZ ;
            player.CurX -= player.JumpZoomIncrementX + .5f;
            player.CurY -= player.JumpZoomIncrementY + .7f;

            this.GetComponent<Camera>().orthographicSize = player.curZ;
            this.GetComponent<CameraTracking>().xPos = player.CurX;
            this.GetComponent<CameraTracking>().yPos = player.CurY;
        }

    }
    public void ZoomOut()
    {
        _touch player = activePlayer.GetComponent<_touch>();
        if (player.zoomLvl < 8)
        {

            player.zoomLvl++;
            //curZ += JumpZoomIncrementZ;
            //CurX += JumpZoomIncrementX;

            player.curZ += player.JumpZoomIncrementZ;
            player.CurX += player.JumpZoomIncrementX + .5f;
            player.CurY += player.JumpZoomIncrementY + .7f;


            this.GetComponent<Camera>().orthographicSize = player.curZ;
            this.GetComponent<CameraTracking>().xPos = player.CurX;
            this.GetComponent<CameraTracking>().yPos = player.CurY;

        }

    }

    public void ResetZoom()
    {
        _touch player = activePlayer.GetComponent<_touch>();




        player.curZ = player.initialZoomZ;
        player.CurX = player.initialZoomX;
        player.CurY = player.initialZoomY;
        this.GetComponent<Camera>().orthographicSize = player.curZ;
        this.GetComponent<CameraTracking>().xPos = player.CurX;
        this.GetComponent<CameraTracking>().yPos = player.CurY;
        player.zoomLvl = 2;

    }
}