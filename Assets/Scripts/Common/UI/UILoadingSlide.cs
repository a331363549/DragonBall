using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NewEngine.Framework.Service;

public class UILoadingSlide : UISlide<UILoadingLogic>
{
    public override UISlideType SlideType
    {
        get
        {
            return UISlideType.opaque;
        }
    }

    private static UILoadingSlide sInstance = null;

    public static void Show()
    {
        //NativeAgent.ShowLoadingUI();
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UILoadingSlide>();
        }
        sInstance.LogicScprit.modenStreetLoading.SetActive(false);
        sInstance.LogicScprit.streetLoading.SetActive(false);
        sInstance.LogicScprit.vrLoading.SetActive(true);
        sInstance.LogicScprit.transparentLoading.SetActive(false);
        UpdateProgress("");
        UpdateProgress(0);
    }
    public static void ModenStreetLoading()
    {
        //NativeAgent.ShowLoadingUI();
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UILoadingSlide>();
        }
        sInstance.LogicScprit.modenStreetLoading.SetActive(true);
        sInstance.LogicScprit.streetLoading.SetActive(false);
        sInstance.LogicScprit.vrLoading.SetActive(false);
        sInstance.LogicScprit.transparentLoading.SetActive(false);
        UpdateProgress("");
    }

    public static void ShowStreetLoading()
    {
        //NativeAgent.ShowLoadingUI();
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UILoadingSlide>();
        }
        sInstance.LogicScprit.modenStreetLoading.SetActive(false);
        sInstance.LogicScprit.streetLoading.SetActive(true);
        sInstance.LogicScprit.vrLoading.SetActive(false);
        sInstance.LogicScprit.transparentLoading.SetActive(false);
        UpdateProgress("");
    }

    public static void ShowTransparent()
    {
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UILoadingSlide>();
        }
        sInstance.LogicScprit.modenStreetLoading.SetActive(false);
        sInstance.LogicScprit.streetLoading.SetActive(false);
        sInstance.LogicScprit.vrLoading.SetActive(false);
        sInstance.LogicScprit.transparentLoading.SetActive(true);
        UpdateProgress("");
    }

    public static void Hide()
    {
        //NativeAgent.HideLoadingUI();
        if (sInstance != null)
        {
            UIService.Instance.RemoveSlide(sInstance);
            sInstance = null;
        }
    }

    public static void UpdateProgress(string progress)
    {
        if (sInstance != null && sInstance.SlideObj)
        {
            sInstance.LogicScprit.progress.text = progress == null ? "" : progress;
        }
    }

    public static void UpdateProgress(float progress)
    {
        Debug.Log("UpdateProgress:" + progress);
        if (sInstance != null && sInstance.SlideObj)
        {
            sInstance.LogicScprit.slider.value = progress;
        }
    }
}
