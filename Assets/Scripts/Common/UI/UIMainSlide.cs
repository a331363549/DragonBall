using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainSlide
{
    public static void Show()
    {
        Unity2Native.ShowMainUI();
    }

    public static void Hide()
    {
        Unity2Native.HideMainUI();
    }
}
