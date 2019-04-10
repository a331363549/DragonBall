using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace NewEngine.Utils
{

    public class FileUtils
    {
        public static AssetBundle LoadAssetBundle(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (File.Exists(filePath) == false)
            {
                Debug.LogError(string.Format("[LoadAssetBundle]LoadAssetBundle {0} Failed", filePath));
                return null;
            }
            AssetBundle ab = null;
            try
            {
                ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(filePath));
            }
            catch
            {
                Debug.LogError(string.Format("[LoadAssetBundle]LoadAssetBundle {0} Failed", fileName));
            }
            return ab;
        }

        public static string ReadAllText(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return string.Empty;
        }

        public static string ModifyPath(string file)
        {
            return file.Replace("\\", "/");
        }

        public static string GetFileMd5(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return null;
            }
            FileStream fs = null;
            string md5Code = "";
            try
            {
                fs = new FileStream(filePath, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] OutBytes = md5.ComputeHash(fs);
                md5Code = "";
                for (int i = 0; i < OutBytes.Length; i++)
                {
                    md5Code += OutBytes[i].ToString("x2");
                }
                md5Code = md5Code.ToLower();
            }
            catch
            {

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return md5Code;
        }


        #region 资源索引字典
        

        /// <summary>
        /// 持久存储路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>持久存储路径</returns>
        public static string GetPersistentPath(string path)
        {
            return Application.persistentDataPath + "/" + path;
        }

        /// <summary>
        /// 获取可以传递给WWW对象的路径
        /// </summary>
        /// <returns>
        /// 目标路径
        /// </returns>
        /// <param name='path'>
        /// 相对路径或完整路径
        /// </param>
        public static string GetWWWPath(string path)
        {
            if (path.StartsWith("http://") || path.StartsWith("ftp://") || path.StartsWith("https://") || path.StartsWith("file://") || path.StartsWith("jar:file://"))
            {
                return path;
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                return path.Insert(0, "file://");
            }
            else if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return path.Insert(0, "file:///");
            }

            return path;
        }

        #endregion 资源索引字典

        #region 异步加载Asset

        /// <summary>
        /// 加载Assetbundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public IEnumerator LoadAssetbundle(string path, GameObject root, string name, System.Type type)
        //{
        //    ResFileInfo fileInfo;
        //    string target = null;
        //    GameObject child;
        //    WWW www = null;

        //    lock (FileDict)
        //    {
        //        if (FileDict.TryGetValue(path, out fileInfo) && fileInfo.bExist)
        //        {
        //            if (fileInfo.Location == (int)ResType.StreamingAsset)
        //            {
        //                target = GetWWWPath(GetAssetPath(path));
        //                www = new WWW(target);
        //            }
        //            else if (fileInfo.Location == (int)ResType.UpdateAsset)
        //            {
        //                target = GetWWWPath(GetPersistentPath(path));
        //                www = new WWW(target);
        //            }
        //            else if (fileInfo.Location == (int)ResType.WWWAssetNotPersistent)
        //            {
        //                target = Context.XapAbsoluteWebPath + path;
        //                www = WWW.LoadFromCacheOrDownload(target, fileInfo.Version);
        //            }

        //        }

        //        yield return www;

        //        if (www.isDone && string.IsNullOrEmpty(www.error))
        //        {
        //            if (string.IsNullOrEmpty(name))
        //            {
        //                child = SpawnManager.Instantiate(www.assetBundle.mainAsset) as GameObject;
        //            }
        //            else
        //            {
        //                child = www.assetBundle.Load(name, type) as GameObject;
        //                child = SpawnManager.Instantiate(child) as GameObject;
        //            }

        //            AssetbundleLoader2.SetParent(root.transform, child.transform);
        //            child.layer = root.layer;

        //            www.assetBundle.Unload(false);
        //        }
        //    }
        //}

        #endregion 异步加载对象

        #region 路径

        /// <summary>
        /// 获取项目的绝对路径
        /// </summary>
        /// <returns>项目路径</returns>
        public static string ProjectPath()
        {
            string localFullPath;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                localFullPath = Application.dataPath + "/";
            else
                localFullPath = "file:///" + Application.dataPath + "/../";

            return localFullPath;
        }

        /// <summary>
        /// 获取项目的StreamingAssets绝对路径
        /// </summary>
        /// <returns>项目路径</returns>
        public static string SteamingAssetsPath(string path = "")
        {
            if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return "file:///" + Application.dataPath + "/StreamingAssets/" + path;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath + "/" + path;
            }
            else
                return "file:///" + Application.streamingAssetsPath + "/" + path;
        }

        /// <summary>
        /// 是否在本地永久存储目录存在
        /// </summary>
        public static Dictionary<string, byte> PersistentPathDict = new Dictionary<string, byte>();

        /// <summary>
        /// 获取Web的绝对路径
        /// </summary>
        /// <param name="uri">相对路径</param>
        /// <returns>Web路径</returns>
        public static string WebPath(string uri)
        {
            //在这里要决定，是访问StreamingAssetsPath目录，还是PersistentPath目录
            byte result = 0;
            if (PersistentPathDict.TryGetValue(uri, out result))
            {
                //string path = "file://"+GetPersistentPath(uri);
                string path = GetPersistentPath(uri);
                path = GetWWWPath(path);
                return path;
            }
            else
            {
                string path = GetPersistentPath(uri);
                if (File.Exists(path)) //如果已经存在，则立刻跳过
                {
                    PersistentPathDict[path] = 1;
                    //return "file://"+path;
                    path = GetWWWPath(path);
                    return path;
                }
            }

            return null;
        }

        #endregion 路径

        #region Asset名称

        /// <summary>
        /// 从资源名称获取Asset名称
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static string GetAssetName(string resName)
        {
            int index = resName.LastIndexOf(".");
            if (index < 0)
            {
                return resName;
            }

            return resName.Substring(0, index);
        }

        #endregion Asset名称
    }
}

