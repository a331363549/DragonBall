using NewEngine.Framework.Service;
using NewEngine.Framework.UI.UGUI;
using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.UI
{

    public class UIMsgBoxCtrl : Singleton<UIMsgBoxCtrl>
    {

        private List<UIMsgBoxCfg> msgList = new List<UIMsgBoxCfg>();
        private UIMsgBoxSlide msgBoxSlide = null;

        public void OpenMsgBox(UIMsgBoxCfg cfg)
        {
            if (cfg == null)
            {
                return;
            }
            cfg.clickClose += OnCompleted;
            cfg.clickConfirm += OnCompleted;
            msgList.Add(cfg);
            if (msgBoxSlide != null)
            {
                return;
            }

            msgBoxSlide = UIService.Instance.AddSlide<UIMsgBoxSlide>();
            msgBoxSlide.SendMessage("Initialize", msgList[0]);
        }

        public void CloseMsgBox()
        {
            if (msgList.Count > 0)
            {
                msgList[0].clickClose();
            }            
        }

        private void OnCompleted()
        {
            msgList[0].clickClose -= OnCompleted;
            msgList[0].clickConfirm -= OnCompleted;
            msgList.RemoveAt(0);
            if (msgList.Count == 0)
            {
                UIService.Instance.RemoveSlide(msgBoxSlide);
                msgBoxSlide = null;
            }
        }
    }
}

