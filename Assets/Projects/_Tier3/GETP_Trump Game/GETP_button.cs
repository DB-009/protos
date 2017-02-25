using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GETP_button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;
    public bool recurring;
    public GETP_Controller myHero;
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

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
//        Debug.Log("Mouse enter");
        if(myHero != null)
        {
        myHero.UIActions(actionID);
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



    }
}

