using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RockToss_button  : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;
    public bool recurring;
    public RockToss_Controller myHero;
    public RockToss_Controller_NET myNetHero;
    public int actionID;


    public void Update()
    {
        if(myHero!=null)
        {
            if (isOver == true && recurring == true)
            {
                myHero.UIActions(actionID);
            }
        }
        else if (myNetHero != null)
        {
            if (isOver == true && recurring == true)
            {
                myNetHero.UIActions(actionID);
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
//        Debug.Log("Mouse enter");
        if(myHero != null)
        {
        myHero.UIActions(actionID);
        isOver = true;
        }
        else if (myNetHero != null)
        {
            myNetHero.UIActions(actionID);
            isOver = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Mouse exit");
        if (myHero != null)
        {
            myHero.UIActions(actionID);
            isOver = false;
        }
        else if (myNetHero != null)
        {
        myNetHero.UIActions(actionID);
        isOver = false;
        }


    }
}

