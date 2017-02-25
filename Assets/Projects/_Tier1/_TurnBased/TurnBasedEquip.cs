using UnityEngine;
using System.Collections;

public class TurnBasedEquip : MonoBehaviour {

    public enum EquipObjType // eLeft, eRight, eHead, eBack, eFeet
    {
        weapon, shield, helm, armor, back , feet, acc
    }

    public EquipObjType objType;
    public GameObject disEquip;
}
