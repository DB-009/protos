using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour {
   
    public enum attackType { projectile, melee, aoe };
    public float time;
    [SerializeField]
    public attackType atktype;

    public playerAttack(float t, attackType atk)
    {
        time = t;
        atktype = atk;
    }
}
