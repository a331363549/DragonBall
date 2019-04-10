using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildFace : MonoBehaviour {

    public Text storeName = null;
    public ShopData shopData = null;

    public GameObject[] sceneItemsRoot = null;
    public ShopHomeTrigger shopHomeTrigger;
    private List<GameObject> subObjs = new List<GameObject>();


    public void Init(ShopData shopData)
    {
        this.shopData = shopData;
        for (int idx = 0; idx < subObjs.Count; idx++)
        {
            BuildFactory.Instance.RecycleGO(subObjs[idx]);
        }
        subObjs.Clear();

        CreateDecoration(shopData);
        InitVRFlag(shopData);
        storeName.text = shopData == null ? "" : shopData.shop_info.c_name;
    }

    public void Release()
    {
        for (int idx = 0; idx < subObjs.Count; idx++)
        {
            BuildFactory.Instance.RecycleGO(subObjs[idx]);
        }
        subObjs.Clear();
    }

    // Use this for initialization
    void Start () {
		
	}

    private const float MAX_SUBOBJ_VISIBLE_DIST = 30f;
    private const float MIN_SUBOBJ_VISIBLE_DIST = -8f;
    // Update is called once per frame
    void Update () {

        for (int idx = 0; idx < subObjs.Count; idx++)
        {
            if (subObjs[idx].activeInHierarchy)
            {
                if (subObjs[idx].transform.position.z > MAX_SUBOBJ_VISIBLE_DIST || subObjs[idx].transform.position.z < MIN_SUBOBJ_VISIBLE_DIST)
                {
                    subObjs[idx].SetActive(false);
                }
            }
            else
            {
                if (subObjs[idx].transform.position.z < MAX_SUBOBJ_VISIBLE_DIST && subObjs[idx].transform.position.z > MIN_SUBOBJ_VISIBLE_DIST)
                {
                    subObjs[idx].SetActive(true);
                }
            }
        }
    }

    private void InitVRFlag(ShopData shopData)
    {
        if (shopData == null || shopData.shop_info == null)
        {
            shopHomeTrigger.gameObject.SetActive(false);
        }
        else
        {
            shopHomeTrigger.gameObject.SetActive(true);
            shopHomeTrigger.targetId = shopData.shop_info.c_ucode;
        }
    }

    private void CreateDecoration(ShopData shopData)
    {
        if (shopData == null)
        {
            return;
        }
        float itemSpace = 0;
        for (int idx = 0; idx < sceneItemsRoot.Length; idx++)
        {
            BoxCollider splot = sceneItemsRoot[idx].GetComponent<BoxCollider>();
            float leftSpace = splot.size.x;
            float costSpace = 0;
            if (idx == 0 && shopData.event_info != null && shopData.event_info.hasTrigger == false)
            {
                GameObject go = BuildFactory.Instance.CreateTrigger(GetEventTrigger(shopData.event_info.c_type), out itemSpace);
                if (go != null)
                {
                    leftSpace -= itemSpace + 1;
                    subObjs.Add(go);
                    go.transform.SetParent(splot.transform);
                    go.transform.localPosition = new Vector3(-splot.size.x * 0.5f + costSpace + itemSpace * 0.5f, 0, splot.center.z - 1);
                    Vector3 angle = UserCamera.Instance.GameCamera.transform.eulerAngles;
                    angle.z = angle.x = 0;
                    go.transform.eulerAngles = angle;
                    costSpace += itemSpace;
                    go.SetActive(false);
                }
            }
            while (leftSpace > 0)
            {
                GameObject go = BuildFactory.Instance.CreateFaceDeco(out itemSpace);
                if (go == null)
                {
                    break;
                }
                leftSpace -= itemSpace;
                subObjs.Add(go);
                go.transform.SetParent(splot.transform);
                go.transform.localPosition = new Vector3(-splot.size.x * 0.5f + costSpace + itemSpace * 0.5f, 0, splot.center.z);
                go.transform.localRotation = Quaternion.identity;
                costSpace += itemSpace;
                go.SetActive(false);
            }
        }
    }

    private int GetEventTrigger(string eventType)
    {
        if (eventType == ShopEventType.BENEFIT.ToString())
        {
            return (int)ShopEventType.BENEFIT;
        }
        else if (eventType == ShopEventType.ENTERTAINMENT.ToString())
        {
            return (int)ShopEventType.ENTERTAINMENT;
        }
        else if (eventType == ShopEventType.PROMOTION.ToString())
        {
            return (int)ShopEventType.PROMOTION;
        }
        return -1;
    }
    
}
