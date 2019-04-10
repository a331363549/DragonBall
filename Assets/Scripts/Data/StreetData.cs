using NewEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WebApiReturnMapList
{
    public string code;
    public string msg;
    public MapList data;
}

[Serializable]
public class MapList
{
    public RoadInfo[] list;
}

[Serializable]
public class RoadsType
{
    public RoadInfo[] info;
    //public RoadInfo MXD;
    //public RoadInfo MWX;
    //public RoadInfo MXJ;
    //public RoadInfo MPJ;
    //public RoadInfo OTH;
}


[Serializable]
public class RoadInfo
{
    public string c_id;
    public string c_industry_id;
    public string c_building_id;
    public string name;
    public string c_street_no;
    public string c_street_name;
    public string c_source_no;
}

public class StreetsMaps : Singleton<StreetsMaps>
{
    public Dictionary<StreetType, string> dic_street2industry = new Dictionary<StreetType, string>();
    public Dictionary<string, string> dic_industry2building = new Dictionary<string, string>();
}