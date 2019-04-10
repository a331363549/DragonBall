using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Service
{

    public class ListenerDriver : CService
    {

        private static ListenerDriver sInstance = null;

        private static bool IsValid()
        {
            if (sInstance == null)
            {
                Debug.LogError("ListenerDriver is NULL");
                return false;
            }
            return true;
        }

        public static void AddListener(int listenerId, Action listener)
        {
            if (IsValid() == false)
            {
                return;
            }
            if (sInstance.OnBeforeAdd(listenerId, listener.GetType()) == false)
            {
                return;
            }
            sInstance.listenerDic[listenerId] = (Action)sInstance.listenerDic[listenerId] + listener;
        }

        public static void AddListener<T>(int listenerId, Action<T> listener)
        {
            if (IsValid() == false)
            {
                return;
            }
            if (sInstance.OnBeforeAdd(listenerId, listener.GetType()) == false)
            {
                return;
            }
            sInstance.listenerDic[listenerId] = (Action<T>)sInstance.listenerDic[listenerId] + listener;
        }

        public static void AddListener<T1, T2>(int listenerId, Action<T1, T2> listener)
        {
            if (IsValid() == false)
            {
                return;
            }
            if (sInstance.OnBeforeAdd(listenerId, listener.GetType()) == false)
            {
                return;
            }
            sInstance.listenerDic[listenerId] = (Action<T1, T2>)sInstance.listenerDic[listenerId] + listener;
        }
        
        public static void RemoveListener(int listenerId, Action listener)
        {
            if (IsValid() == false)
            {
                return;
            }
            if (sInstance.OnBeforeRemove(listenerId, listener.GetType()) == false)
            {
                return;
            }
            sInstance.listenerDic[listenerId] = (Action)sInstance.listenerDic[listenerId] - listener;
            sInstance.OnAfterRemove(listenerId);
        }

        public static void RemoveListener<T>(int listenerId, Action<T> listener)
        {
            if (IsValid() == false)
            {
                return;
            }
            if (sInstance.OnBeforeRemove(listenerId, listener.GetType()) == false)
            {
                return;
            }
            sInstance.listenerDic[listenerId] = (Action<T>)sInstance.listenerDic[listenerId] - listener;
            sInstance.OnAfterRemove(listenerId);

        }

        public static void RemoveListener<T1, T2>(int listenerId, Action<T1, T2> listener)
        {
            if (IsValid() == false)
            {
                return;
            }
            if (sInstance.OnBeforeRemove(listenerId, listener.GetType()) == false)
            {
                return;
            }
            sInstance.listenerDic[listenerId] = (Action<T1, T2>)sInstance.listenerDic[listenerId] - listener;
            sInstance.OnAfterRemove(listenerId);
        }

        public static void DispathMsg(int listenerId, bool nextUpdate = false)
        {
            if (IsValid() == false)
            {
                return;
            }
            if (sInstance.OnBeforeBroadcast(listenerId) == false)
            {
                return;
            }
            Delegate deleg = null;
            if (sInstance.listenerDic.TryGetValue(listenerId, out deleg) && deleg != null)
            {
                Action action = (Action)deleg;
                action();
            }
        }

        public static void DispathMsg<T>(int listenerId, T arg, bool nextUpdate = false)
        {
            if (IsValid() == false)
            {
                return;
            }
            Delegate deleg = null;
            if (sInstance.listenerDic.TryGetValue(listenerId, out deleg) && deleg != null)
            {
                Action<T> action = (Action<T>)deleg;
                action(arg);
            }
            else
            {

            }
        }

        public static void DispathMsg<T1, T2>(int listenerId, T1 arg1, T2 arg2, bool nextUpdate = false)
        {
            if (IsValid() == false)
            {
                return;
            }
            Delegate deleg = null;
            if (sInstance.listenerDic.TryGetValue(listenerId, out deleg) && deleg != null)
            {
                Action<T1, T2> action = (Action<T1, T2>)deleg;
                action(arg1, arg2);
            }
        }

        private Dictionary<int, Delegate> listenerDic = new Dictionary<int, Delegate>();

        protected bool OnBeforeAdd(int id, Type type)
        {
            Delegate deleg;
            if (listenerDic.TryGetValue(id, out deleg) == false)
            {
                listenerDic.Add(id, null);
                deleg = null;
            }
            if (deleg != null && deleg.GetType() != type)
            {
                Debug.LogError("[ListenerDriver]Delegate type is not match");
                return false;
            }
            return true;
        }

        protected bool OnBeforeRemove(int id, Type type)
        {
            Delegate deleg = null;
            if (listenerDic.TryGetValue(id, out deleg) == false)
            {
                return false;
            }
            if (deleg == null)
            {
                listenerDic.Remove(id);
                return false;
            }
            if (deleg.GetType() != type)
            {
                Debug.LogError("[ListenerDriver]Delegate type is not match");
                return false;
            }
            return true;
        }

        protected void OnAfterRemove(int id)
        {
            Delegate deleg = null;
            if (listenerDic.TryGetValue(id, out deleg) == false)
            {
                return ;
            }
            if (deleg == null)
            {
                listenerDic.Remove(id);
            }
        }

        protected bool OnBeforeBroadcast(int id)
        {
            Delegate deleg = null;
            if (listenerDic.TryGetValue(id, out deleg) == false)
            {
                return false;
            }
            if (deleg == null)
            {
                listenerDic.Remove(id);
                return false;
            }
            return true;
        }

        protected override void OnServiceUpdate()
        {
        }

        private void Update()
        {
#if USE_SERVICE_UPDATE
#else
            OnServiceUpdate();
#endif
        }

        private void Awake()
        {
            sInstance = this;
        }

        private void OnDestroy()
        {
            sInstance = null;
        }


    }
}

