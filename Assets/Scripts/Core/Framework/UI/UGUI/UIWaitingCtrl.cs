using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewEngine.Utils;
using NewEngine.Framework.Service;
using NewEngine.Framework.UI.UGUI;

namespace NewEngine.Framework.UI
{
    public class UIWaitingCtrl : Singleton<UIWaitingCtrl>
    {
        private IUISlide waitingSlide = null;
        private int refCount = 0;


        /// <summary>
        /// 显示网络等待窗口
        /// </summary>  
        public void ShowWaiting()
        {
            if (waitingSlide == null)
            {
                waitingSlide = UIService.Instance.AddSlide<UIWaitingSlide>();
            }
            refCount++;
        }

        /// <summary>
        /// 隐藏网络等待窗口
        /// </summary>  
        public void HideWaiting(bool hideAll = false)
        {
            if (waitingSlide == null)
            {
                return;
            }

            refCount--;
            if (UIWaitingCtrl.Instance.refCount == 0 || hideAll)
            {
                refCount = 0;
                UIService.Instance.RemoveSlide(waitingSlide);
                waitingSlide = null;
            }
        }
    }
}

