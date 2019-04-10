using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Async
{
    public abstract class NewAsyncOperation : IEnumerator
    {

        public abstract float Progress { get; }

        public abstract bool IsDone { get; }

        public abstract bool IsSuccess { get; }

        public object Current
        {
            get
            {
                return null;
            }
        }

        public bool MoveNext()
        {
            return !IsDone;
        }

        public void Reset()
        {
        }
    }
}
