using NewEngine.Framework.Table;
using NewEngine.Utils;
using System.Collections.Generic;
using Table;
using UnityEngine;

public class BuildFactory : Singleton<BuildFactory> {
    
    private Dictionary<int, string> faceResMap = new Dictionary<int, string>();

    public void InitFaceCfg(FaceCfg[] faceCfgArr)
    {
        faceResMap.Clear();
        for (int idx = 0; idx < faceCfgArr.Length; idx++)
        {
            if (faceResMap.ContainsKey(faceCfgArr[idx].c_id))
            {
                continue;
            }
            faceResMap.Add(faceCfgArr[idx].c_id, faceCfgArr[idx].c_source_no);
        }
    }
    
    public void RecycleGO(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        BuildBody buildBody = go.GetComponent<BuildBody>();
        if (buildBody != null)
        {
            buildBody.Release();
            GOPool.Instance.PushGO(go);
            return;
        }

        BuildFace buildFace = go.GetComponent<BuildFace>();
        if (buildFace != null)
        {
            buildFace.Release();
            GOPool.Instance.PushGO(go);
            return;
        }

        GOPool.Instance.PushGO(go);
    }

    public GameObject CreateGO(string path)
    {
        GameObject go = GOPool.Instance.PopGO(path);
        if (go != null)
        {
            go.name = path;
        }
        return go;
    }

    public GameObject CreateFace(int type)
    {
        //string id;
        //int build_id;
        //if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
        //    build_id = Random.Range(0, 3);
        //else
        //{
        //    if (StreetsMaps.Instance.dic_industry2building.TryGetValue(type.ToString(), out id))
        //        build_id = int.Parse(id);
        //    else
        //        build_id = Random.Range(13, 27);
        //}
        BuildFaceResTable item = TableReader<BuildFaceResTable>.Context.Find((BuildFaceResTable face) =>
        {
            return face.id == type;
        });
        string resId = item.res;
        GameObject go = GOPool.Instance.PopGO(resId);
        if (go != null)
        {
            go.name = resId;
        }
        else
        {
            Debug.LogError("CreateFace:" + resId);
        }
        return go;
    }

    public GameObject CreateBody(int industry_id)
    {
        //int build_id;
        //string id;
        //if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
        //    build_id = Random.Range(0, 3);
        //else
        //{
        //    if (StreetsMaps.Instance.dic_industry2building.TryGetValue(industry_id.ToString(), out id))
        //        build_id = int.Parse(id);
        //    else
        //        build_id = 25;
        //}
        string res = "";
        GameObject go = null;
        try
        {
            BuildBodyResTable build = TableReader<BuildBodyResTable>.Context.Find((BuildBodyResTable item) =>
            {
                return item.id == industry_id;
            });
            res = build.res;
            go = GOPool.Instance.PopGO(res);
        }
        catch
        {
            if (MainLogic.Instance.cur_streetType != StreetType.古韵街)
                res = "Prefabs/Items/peilou1";
            else
                res = "Prefabs/Body/body_height_00";
            go = GOPool.Instance.PopGO(res);
        }
        //GameObject go = GOPool.Instance.PopGO(res);
        if (go != null)
        {
            go.name = res;
        }
        else
        {
            Debug.LogError(res);
        }
        return go;

    }

    //现代街道招牌
    public GameObject CreateStoreModenFlag(int length)
    {
        List<FlagModenResTable> list = TableReader<FlagModenResTable>.Context.FindAll((FlagModenResTable item) =>
        {
            return item.length == length || item.length == 0;
        });
        int idx = Random.Range(0, list.Count);
        string res = list[idx].res;
        GameObject go = GOPool.Instance.PopGO(res);
        if (go != null)
        {
            go.name = res;
        }
        return go;
    }
    //古街道招牌
    public GameObject CreateStoreFlag(int length)
    {
        List<FlagResTable> list = TableReader<FlagResTable>.Context.FindAll((FlagResTable item) =>
        {
            return item.length == length || item.length == 0;
        });
        int idx = Random.Range(0, list.Count);
        string res = list[idx].res;
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

    public GameObject CreateTrigger(int typeId, out float cost)
    {
        if (typeId < 0 || typeId >= TableReader<EventEnterResTable>.Context.Count)
        {
            cost = 0;
            return null;
        }
        string res = TableReader<EventEnterResTable>.Context[typeId].res;
        cost = TableReader<EventEnterResTable>.Context[typeId].space;
        GameObject go = GOPool.Instance.PopGO(res);
        if (go != null)
        {
            go.name = res;
        }
        return go;
    }

    public GameObject CreateFaceDeco(out float cost)
    {
        int idx = Random.Range(0, TableReader<FaceDecoResTable>.Context.Count);
        string res = TableReader<FaceDecoResTable>.Context[idx].res;
        cost = TableReader<FaceDecoResTable>.Context[idx].space;
        GameObject go = GOPool.Instance.PopGO(res);
        if (go != null)
        {
            go.name = res;
        }
        return go;
    }
}
