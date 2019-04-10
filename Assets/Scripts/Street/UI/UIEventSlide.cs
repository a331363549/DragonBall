using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NewEngine.Framework.Service;

public class UIEventSlide : UISlide<UIEventLogic>
{
    public override UISlideType SlideType
    {
        get
        {
            return UISlideType.transparent;
        }
    }

    private static UIEventSlide sInstance = null;

    public static void Show(ShopData shopData)
    {
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UIEventSlide>();
            sInstance.SendMessage("InitEvent", shopData);
        }
    }

    public static void Hide()
    {
        if (sInstance != null)
        {
            UIService.Instance.RemoveSlide(sInstance);
            sInstance = null;
        }
    }

}
