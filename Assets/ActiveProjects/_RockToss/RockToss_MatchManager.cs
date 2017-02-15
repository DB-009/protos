using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockToss_MatchManager : MonoBehaviour {

    public RockToss_GameManager gameStateManager;

    public GameObject challengeStage,canvas;

    public enum MatchType { RockToss_Challenge, LevelUp_Game, Travelling, Pvp };
    public MatchType matchType;

    public List<RockToss_Controller> createdPlayers = new List<RockToss_Controller>();

    public bool matchStarted;

    public GameObject playerPrefab;

    public Transform leftSpawn, rightSpawn;

    public CameraTracking leftCam, rightCam;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(matchStarted == true)
        {
            if(matchType == MatchType.RockToss_Challenge)
            {
     
            }
        }
    else
        {
            if(gameStateManager.gameState == RockToss_GameManager.GameState.InMatch)
            {
                Debug.Log("match started");


           
                CreatePlayer(rightSpawn);
                CreatePlayer(leftSpawn);

                matchStarted = true;
                Camera.main.gameObject.SetActive(false);
                gameStateManager.titleScreen.SetActive(false);
                canvas.SetActive(true);
                challengeStage.SetActive(true);
                Start_Challenge();
            }
        }
	}


    public void Start_Challenge()
    {
       

          EnableSprinter(createdPlayers[0]);
            EnableAttacker(createdPlayers[1]);        

    }


    public void CreatePlayer(Transform spawnPos)
    {

        GameObject spawnObj = GameObject.Instantiate(playerPrefab , spawnPos.position, Quaternion.identity) as GameObject;


        RockToss_Controller disPlayer = spawnObj.GetComponent<RockToss_Controller>();
        //Debug.Log("you either need to instantiate player or change gfx and set stats delete player1 and player 2 vars ");
        disPlayer.rb = disPlayer.GetComponent<Rigidbody>();
        disPlayer.myTrans = disPlayer.transform;
        disPlayer.hp = disPlayer.vit * disPlayer.lvl + disPlayer.baseHP;
        disPlayer.mp = disPlayer.intel * disPlayer.lvl + disPlayer.baseMp;
        disPlayer.mhp = disPlayer.hp;
        disPlayer.mmp = disPlayer.mp;


        disPlayer.initPos = disPlayer.transform.position;
        disPlayer.gameStateManager = gameStateManager;


        createdPlayers.Add(disPlayer);
    }



    public void EnableAttacker(RockToss_Controller rt_Player)
    {


        rt_Player.curMoveType = RockToss_Controller.MoveType.Attacker;
        //this.transform.position = pathListObjs[0].transform.position+new Vector3(0,1,0);
   

    

        rt_Player.cam = leftCam;

        rt_Player.cam.myTarget = rt_Player.transform;

        rt_Player.cam.transform.position = rt_Player.transform.position;
        rt_Player.cam.myTarget = rt_Player.transform;


        rt_Player.canMove = false;


    }


    public void EnableSprinter(RockToss_Controller rt_Player)
    {



        rt_Player.curMoveType = RockToss_Controller.MoveType.Sprinter;
        //this.transform.position = pathListObjs[0].transform.position+new Vector3(0,1,0);
   
         
        rt_Player.isController = true;
        rt_Player.canMove = true;


        rt_Player.cam = rightCam;
        rt_Player.cam.myTarget = rt_Player.transform;

     
        gameStateManager.activePlayer = rt_Player;

        rt_Player.cam.transform.position = rt_Player.transform.position;
        rt_Player.cam.myTarget = rt_Player.transform;




  



        rt_Player.hpSlider = rt_Player.gameStateManager.hpSlider1;

        rt_Player.cam.transform.position = rt_Player.transform.position;

        rt_Player.gameStateManager.left.myHero = rt_Player;
        rt_Player.gameStateManager.right.myHero = rt_Player;

        rt_Player.gameStateManager.shootLeft.myHero = rt_Player;
        rt_Player.gameStateManager.shootRight.myHero = rt_Player;

        foreach (GameObject disGen in rt_Player.gameStateManager.gens)
        {
            disGen.GetComponent<objGen>().targetPlayer = rt_Player.gameObject;
        }




        rt_Player.hpSlider.maxValue = rt_Player.mhp;
        rt_Player.hpSlider.value = rt_Player.hp;
        // rt_Player.canMove = true;


    }


}

