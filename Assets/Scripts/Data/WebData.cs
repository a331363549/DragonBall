using System;

[Serializable]
public class WebData
{
    public string code;
    public string msg;
}

[Serializable]
public class WebApiReturnSendEventResult
{
    public string code;
    public string msg;
    public NewAchieveData data;
}

[Serializable]
public class NewAchieveData
{
    public string c_name; //: "新生蜜柚",  //成就名称
    public string c_has_get_img; //: "http://tanling.welcomest.com/Source/Wld/App/Achieve/Icon/street_mycj_xs_yhd01.png", //成就图片
}

[Serializable]
public class WebApiRequestTransferResult
{
    public string code;
    public string msg;
    public RequsetTransferResult data;
}

[Serializable]
public class RequsetTransferResult
{
    public string url;
    public string json_url;
}

[Serializable]
public class WebApiBackgroundMusicReturnt
{
    public string code;
    public string msg;
    public BackgroundMusicURL data;
}

[Serializable]
public class BackgroundMusicURL
{
    public string[] url;
}

