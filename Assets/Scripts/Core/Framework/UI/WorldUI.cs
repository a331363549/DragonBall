using NewEngine.Framework.UI;
using NewEngine.Framework.UI.UGUI;
using UnityEngine;

namespace NewEngine.Framework.Service
{

    public class WorldUI : CService
    {
        private static WorldUI sInstance = null;
        public static WorldUI Instance
        {
            get
            {
                return sInstance;
            }
        }

        private WUIRootSlide stageRoot;

        public void SetStandardSize(Vector2Int size)
        {
            if (stageRoot != null)
            {
                stageRoot.SendMessage("SetStandardSize", size);
            }
        }

        public bool IsUIContainsScreenPoint(Vector3 worldPoint)
        {
            return stageRoot != null && stageRoot.IsUIContainsScreenPoint(worldPoint);
        }

        public T AddSlide<T>() where T : IUISlide
        {
            T slide = System.Activator.CreateInstance<T>();
            slide.Initialize();
            stageRoot.AddChild(slide);
            return slide;
        }

        public void RemoveSlide(IUISlide slide)
        {
            if (slide != null)
            {
                slide.SetParent(null);
                slide.Release();
            }
        }

        protected override void OnServiceUpdate()
        {
        }

        private void Awake()
        {
            sInstance = this;
            if (stageRoot == null)
            {
                stageRoot = new WUIRootSlide();
                stageRoot.Initialize();
            }
        }

        private void Start()
        {
        }

        private void Update()
        {
#if USE_SERVICE_UPDATE
#else
            OnServiceUpdate();
#endif
        }
    }
}

