using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace NewEngine.Framework.UI
{
    public class UIRootLogic : UILogic
    {

        public Camera bindCam;
        public Canvas canvas;

        public bool IsUIContainsScreenPoint(Vector2 screenPoint)
        {
            CanvasRenderer[] array = transform.GetComponentsInChildren<CanvasRenderer>();
            for (int idx = 0; idx < array.Length; ++idx)
            {
                if (array[idx].gameObject != canvas.gameObject && 
                    RectTransformUtility.RectangleContainsScreenPoint(array[idx].GetComponent<RectTransform>(), screenPoint, bindCam))
                {
                    return true;
                }
            }
            return false;
        }

        private void Start()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

    }
}

