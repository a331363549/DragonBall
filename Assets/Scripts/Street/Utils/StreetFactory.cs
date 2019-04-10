using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewEngine.Framework;
using NewEngine.Framework.Table;
using Table;
using NewEngine.Utils;

public class StreetFactory : Singleton<StreetFactory>
{

    private string enterLayout = "prefabs/layout/entrance_00";
    public string mainLayout = "prefabs/layout/main_00";
    public string modenLayout = "prefabs/layout/main_01";

    public GameObject CreateLayout(bool isEntrance)
    {
        if (isEntrance)
        {
            GameObject layout = GOPool.Instance.PopGO(enterLayout);
            if (layout != null)
            {
                layout.name = enterLayout;
            }
            return layout;
        }
        else
        {
            GameObject layout;
            if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
            {
                layout = GOPool.Instance.PopGO(mainLayout);
                if (layout != null)
                    layout.name = mainLayout;
            }
            else
            {
                layout = GOPool.Instance.PopGO(modenLayout);
                if (layout != null)
                    layout.name = modenLayout;
            }
            return layout;
        }
    }
    public string GetLayoutId(bool isEntrance)
    {
        if (isEntrance)
        {
            return enterLayout;
        }
        else
        {
            if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                return mainLayout;
            else
                return modenLayout;
        }
    }

    public GameObject CreateBuild(ShopData shopData)
    {
        GameObject go;
        if (shopData == null)
        {
            go = BuildFactory.Instance.CreateBody(100);
        }
        else
        {
            go = BuildFactory.Instance.CreateBody(shopData.shop_info.industry_id);
        }

        return go;
    }

    public GameObject CreateStore(string name = null)
    {
        int idx = name == null ? 
            Random.Range(0, TableReader<StreetStoreResTable>.Context.Count) : 
            TableReader<StreetStoreResTable>.Context.FindIndex((StreetStoreResTable item)=> { return item.res.CompareTo(name) == 0 || item.res.Contains(name); });
        string res = TableReader<StreetStoreResTable>.Context[idx].res;
        GameObject go = GOPool.Instance.PopGO(res);
        if (go != null)
        {
            go.name = res;
        }
        return go;
    }

    public GameObject CreateItem(string name)
    {
        string res = "Prefabs/Items/" + name;
        GameObject go = GOPool.Instance.PopGO(res);
        if (go != null)
        {
            go.name = res;
        }
        return go;
    }

    public void Recycle(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        BuildFactory.Instance.RecycleGO(go);
    }
}
