using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{

    public enum dirType { hor,ver };
    public dirType paddleDir;
    public float paddleSpeed = 1f,startY,startX;


    private Vector3 playerPos;

    public void Awake()
    {
    
    }

    void Update()
    {
        float yPos = transform.position.y + (Input.GetAxis("Vertical") * paddleSpeed);
        Debug.Log(yPos);
        float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);

        if(paddleDir == dirType.hor)
            playerPos = new Vector3(Mathf.Clamp(xPos, startX-2, startX+7), startY -1, 5f);

        if (paddleDir == dirType.ver)
            playerPos = new Vector3(startX, Mathf.Clamp(yPos, startY - 2.5f, startY + 6.5f), 5f);


        transform.position = playerPos;

    }
}