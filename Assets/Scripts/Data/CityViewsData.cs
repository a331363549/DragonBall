using System;
using System.Collections.Generic;

[Serializable]
public class WebApiCityViewsResult
{
    public string code;
    public string msg;
    public Tmp data;

}

[Serializable]
public class Tmp
{
    public ViewInfoURL[] list;
}

[Serializable]
public class ViewInfoURL
{
    public string pic;
    public string source;
}

public class CityViewsData : NewEngine.Utils.Singleton<CityViewsData>
{
    public List<ViewInfoURL> viewsDataList = new List<ViewInfoURL>();
}

