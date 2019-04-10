using NewEngine.Framework.Table;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;

public class StreetObj : MonoBehaviour {

    public string ResId { get; set; }
    public float LOD { get; set; }
    public string LayoutId { get; set; }
    public float Length { get; set; }


    protected bool initialized = false;
    protected GameObject go = null;
    protected GameObject layoutGO = null;
    protected GameObject itemsRoot = null;

    public void InitObj(StreetSaveData streetSaveData)
    {
        enabled = true;

        if (initialized == false)
        {
            InitGO();
        }

        if (itemsRoot == null || streetSaveData == null)
        {
            return;
        }
        tmpList = streetSaveData.tmpList;
        for (int idx = 0; idx < itemsRoot.transform.childCount && streetSaveData.shopDatas.Count > 0; idx++)
        {
            Transform root = itemsRoot.transform.GetChild(idx);
            if (root.gameObject.layer == (int)LayerId.Building)
            {
                BuildingObj buildBody = root.GetComponentInChildren<BuildingObj>();
                if (buildBody != null && buildBody.Data == null)
                {
                    buildBody.Data = streetSaveData.shopDatas.Dequeue();
                }
            }
        }
    }

    public StreetSaveData GetStreetSaveData()
    {
        StreetSaveData streetSaveData = new StreetSaveData();
        for (int i = 0; itemsRoot != null && i < itemsRoot.transform.childCount; i++)
        {
            Transform root = itemsRoot.transform.GetChild(i);
            if (root.gameObject.layer == (int)LayerId.Building)
            {
                BuildingObj build = root.GetComponent<BuildingObj>();
                if (build != null)
                {
                    if (build.Data != null)
                    {
                        build.Data.buildBodyId = build.gameObject.name;
                    }
                    streetSaveData.shopDatas.Enqueue(build.Data);
                }
            }
            else if (root.gameObject.layer == (int)LayerId.Store)
            {
                streetSaveData.streetCfgs.Enqueue(root.name);
            }
        }
        streetSaveData.layoutId = LayoutId;
        streetSaveData.tmpList = tmpList;
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

    public void Fresh()
    {
        LayoutId = StreetFactory.Instance.GetLayoutId(false);
        InitBuilding();
        FreshData();
    }
    [SerializeField]
    public List<int> tmpList = new List<int>();
    public void FreshData()
    {
        if (itemsRoot == null)
        {
            return;
        }

        for (int idx = 0; idx < itemsRoot.transform.childCount && StoreData.Instance.shopDataQueue.Count > 0; idx++)
        {
            if (tmpList.Contains(idx))
                continue;
            ShopBuildHandler(idx, StoreData.Instance.shopDataQueue);
        }
    }

    private void ShopBuildHandler(int idx,Queue<ShopData> queue)
    {
        Transform root = itemsRoot.transform.GetChild(idx);
        if (root.gameObject.layer == (int)LayerId.Building)
        {
            BuildingObj buildBody = root.GetComponentInChildren<BuildingObj>();
            if (buildBody != null && buildBody.Data == null)
            {
                ShopData shopData = queue.Dequeue();
                if (!tmpList.Contains(idx))
                    tmpList.Add(idx);
                //string id;
                //int build_id;
                //if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                //    build_id = Random.Range(0, 3);
                //else
                //{
                //    if (StreetsMaps.Instance.dic_industry2building.TryGetValue(shopData.shop_info.industry_id.ToString(), out id))
                //        build_id = int.Parse(id);
                //    else
                //        build_id = Random.Range(13, 27);
                //}
                if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                    shopData.shop_info.industry_id = Random.Range(0, 3);
                else if (MainLogic.Instance.cur_streetType == StreetType.蜜绣街)
                    shopData.shop_info.industry_id = Random.Range(13, 15);
                else if (MainLogic.Instance.cur_streetType == StreetType.蜜品居)
                    shopData.shop_info.industry_id = Random.Range(16, 19);
                else if (MainLogic.Instance.cur_streetType == StreetType.蜜味巷)
                    shopData.shop_info.industry_id = Random.Range(20, 22);
                else if (MainLogic.Instance.cur_streetType == StreetType.蜜享道)
                    shopData.shop_info.industry_id = Random.Range(23, 27);
                else
                    shopData.shop_info.industry_id = Random.Range(13, 27);
                switch (shopData.shop_info.industry_id)
                {
                    //餐饮 长度2
                    //家居 长度2
                    //教育 长度2
                    case 17:
                    case 20:
                    case 24:
                        if (idx < 18)
                        {
                            tmpList.Add(idx + 2);
                            if (idx % 2 != 0)
                                buildBody = itemsRoot.transform.GetChild(idx + 2).GetComponentInChildren<BuildingObj>();
                            buildBody.Data = shopData;
                        }
                        else
                        {
                            StoreData.Instance.shopDataQueue.Enqueue(shopData);
                            ShopBuildHandler(idx, StoreData.Instance.shopDataQueue);
                        }
                        break;
                    //服饰 长度3
                    case 13:
                        if (idx < 16)
                        {
                            tmpList.Add(idx + 2);
                            tmpList.Add(idx + 4);
                            if (idx % 2 != 0)
                                buildBody = itemsRoot.transform.GetChild(idx + 4).GetComponentInChildren<BuildingObj>();
                            buildBody.Data = shopData;
                        }
                        else
                        {
                            StoreData.Instance.shopDataQueue.Enqueue(shopData);
                            ShopBuildHandler(idx, StoreData.Instance.shopDataQueue);
                        }
                        break;
                    default:
                        buildBody.Data = shopData;
                        break;
                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        if (initialized == false && Mathf.Abs(UserCamera.Instance.transform.position.z - transform.position.z) < LOD)
        {
            InitGO();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized == false && Mathf.Abs(UserCamera.Instance.transform.position.z - transform.position.z) < LOD)
        {
            InitGO();
        }
    }

    private void InitGO()
    {
        initialized = true;
        go = GOPool.Instance.PopGO(ResId);
        if (go != null)
        {
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }
        InitBuilding();

    }

    private void InitBuilding()
    {
        if (itemsRoot == null)
        {
            itemsRoot = new GameObject("itemsRoot");
            itemsRoot.transform.parent = transform;
            itemsRoot.transform.localPosition = Vector3.zero;
            itemsRoot.transform.localRotation = Quaternion.identity;
            itemsRoot.transform.localScale = Vector3.one;
        }
        else
        {
            for (int i = itemsRoot.transform.childCount - 1; i >= 0; i--)
            {
                StreetFactory.Instance.Recycle(itemsRoot.transform.GetChild(i).gameObject);
            }
        }

        if (layoutGO != null)
        {
            StreetFactory.Instance.Recycle(layoutGO);
        }

        layoutGO = GOPool.Instance.PopGO(LayoutId);
        if (layoutGO == null)
        {
            return;
        }

        layoutGO.transform.parent = transform;
        layoutGO.transform.localPosition = Vector3.zero;
        layoutGO.transform.localRotation = Quaternion.identity;
        layoutGO.transform.localScale = Vector3.one;

        for (int idx = 0; idx < layoutGO.transform.childCount; ++idx)
        {
            Transform root = layoutGO.transform.GetChild(idx);
            if (root.gameObject.layer == (int)LayerId.Building)
            {
                GameObject go = new GameObject(string.Format("Build_{0:d2}", idx));
                go.layer = (int)LayerId.Building;
                go.transform.parent = itemsRoot.transform;
                go.transform.localPosition = root.localPosition;
                go.transform.localRotation = root.localRotation;
                go.transform.localScale = root.localScale;
                BuildingObj buildingObj = go.AddComponent<BuildingObj>();
                //buildingObj.enabled = enableOnAwake;
                if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                    buildingObj.LOD = 100;
                else
                    buildingObj.LOD = 200;
            }
            else if (root.gameObject.layer == (int)LayerId.Store)
            {
                GameObject go = new GameObject(string.Format("Decoration_{0:d2}", idx));
                go.layer = (int)LayerId.Store;
                go.transform.parent = itemsRoot.transform;
                go.transform.localPosition = root.localPosition;
                go.transform.localRotation = root.localRotation;
                go.transform.localScale = Vector3.one;
                SceneObj sceneObj = go.AddComponent<SceneObj>();
                int id = Random.Range(0, TableReader<StreetStoreResTable>.Context.Count);
                string res = TableReader<StreetStoreResTable>.Context[id].res;
                sceneObj.ResId = res;
                //sceneObj.enabled = enableOnAwake;
                if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                    sceneObj.LOD = 100;
                else
                    sceneObj.LOD = 200;

            }
            else if (root.gameObject.layer == (int)LayerId.Item)
            {
                GameObject go = new GameObject(string.Format("Item_{0:d2}", idx));
                go.layer = (int)LayerId.Item;
                go.transform.parent = itemsRoot.transform;
                go.transform.localPosition = root.localPosition;
                go.transform.localRotation = root.localRotation;
                go.transform.localScale = root.localScale;
                SceneObj sceneObj = go.AddComponent<SceneObj>();
                sceneObj.ResId = "Prefabs/Items/" + root.name;
                //sceneObj.enabled = enableOnAwake;
                if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                    sceneObj.LOD = 100;
                else
                    sceneObj.LOD = 200;
            }
            else if (root.gameObject.layer == (int)LayerId.Trigger)
            {
                GameObject go = new GameObject(string.Format("Trigger_{0:d2}", idx));
                go.layer = (int)LayerId.Trigger;
                go.transform.parent = itemsRoot.transform;
                go.transform.localPosition = root.localPosition;
                go.transform.localRotation = root.localRotation;
                go.transform.localScale = Vector3.one;
                SceneObj sceneObj = go.AddComponent<SceneObj>();
                sceneObj.ResId = "Prefabs/Items/" + root.name;
                //sceneObj.enabled = enableOnAwake;
                //sceneObj.LOD = 50;
                if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                    sceneObj.LOD = 50;
                else
                    sceneObj.LOD = 100;
            }
        }
    }
}
