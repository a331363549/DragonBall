using NewEngine.Framework.Service;
using NewEngine.Framework.UI.UGUI;
using NewEngine.Utils;
using System;
using System.Collections.Generic;

namespace NewEngine.Framework.UI
{
    public class UIPopMsgCtrl : Singleton<UIPopMsgCtrl>
    {
        private List<string> msgList = new List<string>();
        private Timer popTimer = null;

        public void PopMessage(string message)
        {
            if (popTimer == null)
            {
                popTimer = new Timer("PopMsg", OnPopMsgTick);
                popTimer.Interval = TimeSpan.FromSeconds(0.3);
            }
            msgList.Add(message);
        }

        public void ClearAllPopMsg()
        {

        }

        private void OnPopMsgTick(object sender, long passedTicks)
        {
            if (msgList.Count == 0)
            {
                popTimer.Dispose();
                popTimer = null;
                return;
            }
            UIPopMsgSlide popMsg = UIService.Instance.AddSlide<UIPopMsgSlide>();
            popMsg.SendMessage("PopMessage", new UIPopMsgCfg()
            {
                popMsg = msgList[0],
                onCompleted = OnPopAnimCompleted,
            });
            msgList.RemoveAt(0);
        }

        private void OnPopAnimCompleted(IUISlide slide)
        {
            UIService.Instance.RemoveSlide(slide);
        }
        
    }
}

