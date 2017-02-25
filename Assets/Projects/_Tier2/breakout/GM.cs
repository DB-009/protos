using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour
{

    public int lives = 3;
    public int bricks = 20;
    public float resetDelay = 1f;
    public Text livesText;
    public GameObject gameOver;
    public GameObject youWon;
    public GameObject bricksPrefab;
    public GameObject hPad,vPad;
    public GameObject deathParticles;
    public static GM instance = null;

    public enum padStance {  hor, ver};
    public padStance stance;
    public GameObject hpad1,vpad1, hpad2, vpad2,ball;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Setup();
        
    }

    public void Update()
    {
        ControlSwap();
    }

    public void Setup()
    {
        hpad1 = Instantiate(hPad, transform.position - new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;
        hpad2 = Instantiate(hPad, transform.position - new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;

        hpad1.GetComponent<Paddle>().startY = 6.6f;
        hpad1.GetComponent<Paddle>().startX = -1.97f;

        hpad2.GetComponent<Paddle>().startY = -2f;
        hpad2.GetComponent<Paddle>().startX = -1.97f;

        vpad1 = Instantiate(vPad, transform.position - new Vector3(-5,0,0), Quaternion.identity) as GameObject;
        vpad1.GetComponent<Paddle>().startX = -5;

        vpad2 = Instantiate(vPad, transform.position - new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;
        vpad2.GetComponent<Paddle>().startX = 5;

        hpad1.SetActive(false);

        hpad2.SetActive(false);
        vpad1.SetActive(true);

        vpad2.SetActive(true);



        Instantiate(bricksPrefab, transform.position, Quaternion.identity);
        stance = padStance.hor;
        livesText.text = "Lives: " + lives;
    }

    void CheckGameOver()
    {


        if (bricks < 1)
        {
            //youWon.SetActive(true);
            Time.timeScale = .25f;
            Invoke("Reset", resetDelay);
        }

        if (lives < 1)
        {
           // gameOver.SetActive(true);
            Time.timeScale = .25f;
            Invoke("Reset", resetDelay);
        }



    }

    void Reset()
    {

        Time.timeScale = 1f;
        Application.LoadLevel(Application.loadedLevel);
    }

    void ResetBall()
    {
        ball.GetComponent<Ball>().rb.velocity = Vector3.zero;
        ball.transform.position = ball.GetComponent<Ball>().initPos;
        ball.GetComponent<Ball>().ballInPlay = false;
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;
        //Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);



        CheckGameOver();
        if(lives >= 1)
        {
            Debug.Log("Reset ball pos");
            ResetBall();
        }
    }

    void SetupPaddle()
    {

        hpad1.GetComponent<Paddle>().startY = 6.6f;
        hpad1.GetComponent<Paddle>().startX = -1.97f;

        hpad2.GetComponent<Paddle>().startY = -2f;
        hpad2.GetComponent<Paddle>().startX = -1.97f;

        vpad1 = Instantiate(vPad, transform.position - new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;
        vpad1.GetComponent<Paddle>().startX = -5;

        vpad2 = Instantiate(vPad, transform.position - new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;
        vpad2.GetComponent<Paddle>().startX = 5;

        hpad1.SetActive(false);

        hpad2.SetActive(false);

        vpad1.SetActive(true);

        vpad2.SetActive(true);


    }

    public void DestroyBrick()
    {
        bricks--;
        CheckGameOver();
    }


    public void ControlSwap()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(stance == padStance.hor)
            {

                stance = padStance.ver;

                hpad1.SetActive(false);

                hpad2.SetActive(false);
                vpad1.SetActive(true);

                vpad2.SetActive(true);

            }
            else if(stance == padStance.ver)
            {
                stance = padStance.hor;

                vpad1.SetActive(false);

                vpad2.SetActive(false);

                hpad1.SetActive(true);

                hpad2.SetActive(true);

            }
        }
    }


}