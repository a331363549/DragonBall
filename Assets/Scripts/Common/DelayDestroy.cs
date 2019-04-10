using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour {

    public void ToDestory()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject, 1f);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
