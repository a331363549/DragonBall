using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        //    eulerAngles.z = -Time.deltaTime * 1000;
        //    loadingIcon.rectTransform.Rotate(eulerAngles);
    }
}
