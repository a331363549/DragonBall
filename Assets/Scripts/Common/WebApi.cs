using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using NewEngine.Framework.Service;
using NewEngine.Framework;

public class WebApi{

    private static int page = 1;
    public const string RequestNewMsgApi = "_C=Red&_A=list"; // http://street.service.welcomest.com/?_C=Red&_A=list
    public const string RequestFaceCfgApi = "_C=Source&_A=industry_pic"; //http://street.service.welcomest.com/?_C=Source&_A=industry_pic
    public const string RequestListApi = "_C=Shop&_A=list";//"http://street.service.welcomest.com/?_C=Shop&_A=list&page={0}";
    public const string SendCommentApi = "_C=Praise&_A=save";//"http://street.service.welcomest.com/?_C=Praise&_A=save&busi_ucode=abc123456";
    public const string SendEventResultApi = "_C=Achieve&_A=save";//"http://street.service.welcomest.com/?_C=Achieve&_A=save&event_id=1&button=left";
    public const string SendUserInfoApi = "_C=Geo&_A=userCurrentGeo";//"http://street.service.welcomest.com/?_C=Geo&_A=userCurrentGeo";
    public const string CheckRedPacketApi = "_C=Red&_A=factory";//"http://street.service.welcomest.com/?_C=Shop&_A=redFactory";
    public const string RequestTransferApi = "_C=Source&_A=skyHole";// http://street.service.welcomest.com/?_C=Source&_A=skyHole
    public const string RequestMusicApi = "_C=Source&_A=backgroundMusic";// http://street.service.welcomest.com/?_C=Source&_A=backgroundMusic
    public const string RequestCityViewsApi = "_C=Source&_A=skyHolePic";//http://street.service.welcomest.com/?_C=Source&_A=skyHolePic
    public const string RequestStreetMapApi = "_C=Shop&_A=getIndustryMap";//http://street.weilingdi.com/?_C=Shop&_A=getIndustryMap

    public const string RequestVRShopInfoApi = "getNineProduct";
    public const string RequestJavaVRShopInfoApi = "vr/vrShopInfoByUcode";

    public const string StreetSourceURL = "https://xiaomiservice.oss-cn-shenzhen.aliyuncs.com/street/vr/house/miStreet.unity3d" + Define.platformTag;

    //private string requestStoreListApi = "http://street.service.welcomest.com/?_C=Shop&_A=list&page={0}";

    private static string sStreetApiURL = "http://street.welcomest.com/";
                                        //"https://street.weilingdi.com/";
                                        //"http://street.service.welcomest.com/";//
    private static string sBaseApiURL = "https://ceshiapi.iweilingdi.com/index.php/";
                                        //"https://ceshiapi.iweilingdi.com/api.php/"; //Base/Firstpage/getNineProduct
                                        //https://ceshiapi.iweilingdi.com/index.php/
    private static string sJavaVRApiURL = "https://mall.58wld.com/";


    public static void SetWebApiPath(string baseURL, string streetURL)
    {
        if (string.IsNullOrEmpty(baseURL) == false)
        {
            sBaseApiURL = baseURL;
        }
        if (string.IsNullOrEmpty(streetURL) == false)
        {
            sStreetApiURL = streetURL;
        }
    }

    public static void Reset()
    {
        page = 1;
        CServicesManager.Instance.UnregisterService<WebRequest>();
        WebRequest.Clear();
        CServicesManager.Instance.RegisterService<WebRequest>();
    }

    public static void RequestNewMsg()
    {
        WebRequest.Post(sStreetApiURL, "_C", "Red", "_A", "list");
    }

    public static void RequestFaceCfg()
    {
        WebRequest.Post(sStreetApiURL, "_C", "Source", "_A", "industry_pic");        
    }

