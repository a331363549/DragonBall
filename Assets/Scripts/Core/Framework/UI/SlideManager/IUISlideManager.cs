using NewEngine.Framework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.UI
{
    public abstract class IUISlideManager
    {
        internal abstract void Add(IUISlide slide);

        internal abstract void Remove(IUISlide slide);

        public abstract IUISlide Find(string name);

        public abstract IUISlide TopSlide();

        public abstract int GetClippedSlideCount();

        public abstract int GetNormalSlideCount();

        public abstract int GetMessageSlideCount();
    }
}

