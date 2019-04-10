using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObjManager : Singleton<LightObjManager> {

    private List<LightObj> lightObjList = new List<LightObj>();

    public void Add(LightObj obj)
    {
        if (obj != null && lightObjList.Contains(obj) == false)
        {
            lightObjList.Add(obj);
        }
    }

    public void Remove(LightObj obj)
    {
        lightObjList.Remove(obj);
    }

    public void Clear()
    {

    }

    public void Update()
    {
    }
}
