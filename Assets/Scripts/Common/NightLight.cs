using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLight : MonoBehaviour {

    private static NightLight sInstance = null;
    public static NightLight Instance
    {
        get
        {
            return sInstance;
        }
    }

    private void Awake()
    {
        sInstance = this;
    }

    private void OnDestroy()
    {
        if (sInstance == this)
        {
            sInstance = null;
        }
    }

    // Use this for initialization
    void Start ()
    {
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
