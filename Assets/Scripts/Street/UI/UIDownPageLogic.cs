using NewEngine.Framework.Service;
using NewEngine.Framework.UI;
using NewEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Table;
using UnityEngine;
using UnityEngine.UI;
using NewEngine.Framework.Table;

public class UIDownPageLogic : UILogic
{
    public Slider slider;
    public Button btn_down;
    public Text progress;
    public Button btn_back;
    private WWW downloadOperation;

    string path;


    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
        string name = Path.GetFileName(WebApi.StreetSourceURL);
        path = string.Format("{0}", Path.GetFileName(name));
        path = FileUtils.GetPersistentPath(path);

        //测试环境使用
        //path = Application.streamingAssetsPath + "/miStreet.unity3d";

        if (File.Exists(path))
        {
            Debug.Log("source has downloaded");
            MainLogic.Instance.Init();

            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
            }
            Dictionary<string, string> langDict = new Dictionary<string, string>();
            List<LanguageTable> langList = TableReader<LanguageTable>.Context;
            for (int idx = 0; idx < langList.Count; idx++)
            {
                langDict.Add(langList[idx].id, langList[idx].res);
            }
            LanguageService.SetLang(langDict);
            Unity2Native.RequestContentType();
        }
        else
        {
            btn_back.gameObject.SetActive(true);
            gameObject.SetActive(true);
            slider.gameObject.SetActive(false);
            btn_down.gameObject.SetActive(true);
            btn_down.GetComponentInChildren<Text>().text = "立即下载";
        }
    }
    public void Back2App()
    {
        //MainLogic.Instance.Back2App();
        Unity2Native.Back2App();
    }

    // Update is called once per frame
    void Update()
    {
        if (showMsg && downloadOperation != null)
        {
            slider.value = downloadOperation.progress;
            progress.text = (int)(slider.value * 100) + "%";
        }
    }

    bool showMsg;
    public void OnDownClick()
    {
        //开始下载
        showMsg = true;
        slider.gameObject.SetActive(true);
        btn_down.gameObject.SetActive(false);
        StartCoroutine(DownloadSouce());
        btn_back.gameObject.SetActive(false);
        //btn_down.onClick.RemoveListener(OnDownClick);
    }


    public IEnumerator DownloadSouce()
    {
        downloadOperation = new WWW(WebApi.StreetSourceURL);
        yield return downloadOperation;

        if (string.IsNullOrEmpty(downloadOperation.error) == false)
        {
            Unity2Native.ShowMessage(downloadOperation.error);
            downloadOperation.Dispose();
            downloadOperation = null;
            showMsg = false;
            btn_back.gameObject.SetActive(true);
            gameObject.SetActive(true);
            slider.gameObject.SetActive(false);
            btn_down.gameObject.SetActive(true);
            btn_down.GetComponentInChildren<Text>().text = "立即下载";
            yield break;
        }
      
        ////下载完成，关闭进度面板	
        UIDownPageSlide.Close();
        //生成文件
        Byte[] b = downloadOperation.bytes;
        File.WriteAllBytes(path, b);
        //downloadOperation.assetBundle.name = Path.GetFileName(path);
        //GOPool.Instance.MainAsset = downloadOperation.assetBundle;
        FileUtils.PersistentPathDict.Add(Path.GetFileName(path), 1);
        downloadOperation.Dispose();
        downloadOperation = null;
        Debug.Log("finish:" + path);
        AsyncEventDriver.QueueOnMainThread(() =>
        {
            MainLogic.Instance.Init();
            Debug.Log("RequestContentType");
            Unity2Native.RequestContentType();
        });
    }


    public void CancleDown()
    {
        if (downloadOperation == null)
            return;
        StopAllCoroutines();
        downloadOperation.Dispose();
        DirectoryInfo folder = new DirectoryInfo(Application.persistentDataPath);
        foreach (var p in folder.GetFiles())
        {
            if (p.FullName.Contains(Path.GetFileName(path)))
                File.Delete(FileUtils.GetPersistentPath(p.Name));
        }
        showMsg = false;
        slider.value = 0;
        progress.text = "";
        slider.gameObject.SetActive(false);
        btn_down.GetComponentInChildren<Text>().text = "立即下载";
        btn_down.onClick.RemoveListener(CancleDown);
        btn_down.onClick.AddListener(OnDownClick);
    }
}

