using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.UI
{
    public class UGUISlideManager : IUISlideManager
    {
        private List<IUISlide> messageList = new List<IUISlide>();
        private List<IUISlide> visibleList = new List<IUISlide>();
        private List<IUISlide> clippedList = new List<IUISlide>();

        public void ClearAll()
        {

        }

        internal override void Add(IUISlide slide)
        {
            if (slide.SlideType == UISlideType.message)
            {
                messageList.Add(slide);
            }
            else if (slide.SlideType == UISlideType.opaque)
            {
                clippedList.AddRange(visibleList);
                visibleList.Clear();
                visibleList.Add(slide);
            }
            else
            {
                visibleList.Add(slide);
            }
            FreshSlide();
        }

        internal override void Remove(IUISlide slide)
        {
            if (slide.SlideType == UISlideType.message)
            {
                messageList.Remove(slide);
            }
            else if (clippedList.Contains(slide))
            {
                clippedList.Remove(slide);
            }
            else if (visibleList.Contains(slide))
            {
                visibleList.Remove(slide);
                int pos = clippedList.FindLastIndex((e) => { return e.SlideType == UISlideType.opaque; });
                if (pos == -1)
                {
                    return;
                }
                visibleList.InsertRange(0, clippedList.GetRange(pos, clippedList.Count - pos));
                clippedList.RemoveRange(pos, clippedList.Count - pos);
            }
            FreshSlide();
        }

        public override IUISlide Find(string name)
        {
            IUISlide slide = visibleList.Find((e) => FindFunc(e, name));
            if (slide != null)
            {
                return slide;
            }
            slide = clippedList.Find((e) => FindFunc(e, name));
            if (slide != null)
            {
                return slide;
            }
            return messageList.Find((e) => FindFunc(e, name));
        }

        public override IUISlide TopSlide()
        {
            if (visibleList.Count == 0)
            {
                return null;
            }
            return visibleList[visibleList.Count - 1];
        }

        public override int GetClippedSlideCount()
        {
            return clippedList.Count;
        }

        public override int GetNormalSlideCount()
        {
            return visibleList.Count;
        }

        public override int GetMessageSlideCount()
        {
            return messageList.Count;
        }

        private bool FindFunc(IUISlide slide, string name)
        {
            return slide.Name == name;
        }

        private void FreshSlide()
        {
            // 对visiblelist中的slide排序并重新设置顺序
        }
    }
}

