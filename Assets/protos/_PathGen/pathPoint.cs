using UnityEngine;
using System.Collections;

public class pathPoint : MonoBehaviour {

    public enum ObjType { path, room, etc};
    public ObjType type;

    public enum PathDir { hor, ver, threeWay, fourWay, upRTurn, upLTurn, dwnRTurn, dwnLTurn };
    public PathDir dir;

    public GameObject owner;//the bug who created this path
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
