using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetManager : Singleton<StreetManager> {
    
    private GameObject streetEventObj = null;
    public GameObject StreetEvtObj
    {
        get { return streetEventObj; }
        set { streetEventObj = value; }
    }

    public float streetLength = 130f;
    public StreetObj[] mainStreets;

    public void CreateStreet(Transform streetRoot, int num, float streetLen, StreetType type, List<StreetSaveData> streetSaveDatas = null)
    {
        this.streetLength = streetLen;
        GameObject go;
        mainStreets = new StreetObj[num];
       

        for (int idx = 0; idx < num; idx++)
        {
            go = new GameObject("streetObj_" + idx);
            go.transform.SetParent(streetRoot);
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            mainStreets[idx] = go.AddComponent<StreetObj>();
            if (type == StreetType.古韵街)
                mainStreets[idx].ResId = "Prefabs/street";
            else
                mainStreets[idx].ResId = "Prefabs/street_moden";
            mainStreets[idx].Length = streetLen;
            mainStreets[idx].LOD = 300;
            mainStreets[idx].enabled = false;
            if (streetSaveDatas == null || streetSaveDatas.Count <= idx)
            {
                go.transform.localPosition = Vector3.forward * streetLength * idx;
                if (type == StreetType.古韵街)
                    mainStreets[idx].LayoutId = idx == 0 ? "Prefabs/Layout/entrance_00" : "Prefabs/Layout/main_00";
                else
                    mainStreets[idx].LayoutId = "Prefabs/Layout/main_01";
                mainStreets[idx].InitObj(null);
            }
            else
            {
                go.transform.localPosition = streetSaveDatas[idx].localPosition;
                mainStreets[idx].LayoutId = streetSaveDatas[idx].layoutId;
                mainStreets[idx].InitObj(streetSaveDatas[idx]);
            }
        }
    }

    public void UpdateData()
    {
        for (int idx = 0; idx < mainStreets.Length; idx++)
        {
            mainStreets[idx].FreshData();
        }
    }
        
    public void ToBack()
    {
        for (int idx = 0; idx < mainStreets.Length; idx++)
        {
            mainStreets[idx].ToBack();
        }
    }

    public void ToFront()
    {
        for (int idx = 0; idx < mainStreets.Length; idx++)
        {
            mainStreets[idx].ToFront();
        }
    }

    public List<StreetSaveData> GetStreetSaveData()
    {
        List<StreetSaveData> streetSaveDatas = new List<StreetSaveData>();
        for (int idx = 0; idx < mainStreets.Length; idx++)
        {
            streetSaveDatas.Add(mainStreets[idx].GetStreetSaveData());
        }
        return streetSaveDatas;
    }
    
    // Update is called once per frame
    public void Update(Vector3 moved)
    {
        mainStreets[0].transform.localPosition = mainStreets[0].transform.localPosition - moved;
        for (int idx = 1; idx < mainStreets.Length; idx++)
        {
            mainStreets[idx].transform.localPosition = mainStreets[idx - 1].transform.localPosition + Vector3.forward * streetLength;
        }
        if (mainStreets[0].transform.localPosition.z < -streetLength * 2)
        {
            mainStreets[0].transform.localPosition = mainStreets[mainStreets.Length - 1].transform.localPosition + Vector3.forward * streetLength;
            mainStreets[0].tmpList.Clear();
            mainStreets[0].Fresh();
            GOPool.Instance.ReleaseCache();
            StreetObj temp = mainStreets[0];
            for (int idx = 0; idx < mainStreets.Length - 1; idx++)
            {
                mainStreets[idx] = mainStreets[idx + 1];
            }
            mainStreets[mainStreets.Length - 1] = temp;
            mainStreets[0].FreshData();
        }
    }
}
