using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class myFunctions : MonoBehaviour {

    // Use this for initialization
    public GameObject textPrefab;

    // The damage to show as a popup
    public void CreateDamagePopup(int damage,Transform damageTransform ,GameObject damagePrefab)
    {
        GameObject damageGameObject = (GameObject)Instantiate(damagePrefab,
                                                              damageTransform.position,
                                                              damageTransform.rotation);
        damageGameObject.transform.Rotate(new Vector3(90,0,0));

       TextMesh disText = damageGameObject.GetComponent<TextMesh>();

        disText.text = damage.ToString();

    }

    public void CreateTextPopup(string texto, Transform textTransform, GameObject textPrefab)
    {
       


        


    }

}
