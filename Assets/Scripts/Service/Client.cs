using NewEngine;
using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class Client : NewClient
{

    public void onDestroy()
    {
        //UnregisterService<ConsoleService>();AudioManager
        UnregisterService<MainLogic>();
        //UnregisterService<WebRequest>();
        UnregisterService<AudioManager>();
        //UnregisterService<DownloadService>();
        UnregisterService<GlobalSetting>();
        UnregisterService<LanguageService>();
        UnregisterService<AsyncEventDriver>();
        //UnregisterService<ListenerDriver>();
        //UnregisterService<FingerGestures>();
        //UnregisterService<Native2Unity>();
        //UnregisterService<Unity2Native>();
        UnregisterService<UIService>();
        UnregisterService<GOPool>();
    }

    private void Start()
    {
        Debug.Log(Application.streamingAssetsPath);
        RegisterService<GOPool>();
        RegisterService<UIService>();
        //RegisterService<Native2Unity>();
        //RegisterService<Unity2Native>();
        //RegisterService<FingerGestures>();
        RegisterService<ListenerDriver>();
        RegisterService<LanguageService>();
        RegisterService<AsyncEventDriver>();
        RegisterService<GlobalSetting>();
        //RegisterService<DownloadService>();
        RegisterService<AudioManager>();
        //RegisterService<WebRequest>();
        RegisterService<MainLogic>();
    }


}
