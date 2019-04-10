using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj : SceneObj {

    public ShopData shopData = null;
    public ShopData Data
    {
        get
        {
            return shopData;
        }
        set
        {
            shopData = value;
            if (buildBody != null)
            {
                buildBody.Fresh(shopData);
            }
        }
    }

    private BuildBody buildBody;

    public override void InitGO()
    {
        initialized = true;
        // 传入shopData 根据shop_info.industry_id 来创建相应建筑模型
        GameObject go = StreetFactory.Instance.CreateBuild(shopData);
        go.gameObject.SetActive(true);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        if (shopData != null)
        {
            buildBody = go.GetComponent<BuildBody>();
            buildBody.Init(shopData);
        }
        else
        {
            if (MainLogic.Instance.cur_streetType != StreetType.古韵街)
                go.transform.localPosition = new Vector3(23.5f, 0, -10);
            else
                go.transform.localPosition = Vector3.zero;
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

}
