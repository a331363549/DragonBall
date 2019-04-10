using NewEngine.Framework.ObjAction;
using NewEngine.Logic.ObjAction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour {

    public GameObject npcPrefab;
    public Rect range;
    public bool loadOnPlay;
    public int maxNum;

    public float speed;
    public GameObject[] npcs;
    
    private Vector3 newPos = Vector3.zero;
    public Vector3 NewPos
    {
        set
        {
            newPos = value;
        }
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		for (int idx = 0; idx < npcs.Length; ++idx)
        {
            npcs[idx].transform.localPosition = npcs[idx].transform.localPosition + Vector3.back * Time.deltaTime * speed - newPos;
            if (npcs[idx].transform.localPosition.z < 0)
            {
                npcs[idx].transform.localPosition = npcs[idx].transform.localPosition + Vector3.forward * 96f;
            }
        }
        newPos = Vector3.zero;

    }
}
