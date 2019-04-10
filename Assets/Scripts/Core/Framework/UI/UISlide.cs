/*************************************************************************************

* Author:   weimin
* Desc:     幻灯片，一个界面定义为一个幻灯片，幻灯片可以叠加显示，
*           UIService通过UISlide加载和管理界面
*           UISlide就像一个容器，自动加载预设，但并不参与界面内部的逻辑行为
*           内部的逻辑行为由预设上的脚本负责，界面之间通过消息通信

* Date:     
* Version:	1.0

*************************************************************************************/


using NewEngine.Framework.Service;
using NewEngine.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NewEngine.Framework.UI
{
    public enum UISlideType
    {
        none,
        root,
        opaque,
        transparent,
        message,
    }

    public enum UISlideState
    {
        BeforeInit,
        AfterInit,
        BeforeRelease,
        AfterRelease,
    }

    public abstract class IUISlide
    {

        public abstract UISlideType SlideType { get; }

        public abstract Vector3 Position
        {
            get; set;
        }

        public abstract Vector3 EulerAngles
        {
            get; set;
        }

        public abstract Vector3 LossyScale
        {
            get;
        }

        public abstract Vector3 LocalPosition
        {
            get; set;
        }

        public abstract Vector3 LocalEulerAngles
        {
            get; set;
        }

        public abstract Vector3 LocalScale
        {
            get; set;
        }

        public abstract string Name { get; }

        public abstract uint Uid { get; }

        public abstract bool Sleep { set; get; }

        public abstract bool Visible { set; get; }

        public abstract bool Active { set; get; }

        public abstract void AddStateListener(Action<UISlideState> listener);

        public abstract void RemoveStateListener(Action<UISlideState> listener);

        public abstract void ClearStateListener();

        public abstract void Initialize();

        public abstract void Release();

        public abstract void SetParent(Transform parent);

        public abstract void SendMessage(string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver);

        public abstract void BroadcastMessage(string methodName, object parameter = null, SendMessageOptions options = SendMessageOptions.RequireReceiver);
    }

    public abstract class UISlide<T> : IUISlide 
        where T : UILogic
    {

        public virtual string PrefabPath
        {
            get
            {
                return GetType().ToString().Replace('.', '/');
            }
        }

        public sealed override Vector3 Position
        {
            get { return LogicScprit ? LogicScprit.transform.position : Vector3.zero; }
            set { if (LogicScprit) { LogicScprit.transform.position = value; } }
        }

        public sealed override Vector3 EulerAngles
        {
            get { return LogicScprit ? LogicScprit.transform.eulerAngles : Vector3.zero; }
            set { if (LogicScprit) { LogicScprit.transform.eulerAngles = value; } }
        }

        public sealed override Vector3 LossyScale
        {
            get { return LogicScprit ? LogicScprit.transform.lossyScale : Vector3.one; }
        }

        public sealed override Vector3 LocalPosition
        {
            get { return LogicScprit ? LogicScprit.transform.localPosition : Vector3.zero; }
            set { if (LogicScprit) { LogicScprit.transform.localPosition = value; } }
        }

        public sealed override Vector3 LocalEulerAngles
        {
            get { return LogicScprit ? LogicScprit.transform.localEulerAngles : Vector3.zero; }
            set { if (LogicScprit) { LogicScprit.transform.localEulerAngles = value; } }
        }

        public sealed override Vector3 LocalScale
        {
            get { return LogicScprit ? LogicScprit.transform.localScale : Vector3.one; }
            set { if (LogicScprit) { LogicScprit.transform.localScale = value; } }
        }

        private Action<UISlideState> stateListener = null;

        private GameObject slideObj;
        protected GameObject SlideObj
        {
            get { return slideObj; }
        }

        private T uiLogic;
        protected T LogicScprit
        {
            get { return uiLogic; }
        }

        private string name = string.Empty;
        public sealed override string Name
        {
            get
            {
                return name;
            }
        }

        public sealed override uint Uid
        {
            get
            {
                return 0;
            }
        }

        public sealed override bool Sleep
        {
            get
            {
                if (slideObj == null)
                {
                    Debug.LogError("slide is NULL");
                    return false;
                }

                if (uiLogic == null)
                {
                    return true;
                }
                return uiLogic.enabled == false;
            }
            set
            {
                if (slideObj == null)
                {
                    Debug.LogError("slide is NULL");
                    return ;
                }

                if (uiLogic == null)
                {
                    return ;
                }
                uiLogic.enabled = !value;
            }
        }

        public sealed override bool Visible
        {
            get
            {
                if (slideObj == null)
                {
                    Debug.LogError("slide is NULL");
                    return false;
                }
                return slideObj.layer == 5;
            }
            set
            {
                if (slideObj == null)
                {
                    Debug.LogError("slide is NULL");
                    return;
                }
                GOUtils.SetLayer(slideObj, value ? 5 : 31);
            }
        }

        public sealed override bool Active
        {
            get
            {
                if (slideObj == null)
                {
                    Debug.LogError("slide is NULL");
                    return false;
                }
                return slideObj.activeInHierarchy;
            }
            set
            {
                if (slideObj == null)
                {
                    Debug.LogError("slide is NULL");
                    return;
                }
                slideObj.SetActive(value);
            }
        }

        public sealed override void AddStateListener(Action<UISlideState> listener)
        {
            stateListener -= listener;
            stateListener += listener;
        }

        public sealed override void RemoveStateListener(Action<UISlideState> listener)
        {
            stateListener -= listener;
        }

        public sealed override void ClearStateListener()
        {
            stateListener = null;
        }

        public sealed override void Initialize()
        {
            CallListener(UISlideState.BeforeInit);
            OnInitialize();
            ///
            if (slideObj == null)
            {
                Object obj = (Object)Resources.Load(PrefabPath);
                slideObj = obj == null ? null: GameObject.Instantiate(obj) as GameObject;
                if (slideObj != null)
                {
                    uiLogic = slideObj.GetComponent<T>();
                    if (uiLogic == null)
                    {
                        uiLogic = slideObj.AddComponent<T>();
                    }
                    uiLogic.bindSlide = this;
                }
            }
            if (slideObj != null)
            {
                name = FileUtils.ModifyPath(PrefabPath).Replace('/', '.');
                slideObj.name = name.Substring(name.LastIndexOf('.') + 1);
                slideObj.transform.SetParent(null);
                Text[] textArray = slideObj.GetComponentsInChildren<Text>(true);
                for (int idx = textArray.Length - 1; idx >= 0; --idx)
                {
                    if (textArray[idx].name.StartsWith("+"))
                    {
                        textArray[idx].text = LanguageService.GetLang(string.Format("{0}.{1}", slideObj.name, textArray[idx].name.Substring(1)));
                    }
                }
            }
            OnInitialized();
            CallListener(UISlideState.AfterInit);
        }

        public sealed override void Release()
        {
            CallListener(UISlideState.BeforeRelease);
            OnRelease();
            ///
            if (slideObj != null)
            {
                //slideObj.SetActive(false);
                GameObject.Destroy(slideObj);
                slideObj = null;
            }
            OnReleased();
            CallListener(UISlideState.AfterRelease);
        }

        public override void SetParent(Transform parent)
        {
            if (slideObj == null)
            {
                Debug.LogError("slide is NULL");
                return;
            }
            RectTransform rectTrans = slideObj.GetComponent<RectTransform>();
            rectTrans.SetParent(parent, false);
        }

        public override void SendMessage(string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            if (slideObj == null)
            {
                Debug.LogError("slide is NULL");
                return;
            }
            slideObj.SendMessage(methodName, value, options);
        }

        public override void BroadcastMessage(string methodName, object parameter = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
        {
            if (slideObj == null)
            {
                Debug.LogError("slide is NULL");
                return;
            }
            slideObj.BroadcastMessage(methodName, parameter, options);
        }

        protected virtual void OnInitialize()
        {

        }

        protected virtual void OnInitialized()
        {

        }

        protected virtual void OnRelease()
        {

        }

        protected virtual void OnReleased()
        {

        }

        private void CallListener(UISlideState state)
        {
            if (stateListener != null)
            {
                stateListener(state);
            }
        }
    }
}

