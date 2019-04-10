using NewEngine.Utils;
using System;

public class VRShopData : Singleton<VRShopData> {

    private VRShopInfo vrShopInfo = null;
    public VRShopInfo VRShopInfo
    {
        get
        {
            return vrShopInfo;
        }
        set
        {
            vrShopInfo = value;
        }
    }

    private string uCode = "";
    public string UCode
    {
        get
        {
            return uCode;
        }
        set
        {
            uCode = value;
        }
    }

    private string resURL = "";
    public string ResURL
    {
        get
        {
            return resURL;
        }
        set
        {
            resURL = value;
        }
    }

    private bool isError = false;
    public bool IsError
    {
        get
        {
            return isError;
        }
        set
        {
            isError = value;
        }
    }

    private bool isCompleted = false;
    public bool IsCompleted
    {
        get
        {
            return isCompleted;
        }
        set
        {
            isCompleted = value;
        }
    }
}

[Serializable]
public class WebApiReturnVRShopInfo
{
    public string code;
    public string msg;
    public VRShopInfo data;
}

[Serializable]
public class VRShopInfo
{
    public VRShopGoodsInfo[] vrShopGoodsInfos;
    public VRShopActivityInfo[] vrShopActivityInfos;
    public VRShopBaseInfo vrShopBaseInfo;
    public float CD = 10f;
}

[Serializable]
public class VRShopGoodsInfo
{
    public string cId;
    public string pcode; //商品code
    public string img; //商品图片
    public string name; //商品名称
    public string desc; //商品描述
    public string minPrice; //商品价格
}

[Serializable]
public class VRShopActivityInfo
{
    public string name; //名称
    //public string c_type; //类型
    //public string c_img; //图片
    //public string c_money; //金额
    //public string c_limit_money; //满减
}

[Serializable]
public class VRShopBaseInfo
{
    public string headImg; //店铺头像
    public string name; //店铺名称
    public string level; // c_level == "1" 为实力商家
    public string vrImg;//全景资源的url
    public string vrJson;//全景资源的url
    public string shopTrade;//行业分类
}