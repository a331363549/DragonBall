using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObj : MonoBehaviour {

    public float lod = 100;
    public GameObject[] lightGO = null;
    public MeshRenderer[] lightMesh = null;
    public Material dayMat;
    public Material nightMat;

    public bool LightVisible
    {
        get;set;
    }

	// Use this for initialization
	void Start () {
        for (int idx = 0; idx < lightGO.Length; idx++)
        {
            lightGO[idx].SetActive(false);
        }
        LightVisible = false;
        OnDayNightSwitch(EnvironmentMng.IsDay);
        ListenerDriver.AddListener<bool>((int)BCListenerId.DayNightSwitch, OnDayNightSwitch);
    }

    private void OnDestroy()
    {
        ListenerDriver.RemoveListener<bool>((int)BCListenerId.DayNightSwitch, OnDayNightSwitch);
    }

    private void OnDayNightSwitch(bool isDay)
    {
        enabled = !isDay;
        for (int idx = 0; lightGO != null && idx < lightGO.Length; idx++)
        {
            lightGO[idx].SetActive(enabled);
        }
        Material curMat = isDay ? dayMat : nightMat;
        for (int idx = 0; lightMesh != null && idx < lightMesh.Length; idx++)
        {
            lightMesh[idx].sharedMaterial = curMat;
        }
    }

    private static float dist = 0f;
	// Update is called once per frame
	void Update () {
        if (lightGO == null)
        {
            return;
        }
        for (int idx = 0; idx < lightGO.Length; idx++)
        {
            dist = Vector3.Distance(lightGO[idx].transform.position, UserCamera.Instance.GameCamera.transform.position);
            if (lightGO[idx].activeInHierarchy)
            {
                if (dist > lod || dist < -8)
                {
                    lightGO[idx].SetActive(false);
                }
            }
            else
            {
                if (dist < lod && dist > -8)
                {
                    lightGO[idx].SetActive(true);
                }
            }
        }
    }
}
