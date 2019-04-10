using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObj : MonoBehaviour {
    public string ResId { get; set; }
    public float LOD { get; set; }
    protected bool initialized = false;
    protected GameObject go = null;

    public virtual void InitGO()
    {
        initialized = true;
        go = GOPool.Instance.PopGO(ResId);
        if (go != null)
        {
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.layer = gameObject.layer;
        }
    }

    // Use this for initialization
    void Start () {

        if (initialized == false && Mathf.Abs(UserCamera.Instance.transform.position.z - transform.position.z) < LOD)
        {
            InitGO();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (initialized == false && Mathf.Abs(UserCamera.Instance.transform.position.z - transform.position.z) < LOD)
        {
            InitGO();
        }
    }
}
