using NewEngine.Framework;
using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting : CService
{
    private static GlobalSetting sInstance = null;
    public static GlobalSetting Instance
    {
        get
        {
            return sInstance;
        }
    }

    public void SetHalfQuality()
    {
        isHalfResolution = true;
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            Screen.SetResolution(originWidth >> 1, originHeight >> 1, false);
        }
        else if (Screen.orientation == ScreenOrientation.Landscape)
        {
            Screen.SetResolution(originHeight >> 1, originWidth >> 1, false);
        }
        QualitySettings.SetQualityLevel(1, true);
    }

    public void SetNormalQuality()
    {
        //isHalfResolution = false;
        //if (Screen.orientation == ScreenOrientation.Portrait)
        //{
        //    Screen.SetResolution(originWidth, originHeight, false);
        //}
        //else if (Screen.orientation == ScreenOrientation.Landscape)
        //{
        //    Screen.SetResolution(originHeight, originWidth, false);
        //}
        //QualitySettings.SetQualityLevel(2, true);
        //if (EnvironmentMng.Instance.IsDay)
        //{
        //    QualitySettings.shadows = ShadowQuality.All;
        //}
        //else
        //{
        //    QualitySettings.shadows = ShadowQuality.Disable;
        //}
    }

    public bool IsLowResolution
    {
        get
        {
            return isHalfResolution;
        }
    }



    private int originHeight;
    private int originWidth;
    private bool isHalfResolution = false;

    private void Start()
    {
        sInstance = this;
        originHeight = Screen.height;
        originWidth = Screen.width;
        //SetHalfResolution();
        Application.targetFrameRate = 30;
        //QualitySettings.shadows = ShadowQuality.Disable;
    }
    private void Update()
    {
#if USE_SERVICE_UPDATE
#else
        OnServiceUpdate();
#endif
    }

    protected override void OnServiceUpdate()
    {
    }
}
