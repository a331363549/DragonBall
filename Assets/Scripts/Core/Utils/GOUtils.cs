using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NewEngine.Utils
{
    public class GOUtils
    {
        public static void SetLayer(GameObject go, int layer, bool updateChildren = true)
        {
            go.layer = layer;
            if (updateChildren == false)
            {
                return;
            }
            Transform[] children = go.transform.GetComponentsInChildren<Transform>();
            for (int idx = children.Length - 1; idx >= 0; --idx)
            {
                children[idx].gameObject.layer = layer;
            }
        }

        public static void AddBtnEvtListener(Button btn, UnityAction listener)
        {
            if (btn == null)
            {
                return;
            }
            if (btn.onClick == null)
            {
                btn.onClick = new Button.ButtonClickedEvent();
            }
            btn.onClick.RemoveListener(listener);
            btn.onClick.AddListener(listener);
        }

        public static void RemoveBtnEvtListener(Button btn, UnityAction listener)
        {
            if (btn == null)
            {
                return;
            }
            btn.onClick.RemoveListener(listener);
        }

        public static void RemoveAllBtnEvtListener(Button btn)
        {
            if (btn == null)
            {
                return;
            }
            btn.onClick.RemoveAllListeners();
        }
    }
}
