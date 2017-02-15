using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class inputFields : MonoBehaviour {

    public int inputID;
    public controlledGameStateManager gameStateManager;
    public controlledStageGenerator stageGen;

        void Start()
        {
            var input = this.gameObject.GetComponent<InputField>();
            var se = new InputField.SubmitEvent();
            se.AddListener(SubmitName);
            input.onEndEdit = se;

       
            if (inputID == 0)
            {
            input.text = stageGen.xtiles.ToString();
            }
            else if (inputID == 1)
            {
            input.text = stageGen.ytiles.ToString();
        }
        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }

        private void SubmitName(string arg0)
        {
            Debug.Log(arg0);
            if(inputID == 0)
            {
                if (int.Parse(arg0) >= 3)
                    stageGen.xtiles = int.Parse(arg0);
                else
                    Debug.Log("Values must be greater than 3");
            }
            else if(inputID==1)
            {
                if (int.Parse(arg0) >= 3)
                      stageGen.ytiles = int.Parse(arg0);
                 else
                      Debug.Log("Values must be greater than 3");
             }

        }
    
}