    public static void SendUserInfo()
    {
        List<string> args = new List<string>();
        args.AddRange(new string[] { "_C", "Geo", "_A", "userCurrentGeo" });
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.c_city) == false)
        {
            args.Add("c_city");
            args.Add(UserData.Instance.UserInfo.c_city);
        }
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.province) == false)
        {
            args.Add("province");
            args.Add(UserData.Instance.UserInfo.province);
        }
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.lat) == false)
        {
            args.Add("lat");
            args.Add(UserData.Instance.UserInfo.lat);
        }
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.lng) == false)
        {
            args.Add("lng");
            args.Add(UserData.Instance.UserInfo.lng);
        }
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId) == false)
        {
            args.Add("ucode_m");
            args.Add(UserData.Instance.UserInfo.userId);
        }
        Debug.Log("SendUserInfo:");
        WebRequest.Post(sStreetApiURL, args.ToArray());
    }

    public static void RequestStoreList(string str)
    {
        //Debug.LogError(str);
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId) == false)
        {
            WebRequest.Post(sStreetApiURL, "_C", "Shop", "_A", "list", "page", page.ToString(), "ucode_m", "u" + UserData.Instance.UserInfo.userId, "industry_id", str);
        }
        else
        {
            WebRequest.Post(sStreetApiURL, "_C", "Shop", "_A", "list", "page", page.ToString());
        }
        ++page;
    }

    public static void SendComment(string storeId, string storeUCode)
    {
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId) == false)
        {
            WebRequest.Post(sStreetApiURL, "_C", "Praise", "_A", "save", "store_id", storeId, "target_ucode_m", storeUCode, "ucode_m", "u" + UserData.Instance.UserInfo.userId);
        }
        else
        {
            WebRequest.Post(sStreetApiURL, "_C", "Praise", "_A", "save", "store_id", storeId, "target_ucode_m", storeUCode);
        }
    }

    public static void SendEventResult(string eventId, string result)
    {
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId) == false)
        {
            WebRequest.Post(sStreetApiURL, "_C", "Achieve", "_A", "save", "event_id", eventId, "button", result, "ucode_m", "u" + UserData.Instance.UserInfo.userId);
        }
        else
        {
            WebRequest.Post(sStreetApiURL, "_C", "Achieve", "_A", "save", "event_id", eventId, "button", result);
        }
    }

    public static void CheckRedPacketInfo(string storeUCode)
    {
        WebRequest.Post(sStreetApiURL, "_C", "Red", "_A", "factory", "target_ucode_m", "u"+storeUCode, "ucode_m", "u" + UserData.Instance.UserInfo.userId);
    }

    public static void RequestTransfer()
    {
        //if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId) == false)
        //{
        //    WebRequest.Post(sStreetApiURL, "_C", "Source", "_A", "skyHole", "ucode_m", "u" + UserData.Instance.UserInfo.userId);
        //}
        //else
        //{
        //    WebRequest.Post(sStreetApiURL, "_C", "Source", "_A", "skyHole");
        //}
        WebRequest.Post(sStreetApiURL, "_C", "Source", "_A", "skyHole");
    }

    public static void RequestMusic()
    {
        WebRequest.Post(sStreetApiURL, "_C", "Source", "_A", "backgroundMusic");
        //MainLogic.Instance.OnWebMsgProc(RequestMusicApi, "http://localhost:8080/XiaoMiGame/Musics/music-00.unity3d");
    }

    public static void RequstVRShopInfo(string shop_ucode)
    {
        //WebRequest.Get(sBaseApiURL + "Base/Firstpage/getNineProduct", "ucode", shop_ucode);
        //WebRequest.PostMd5("http://192.168.1.126:9904/vr/vrShopInfoByUcode", "param", shop_ucode);
        WebRequest.Put2JavaWithMd5(sBaseApiURL + RequestJavaVRShopInfoApi, "param", shop_ucode);
    }

    public static void RequestCityViews()
    {
        WebRequest.Post(sStreetApiURL, "_C", "Source", "_A", "skyHolePic");
    }
    
    public static void RequestStreetMap()
    {
       
        WebRequest.Post(sStreetApiURL, "_C", "Shop", "_A", "getIndustryMap");
    }
}
