using NewEngine.Framework.Service;
using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameUISlide : UISlide<UIGameUILogic>
{
    public override UISlideType SlideType
    {
        get
        {
            return UISlideType.transparent;
        }
    }

    private static UIGameUISlide sInstance = null;

    public static void Open()
    {
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UIGameUISlide>();
        }
    }

    public static void Close()
    {
        if (sInstance != null)
        {
            UIService.Instance.RemoveSlide(sInstance);
            sInstance = null;
        }
    }
}
