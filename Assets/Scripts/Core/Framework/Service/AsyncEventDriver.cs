using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewEngine.Framework.Service
{
    public class AsyncEventDriver : CService
    {

        private const int MaxEventPerFrame = 10;

        private static AsyncEventDriver sInstance = null;

        public struct AsyncQueueItem
        {
            public float time;
            public Action action;
        }

        private static List<Action> _actions = new List<Action>();
        private static List<AsyncQueueItem> _delayed = new List<AsyncQueueItem>();

        private List<Action> _currentActions = new List<Action>();
        private List<AsyncQueueItem> _currentDelayed = new List<AsyncQueueItem>();

        public static void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }

        public static void QueueOnMainThread(Action action, float time)
        {
            if (sInstance == null)
            {
                Debug.LogError("AsyncEventDriver is NULL");
                return;
            }

            if (time != 0)
            {
                lock (_delayed)
                {
                    _delayed.Add(new AsyncQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (_actions)
                {
                    _actions.Add(action);
                }
            }
        }

        private int sleepFrames = 0;
        protected override void OnServiceUpdate()
        {
            lock (_actions)
            {
                if (_actions.Count > 0)
                {
                    _currentActions.AddRange(_actions);
                    _actions.Clear();
                }
                else
                {
                    ++sleepFrames;
                }
            }
            

            if (_currentActions.Count > 0)
            {
                int count = _currentActions.Count < MaxEventPerFrame ? _currentActions.Count : MaxEventPerFrame;
                //Debug.LogError(string.Format(
                //    "[AsyncEventDriver] Sleep Frames:{0}, Process Count:{1}, Total Count:{2}", 
                //    sleepFrames, count, _currentActions.Count));
                if (sleepFrames > 0)
                {
                    sleepFrames = 0;
                }
                else
                {
                    //count = _currentActions.Count;
                }
                for (int idx = 0; idx < count; ++idx)
                {
                    try
                    {
                        _currentActions[idx]();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
                _currentActions.RemoveRange(0, count);
                // _currentActions.Clear();
            }
            else
            {
            }

            lock (_delayed)
            {
                if (_delayed.Count > 0)
                {
                    _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                    for (int idx = 0; idx < _currentDelayed.Count; ++idx)
                    {
                        _delayed.Remove(_currentDelayed[idx]);
                    }
                }
            }

            if (_currentDelayed.Count > 0)
            {
                //Debug.LogError("_currentDelayed.Count:" + _currentActions.Count);
                for (int idx = 0; idx < _currentDelayed.Count; ++idx)
                {
                    try
                    {
                        _currentDelayed[idx].action();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
                _currentDelayed.Clear();
            }
        }

        private void Awake()
        {
            sInstance = this;
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
        }
    }
}
