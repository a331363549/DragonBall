/*************************************************************************************

* Author:   weimin
* Desc:       

* Date:     
* Version:	1.0

*************************************************************************************/


using UnityEngine;
using System.Collections;
using System.IO;

public class CommonResDefine {


    public static readonly string NetResRoot =
#if UNITY_EDITOR || YW_Local
        "http://192.168.0.201:280"
#elif YW_Release
        "http://122.114.103.53:10080"
#else
        "http://192.168.0.201:280"
#endif
        ;

    public static readonly string Platform =
#if UNITY_EDITOR
        "Other"
#elif UNITY_IPHONE
        "iPhone"
#elif UNITY_ANDROID
        "Android"
#else
        "Other"
#endif
;

    public static readonly string LocalResRoot =
#if UNITY_EDITOR
 Application.dataPath.Remove(Application.dataPath.LastIndexOf('/'))
#elif UNITY_STANDALONE_WIN
 Application.persistentDataPath
#elif UNITY_IPHONE
 Application.persistentDataPath
#elif UNITY_ANDROID
 Application.persistentDataPath
#else
 Application.persistentDataPath
#endif
 + "/Download"
;

    public static readonly string StreamingAssetsPath =
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        "file://" + Application.dataPath + "/StreamingAssets"
#elif UNITY_ANDROID
        "jar:file://" + Application.dataPath + "!/assets"
#elif UNITY_IPHONE
        "file://" + Application.dataPath + "/Raw"
#else
        "file://" + Application.dataPath + "/StreamingAssets"
#endif
        ;
    
    public static readonly string s_WriteblePath =
#if UNITY_ANDROID   //安卓
    "jar:file://" + Application.dataPath + "!/assets/"
#elif UNITY_IPHONE  //iPhone
    Application.dataPath + "/Raw/"
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
    "file://" + Application.dataPath + "/StreamingAssets/"
#else
   Application.persistentDataPath
#endif
   ;

    
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
}
