using UnityEngine;
using System.Collections;

public class mapItem : MonoBehaviour
{

    public int itemID, itemType;
    public Vector3 initialPos;


    public mapItem(int id, int type,Vector3 initPos)
    {
        itemID = id;
        itemType = type;
        initialPos = initPos;
    }
}
