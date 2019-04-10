using System.Collections;
using UnityEngine;
using NewEngine.Framework;
using NewEngine.Framework.Service;
using NewEngine.Framework.Table;
using Table;
using System.Collections.Generic;

public partial class MainLogic : CService
{
    private static MainLogic sInstance = null;
    public static MainLogic Instance
    {
        get
        {
            return sInstance;
        }
    }


    private float moveDist = 0.024f;
    private float maxBackDist = 100f;
    private float enableBackDist = 0;
    public StreetType cur_streetType;
    public List<Texture2D> views_icon;


    public int m_CurMission = 0;
    public Dictionary<string, string> curMission_Data = new Dictionary<string, string>();

    private void OnApplicationQuit()
    {
        GOPool.Instance.ReleaseCache();
        if (GOPool.Instance.MainAsset != null)
        {
            GOPool.Instance.MainAsset.Unload(true);
            GOPool.Instance.MainAsset = null;
        }
        Resources.UnloadUnusedAssets();
    }

    private void Start()
    {
        sInstance = this;
        UIIndexSlide.Open();
        //cur_streetType = StreetType.广场;
        //UIMainSlide.Hide();
        //UIDownPageSlide.Open();
    }

    bool isMusicMute;
    bool isDanMuVisible;
    public void Init()
    {
        //WebApi.Reset();
        //cur_streetType = StreetType.广场;
        //WebRequest.onMsgProcess -= MainLogic.Instance.OnWebMsgProc;
        //WebRequest.onMsgProcess += MainLogic.Instance.OnWebMsgProc;
        //UILoadingSlide.ModenStreetLoading();
        //UserCurState.Instance.moveSpd = moveDist;
        //UserCurState.Instance.maxBackDist = maxBackDist;
        //UserCurState.Instance.enableBackDist = enableBackDist;
        //isMusicMute = PlayerPrefs.GetInt("MusicIsMute", 0) == 1;
        //AudioManager.Instance.MusicMute = isMusicMute;
        //isDanMuVisible = PlayerPrefs.GetInt("DanMuVisible", 1) == 1;
        //UIBroadcastSlide.isEnableShow = isDanMuVisible;

    }

    private void OnDestroy()
    {
    }

    private void Update()
    {
#if USE_SERVICE_UPDATE
#else
        OnServiceUpdate();
#endif
    }

    private readonly float CD = 5f;
    private float shopRequestCD = -1;
    protected override void OnServiceUpdate()
    {
        if (msgReady && StoreData.Instance.shopDataQueue.Count == 0)
        {
            shopRequestCD -= Time.deltaTime;
            if (shopRequestCD < 0f)
            {
                msgReady = false;
                WebApi.RequestStoreList(StoreArgs());
                shopRequestCD = CD;
            }
        }
    }

    public void Back2Pre()
    {
        switch (fromLev)
        {
            case LevelID.Native:
                UIPageSlide.Show();
                break;
            case LevelID.Street:
                StartCoroutine(Back2Street());
                break;
            default:
                break;
        }
    }

 
    public string StoreArgs()
    {
        string store_args = "0";
        //if (cur_streetType == StreetType.古韵街 || cur_streetType == StreetType.精品街)
        //    store_args = "0";
        //else
        //    store_args = store_args = StreetsMaps.Instance.dic_street2industry[cur_streetType];
        return store_args;
    }

    public void Back2App()
    {
        AudioManager.Instance.MusicMute = true;
        UIStreetSlide.Close();
        UIBroadcastSlide.Hide();
        UITriggerSlide.Close();
        Application.Quit();
    }


}
