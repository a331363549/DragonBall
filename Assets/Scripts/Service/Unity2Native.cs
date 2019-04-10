using NewEngine.Framework;
using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class Unity2Native : CService
{
    private static Unity2Native sInstance = null;
    public static Unity2Native Instance
    {
        get
        {
            return sInstance;
        }
    }

    public Action<string> onNewBroadcastMsg = null;

    public void Start()
    {
        sInstance = this;
    }

    public void OnDestroy()
    {
        sInstance = null;
    }

    public void BroadcastNewMsg(string msg)
    {
        if (this.onNewBroadcastMsg != null)
        {
            this.onNewBroadcastMsg(msg);
        }
    }

    public static void OpenStorePageByCode(string u_code, string fromStreet)
    {
        OpenStorePage(u_code, fromStreet);
    }

#if UNITY_EDITOR || UNITY_STANDALONE || DEMO

    public static void RequestContentType()
    {
        Debug.Log("RequestContentType");
        ContentCfg content = new ContentCfg()
        {
            code = "",//T10006 //wld0339142ca8fb769b //1001020503066404716544
            isRoad = "",
            isDirect = "1",
#if TEST_API
            baseUrl = "https://mall.58wld.com/",//"https://ceshiapi.iweilingdi.com/index.php/",
            streetUrl = "http://street.service.welcomest.com/",
#else
            baseUrl = "https://mall.58wld.com/",// "https://mapi.weilingdi.com/index.php/",
            streetUrl = "https://street.weilingdi.com/",
#endif
        };
        Instance.SendMessage("SetContent", JsonUtility.ToJson(content));
    }

    private static readonly string WldVR = "http://wldvr.weilingdi.com/wldVR/?";
    public static void OpenWeb(string url, string desc)
    {
        Debug.Log("OpenWeb:" + url);
        //System.Diagnostics.Process.Start(WldVR + url);
        if (url.EndsWith(".json"))
        {
            Application.OpenURL(WldVR + "vrJson=" + url);
        }
        else
        {
            Application.OpenURL(WldVR + "uCode=" + url);
        }
    }

    public static void OpenStorePage(string msg, string fromStreet)
    {
        Debug.LogError("OpenStorePage:" + msg);
    }


    public static void ShowMessage(string msg)
    {
        Debug.Log(msg);
    }

    public static void ShowMainUI()
    {

    }

    public static void HideMainUI()
    {

    }

    public static void ShowLoadingUI()
    {

    }

    public static void HideLoadingUI()
    {

    }

    public static void GoToPortrait()
    {

    }

    public static void GoToLandscape()
    {

    }

    public static void GoToLogin()
    {

    }

    public static void GoToAchieve()
    {
        Debug.Log("GoToAchieve");
    }

    public static void GoToGoodsPage(string goodsId)
    {
        Debug.Log("GoToGoodsPage:" + goodsId);
    }

    public static void Back2App()
    {
        MainLogic.Instance.Back2App();
        //Unity2Native.Back2App();
        Debug.Log("Back2App");
    }

    public static void OnUnityLoading()
    {
        Debug.Log("OnUnityLoading");
    }


#elif UNITY_ANDROID

    private const string AssistantCls = "com.unity.AndroidAssistant";
    private static AndroidJavaClass assistantClsObj = new AndroidJavaClass(AssistantCls);
    
    public static void RequestContentType()
    {
        Debug.Log("RequestUserInfo");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("RequestContentType");
        }
    }

    public static void OpenStorePage(string msg, string fromStreet)
    {
        Debug.Log("OpenStorePage:" + msg);
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("OpenStorePage", msg, fromStreet);
        }
    }    

    public static void RequestUserInfo()
    {
        Debug.Log("RequestUserInfo");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("RequestUserInfo");
        }
    }

    public static void ShowMessage(string msg)
    {
        Debug.Log("ShowMessage");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("ShowMessage", msg);
        }
    }

    public static void ShowMainUI()
    {
        Debug.Log("ShowMainUI");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("ShowMainUI");
        }
    }

    public static void HideMainUI()
    {
        Debug.Log("HideMainUI");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("HideMainUI");
        }
    }

    public static void ShowLoadingUI()
    {
        Debug.Log("ShowLoadingUI");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("ShowLoadingUI");
        }
    }

    public static void HideLoadingUI()
    {
        Debug.Log("HideLoadingUI");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("HideLoadingUI");
        }
    }

    public static void GoToLogin()
    {
        Debug.Log("GoToLogin");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("GoToLogin");
        }
    }

    public static void GoToAchieve()
    {
        Debug.Log("GoToAchieve");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("GoToAchieve");
        }
    }

    public static void GoToGoodsPage(string goodsId)
    {
        Debug.Log("GoToGoodsPage:" + goodsId);
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("GoToGoodsPage", goodsId);
        }
    }

    public static void Back2App()
    {
        Debug.Log("Back2App");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("Back2App");
        }
    }

    public static void RequestVRShopInfo(string ucode)
    {
        Debug.Log("RequestVRShopInfo:" + ucode);
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("RequestVRShopInfo", ucode);
        }
    }

    public static void OnUnityLoading()
    {
        Debug.Log("OnUnityLoading");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("OnUnityLoading");
        }
    }

    public static void OpenWeb(string url, string desc)
    {
        Debug.Log("OpenWeb");
        if (assistantClsObj != null){
            assistantClsObj.CallStatic("OpenWeb", url);
        }
    }

    public static void TransOrientation()
    {

    }

    public static void GoToPortrait()
    {

    }

    public static void GoToLandscape()
    {

    }

