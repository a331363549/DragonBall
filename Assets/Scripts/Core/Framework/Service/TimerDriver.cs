using NewEngine.Framework.Service;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Service
{

    public class TimerDriver : CService
    {

        private static TimerDriver sInstance = null;

        /// <summary>
        /// 计时器列表
        /// </summary>
        private List<Timer> timersList = new List<Timer>();

        /// <summary>
        /// 添加一个新的计时器到队列中
        /// </summary>
        /// <param name="timer"></param>
        public static void AddTimer(Timer timer)
        {
            if (sInstance != null)
            {
                sInstance.timersList.Add(timer);
            }
            else
            {
                Debug.LogError("MainThreadTimerDriver Null");
            }
        }

        /// <summary>
        /// 从队列中删除一个计时器
        /// </summary>
        /// <param name="timer"></param>
        public static void RemoveTimer(Timer timer)
        {
            if (sInstance != null)
            {
                sInstance.timersList.Remove(timer);
            }
            else
            {
                Debug.LogError("MainThreadTimerDriver Null");
            }
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        public static void ClearAll()
        {
            if (sInstance != null)
            {
                sInstance.timersList.Clear();
            }
            else
            {
                Debug.LogError("MainThreadTimerDriver Null");
            }
        }

        protected override void OnServiceUpdate()
        {
            //Profiler.BeginSample("DispatcherTimerDriver.ExecuteTimers");
            int count = timersList.Count;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    timersList[i].ExecuteTimer();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            //Profiler.EndSample();
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
    }


    /// <summary>
    /// 回调通知函数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate void TimerEventHandler(object sender, long passedTicks);

    /// <summary>
    /// Unity中没有找到主线程Timer，模拟实现
    /// </summary>
    public class Timer : IDisposable
    {
        //超过计时器间隔时发生。
        private TimerEventHandler _Tick = null;
        private string _Name = "未知";
        private bool _Started = false; //是否已经开始
        private long _LastTicks = 0;
        private long _passedTicks = 0;

        //必须赋值一个特殊的名称，用于监控cpu占用
        public Timer(string name, TimerEventHandler evtHandler)
        {
            _Name = name;
            _Tick = evtHandler;
            TimerDriver.AddTimer(this);
        }

        /// <summary>
        /// 计时器名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private TimeSpan _Interval = TimeSpan.FromSeconds(1);

        /// <summary>
        /// 设置计时器的时间间隔
        /// </summary>
        public TimeSpan Interval
        {
            get { return _Interval; }
            set { _Interval = value; }
        }

        /// <summary>
        /// 开始计时器
        /// </summary>
        public void Start()
        {
            _Started = true;
            _LastTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        public void Stop()
        {
            _Started = false;
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务
        /// </summary>
        public void Dispose()
        {
            TimerDriver.RemoveTimer(this);
        }

        /// <summary>
        /// 执行timer
        /// </summary>
        public void ExecuteTimer()
        {
            if (_Started == false)
            {
                return;
            }
            long ticks = DateTime.Now.Ticks;
            if (ticks - _LastTicks < _Interval.Ticks)
            {
                return;
            }
            _passedTicks = ticks - _LastTicks;
            _LastTicks = ticks;

            if (null != _Tick)
            {
                long startTicks = DateTime.Now.Ticks / 10000;

                _Tick(this, _passedTicks);

                long elapsedTicks = (DateTime.Now.Ticks / 10000) - startTicks;
                if (elapsedTicks >= 100)
                {
                    Debug.Log("DispatcherTimer.ExecuteTimer, Name=" + _Name + ", Used ticks=" + elapsedTicks);
                }
            }
        }
    }
}