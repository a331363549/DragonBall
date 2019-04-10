using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayerId
{
    Default,
    TransparentFX,
    IgnoreReycast,
    Layer3,
    Water,
    UI,
    Layer6,
    Layer7,
    Building,
    Item,
    Store,
    Trigger
}

public enum BCListenerId
{
    DayNightSwitch
}

public class Define
{
    public const string platformTag =
#if UNITY_EDITOR || UNITY_STANDALONE
        "-apk"
#elif UNITY_IPHONE
        "-ios"
#else
        "-apk"
#endif
        ;
}

