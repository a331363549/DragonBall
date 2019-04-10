using NewEngine.Framework.Service;
using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MainLogic {

    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);
    
    public enum LevelID
    {
        Native,
        Street,
        View,
    }

    private LevelID fromLev = LevelID.Native;
    private const string guMusic = "music-00.unity3d";
    private const string modenMusic = "music_01.unity3d";
    private const string streetSource = "miStreet.unity3d" + Define.platformTag;
    private const string animi2Square = "Move2Square"; 
    private const string anim2Street = "Move2Street";
    private const string anim2GuStreet = "UserCamera_Show";

    public IEnumerator LoadSquare()
    {
        WebApi.Reset();
        UserCurState.Instance.EnableMove = false;
        DownloadService.ClearDownloadTask();
        Unity2Native.OnUnityLoading();
        VRShopData.Instance.VRShopInfo = null;
        UserCamera.Instance.InitShow("Square");
        UserCamera.Instance.GameCamera.fieldOfView = 60;
        WebApi.RequestStreetMap();
        AsyncOperation asynOp = SceneManager.LoadSceneAsync("Square");
        yield return asynOp;

        yield return waitForSeconds;
        GOPool.Instance.ReleaseCache();
        if (GOPool.Instance.MainAsset != null)
        {
            GOPool.Instance.MainAsset.Unload(true);
            GOPool.Instance.MainAsset = null;
        }

        string abUrl = FileUtils.GetPersistentPath(streetSource);
        AssetBundleCreateRequest assetBundleCreate = AssetBundle.LoadFromFileAsync(abUrl);

        if (assetBundleCreate == null)
        {
            UIPageSlide.Show();
        }

        do
        {
            UILoadingSlide.UpdateProgress(assetBundleCreate.progress);
            yield return waitForSeconds;
        } while (assetBundleCreate.isDone == false);

        if (assetBundleCreate.assetBundle == null)
        {
            UIPageSlide.Show();
        }

        assetBundleCreate.assetBundle.name = Path.GetFileNameWithoutExtension(streetSource);
        GOPool.Instance.MainAsset = assetBundleCreate.assetBundle;
        Resources.UnloadUnusedAssets();

        GameObject go = GOPool.Instance.PopGO("prefabs/square");
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = new Vector3(0,90,0);
        go.transform.localScale = Vector3.one * 5;

        yield return waitForSeconds;

        UILoadingSlide.Hide();

        AsyncEventDriver.QueueOnMainThread(() =>
        {
            UserCamera.Instance.PlayShow(this.OnSquareComloeted, animi2Square);
        });
    }


    public void OnSquareComloeted()
    {
        UIBroadcastSlide.Show();
        UIStreetSlide.Open();
        //UserCamera.Instance.DisableCamAnim("Square");
        AsyncEventDriver.QueueOnMainThread(() =>
        {
            //UIMainSlide.Show();

            WebApi.RequestNewMsg();
            UserCamera.Instance.transform.localPosition = Vector3.zero;
            UserCamera.Instance.transform.localEulerAngles = Vector3.zero;
            UserCurState.Instance.EnableMove = true;
        });
    }



    public IEnumerator LoadStreet(StreetType type)
    {
        isLoadCompleted = false;
        WebApi.Reset();
        UIStreetSlide.Close();
        cur_streetType = type;
        UserCurState.Instance.EnableMove = false;
      
        if (type == StreetType.古韵街)
        {
            UserCamera.Instance.InitShow("GuStreet");
            UILoadingSlide.ShowStreetLoading();
        }
        else
        {
            UserCamera.Instance.InitShow("Street");
            UILoadingSlide.ModenStreetLoading();
        }
        VRShopData.Instance.VRShopInfo = null;
        UserCamera.Instance.GameCamera.fieldOfView = 85;

        GOPool.Instance.ReleaseCache();
        AsyncOperation asynOp = SceneManager.LoadSceneAsync("Street");
        yield return asynOp;

        WebApi.RequestMusic();
        yield return waitForSeconds;
        if (GOPool.Instance.MainAsset == null)
        {
            string abUrl = FileUtils.GetPersistentPath(streetSource);
            AssetBundleCreateRequest assetBundleCreate = AssetBundle.LoadFromFileAsync(abUrl);

            if (assetBundleCreate == null)
            {
                UIPageSlide.Show();
            }

            do
            {
                UILoadingSlide.UpdateProgress(assetBundleCreate.progress);
                yield return waitForSeconds;
            } while (assetBundleCreate.isDone == false);

            if (assetBundleCreate.assetBundle == null)
            {
                UIPageSlide.Show();
            }

            assetBundleCreate.assetBundle.name = Path.GetFileNameWithoutExtension(streetSource);
            GOPool.Instance.MainAsset = assetBundleCreate.assetBundle;
        }

        Resources.UnloadUnusedAssets();
        StoreData.Instance.shopDataQueue.Clear();
        do
        {
            WebApi.RequestStoreList(StoreArgs());
            yield return new WaitForSeconds(1);
            Debug.Log("IsStoreDataReady:" + (StoreData.Instance.shopDataQueue.Count < 14));
        } while (StoreData.Instance.shopDataQueue.Count < 14);
        Debug.Log("StoreDataReady");
        Debug.Log("isStreetFirstReady:" + streetFirstReady);
        do
        {
            yield return waitForSeconds;
        } while (streetFirstReady == false);


        GameObject go = null;
        float length = 386.5f;
        string anim ;
        if (type == StreetType.古韵街)
        {
            go = GOPool.Instance.PopGO("prefabs/scene");
            anim = anim2GuStreet;
            length = 130.8f;
        }
        else
        {
            go = GOPool.Instance.PopGO("prefabs/scene_moden");
            anim = anim2Street;
            length = 386.5f;
        }
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
        StreetManager.Instance.CreateStreet(go.transform, 3, length, type);
        isLoadCompleted = true;
        StreetManager.Instance.UpdateData();

        UserCurState.Instance.enableBackDist = 0;
        UIBroadcastSlide.Show();
        
        yield return waitForSeconds;
        UILoadingSlide.Hide();
        AsyncEventDriver.QueueOnMainThread(() =>
        {
            UserCamera.Instance.PlayShow(this.OnCameraAnimCompleted, anim);
        });
    }

    private void OnCameraAnimCompleted()
    {
        UIStreetSlide.Open();
        Vector3 pos = new Vector3(0, 4.0f, 0);
        if (cur_streetType == StreetType.古韵街)
            pos = new Vector3(0, 1.8f, 0);
        AsyncEventDriver.QueueOnMainThread(() =>
        {
            //UIMainSlide.Show();
            if (!viewsLoad)
                WebApi.RequestCityViews();
            StreetManager.Instance.Update(Vector3.forward * UserCamera.Instance.transform.position.z);
            UserCamera.Instance.transform.position = pos;
            UserCurState.Instance.EnableMove = true;
        });
    }

    

    private IEnumerator Back2Street()
    {
        UserCurState.Instance.EnableMove = false;
        fromLev = LevelID.Native;
        UILoadingSlide.Show();
        Unity2Native.OnUnityLoading();

        AsyncOperation asynOp = SceneManager.LoadSceneAsync("Street");

        if (asynOp == null)
        {
            UIPageSlide.Show();
            yield break;
        }

        yield return asynOp;

        GOPool.Instance.ReleaseCache();
        if (GOPool.Instance.MainAsset == null)
        {
            string abUrl = FileUtils.GetPersistentPath(streetSource);
            AssetBundleCreateRequest assetBundleCreate = AssetBundle.LoadFromFileAsync(abUrl);
            if (assetBundleCreate == null)
            {
                UIPageSlide.Show();
            }

            do
            {
                UILoadingSlide.UpdateProgress(assetBundleCreate.progress);
                yield return waitForSeconds;
            } while (assetBundleCreate.isDone == false);

            if (assetBundleCreate.assetBundle == null)
            {
                UIPageSlide.Show();
            }

            assetBundleCreate.assetBundle.name = Path.GetFileNameWithoutExtension(streetSource);
            GOPool.Instance.MainAsset = assetBundleCreate.assetBundle;
            string url = FileUtils.WebPath(streetSource);
        }
        
        Resources.UnloadUnusedAssets();
        Skybox skybox = UserCamera.Instance.GetComponent<Skybox>();
        if (skybox != null && skybox.material != null)
        {
            skybox.material = null;
        }

        GameObject go;
        float length = 386.5f;
        if (cur_streetType == StreetType.古韵街)
        {
            go = GOPool.Instance.PopGO("prefabs/scene");
            length = 130.8f;
        }
        else
        {
            go = GOPool.Instance.PopGO("prefabs/scene_moden");
            length = 386.5f;
        }
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
        StreetManager.Instance.CreateStreet(go.transform, 3, length, cur_streetType, streetSaveDatas);
        yield return waitForSeconds;
        StreetManager.Instance.UpdateData();


        UserCamera.Instance.GameCamera.fieldOfView = 85;
        UserCamera.Instance.ResetCamTrans();
        UserCurState.Instance.enableBackDist = enableBackDist;
        UserCurState.Instance.EnableMove = true;
        UserCurState.Instance.TargetAngle = UserCamera.Instance.GameCamera.transform.localEulerAngles.y;
        yield return waitForSeconds;

        isLoadCompleted = true;

        yield return waitForSeconds;
        UIBroadcastSlide.Show();
        UIStreetSlide.Open();
        yield return waitForSeconds;

        UILoadingSlide.Hide();
    }
    
    private List<StreetSaveData> streetSaveDatas = null;
    public IEnumerator Street2Shop(string uCode, string name)
    {
        fromLev = LevelID.Street;
        UILoadingSlide.Show();
        Unity2Native.OnUnityLoading();

        VRShopData.Instance.VRShopInfo = null;
        VRShopData.Instance.IsCompleted = false;

        streetSaveDatas = StreetManager.Instance.GetStreetSaveData();
        enableBackDist = UserCurState.Instance.enableBackDist;
        UserCamera.Instance.SaveCamTrans();

        GOPool.Instance.ReleaseCache();
        AsyncOperation asynOp = SceneManager.LoadSceneAsync("View");
        yield return asynOp;

        Resources.UnloadUnusedAssets();
        yield return waitForSeconds;
        Unity2Native.OpenWeb(uCode, name);
    }

    public IEnumerator Street2Sight(string res_url = null)
    {
        fromLev = LevelID.Street;
        UILoadingSlide.Show();
        Unity2Native.OnUnityLoading();

        VRShopData.Instance.ResURL = null;
        VRShopData.Instance.IsCompleted = false;
        if (res_url == null)
        {
            WebApi.RequestTransfer();
            do
            {
                yield return waitForSeconds;
            } while (VRShopData.Instance.IsCompleted == false);

            if (VRShopData.Instance.IsError ||
                VRShopData.Instance.ResURL == null)
            {
                UILoadingSlide.Hide();
                yield break;
            }
            res_url = VRShopData.Instance.ResURL;
        }

        streetSaveDatas = StreetManager.Instance.GetStreetSaveData();
        enableBackDist = UserCurState.Instance.enableBackDist;
        UserCamera.Instance.SaveCamTrans();

        GOPool.Instance.ReleaseCache();
        AsyncOperation asynOp = SceneManager.LoadSceneAsync("View");
        yield return asynOp;

        Resources.UnloadUnusedAssets();
        yield return waitForSeconds;
        Unity2Native.OpenWeb(res_url, "");

    }

    public void SaveSteetData()
    {
        streetSaveDatas = StreetManager.Instance.GetStreetSaveData();
        enableBackDist = UserCurState.Instance.enableBackDist;
        UserCamera.Instance.SaveCamTrans();
    }
    /*
    private bool isSuccess = false;
    private bool isDownloadCompleted = false;
    private void OnDownloadResult(WWW www)
    {
        UILoadingSlide.UpdateProgress(1);
        isSuccess = false;
        isDownloadCompleted = true;
        if (string.IsNullOrEmpty(www.error) == false || 
            www.assetBundle == null)
        {
            isSuccess = false;
            Debug.LogError("[DownloadError]file:" + www.url + ",error:" + www.error);
            UIMessageSlide.ShowMessage(www.error);
            return;
        }

        www.assetBundle.name = Path.GetFileNameWithoutExtension(www.url);
        GOPool.Instance.MainAsset = www.assetBundle;
        isSuccess = true;
    }

    */
    private void OnDownloadProgress(float progress)
    {
        UILoadingSlide.UpdateProgress(progress);
    }

    private string getResPath(string viewURL, string shopType)
    {
        if (string.IsNullOrEmpty(viewURL))
        {
            if (string.IsNullOrEmpty(shopType))
            {
                return FileUtils.WebPath("miVirtual_def.unity3d");
            }
            else if (shopType == "10" ||
                    shopType == "11")
            {
                return FileUtils.WebPath("miVirtual_" + shopType + ".unity3d");
            }
            else
            {
                return FileUtils.WebPath("miVirtual_def.unity3d");
            }
        }
        else if (viewURL.EndsWith(".unity3d"))
        {
            return viewURL + Define.platformTag;
        }
        else
        {
            return FileUtils.WebPath("miVirtual_def.unity3d");
        }
    }

    private void OnDownPicResult(WWW www)
    {
        //Texture2D newTexture = www.texture;
        //byte[] pngData = newTexture.EncodeToPNG();
        //string name = Path.GetFileNameWithoutExtension(www.url);
        //File.WriteAllBytes(Application.persistentDataPath + string.Format("/{0}.png", name), pngData);
    }


    private void onDownMusicResult(WWW www)
    {
        UILoadingSlide.UpdateProgress(1);
        if (string.IsNullOrEmpty(www.error) == false ||
            www.assetBundle == null)
        {
            Debug.LogError(www.error);
            UIMessageSlide.ShowMessage(www.error);
            return;
        }
        musicAsset = www.assetBundle;
        www.assetBundle.name = Path.GetFileNameWithoutExtension(www.url);
        AsyncEventDriver.QueueOnMainThread(() =>
        {
            AudioClip clip = musicAsset.LoadAsset<AudioClip>(musicAsset.name);
            AudioManager.Instance.PlayMusic(clip);
            if (oldMuiscAsset != null)
            {
                oldMuiscAsset.Unload(false);
                oldMuiscAsset = null;
            }
        });
    }
}
