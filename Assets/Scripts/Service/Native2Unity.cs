//using NewEngine.Framework;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Native2Unity : CService
//{
//    public void SetContent(string content)
//    {
//        ContentCfg contentCfg = JsonUtility.FromJson<ContentCfg>(content);
//        WebApi.SetWebApiPath(contentCfg.baseUrl, contentCfg.streetUrl);
//        if (Camera.main)
//        {
//            Skybox skyBox = Camera.main.GetComponent<Skybox>();
//            skyBox.material = null;
//        }
//        if (string.IsNullOrEmpty(contentCfg.code))
//        {
//            MainLogic.Instance.Switch2Street(contentCfg.isRoad == "1");
//        }
//        else
//        {
//            MainLogic.Instance.Switch2Shop(contentCfg.code, contentCfg.isDirect == "1");
//        }
//    }
//    // 启动unity时，unity端逻辑就绪后向app端请求用户信息的返回接口
//    // 包括用户id，经度，纬度，身份，城市，可选参数商户id
//    // 商户id不为空时，打开商户的全景模式
//    // 防止unity在后台被销毁再重新创建时收不到OnResumeUnity的消息，或中间有间隔，出现画面跳转
//    public void SetUserInfo(string userInfo)
//    {
//        Debug.Log(userInfo);
//        try
//        {
//            UserInfo data = JsonUtility.FromJson<UserInfo>(userInfo);
//            UserData.Instance.UserInfo = data;
//            if (string.IsNullOrEmpty(data.c_city) &&
//                string.IsNullOrEmpty(data.province) &&
//                string.IsNullOrEmpty(data.lat) &&
//                string.IsNullOrEmpty(data.lng))
//            {
//                WebApi.RequestFaceCfg();
//                WebApi.RequestStoreList();
//            }
//            else
//            {
//                WebApi.SendUserInfo();
//            }
//        }
//        catch (System.Exception e)
//        {
//            UIMessageSlide.ShowMessage(e.Message);
//        }
//    }
//    public void SetUserId(string userId)
//    {
//        if (UserData.Instance.UserInfo != null)
//        {
//            UserData.Instance.UserInfo.userId = userId;
//        }
//        else
//        {
//            UserData.Instance.UserInfo = new UserInfo();
//            UserData.Instance.UserInfo.userId = userId;
//        }
//    }
//}
