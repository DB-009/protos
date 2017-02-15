using UnityEngine;
using System.Collections;

public class itemBuff : MonoBehaviour
{

    public int itemID,itemCount, buffTime;


    public itemBuff(int id, int cnt, int bufTime)//buf time 0 = infinite
    {
        itemID = id;
        itemCount = cnt;
        buffTime = bufTime;
    }
}
