using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class casualButton  : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;
    public bool recurring;
    public LaneShift_TopDown myHero;
    public LaneShift_TopDown_NET myNetHero;
    public SlingShotPlayer tempHero;
    public int actionID;


    public void Update()
    {
        if(myHero!=null)
        {
            if (isOver == true && recurring == true)
            {
                myHero.inputController.UIActions(actionID);
            }
        }
        else if (myNetHero != null)
        {
            if (isOver == true && recurring == true)
            {
                myNetHero.UIActions(actionID);
            }
        }
        else
        {
            tempHero.UIActions(actionID);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
//        Debug.Log("Mouse enter");
        if(myHero != null)
        {
        myHero.inputController.UIActions(actionID);
        isOver = true;
        }
        else if (myNetHero != null)
        {
            myNetHero.UIActions(actionID);
            isOver = true;
        }
        else
        {
            tempHero.UIActions(actionID);
            isOver = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Mouse exit");
        if (myHero != null)
        {
            myHero.inputController.UIActions(actionID);
            isOver = false;
        }
        else if (myNetHero != null)
        {
        myNetHero.UIActions(actionID);
        isOver = false;
        }
        else
        {
            tempHero.UIActions(actionID);
            isOver = false;
        }


    }
}

