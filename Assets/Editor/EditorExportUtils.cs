using NewEngine.Framework.Table;
using NewEngine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;



namespace Assets.TimTools.Editor
{

    class EditorExportUtils
    {

        public static BuildAssetBundleOptions sBuildOption = //BuildAssetBundleOptions.None;
            BuildAssetBundleOptions.DeterministicAssetBundle |
            BuildAssetBundleOptions.ForceRebuildAssetBundle |
            BuildAssetBundleOptions.StrictMode;

        //private static string resExtension = "*.mp3,*.xml,*.txt,*.prefab,*.png,*.jpg,*.tga,*.unity";
        
        static List<string> resExtension = new List<string> { ".wav", ".ogg", ".mp3", ".xml", ".prefab", ".txt", ".png", ".jpg", ".tga", ".unity", ".fbx" };


        [MenuItem("Assets/NewTool/SetToSky", validate = true)]
        public static bool IsTexture()
        {
            return (Selection.activeObject is Texture);
        }

        [MenuItem("Assets/NewTool/SetToSky &1")]
        public static void SetToSky()
        {
            Skybox skybox = Camera.main.GetComponent<Skybox>();
            if (skybox == null)
            {
                skybox = Camera.main.gameObject.AddComponent<Skybox>();
            }
            Material mat = AssetDatabase.LoadMainAssetAtPath("assets/localres/Materials/shopInsideView_test.mat") as Material;
            skybox.material = new Material(mat);
            skybox.material.SetTexture("_MainTex", Selection.activeObject as Texture2D);
        }

