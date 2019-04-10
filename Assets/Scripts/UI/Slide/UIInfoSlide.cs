using NewEngine.Framework.Service;
using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInfoSlide : UISlide<UIInfoLogic>
{
    public override UISlideType SlideType
    {
        get
        {
            return UISlideType.transparent;
        }
    }

    private static UIInfoSlide sInstance = null;

    public static void Open()
    {
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UIInfoSlide>();
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
