using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_AtkPatternTracker : MonoBehaviour {


    [SerializeField]
    public Queue<playerAttack> nearPatterns = new Queue<playerAttack>(6);
    [SerializeField]
    public Queue<playerAttack> farPatterns = new Queue<playerAttack>(6);

    public List<playerAttack> tempNearPatterns = new List<playerAttack>();

    public List<playerAttack> tempFarPatterns = new List<playerAttack>();


    public void nearAtklanded(playerAttack.attackType atk)
    {

            nearPatterns.Enqueue(new playerAttack(Time.time, atk));
        tempNearPatterns.Clear();
        foreach(playerAttack disAtk in nearPatterns)
        {
            tempNearPatterns.Add(disAtk);
        }
    }

    public void farAtklanded(playerAttack.attackType atk)
    {
        farPatterns.Enqueue(new playerAttack(Time.time, atk));
        tempFarPatterns.Clear();
        foreach (playerAttack disAtk in farPatterns)
        {
            tempFarPatterns.Add(disAtk);
        }
    }



    
    

}
