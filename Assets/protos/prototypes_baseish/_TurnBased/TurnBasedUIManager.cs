using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;


public class TurnBasedUIManager : MonoBehaviour {

    public BSys_1 battleSystem1;

    public Text TitleScreen,battleScreen;
    public GameObject battleMenu, bs_skillMenu;
    public RectTransform bs_skillMenuTrans;

    public List<Button> battleButtons = new List<Button>();
    public GameObject myButtonPrefab;

    public float screenW,screenH;

    public float bs_padding;
    public float updatePhase;
    // Use this for initialization
    void Awake () {

        screenW = Screen.width;
        screenH = Screen.height;

    }

    public void Update()
    {
        
             TurnBasedBS(battleSystem1.curObjTurn);
    }

    public void TurnBasedBS(TurnBasedBattleObject curPlayer)//temp functions
    {


                if (battleSystem1.state == BSys_1.BS_States.off)
                {

                    battleScreen.gameObject.SetActive(false);
                    TitleScreen.gameObject.SetActive(true);



                }
                else if (battleSystem1.state == BSys_1.BS_States.on)
                {
            
                        TitleScreen.gameObject.SetActive(false);

                        battleScreen.text = "It is player #" + (battleSystem1.playerTurn + 1) + "'s Turn Press Enter to attack";
                        battleScreen.gameObject.SetActive(true);

                        
                    
                



                }
                else if (battleSystem1.state == BSys_1.BS_States.wait)
                {
                    TitleScreen.gameObject.SetActive(false);

                    battleScreen.text = "waiting...";
                    battleScreen.gameObject.SetActive(true);

                }
                else if (battleSystem1.state == BSys_1.BS_States.endgame)
                {
                    TitleScreen.gameObject.SetActive(false);
                    if (Time.time < battleSystem1.endgamePhase)
                        battleScreen.text = "Player number " + (battleSystem1.winner.myID + 1) + "(" + battleSystem1.winner.battlerClass + ") Won!! (" + (battleSystem1.endgamePhase - Time.time) + ")secs";
                    else
                        battleScreen.text = "Player number " + (battleSystem1.winner.myID + 1) + "(" + battleSystem1.winner.battlerClass + ") Won!! Press F to start Over";


                    battleScreen.gameObject.SetActive(true);

                }

        

    }


    public void DisableButton(Button but)
    {
        but.interactable = false;
    }





    public void battleButton(myButton disBut)
    {
        // Player curPlayer = matchPhaser.battlers[matchPhaser.playerTurn].GetComponent<Player>();

        if (disBut.id == 0)
        {

            Debug.Log("select target mode here on select run this line of code on bottom");
            //
            battleSystem1.curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.choosingTarget;

            bs_skillMenu.gameObject.SetActive(false);
        }
        else if (disBut.id == 1)
        {

            battleSystem1.curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.defending;
            battleSystem1.SwitchTurn();

        }
        else if (disBut.id == 2)
        {
            BS_SkillMenu();
            updatePhase = 0;

        }
        else if (disBut.id == 3)
        {

        }


    }



    public void ResetBattleButtons(TurnBasedPlayer curPlayer)
    {

        foreach (Button disButton in battleButtons)
        {
            


                if (disButton.GetComponent<myButton>().id == 2 && curPlayer.mp >=1 )//if player has mp allow him to access magic menu
                {
                    disButton.interactable = true;
                }
                else
                {
                    disButton.interactable = true;
                }
            
        }
    }

    public void DisableBattleButtons()
    {

        foreach (Button disButton in battleButtons)
        {
            disButton.interactable = false;
        }
    }





    public void BS_SkillMenu()
    {
        if(updatePhase == 1 || updatePhase == 0)
        {
            bs_skillMenu.SetActive(true);
            battleSystem1.curObjTurn.turnPhase = TurnBasedBattleObject.TurnPhase.skillMenu;
            foreach (Transform child in bs_skillMenu.transform)
            {
                if(child.GetComponent<Button>() != null)
                GameObject.Destroy(child.gameObject);
            }

           
            RectTransform rectg = bs_skillMenu.GetComponent<RectTransform>();
   



            float buttonHeight = myButtonPrefab.GetComponent<RectTransform>().rect.height;
            float buttonWidth = myButtonPrefab.GetComponent<RectTransform>().rect.width;

            float scrollViewH = bs_skillMenu.GetComponent<RectTransform>().rect.height;
            float scrollViewW = bs_skillMenu.GetComponent<RectTransform>().rect.width;

            for (int i = 0; i < 10; i++)
            {
                GameObject goButton = (GameObject)Instantiate(myButtonPrefab);
                goButton.transform.SetParent(bs_skillMenuTrans.GetChild(0).GetChild(0).GetChild(0), false);

                goButton.transform.localScale = new Vector3(1, 1, 1);

    
                     goButton.transform.localPosition = new Vector3(0 + (buttonWidth/2)   , (0  ) - (i * buttonHeight) - bs_padding*5, 0);


                Button tempButton = goButton.GetComponent<Button>();
                int tempInt = i;

                tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
            }

            battleSystem1.curObjTurn.updatePhase = 1;
           


        }

       
       
    }

    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button clicked = " + buttonNo);
    }
}
