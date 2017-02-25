using UnityEngine;
using System.Collections;

public class TurnBasedBattlerClass : MonoBehaviour {

    public enum PlayerClasses
    {
        Fighter,
        Mage,
        Healer,
        Archer,
    }

    public PlayerClasses playerClass;
}
