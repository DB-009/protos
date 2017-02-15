using UnityEngine;
using System.Collections;

public class interactableObject : MonoBehaviour {

    public OneTap_StateManager gameStateManager;
    public int id;


    void OnMouseDown()
    {
      

  
            gameStateManager.clicks += 1;


            this.gameObject.SetActive(false);

            gameStateManager.myObjectPooler.Add(this.gameObject);

            gameStateManager.myInteractbleObjects.Remove(this.gameObject);

            gameStateManager.ObjClicked();
        



    }
}
