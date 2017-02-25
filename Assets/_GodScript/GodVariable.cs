using UnityEngine;
using System.Collections;
using System.Reflection;
public class GodVariable : MonoBehaviour {

    //thanks to advice posted by vexe and fogsight in StackOverflow I was able to make the core logic
    //http://answers.unity3d.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html?page=1&pageSize=5&sort=votes
    //^^if anyone wants to learn 


    public GameObject myGod, myVessel, disVessel;//these are temp variables normally you would call this from outside this script
    //god variable is the one you copy components from 
    //(ideally you can have a array of scripts/components and read from that im basing this off prefabs for my idea
    //disVessel is empty model that will get component

	// Use this for initialization
	void Awake () {

        disVessel = (GameObject)Instantiate(myVessel, Vector3.zero, Quaternion.identity); //create a Empty Player or vehicle model

        var disGod = GodClass(new string[] { "hey", "GETP_Controller", "hi" },disVessel, myGod  ) ;
        //the string are possible classes  to detect, disVessel being the object thats getting compnent, my
        


    }

    //public System.Type GodClass<T>(string[] possibleClasses, GameObject vessel, GameObject god ,T scriptType) where T : Component
    //public System.Type GodClass(string[] possibleClasses, GameObject vessel, GameObject god )
   public Component GodClass(string[] possibleClasses, GameObject vessel, GameObject god ) 
    {
        //bool hasComponent; // boolean to see if object actually has the component if not null is returned


        for (int i = 0; i < possibleClasses.Length; i++)//iteration through string of component Types
        {
            string x = possibleClasses[i];//store component name in temp var
           // Debug.Log("check " + i);
            if (god.GetComponent(x) != null)
            {
               // hasComponent = true;//has compnent is true so we will return something
               // Debug.Log("Its working! its working!");
               // Debug.Log(x);

                Component disCom = god.GetComponent(x);//grab the first instance of component in this object you were searching for
                CopyComponent(disCom , vessel);//copy component via reflection


                return disCom;//return the component 
            }
        }

        return null;//null return if God object doesnt have this component
    }




    void CopyComponent(Component component, GameObject target)
    {
        System.Type type = component.GetType();
        target.AddComponent(type);
        PropertyInfo[] propInfo = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
        foreach (var property in propInfo)
        {
            if (property.Name == "rect") continue;
            property.SetValue(target.GetComponent(type), property.GetValue(component, null), null);
        }
    }

}

