using NewEngine.Framework.UI;
using NewEngine.Framework.UI.UGUI;
using UnityEngine;

public enum ChildWindowModalType
{
    //非模态窗口
    None = 0,

    //纯黑背景的模态窗口，一级窗口使用，解决手机上显示窗口难看的问题
    BlackBak = 1,

    //透明背景的模态窗口，二级窗口或小窗口使用
    TransBak = 2,

    //半透明背景的模态窗口
    Translucent = 3,

    //半透明背景的模态窗口，副本通关等
    Translucent2 = 4,

    //半透明背景的模态窗口，同3，但此窗口为特殊类型，当父窗口显示了模型，子窗口显示时又切换成了gui图层时，需要使用此类型
    TranslucentGUI = 5,
};


namespace NewEngine.Framework.Service
{

    public class UIService : CService
    {
        private static UIService sInstance = null;
        public static UIService Instance { get { return sInstance; } }

        private UIRootSlide stageRoot;

        public IUISlideManager SlideManager { get; private set; }

        public bool IsUIContainsScreenPoint(Vector3 screenPoint)
        {
            return stageRoot != null && stageRoot.IsUIContainsScreenPoint(screenPoint);
        }

        public Camera UICamera
        {
            get
            {
                return stageRoot != null ? stageRoot.UIRootCamera : null;
            }
        }

        public T AddSlide<T>() where T : IUISlide
        {
            T slide = System.Activator.CreateInstance<T>();
            slide.Initialize();
            SlideManager.Add(slide);
            stageRoot.AddChild(slide);
            return slide;
        }

        public void RemoveSlide(IUISlide slide)
        {
            if (slide != null)
            {
                SlideManager.Remove(slide);
                //slide.SetParent(null);
                slide.Release();
            }
        }

        protected override void OnServiceUpdate()
        {
        }

        private void Awake()
        {
            sInstance = this;
            SlideManager = new UGUISlideManager();
        }

        private void Start()
        {
            if (GameObject.Find("UIRootSlide") == null)
            {
                stageRoot = new UIRootSlide();
                stageRoot.Initialize();
            }
        }

        private void Update()
        {
#if USE_SERVICE_UPDATE
#else
            OnServiceUpdate();
#endif
        }

        private void OnDestroy()
        {
            sInstance = null;
            SlideManager = null;
        }

    }
}
