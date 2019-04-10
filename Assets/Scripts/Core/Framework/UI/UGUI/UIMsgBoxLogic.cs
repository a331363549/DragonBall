using NewEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewEngine.Framework.UI
{
    public class UIMsgBoxCfg
    {
        public string title;
        public string message;
        public string confirmTxt;
        public Action clickClose;
        public Action clickConfirm;
    }

    public class UIMsgBoxLogic : UILogic
    {

        public Text title;
        public Text message;
        public Button closeBtn;
        public Button confirmBtn;
        public Text confirmTxt;

        private Action onClose;
        private Action onConfirm;

        // Call by SendMessage
        public void Initialize(UIMsgBoxCfg args)
        {
            title.text = args.title;
            message.text = args.message;
            confirmTxt.text = args.confirmTxt;
            onClose = args.clickClose;
            onConfirm = args.clickConfirm;
        }

        private void Start()
        {
            GOUtils.AddBtnEvtListener(confirmBtn, OnClickConfirm);
            GOUtils.AddBtnEvtListener(closeBtn, OnClickClose);
        }

        private void OnDestroy()
        {
            GOUtils.RemoveAllBtnEvtListener(confirmBtn);
            GOUtils.RemoveAllBtnEvtListener(closeBtn);
        }

        // Call by ConfirmButton
        private void OnClickConfirm()
        {
            if (onConfirm != null)
            {
                onConfirm();
            }
            Debug.Log("OnClickConfirm");
        }

        // Call by CloseButton
        private void OnClickClose()
        {
            if (onClose != null)
            {
                onClose();
            }
        }
    }
}

