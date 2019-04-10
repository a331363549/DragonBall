using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewEngine.Framework.UI;
using NewEngine.Framework.Service;

public class UIBroadcastSlide : UISlide<UIBroadcastLogic>
{
    public override UISlideType SlideType
    {
        get
        {
            return UISlideType.transparent;
        }
    }

    public static bool isEnableShow = true;

    private static UIBroadcastSlide sInstance = null;

    public static void Show()
    {
        if (sInstance == null && isEnableShow)
        {
            sInstance = UIService.Instance.AddSlide<UIBroadcastSlide>();
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
