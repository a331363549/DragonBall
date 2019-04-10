using NewEngine.Framework.Service;
using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITriggerSlide : UISlide<UITriggerLogic>
{
    public override UISlideType SlideType
    {
        get
        {
            return UISlideType.transparent;
        }
    }

    private static UITriggerSlide sInstance = null;

    
    public static void Show()
    {
        if (sInstance == null)
        {
            sInstance = UIService.Instance.AddSlide<UITriggerSlide>();
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
