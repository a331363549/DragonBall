using NewEngine.Framework;
using NewEngine.Framework.Service;
using System;
using UnityEngine;

namespace NewEngine.Utils
{

    public class PlatformUtils
    {
        
        public static void AddListener(int msgId, Action<Msg> listener)
        {
            ListenerDriver.AddListener(msgId, listener);
        }

        public static void RemoveListener(int msgId, Action<Msg> listener)
        {
            ListenerDriver.RemoveListener(msgId, listener);
        }

        public static bool IsInEditor()
        {
            return Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.LinuxEditor ||
                Application.platform == RuntimePlatform.OSXEditor;
        }

        public static bool IsInWinOS()
        {
            return Application.platform == RuntimePlatform.WindowsPlayer;
        }

        public static bool IsInIOS()
        {
            return Application.platform == RuntimePlatform.OSXPlayer;
        }

        public static bool IsInIPhone()
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }

        public static bool IsInAndroid()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        public static bool IsInWebGL()
        {
            return Application.platform == RuntimePlatform.WebGLPlayer;
        }
    }
}

