using NewEngine;
using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    private GameObject storeMap;
    private List<GameObject> streetObjs = new List<GameObject>();
    private bool isEntrance;

    public float streetLength = 130.8f;

    public StreetSaveData GetStreetSaveData()
    {
        StreetSaveData streetSaveData = new StreetSaveData();
        for (int i = 0; i < streetObjs.Count; i++)
        {
            if (streetObjs[i].transform.parent.gameObject.layer == (int)LayerId.Building)
            {
                BuildBody build = streetObjs[i].GetComponent<BuildBody>();
                if (build != null)
                {
                    if (build.shopData != null)
                    {
                        build.shopData.buildBodyId = build.gameObject.name;
                    }
                    streetSaveData.shopDatas.Enqueue(build.shopData);
                }
            }
            else if (streetObjs[i].transform.parent.gameObject.layer == (int)LayerId.Store)
            {
                streetSaveData.streetCfgs.Enqueue(streetObjs[i].name);
            }
        }
        streetSaveData.isEntrance = isEntrance;
        streetSaveData.localPosition = gameObject.transform.localPosition;
        return streetSaveData;
    }

    public void ToBack()
    {
        gameObject.SetActive(false);
        enabled = false;
    }

    public void ToFront()
    {
        enabled = true;
        gameObject.SetActive(true);
    }

    public IEnumerator InitAsyn(int id)
    {
        transform.localPosition = Vector3.forward * streetLength * id;
        gameObject.SetActive(true);
        Debug.Log(gameObject.name);
        if (storeMap != null)
        {
            StreetFactory.Instance.Recycle(storeMap);
        }
        this.isEntrance = id == 0;
        storeMap = StreetFactory.Instance.CreateLayout(isEntrance);
        yield return new WaitForSeconds(0.1f);
        if (storeMap == null)
        {
            yield break;
        }

        storeMap.transform.SetParent(this.transform);
        storeMap.transform.localPosition = Vector3.zero;
        storeMap.transform.localRotation = Quaternion.identity;
        storeMap.gameObject.SetActive(true);

        for (int i = 0; i < streetObjs.Count; i++)
        {
            StreetFactory.Instance.Recycle(streetObjs[i]);
        }
        streetObjs.Clear();

        for (int idx = 0; idx < storeMap.transform.childCount; ++idx)
        {
            Transform root = storeMap.transform.GetChild(idx);
            if (root.gameObject.layer == (int)LayerId.Building)
            {
                GameObject go = StreetFactory.Instance.CreateBuild(null);
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
                if (go != null)
                {
                    BuildBody buildBody = go.GetComponent<BuildBody>();
                    buildBody.Init(null);
                }
            }
            else if (root.gameObject.layer == (int)LayerId.Store)
            {
                GameObject go = StreetFactory.Instance.CreateStore();
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
            }
            else if (root.gameObject.layer == (int)LayerId.Item)
            {
                GameObject go = StreetFactory.Instance.CreateItem(root.name);
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator LoadAsyn(StreetSaveData streetSaveData)
    {
        transform.localPosition = streetSaveData.localPosition;
        gameObject.SetActive(true);
        Debug.Log(gameObject.name);
        if (storeMap != null)
        {
            StreetFactory.Instance.Recycle(storeMap);
        }
        this.isEntrance = streetSaveData.isEntrance;
        storeMap = StreetFactory.Instance.CreateLayout(isEntrance);
        yield return new WaitForSeconds(0.1f);
        if (storeMap == null)
        {
            yield break;
        }

        storeMap.transform.SetParent(this.transform);
        storeMap.transform.localPosition = Vector3.zero;
        storeMap.transform.localRotation = Quaternion.identity;
        storeMap.gameObject.SetActive(true);

        for (int i = 0; i < streetObjs.Count; i++)
        {
            StreetFactory.Instance.Recycle(streetObjs[i]);
        }
        streetObjs.Clear();

        ShopData shopData;
        for (int idx = 0; idx < storeMap.transform.childCount; ++idx)
        {
            Transform root = storeMap.transform.GetChild(idx);
            if (root.gameObject.layer == (int)LayerId.Building)
            {
                shopData = streetSaveData.shopDatas.Count > 0 ? streetSaveData.shopDatas.Dequeue() : null;
                GameObject go = StreetFactory.Instance.CreateBuild(shopData);
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
                if (go != null)
                {
                    BuildBody buildBody = go.GetComponent<BuildBody>();
                    buildBody.Init(shopData);
                }
            }
            else if (root.gameObject.layer == (int)LayerId.Store)
            {
                GameObject go = StreetFactory.Instance.CreateStore(streetSaveData.streetCfgs.Count > 0 ? streetSaveData.streetCfgs.Dequeue() : null);
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
            }
            else if (root.gameObject.layer == (int)LayerId.Item)
            {
                GameObject go = StreetFactory.Instance.CreateItem(root.name);
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Fresh(bool isEntrance = false,Queue<ShopData> shopDatas = null)
    {
        if (storeMap != null)
        {
            StreetFactory.Instance.Recycle(storeMap);
        }
        storeMap = StreetFactory.Instance.CreateLayout(isEntrance);
        if (storeMap == null)
        {
            return;
        }

        storeMap.transform.SetParent(this.transform);
        storeMap.transform.localPosition = Vector3.zero;
        storeMap.transform.localRotation = Quaternion.identity;
        storeMap.gameObject.SetActive(true);

        for (int i = 0; i < streetObjs.Count; i++)
        {
            StreetFactory.Instance.Recycle(streetObjs[i]);
        }
        streetObjs.Clear();

        for (int idx = 0; idx < storeMap.transform.childCount; ++idx)
        {
            Transform root = storeMap.transform.GetChild(idx);
            if (root.gameObject.layer == (int)LayerId.Building)
            {
                ShopData shopData = null;
                if (shopDatas != null)
                {
                    if (shopDatas.Count > 0)
                        shopData = shopDatas.Dequeue();
                    else
                        shopData = StoreData.Instance.shopDataQueue.Count > 0 ? StoreData.Instance.shopDataQueue.Dequeue() : null;
                }
                else
                    shopData = StoreData.Instance.shopDataQueue.Count > 0 ? StoreData.Instance.shopDataQueue.Dequeue() : null;
                GameObject go = StreetFactory.Instance.CreateBuild(shopData);
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
                if (go != null)
                {
                    BuildBody buildBody = go.GetComponent<BuildBody>();
                    buildBody.Init(shopData);
                }
            }
            else if (root.gameObject.layer == (int)LayerId.Store)
            {
                GameObject go = StreetFactory.Instance.CreateStore();
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
            }
            else if (root.gameObject.layer == (int)LayerId.Item)
            {
                GameObject go = StreetFactory.Instance.CreateItem(root.name);
                go.gameObject.SetActive(true);
                go.transform.SetParent(root);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                streetObjs.Add(go);
            }
        }
    }
    
    public void FreshData()
    {
        for (int idx = 0; idx < streetObjs.Count && StoreData.Instance.shopDataQueue.Count > 0; idx++)
        {
            if (streetObjs[idx].transform.parent.gameObject.layer == (int)LayerId.Building)
            {
                BuildBody buildBody = streetObjs[idx].GetComponent<BuildBody>();
                if (buildBody != null && buildBody.shopData == null)
                {
                    buildBody.Fresh(StoreData.Instance.shopDataQueue.Dequeue());
                }
            }
        }
    }

    /// <summary>
    /// 保存当前街道所有店铺的信息
    /// </summary>
    public Queue<ShopData> SaveData()
    {
        Queue<ShopData> list = new Queue<ShopData>();
        BuildBody[] bodys = transform.GetComponentsInChildren<BuildBody>(true);
        for (int idx = 0; idx < bodys.Length; idx++)
        {
            if (bodys[idx] != null && bodys[idx].shopData != null)
            {
                list.Enqueue(bodys[idx].shopData);
            }
        }
        return list;
    }
    
    private void Start()
    {
        Transform trans = transform.Find("StreetTransfer");
        if (trans != null)
        {
            transfer = trans.gameObject;
        }
    }

    private GameObject transfer;
    private float CD = 10f;
    private void Update()
    {
        for (int idx = 0; idx < streetObjs.Count; idx++)
        {
            if (streetObjs[idx].activeInHierarchy)
            {
                if (streetObjs[idx].transform.position.z > 130 || streetObjs[idx].transform.position.z < -8)
                {
                    streetObjs[idx].SetActive(false);
                }
            }
            else
            {
                if (streetObjs[idx].transform.position.z < 130 && streetObjs[idx].transform.position.z > -8)
                {
                    streetObjs[idx].SetActive(true);
                }
            }
        }

        if (CD > 0)
        {
            CD -= Time.deltaTime;
            return;
        }

        if (transfer != StreetManager.Instance.StreetEvtObj &&
            StreetManager.Instance.StreetEvtObj != null)
        {
            transfer.SetActive(false);
            return;
        }

        if (transfer.activeInHierarchy)
        {
            if (transfer.transform.position.z < 5f || transfer.transform.position.z > 30)
            {
                transfer.SetActive(false);
            }

            if (transfer.activeInHierarchy == false)
            {
                StreetManager.Instance.StreetEvtObj = null;
            }
        }
        else
        {
            if (transfer.transform.position.z < 30f && transfer.transform.position.z > 5f)
            {
                transfer.SetActive(true);
            }

            if (transfer.activeInHierarchy)
            {
                StreetManager.Instance.StreetEvtObj = transfer;
            }
        }

    }
    
}
