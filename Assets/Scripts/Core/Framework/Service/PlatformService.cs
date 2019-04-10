using System;
using UnityEngine;

namespace NewEngine.Framework.Service
{
    public class PlatformService : CService
    {
        private static PlatformService sInstance = null;

        public static void SendMessageToPlatform<T>(T msg) where T : Msg
        {
            if (sInstance == null)
            {
                Debug.LogError("PlatformService is NULL");
                return;
            }
            if (sInstance.assistant == null)
            {
                Debug.LogError("PlatformService hasn't Init");
                return;
            }
            if (msg == null)
            {
                Debug.LogError("msg");
                return;
            }
            sInstance.assistant.SendMessage(JsonUtility.ToJson(msg));
        }

        private Assistant assistant = null;
        public static void Init(Assistant platformAssistant)
        {
            if (sInstance == null)
            {
                Debug.LogError("PlatformService is NULL");
                return;
            }
            sInstance.assistant = platformAssistant;
        }


        public void OnPlatformMessage(string msg)
        {
            if (sInstance == null)
            {
                Debug.LogError("PlatformService is NULL");
                return;
            }
            if (sInstance.assistant == null)
            {
                Debug.LogError("PlatformService hasn't Init");
                return;
            }
            sInstance.assistant.MsgCallback(msg);
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

    public interface Assistant
    {
        void SendMessage(string msg);
        void MsgCallback(string msg);
    }
    
}

