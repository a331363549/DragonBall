using NewEngine.Framework.Service;
using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPassSlide : UISlide<UIPassLogic>
{
    public override UISlideType SlideType
    {
        get
        {
            return UISlideType.transparent;
        }
    }

    private static UIPassSlide sInstance = null;

    public static void Open()
    {
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UIPassSlide>();
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