#elif UNITY_IPHONE
            
    [DllImport("__Internal")]
    public static extern void RequestContentType();

    [DllImport("__Internal")]
    public static extern void OpenStorePage(string msg, string fromStreet);
    
    [DllImport("__Internal")]
    public static extern void RequestUserInfo();
        
    [DllImport("__Internal")]
    public static extern void ShowMessage(string msg);
    
    [DllImport("__Internal")]
    public static extern void ShowMainUI();
    
    [DllImport("__Internal")]
    public static extern void HideMainUI();
    
    [DllImport("__Internal")]
    public static extern void ShowLoadingUI();
    
    [DllImport("__Internal")]
    public static extern void HideLoadingUI();
    
    [DllImport("__Internal")]
    public static extern void BackToXiaoMi();
    
    [DllImport("__Internal")]
    public static extern void TransOrientation();
    
    [DllImport("__Internal")]
    public static extern void GoToLandscape();
    
    [DllImport("__Internal")]
    public static extern void GoToPortrait();
    
    [DllImport("__Internal")]
    public static extern void GoToLogin();
    
    [DllImport("__Internal")]
    public static extern void GoToAchieve();

    [DllImport("__Internal")]
    public static extern void GoToGoodsPage(string goodsId);
    
    [DllImport("__Internal")]
    public static extern void Back2App();
    
    [DllImport("__Internal")]
    public static extern void RequestVRShopInfo(string ucode);
    
    [DllImport("__Internal")]
    public static extern void OpenWeb(string url, string desc);

    public static void OnUnityLoading()
    {
        Debug.Log("OnUnityLoading");
    }
    
#else
    public static void RequestContentType()
    {
        Instance.SendMessage("SetContent", "street");
    }
    
    public static void OpenStorePage(string msg, string fromStreet)
    {
        Debug.Log("OpenStorePage:" + msg);
    }

    public static void RequestUserInfo()
    {
        Debug.Log("RequestUserId");
        //Instance.SetUserId("aba123456");
        UserInfo userInfo = new UserInfo()
        {
            lng = "112.903759",
            lat = "28.219076",
            province = "湖南省",
            c_city = "长沙市",
            userId = "aba123456"
        };
        Instance.SendMessage("SetUserInfo", JsonUtility.ToJson(userInfo));
    }

    public static void ShowMessage(string msg)
    {

    }

    public static void ShowMainUI()
    {

    }

    public static void HideMainUI()
    {

    }

    public static void ShowLoadingUI()
    {

    }

    public static void HideLoadingUI()
    {

    }

    public static void TransOrientation()
    {

    }

    public static void GoToLogin()
    {

    }

    public static void GoToAchieve()
    {

    }

    public static void GoToGoodsPage(string goodsId)
    {
        Debug.Log("GoToGoodsPage:" + goodsId);
    }

    public static void Back2App()
    {
        Debug.Log("Back2App");
    }

    public static void RequestVRShopInfo(string ucode)
    {

    }

    public static void OnUnityLoading()
    {
        Debug.Log("OnUnityLoading");
    }

#endif
}
