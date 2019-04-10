using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class WebApiReturnFaceCfgData
{
    public string code;
    public string msg;
    public FaceCfgData data;
}

[Serializable]
public class FaceCfgData
{
    public FaceCfg[] list;
}

[Serializable]
public class FaceCfg
{
    public int c_id;
    public string c_source_no;
}