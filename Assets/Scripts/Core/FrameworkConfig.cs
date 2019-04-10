
using NewEngine.Utils;
using UnityEngine;

namespace NewEngine
{

    public class FrameworkConfig
    {
        public static bool UseLocalRes = false;

        /// <summary>
        /// 可加载的使用的内存值
        /// </summary>
        public const int OnLoadMemory = 150;
        /// <summary>
        /// 是否启用缓存策略
        /// </summary>
        public const bool IsCacheStrategy = false;

        public static int CountMemory = -1;
        /// <summary>
        /// 剩余内存
        /// </summary>
        public static int ResidualMemory = -1;
        
        public static string NetResRoot;

        public static string Platform;

        public static string DownloadDirName;

        public static string NetResDownloadPath;

        public static string StreamingAssetsPath;

        public static void Init()
        {
            NetResRoot = "127.0.0.1";
            DownloadDirName = "Download";
            Platform = Application.platform.ToString();
            if (PlatformUtils.IsInEditor())
            {
                Platform = "Other";
                NetResDownloadPath = Application.dataPath.Remove(Application.dataPath.LastIndexOf('/'));
                StreamingAssetsPath = "file://" + Application.dataPath + "/StreamingAssets";
            }
            else if (PlatformUtils.IsInWinOS())
            {
                NetResDownloadPath = Application.persistentDataPath;
                StreamingAssetsPath = "file://" + Application.dataPath + "/StreamingAssets";
            }
            else if (PlatformUtils.IsInAndroid())
            {
                NetResDownloadPath = Application.persistentDataPath;
                StreamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets";
            }
            else if (PlatformUtils.IsInIPhone())
            {
                NetResDownloadPath = Application.persistentDataPath;
                StreamingAssetsPath = "file://" + Application.dataPath + "/Raw";
            }
            else
            {
                Platform = "Other";
                NetResDownloadPath = Application.persistentDataPath;
                StreamingAssetsPath = "file://" + Application.dataPath + "/StreamingAssets";
            }
            NetResDownloadPath = string.Format("{0}/{1}", NetResDownloadPath, DownloadDirName);
        }
    }
    
}
