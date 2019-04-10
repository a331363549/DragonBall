using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewEngine.Framework.UI
{

    public class UIPopMsgCfg
    {
        public string popMsg;
        public Action<IUISlide> onCompleted;
    }

    public class UIPopMsgLogic : UILogic
    {
        public Text popMsg;
        private Action<IUISlide> onCompleted;
        
        public void PopMessage(UIPopMsgCfg args)
        {
            popMsg.text = args.popMsg;
            onCompleted = args.onCompleted;
        }

        public void OnAnimComplete()
        {
            if (onCompleted != null)
            {
                onCompleted(bindSlide);
            }
        }
    }
}
