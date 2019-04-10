using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.UI.UGUI
{

    public class UIPopMsgSlide : UISlide<UIPopMsgLogic>
    {

        public override UISlideType SlideType
        {
            get
            {
                return UISlideType.message;
            }
        }
    }
}
