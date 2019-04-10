using NewEngine.Framework.Service;
using NewEngine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class MainLogic {


    #region webmsg

    public void OnWebMsgProc(string url, string message)
    {
        string api = url.Split('?')[1];
        if (url.Contains(WebApi.RequestListApi))
        {
            OnStoreListData(url, message);
        }
        else if (url.Contains(WebApi.RequestFaceCfgApi))
        {
            OnFaceCfg(url, message);
        }
        else if (url.Contains(WebApi.SendUserInfoApi))
        {
            OnSendUserInfoReturn(url, message);
        }
        else if (url.Contains(WebApi.SendEventResultApi))
        {
            OnSendEventResult(url, message);
        }
        else if (url.Contains(WebApi.RequestNewMsgApi))
        {
            OnRequestNewMsgReturn(url, message);
        }
        else if (url.Contains(WebApi.CheckRedPacketApi))
        {
            OnCheckRedPacketResult(url, message);
        }
        else if (url.Contains(WebApi.RequestJavaVRShopInfoApi))
        {
            OnRequestVRShopInfo(url, message);
        }
        else if (api.Equals(WebApi.RequestTransferApi))
        {
            OnRequestTransfer(url, message);
        }
        else if (url.Contains(WebApi.RequestMusicApi))
        {
            OnMusicURLReturn(url, message);
        }
        else if (api.Equals( WebApi.RequestCityViewsApi))
        {
            OnCityViewsURLReturn(url, message);
        }
        else if (url.Contains(WebApi.RequestStreetMapApi))
        {
            OnStreetMapListURLReturn(url, message);
        }
    }

    private void OnStreetMapListURLReturn(string url,string message)
    {
        try
        {
            WebApiReturnMapList data = JsonUtility.FromJson<WebApiReturnMapList>(message);
            if (data.code == "0")
            {
                AsyncEventDriver.QueueOnMainThread(() =>
                {
                    if (data.data != null)
                    {
                        for (int i = 0; i < data.data.list.Length; i++)
                        {
                            StreetType type = StreetType.精品街;
                            try
                            {
                                type = (StreetType)Enum.Parse(typeof(StreetType), data.data.list[i].c_street_name);
                            }
                            catch
                            {
                                type = StreetType.精品街;
                            }
                            if (StreetsMaps.Instance.dic_street2industry.ContainsKey(type))
                                StreetsMaps.Instance.dic_street2industry[type] += data.data.list[i].c_industry_id + ",";
                            else
                                StreetsMaps.Instance.dic_street2industry.Add(type, data.data.list[i].c_industry_id + ",");
                            StreetsMaps.Instance.dic_industry2building.Add(data.data.list[i].c_industry_id, data.data.list[i].c_building_id);
                        }
                    }
                });
            }
            else
            {
                UIMessageSlide.ShowMessage(message);
            }
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private bool viewsLoad = false;
    private void OnCityViewsURLReturn(string url, string message)
    {
        try
        {
            WebApiCityViewsResult data = JsonUtility.FromJson<WebApiCityViewsResult>(message);
            if (data.code == "0")
            {
                if (streetFirstReady == false)
                {
                    streetFirstReady = true;
                }
                AsyncEventDriver.QueueOnMainThread(() =>
                {
                    if (data.data != null)
                    {
                        for (int idx = 0; idx < data.data.list.Length; idx++)
                        {
                            CityViewsData.Instance.viewsDataList.Add(data.data.list[idx]);
                            DownloadService.AddDownloadTask(data.data.list[idx].pic, OnDownPicResult, OnDownloadProgress);
                        }
                        viewsLoad = true;
                    }
                });
            }
            else
            {
                UIMessageSlide.ShowMessage(message);
            }
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private AssetBundle oldMuiscAsset = null;
    private AssetBundle musicAsset = null;
    private void OnMusicURLReturn(string url, string message)
    {
        try
        {
            WebApiBackgroundMusicReturnt data = JsonUtility.FromJson<WebApiBackgroundMusicReturnt>(message);
            if (data.code != "0")
            {
                UIMessageSlide.ShowMessage(data.msg);
                return;
            }

            string guMusicUrl = data.data.url[0].Contains(guMusic) ? data.data.url[0] : data.data.url[1];
            string modenMusicURL = data.data.url[0].Contains(modenMusic) ? data.data.url[0] : data.data.url[1];
            string tmpURL = (cur_streetType == StreetType.古韵街 ? guMusicUrl : modenMusicURL) + Define.platformTag;

            string musicURL = Path.GetFileName(tmpURL);
            musicURL = FileUtils.WebPath(musicURL) != null ? FileUtils.WebPath(musicURL) : tmpURL;

            if (AudioManager.Instance.MusicName == Path.GetFileNameWithoutExtension(musicURL))
            {
                //AudioManager.Instance.PlayMusic(null);
                return;
            }

            DownloadService.AddDownloadTask(musicURL, (WWW www) =>
            {
                if (string.IsNullOrEmpty(www.error) == false || www.assetBundle == null)
                {
                    Debug.LogError(www.error);
                    UIMessageSlide.ShowMessage(www.error);
                    return;
                }

                oldMuiscAsset = musicAsset;
                musicAsset = www.assetBundle;
                musicAsset.name = Path.GetFileNameWithoutExtension(www.url);
                AsyncEventDriver.QueueOnMainThread(() =>
                {
                    AudioClip clip = musicAsset.LoadAsset<AudioClip>(musicAsset.name);
                    AudioManager.Instance.PlayMusic(clip);
                    if (oldMuiscAsset != null)
                    {
                        oldMuiscAsset.Unload(false);
                        oldMuiscAsset = null;
                    }
                });
            }
            , null);
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private void OnRequestTransfer(string url, string message)
    {
        VRShopData.Instance.IsCompleted = true;
        try
        {
            WebApiRequestTransferResult data = JsonUtility.FromJson<WebApiRequestTransferResult>(message);
            if (data.code != "0")
            {
                VRShopData.Instance.IsError = true;
                UIMessageSlide.ShowMessage(data.msg);
                return;
            }
            VRShopData.Instance.IsError = false;
            VRShopData.Instance.ResURL = data.data.json_url;
        }
        catch (System.Exception e)
        {
            VRShopData.Instance.IsError = true;
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private void OnCheckRedPacketResult(string url, string message)
    {
        try
        {
            WebApiCheckRedPacketData data = JsonUtility.FromJson<WebApiCheckRedPacketData>(message);
            if (data.code != "0")
            {
                UIMessageSlide.ShowMessage(data.msg);
                return;
            }
            Unity2Native.OpenStorePageByCode(data.data.target_ucode_m.Substring(1), "0");
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private void OnRequestVRShopInfo(string url, string message)
    {
        VRShopData.Instance.IsCompleted = true;
        try
        {
            WebApiReturnVRShopInfo data = JsonUtility.FromJson<WebApiReturnVRShopInfo>(message);
            int code = -1;
            if (int.TryParse(data.code, out code) == false || code != 0)
            {
                VRShopData.Instance.IsError = true;
                UIMessageSlide.ShowMessage(data.msg);
                return;
            }
#if DEMO
            //data.data.vrShopBaseInfo.vrImg = FileUtils.WebPath("aluoduoshi.unity3d");
            data.data.vrShopBaseInfo.vrImg = "https://img.weilingdi.com/vrsource/xiyueHotel.unity3d";
            //data.data.vrShopBaseInfo.vrImg = "http://unity.58wld.com/index.html?https://img.weilingdi.com/test/weimin/1010.json";
            //data.data.baseinfo.c_shoptrade = "10";
            //data.data.vrShopBaseInfo.shopTrade = "";
            //data.data.vrShopBaseInfo.vrImg = "2littlefriends.unity3d";
            //data.data.vrShopBaseInfo.vrImg = "http://unity.58wld.com/?http://testimg.iweilingdi.com/upload/VR/82606d31aa614cb7bea0d7140486de29.json";
            //data.data.vrShopBaseInfo.vrImg = "https://img.weilingdi.com/vrsource/xiyueHotel.unity3d";
#endif
            VRShopData.Instance.VRShopInfo = data.data;
            VRShopData.Instance.IsError = false;
        }
        catch (System.Exception e)
        {
            VRShopData.Instance.IsError = true;
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private void OnRequestNewMsgReturn(string url, string message)
    {
        try
        {
            WebNewMsgData data = JsonUtility.FromJson<WebNewMsgData>(message);
            if (data.code != "0" || data.data == null || data.data.list == null)
            {
                UIMessageSlide.ShowMessage(data.msg);
                return;
            }
            for (int idx = 0; data.data.list.red != null && idx < data.data.list.red.Length; idx++)
            {
                NewMsgData.Instance.MsgQueue.Enqueue(new NewMsg()
                {
                    msg = data.data.list.red[idx],
                    msgType = 0
                });
            }
            for (int idx = 0; data.data.list.coupon != null && idx < data.data.list.coupon.Length; idx++)
            {
                NewMsgData.Instance.MsgQueue.Enqueue(new NewMsg()
                {
                    msg = data.data.list.coupon[idx],
                    msgType = 1
                });
            }
            for (int idx = 0; data.data.list.street_visit != null && idx < data.data.list.street_visit.Length; idx++)
            {
                NewMsgData.Instance.MsgQueue.Enqueue(new NewMsg()
                {
                    msg = data.data.list.street_visit[idx],
                    msgType = 2
                });
            }
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private void OnSendEventResult(string url, string message)
    {
        try
        {
            WebApiReturnSendEventResult data = JsonUtility.FromJson<WebApiReturnSendEventResult>(message);
            if (data.data == null ||
                string.IsNullOrEmpty(data.data.c_name) ||
                string.IsNullOrEmpty(data.data.c_has_get_img))
            {
                return;
            }
            UINewAchieveSlide slide = UIService.Instance.AddSlide<UINewAchieveSlide>();
            slide.SendMessage("InitData", data.data);
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private void OnSendUserInfoReturn(string url, string message)
    {
        try
        {
            WebData data = JsonUtility.FromJson<WebData>(message);
            if (data.code == "0")
            {
                Debug.Log("user info return");
                //AsyncEventDriver.QueueOnMainThread(() =>
                //{
                //    WebApi.RequestFaceCfg();
                //});
            }
            else
            {
                UIMessageSlide.ShowMessage(message);
            }
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private void OnFaceCfg(string url, string message)
    {
        try
        {
            WebApiReturnFaceCfgData data = JsonUtility.FromJson<WebApiReturnFaceCfgData>(message);
            if (data.code == "0")
            {
                msgReady = false;
                BuildFactory.Instance.InitFaceCfg(data.data.list);
                //AsyncEventDriver.QueueOnMainThread(() =>
                //{
                //    WebApi.RequestStoreList(getStoreArgs());
                //});
            }
            else
            {
                UIMessageSlide.ShowMessage(message);
            }
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    private bool msgReady = false;
    private bool streetFirstReady = false;
    private bool isLoadCompleted = false;
    private void OnStoreListData(string url, string message)
    {
        try
        {
            WebApiReturnStoreListData data = JsonUtility.FromJson<WebApiReturnStoreListData>(message);
            if (data.code == "0")
            {
                if (streetFirstReady == false)
                {
                    streetFirstReady = true;
                }
                AsyncEventDriver.QueueOnMainThread(() =>
                {
                    if (data.data.list != null)
                    {
                        for (int idx = 0; idx < data.data.list.Length; idx++)
                        {
                            StoreData.Instance.shopDataQueue.Enqueue(data.data.list[idx]);
                        }
                        Debug.Log("storeNum:" + StoreData.Instance.shopDataQueue.Count);
                        if (isLoadCompleted)
                        {
                            StreetManager.Instance.UpdateData();
                        }
                    }
                    msgReady = true;
                });
            }
            else
            {
                UIMessageSlide.ShowMessage(message);
            }
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }
#endregion

}