        [MenuItem("Assets/NewTool/ChangeTexSize &2")]
        public static void ChageSizeTex()
        {
            Object[] selectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
            for (int i = 0; i < selectedAsset.Length; i++)
            {
                Texture2D tex = selectedAsset[i] as Texture2D;
                TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex));
                ti.maxTextureSize = 4096;
                ti.isReadable = true;
                ti.wrapMode = TextureWrapMode.Clamp;
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex));
                string imagePath = AssetDatabase.GetAssetPath(tex);
                byte[] fileData = File.ReadAllBytes(imagePath);
                tex.LoadImage(fileData);
                Texture2D temp = ScaleTexture(tex, 4096, 2048);
                byte[] pngData = temp.EncodeToJPG();
                string miniImagePath = imagePath.Replace(".png", "_min.jpg");
                File.WriteAllBytes(miniImagePath, pngData);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex));
            }
        }

        private static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
            float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
        [MenuItem("Assets/NewTool/GetDataInfo")]
        public static void GetDataInfo()
        {
            ReadExcel.ReadConfig();
        }


        #region AssetsTools
        [MenuItem("Assets/NewTool/打包AB/打包成合并的文件")]
        public static void ExportAssets2OneBundle()
        {
            string save_name = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel)[0].name;
            string outputPath = EditorUtility.SaveFilePanel("Save Resource", Application.streamingAssetsPath, save_name, "unity3d").Replace("\\", "/");
            if (string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (selection.Length == 0)
            {
                return;
            }

            string filePath;
            List<string> assetPath = new List<string>();
            for (int idx = 0, count = selection.Length; idx < count; ++idx)
            {
                filePath = AssetDatabase.GetAssetPath(selection[idx]).Replace("\\", "/");
                if (File.Exists(filePath))
                {
                    assetPath.Add(filePath);
                }
            }
            ExportRes2OneBundle(outputPath, assetPath.ToArray(), EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("Assets/NewTool/打包AB/打包成独立的文件")]
        public static void ExportAssets2IndependentBundles()
        {
            string outputPath = EditorUtility.SaveFolderPanel("Save Resource", Application.dataPath, "").Replace("\\", "/");
            if (string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (selection.Length == 0)
            {
                return;
            }

            Debug.Log(outputPath);
            //string outputFilePath;
            string filePath;
            List<string> assetPath = new List<string>();
            for (int idx = 0, count = selection.Length; idx < count; ++idx)
            {
                filePath = AssetDatabase.GetAssetPath(selection[idx]).Replace("\\", "/");
                if (File.Exists(filePath))
                {
                    assetPath.Add(filePath);
                }
            }
            ExportRes2IndependentBundles(outputPath, assetPath.ToArray(), EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("Assets/NewTool/打包AB/打包场景")]
        public static void ExportScenes2AssetBundle()
        {
            Object[] selection = Selection.GetFiltered(typeof(SceneAsset), SelectionMode.DeepAssets);
            if (selection.Length == 0)
            {
                return;
            }
            string outputPath = EditorUtility.SaveFolderPanel("Save Resource", Application.dataPath, "").Replace("\\", "/");

            ExportScenes(outputPath, selection, EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("Assets/NewTool/打包AB/打包成合并的ios文件 #1")]
        public static void ExportAssets2OneBundle_IOS()
        {
            //Object[] arr = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);
            string save_name = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel)[0].name;
            string outputPath = EditorUtility.SaveFilePanel("Save Resource", Application.streamingAssetsPath, save_name, "unity3d-ios").Replace("\\", "/");
            if (string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (selection.Length == 0)
            {
                return;
            }

            string filePath;
            List<string> assetPath = new List<string>();
            for (int idx = 0, count = selection.Length; idx < count; ++idx)
            {
                filePath = AssetDatabase.GetAssetPath(selection[idx]).Replace("\\", "/");
                if (File.Exists(filePath))
                {
                    assetPath.Add(filePath);
                }
            }
            ExportRes2OneBundle(outputPath, assetPath.ToArray(), EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("Assets/NewTool/打包AB/打包成合并的apk文件 #2")]
        public static void ExportAssets2OneBundle_APK()
        {
            string save_name = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel)[0].name;
            string outputPath = EditorUtility.SaveFilePanel("Save Resource", Application.streamingAssetsPath, save_name, "unity3d-apk").Replace("\\", "/");
            //string outpath = string.Format("{0}/{1}{2}", Application.streamingAssetsPath, save_name, ".unity3d-apk");
            if (string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (selection.Length == 0)
            {
                return;
            }

            string filePath;
            List<string> assetPath = new List<string>();
            for (int idx = 0, count = selection.Length; idx < count; ++idx)
            {
                filePath = AssetDatabase.GetAssetPath(selection[idx]).Replace("\\", "/");
                if (File.Exists(filePath))
                {
                    assetPath.Add(filePath);
                }
            }
            ExportRes2OneBundle(outputPath, assetPath.ToArray(), EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("Assets/NewTool/打包AB/输出AssetBundle的内容列表")]
        public static void ListAssetNames()
        {
            string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (filePath.EndsWith(".unity3d", true, null) == false)
            {
                return;
            }
            AssetBundle ab = AssetBundle.LoadFromFile(filePath);
            string[] assetName = ab.GetAllAssetNames();
            string output = string.Format("asset count:{0}", assetName.Length);
            for (int idx = 0, count = assetName.Length; idx < count; ++idx)
            {
                output = string.Format("{0}\n{1}", output, assetName[idx]);
            }
            Debug.Log(output);
            ab.Unload(false);
            ab = null;
        }

        #endregion

        #region TimoTools
        //[MenuItem("NewTool/资源打包/输出文件MD5")]
        //public static void ExportBundleMd5()
        //{
        //    string directory = EditorUtility.OpenFolderPanel("Open", Application.dataPath, "");
        //    if (string.IsNullOrEmpty(directory))
        //    {
        //        return;
        //    }
        //    List<string> fileList = new List<string>();
        //    GetFiles(directory, ref fileList, true);
        //    List<Md5Info> resInfoList = new List<Md5Info>();
        //    for (int idx = 0; idx < fileList.Count; ++idx)
        //    {
        //        string fileName = fileList[idx].Substring(fileList[idx].IndexOf("Assets"));
        //        string md5Code = FileUtils.GetFileMd5(fileList[idx]);
        //        byte[] byteArr = File.ReadAllBytes(fileList[idx]);
        //        resInfoList.Add(new Md5Info(fileName, md5Code, byteArr.Length));
        //    }

        //    string filePath = EditorUtility.OpenFilePanel("Export", Application.dataPath, "");// FileUtils.ModifyPath(Path.Combine(GetOutDir(buildTarget), "Md5.txt"));
        //    TableSaver.SaveTxt(filePath, resInfoList.ToArray());
        //    Debug.Log("ExportBundleMd5 Success");
        //}
                
        #endregion

        #region method

        public static void ExportScenes(string outputPath, Object[] level, BuildTarget buildTarget)
        {
            string msg = string.Format("打包到{0}", outputPath);
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.1f);
            string fileName = outputPath.GetHashCode().ToString();
            string outputDir = string.Format("Export/{0}", fileName);
            if (Directory.Exists(outputDir) == false)
            {
                Directory.CreateDirectory(outputDir);
            }
            Scene current = EditorSceneManager.GetActiveScene();
            string defaultPath = current.path;
            string filePath, desFilePath;
            BuildPlayerOptions buildOptions;
            for (int idx = 0; idx < level.Length; ++idx)
            {
                filePath = AssetDatabase.GetAssetPath(level[idx]);
                buildOptions = new BuildPlayerOptions();
                buildOptions.locationPathName = string.Format("{0}/{1}.unity3d", outputDir, Path.GetFileNameWithoutExtension(filePath));
                buildOptions.scenes = new string[] { filePath };
                buildOptions.target = buildTarget;
                buildOptions.options = BuildOptions.BuildAdditionalStreamedScenes;
                EditorUtility.DisplayProgressBar("资源打包", filePath, 0.1f + 0.9f * idx / level.Length);
                BuildPipeline.BuildPlayer(buildOptions);
                desFilePath = string.Format("{0}/{1}.unity3d", outputPath, Path.GetFileNameWithoutExtension(filePath));
                if (File.Exists(desFilePath))
                {
                    File.Delete(desFilePath);
                }
                File.Move(buildOptions.locationPathName, desFilePath);
            }
            Directory.Delete(outputDir, true);
            if (string.IsNullOrEmpty(defaultPath) == false)
            {
                EditorSceneManager.OpenScene(defaultPath);
            }
            EditorUtility.ClearProgressBar();
        }

        public static void ExportFolderToBundlesByDir(string outputPath, string inputPath, BuildTarget buildTarget, string searchPattern = "*.*")
        {
            string[] files = Directory.GetFiles(inputPath, searchPattern, SearchOption.TopDirectoryOnly);
            List<string> fileList = new List<string>();
            for (int idx = 0; idx < files.Length; ++idx)
            {
                if (files[idx].EndsWith(".meta", true, null))
                {
                    continue;
                }
                fileList.Add(files[idx]);
            }
            string[] directories = Directory.GetDirectories(inputPath);
            if (directories.Length + fileList.Count == 0)
            {
                return;
            }
            string msg = string.Format("打包到{0}", outputPath);
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.1f);

            AssetBundleBuild[] buildMap = new AssetBundleBuild[directories.Length + fileList.Count];

            string dirName = Path.GetFileName(inputPath);
            string[] assetNames;
            List<string> assetList = new List<string>();
            for (int idx = 0, count = directories.Length; idx < count; ++idx)
            {
                assetList.Clear();
                assetNames = Directory.GetFiles(directories[idx].Replace("\\", "/"), searchPattern, SearchOption.AllDirectories);
                for (int idx2 = 0, count2 = assetNames.Length; idx2 < count2; ++idx2)
                {
                    if (resExtension.Contains(Path.GetExtension(assetNames[idx2])) == false)
                    {
                        continue;
                    }
                    assetList.Add(assetNames[idx2].Replace("\\", "/"));
                }
                if (assetList.Count == 0)
                {
                    continue;
                }
                buildMap[idx].assetBundleName = string.Format("{0}{1}.unity3d", dirName, Path.GetFileName(directories[idx]));
                buildMap[idx].assetNames = assetList.ToArray();
                EditorUtility.DisplayProgressBar("资源打包", msg, 0.1f + 0.3f * idx / count);
            }

            for (int idx = directories.Length, count = fileList.Count + directories.Length; idx < count; ++idx)
            {
                if (resExtension.Contains(Path.GetExtension(files[idx - directories.Length])) == false)
                {
                    continue;
                }
                buildMap[idx].assetBundleName = string.Format("{0}.unity3d", Path.GetFileNameWithoutExtension(files[idx - directories.Length]));
                buildMap[idx].assetNames = new string[] { files[idx - directories.Length].Replace("\\", "/") };
                EditorUtility.DisplayProgressBar("资源打包", msg, 0.1f + 0.3f * idx / count);
            }

            string fileName = outputPath.GetHashCode().ToString();
            string outputDir = string.Format("Export/{0}", fileName);
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            Directory.CreateDirectory(outputDir);
            BuildPipeline.BuildAssetBundles(outputDir, buildMap, sBuildOption, buildTarget);
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.7f);

            string srcPath, desPath;
            for (int idx = 0; idx < buildMap.Length; ++idx)
            {
                srcPath = string.Format("{0}/{1}", outputDir, buildMap[idx].assetBundleName);
                desPath = string.Format("{0}/{1}", outputPath, buildMap[idx].assetBundleName);
                try
                {
                    File.Copy(srcPath, desPath, true);
                    EditorUtility.DisplayProgressBar("资源打包", msg, 0.7f + 0.3f * idx / buildMap.Length);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            Directory.Delete(outputDir, true);
            Debug.Log(outputPath);
            EditorUtility.ClearProgressBar();
        }
        
        public static void ExportRes2IndependentBundles(string outputPath, string[] assetPath, BuildTarget buildTarget)
        {
            string msg = string.Format("打包到{0}", outputPath);
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.1f);
            if (Directory.Exists("Temp") == false)
            {
                Directory.CreateDirectory("Temp");
            }
            AssetBundleBuild[] builds = new AssetBundleBuild[assetPath.Length];
            string outputFilePath;
            for (int idx = 0, count = assetPath.Length; idx < count; ++idx)
            {
                outputFilePath = string.Format("{0}/{1}", outputPath, Path.GetDirectoryName(assetPath[idx]));
                outputFilePath = string.Format("{0}/{1}.unity3d", outputFilePath, Path.GetFileNameWithoutExtension(assetPath[idx]));
                builds[idx].assetBundleName = outputFilePath.GetHashCode().ToString();
                builds[idx].assetNames = new string[] { assetPath[idx] };
                EditorUtility.DisplayProgressBar("资源打包", msg, 0.1f + idx * 1.0f / count * 0.3f);
            }

            BuildPipeline.BuildAssetBundles("Temp", builds, sBuildOption, EditorUserBuildSettings.activeBuildTarget);
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.7f);

            string srcFilePath;
            for (int idx = 0, count = assetPath.Length; idx < count; ++idx)
            {
                outputFilePath = string.Format("{0}/{1}", outputPath, Path.GetDirectoryName(assetPath[idx]));
                outputFilePath = string.Format("{0}/{1}.unity3d", outputFilePath, Path.GetFileNameWithoutExtension(assetPath[idx]));
                srcFilePath = string.Format("Temp/{0}", outputFilePath.GetHashCode().ToString());
                if (File.Exists(srcFilePath) == false)
                {
                    Debug.LogError(string.Format("{0} is invalid file", srcFilePath));
                    Debug.LogError(string.Format("{0}->{1} Failed", srcFilePath, outputFilePath));
                    continue;
                }
                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }
                if (Directory.Exists(Path.GetDirectoryName(outputFilePath)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                }
                try
                {
                    File.Move(srcFilePath, outputFilePath);
                    File.Delete(string.Format("{0}.manifest", srcFilePath));
                }
                catch (Exception ex)
                {
                    Debug.LogError(string.Format("{0}->{1} Failed! Exception: \n{2}", srcFilePath, outputFilePath, ex));
                }
                EditorUtility.DisplayProgressBar("资源打包", msg, 0.7f + idx * 1.0f / count * 0.3f);
            }
            EditorUtility.ClearProgressBar();
        }

        public static void ExportRes2OneBundle(string outputPath, string[] assetPath, BuildTarget buildTarget)
        {
            string msg = string.Format("打包到{0}", outputPath);
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.1f);
            if (Directory.Exists("Export") == false)
            {
                Directory.CreateDirectory("Export");
            }
            string fileName = outputPath.GetHashCode().ToString();
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = fileName;
            buildMap[0].assetNames = assetPath;
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.5f);
            BuildPipeline.BuildAssetBundles("Export", buildMap, sBuildOption, buildTarget);
            EditorUtility.DisplayProgressBar("资源打包", msg, 0.9f);
            string srcPath = string.Format("Export/{0}", fileName);
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            File.Move(srcPath, outputPath);
            EditorUtility.ClearProgressBar();
            Debug.Log(string.Format("{0}->{1}", srcPath, outputPath));
        }

        private static void GetFiles(string path, ref List<string> fileList, bool isAllDir = true)
        {
            string[] fileArr = Directory.GetFiles(ModifyPath(path), "*.*", isAllDir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (string filePath in fileArr)
            {
                if (filePath.EndsWith(".meta") == false)
                {
                    fileList.Add(ModifyPath(filePath));
                }
            }
        }

        private static string ModifyPath(string file)
        {
            return file.Replace("\\", "/");
        }
        #endregion

    }
}