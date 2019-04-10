using System;

[Serializable]
public class UserInfo
{
    public string userId = null;
    public string lat = null;
    public string lng = null;
    public string province = null;
    public string c_city = null;
}

[Serializable]
public class ContentCfg
{
    public string code;
    public string isRoad;
    public string isDirect;
    public string baseUrl;
    public string streetUrl;
}

public class UserData : NewEngine.Utils.Singleton<UserData>
{
    private UserInfo userInfo = new UserInfo();
    public UserInfo UserInfo
    {
        get
        {
            return userInfo;
        }
        set
        {
            userInfo = value;
        }
    }
}
