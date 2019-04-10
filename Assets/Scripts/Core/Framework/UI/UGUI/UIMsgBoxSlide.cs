using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.UI.UGUI
{
    public class UIMsgBoxSlide : UISlide<UIMsgBoxLogic>
    {
        public override UISlideType SlideType
        {
            get
            {
                return UISlideType.transparent;
            }
        }
    }
}

