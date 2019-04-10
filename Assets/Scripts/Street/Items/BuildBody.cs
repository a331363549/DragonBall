using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBody : MonoBehaviour {

    public ShopData shopData = null;
    public GameObject faceRoot = null;
    public GameObject[] itemRoots = null;
    public GameObject[] flagsRoot = null;
    public GameObject[] adLeft = null;
    public GameObject[] adRight = null;
    public GameObject eventRoot = null;
    
    public ShopVRTrigger shopVRTrigger;

    private StreetEvent streetEvt;

    private List<GameObject> subObjs = new List<GameObject>();
    

    private readonly string[] ad_prefabs = new string[]
    {
        "Prefabs/Items/ad_KAN_JIA",
        "Prefabs/Items/ad_PIN_TUAN",
    };

    public void Fresh(ShopData shopData)
    {
        this.shopData = shopData;
        for (int idx = 0; idx < subObjs.Count; idx++)
        {
            BuildFace face = subObjs[idx].GetComponent<BuildFace>();
            if (face != null)
            {
                GameObject go = BuildFactory.Instance.CreateFace(shopData == null ? 0 : shopData.shop_info.industry_id);
                if (go != null)
                {
                    go.transform.SetParent(faceRoot.transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                    go.SetActive(true);
                    BuildFace newFace = go.GetComponent<BuildFace>();
                    if (newFace != null)
                    {
                        newFace.Init(shopData);
                    }
                    BuildFactory.Instance.RecycleGO(subObjs[idx]);
                    subObjs[idx] = go;
                }
                else
                {
                    face.Init(shopData);
                }
                continue;
            }

            StoreFlag storeFlag = subObjs[idx].GetComponent<StoreFlag>();
            if (storeFlag != null)
            {
                storeFlag.SetStoreInfo(shopData != null ? shopData.shop_info : null);
            }
        }

        InitVRFlag(shopData);
        CreateActivityTag(shopData);
        CreateStreetEvent(shopData);
    }

    public void Init(ShopData shopData)
    {
        this.shopData = shopData;
        for (int idx = 0; idx < subObjs.Count; idx++)
        {
            BuildFactory.Instance.RecycleGO(subObjs[idx]);
        }
        subObjs.Clear();

        CreateFace(shopData);
        CreateFlag(shopData);
        CreateLattern(shopData);
        CreateActivityTag(shopData);
        CreateStreetEvent(shopData);

        InitVRFlag(shopData);
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
	
	// Update is called once per frame
	void Update () {

        ShowSubObjInLOD();
        ShowFaceInLOD();
        ShowStreetEvtInLOD();
    }

    private void OnDisable()
    {
        if (streetEvt != null &&
            streetEvt.gameObject == StreetManager.Instance.StreetEvtObj &&
            StreetManager.Instance.StreetEvtObj != null)
        {
            streetEvt.gameObject.SetActive(false);
            StreetManager.Instance.StreetEvtObj = null;
        }
    }

    private void CreateFace(ShopData shopData)
    {
        // face
        GameObject face = BuildFactory.Instance.CreateFace(shopData == null ? 0 : shopData.shop_info.industry_id);
        if (face != null)
        {
            face.transform.SetParent(faceRoot.transform);
            face.transform.localPosition = Vector3.zero;
            face.transform.localRotation = Quaternion.identity;
            face.SetActive(true);
            BuildFace buildFace = face.GetComponent<BuildFace>();
            //face位置调整
            buildFace.transform.localScale = Vector3.one;
            buildFace.storeName.transform.localScale = transform.parent.localScale;
            if (buildFace != null)
            {
                buildFace.Init(shopData);
            }
            subObjs.Add(face);
        }
        else
        {
            Debug.LogError(shopData.shop_info.industry_id);
        }
    }

    private void InitVRFlag(ShopData shopData)
    {
        if (shopData == null || shopData.shop_info == null || string.IsNullOrEmpty(shopData.shop_info.c_vr_url))
        {
            shopVRTrigger.gameObject.SetActive(false);
        }
        else if (string.IsNullOrEmpty(shopData.shop_info.c_vr_url))
        {
            shopVRTrigger.gameObject.SetActive(false);
        }
        else
        {
            shopVRTrigger.gameObject.SetActive(true);
            shopVRTrigger.transform.localScale = transform.parent.localScale;
            shopVRTrigger.shopName = shopData.shop_info.c_name;
            shopVRTrigger.shopUcode = shopData.shop_info.c_ucode;
            shopVRTrigger.isVip = shopData.shop_info.c_is_vip_vr == "1";
        }
    }

    private void CreateFlag(ShopData shopData)
    {
        // flag
        string nameDesc = shopData == null ? "" : shopData.shop_info.c_name;
        for (int idx = 0; idx < flagsRoot.Length; idx++)
        {
            GameObject flag;
            if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                flag = BuildFactory.Instance.CreateStoreFlag(nameDesc.Length);
            else
                flag = BuildFactory.Instance.CreateStoreModenFlag(nameDesc.Length);
            flag.transform.SetParent(flagsRoot[idx].transform);
            flag.transform.localPosition = Vector3.zero;
            flag.transform.localRotation = Quaternion.identity;
            flag.transform.localScale = Vector3.one;
            flag.SetActive(false);
            StoreFlag storeFlag = flag.GetComponent<StoreFlag>();
            storeFlag.SetStoreInfo(shopData != null ? shopData.shop_info : null);
            subObjs.Add(flag);
        }
    }

    private void CreateLattern(ShopData shopData)
    {

        // lanter
        for (int idx = UnityEngine.Random.Range(0, 100) < 40 ? 0 : 3; idx < itemRoots.Length; idx++)
        {
            GameObject item = BuildFactory.Instance.CreateItem(itemRoots[idx].name);
            item.transform.SetParent(itemRoots[idx].transform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.SetActive(false);
            subObjs.Add(item);
        }
    }

    private void CreateActivityTag(ShopData shopData)
    {
        if(adLeft.Length<=0 || adRight.Length<=0)
        // shop activity
        if (shopData != null && shopData.active_info != null)
        {
            string ad_id;
            if (string.IsNullOrEmpty(shopData.active_info.ad_id))
            {
                ad_id = ad_prefabs[UnityEngine.Random.Range(0, ad_prefabs.Length)];
                shopData.active_info.ad_id = ad_id;
            }
            else
            {
                ad_id = shopData.active_info.ad_id;
            }
                if (adLeft.Length > 0)
                {
                    GameObject goLeft = GOPool.Instance.PopGO(ad_id + "_left");
                    goLeft.transform.SetParent(adLeft[0].transform);
                    goLeft.transform.localPosition = Vector3.zero;
                    goLeft.transform.localRotation = Quaternion.identity;
                    goLeft.transform.localScale = Vector3.one;
                    subObjs.Add(goLeft);
                }
                if (adRight.Length > 0)
                {
                    GameObject goRight = GOPool.Instance.PopGO(ad_id + "_right");
                    goRight.transform.SetParent(adRight[0].transform);
                    goRight.transform.localPosition = Vector3.zero;
                    goRight.transform.localRotation = Quaternion.identity;
                    goRight.transform.localScale = Vector3.one;
                    subObjs.Add(goRight);
                }
        }
    }

    private void CreateStreetEvent(ShopData shopData)
    {
        if (shopData == null || 
            shopData.event_info == null || 
            shopData.event_info.c_type == null ||
            shopData.event_info.hasTrigger)
        {
            return;
        }
        GameObject streetEvent = GOPool.Instance.PopGO("UIStreetEvent");
        if (streetEvent != null)
        {
            streetEvent.transform.SetParent(eventRoot.transform);
            streetEvent.transform.localPosition = Vector3.zero;
            streetEvent.transform.rotation = Quaternion.identity;
            streetEvent.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
            streetEvt = streetEvent.GetComponent<StreetEvent>();
            if (streetEvt != null)
            {
                streetEvt.InitSlide(shopData, transform.position.x <= 0);
                //文本信息位置调整
                //streetEvt.bgImg.transform.localScale = Vector3.one;
            }
            else
            {
                GOPool.Instance.PushGO(streetEvent);
            }
        }
    }

    private const float MAX_EVTUI_VISIBLE_DIST = 40;
    private const float MIN_EVTUI_VISIBLE_DIST = 10f;
    private void ShowStreetEvtInLOD()
    {
        if (streetEvt == null || UserCurState.Instance.EnableMove == false)
        {
            return;
        }

        if (streetEvt.gameObject != StreetManager.Instance.StreetEvtObj &&
            StreetManager.Instance.StreetEvtObj != null)
        {
            streetEvt.gameObject.SetActive(false);
            return;
        }

        if (streetEvt.gameObject.activeInHierarchy)
        {
            if (streetEvt.transform.position.z < MIN_EVTUI_VISIBLE_DIST || streetEvt.transform.position.z > MAX_EVTUI_VISIBLE_DIST)
            {
                streetEvt.Hide();
                if (streetEvt.transform.position.z < MIN_EVTUI_VISIBLE_DIST)
                {
                    shopData.event_info.hasTrigger = true;
                }
            }
            if (streetEvt.gameObject.activeInHierarchy == false)
            {
                StreetManager.Instance.StreetEvtObj = null;
            }
        }
        else
        {
            if (shopData.event_info.hasTrigger == false && streetEvt.transform.position.z < MAX_EVTUI_VISIBLE_DIST && streetEvt.transform.position.z > MIN_EVTUI_VISIBLE_DIST)
            {
                streetEvt.Pop();
            }
            if (streetEvt.gameObject.activeInHierarchy)
            {
                StreetManager.Instance.StreetEvtObj = streetEvt.gameObject;
            }
        }
    }

    private const float MAX_SUBOBJ_VISIBLE_DIST = 80f;
    private const float MIN_SUBOBJ_VISIBLE_DIST = -8f;
    private void ShowSubObjInLOD()
    {
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

    private const float MAX_FACE_VISIBLE_DIST = 80f;
    private const float MIN_FACE_VSIIBLE_DIST = -8f;
    private void ShowFaceInLOD()
    {
        if (faceRoot.activeInHierarchy)
        {
            if (Mathf.Abs(faceRoot.transform.eulerAngles.y - 0f) < float.Epsilon)
            {
                return;
            }
            if (faceRoot.transform.position.z > MAX_FACE_VISIBLE_DIST || faceRoot.transform.position.z < MIN_FACE_VSIIBLE_DIST)
            {
                faceRoot.SetActive(false);
            }
        }
        else
        {
            if (Mathf.Abs(faceRoot.transform.eulerAngles.y - 0f) < float.Epsilon)
            {
                faceRoot.SetActive(true);
                return;
            }
            if (faceRoot.transform.position.z < MAX_FACE_VISIBLE_DIST && faceRoot.transform.position.z > MIN_FACE_VSIIBLE_DIST)
            {
                faceRoot.SetActive(true);
            }
        }
    }
}
