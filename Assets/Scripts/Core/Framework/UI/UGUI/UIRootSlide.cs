using UnityEngine;

namespace NewEngine.Framework.UI.UGUI
{

    public class UIRootSlide : UISlide<UIRootLogic>
    {

        public override UISlideType SlideType
        {
            get
            {
                return UISlideType.root;
            }
        }

        public Camera UIRootCamera
        {
            get
            {
                return LogicScprit.bindCam;
            }
        }

        public void AddChild(IUISlide uiSlide)
        {
            if (uiSlide != null)
            {
                uiSlide.SetParent(LogicScprit.canvas.transform);
            }
        }

        public bool IsUIContainsScreenPoint(Vector2 screenPoint)
        {
            return LogicScprit.IsUIContainsScreenPoint(screenPoint);
        }
    }
}

