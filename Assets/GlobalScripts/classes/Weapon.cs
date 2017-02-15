using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

   public int wepCount, wepType;


    public Weapon(int wepCnt,int type)
    {
        wepCount = wepCnt;
        wepType = type;
    }
}
