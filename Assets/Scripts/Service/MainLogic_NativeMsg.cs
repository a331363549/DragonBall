using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class MainLogic
{
    public void SetContent(string content)
    {
        Init();
        if (!File.Exists(FileUtils.GetPersistentPath(streetSource)))
        {
            string filePath = FileUtils.GetPersistentPath(streetSource);
            Debug.Log("SetContent Error, file is not exist->" + filePath);
            return;
        }
        ContentCfg contentCfg = JsonUtility.FromJson<ContentCfg>(content);
        WebApi.SetWebApiPath(contentCfg.baseUrl, contentCfg.streetUrl);
        if (UserCamera.Instance.GameCamera)
        {
            Skybox skyBox = UserCamera.Instance.GameCamera.GetComponent<Skybox>();
            skyBox.material = null;
        }
        if (contentCfg.isRoad == "1" || streetSaveDatas == null)
        {
            StartCoroutine(LoadSquare());
        }
        else
        {
            StartCoroutine(Back2Street());
        }
       // UserCamera.Instance.logTxt = new GUIContent(content.Replace(",", "\n") + "\n" + msg);
    }


    // 启动unity时，unity端逻辑就绪后向app端请求用户信息的返回接口
    // 包括用户id，经度，纬度，身份，城市，可选参数商户id
    // 商户id不为空时，打开商户的全景模式
    // 防止unity在后台被销毁再重新创建时收不到OnResumeUnity的消息，或中间有间隔，出现画面跳转
    public void SetUserInfo(string userInfo)
    {
        Debug.Log(userInfo);
        try
        {
            UserInfo data = JsonUtility.FromJson<UserInfo>(userInfo);
            UserData.Instance.UserInfo = data;
            if (string.IsNullOrEmpty(data.c_city) &&
                string.IsNullOrEmpty(data.province) &&
                string.IsNullOrEmpty(data.lat) &&
                string.IsNullOrEmpty(data.lng))
            {
                Debug.Log("user Info has empty");
                //WebApi.RequestFaceCfg();
                //WebApi.RequestStoreList(getStoreArgs());
            }
            else
            {
                WebApi.SendUserInfo();
            }
        }
        catch (System.Exception e)
        {
            UIMessageSlide.ShowMessage(e.Message);
        }
    }

    public void SetUserId(string userId)
    {
        if (UserData.Instance.UserInfo != null)
        {
            UserData.Instance.UserInfo.userId = userId;
        }
        else
        {
            UserData.Instance.UserInfo = new UserInfo();
            UserData.Instance.UserInfo.userId = userId;
        }
    }
}
