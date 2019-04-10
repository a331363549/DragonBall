using System;
using UnityEngine;

namespace NewEngine.Framework.UI.UGUI
{

    public class WUIRootSlide : UISlide<WUIRootLogic>
    {

        public override UISlideType SlideType
        {
            get
            {
                return UISlideType.root;
            }
        }

        public void AddChild(IUISlide uiSlide)
        {
            if (uiSlide != null)
            {
                uiSlide.SetParent(LogicScprit.canvas.transform);
            }
        }

        public bool IsUIContainsScreenPoint(Vector3 worldPoint)
        {
            return LogicScprit.IsUIContainsScreenPoint(worldPoint);
        }
    }
}
