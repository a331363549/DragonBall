using NewEngine.Framework;
using NewEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DownloadService : CService
{

    public delegate void OnDownloadResult(WWW www);
    public delegate void OnDownloadProgress(float progress);

    public class DownloadTask
    {
        public string url;
        public OnDownloadResult onResult = null;
        public OnDownloadProgress onProgress = null;
    }

    private static DownloadService sInstance = null;

    public static void AddDownloadTask(string url, OnDownloadResult callback, OnDownloadProgress progress = null)
    {
        if (sInstance == null)
        {
            Debug.Log("no download moudle");
            return;
        }
        sInstance.AddTask(url, callback, progress);
    }

    public static void RemoveDownloadTask(string url, OnDownloadResult callback, OnDownloadProgress progress = null)
    {
        if (sInstance == null)
        {
            Debug.Log("no download moudle");
            return;
        }
        sInstance.RemoveTask(url, callback, progress);
    }

    public static void ClearDownloadTask()
    {
        if (sInstance == null)
        {
            Debug.Log("no download moudle");
            return;
        }
        sInstance.ClearTask();
    }

    private DownloadProcess downloadProcess = null;


    public void AddTask(string url, OnDownloadResult callback, OnDownloadProgress progress = null)
    {
        DownloadTask result = downloadProcess.downloadTasksList.Find((DownloadTask task) =>
        {
            return string.Compare(task.url, url, true) == 0;
        });
        if (result == null)
        {
            downloadProcess.downloadTasksList.Add(new DownloadTask()
            {
                url = url,
                onResult = callback,
                onProgress = progress
            });
        }
        else
        {
            if (callback != null)
            {
                result.onResult += callback;
            }
            if (progress != null)
            {
                result.onProgress += progress;
            }
        }
    }

    public void RemoveTask(string url, OnDownloadResult callback, OnDownloadProgress progress = null)
    {
        DownloadTask result = downloadProcess.downloadTasksList.Find((DownloadTask task) =>
        {
            return string.Compare(task.url, url, true) == 0;
        });
        if (result == null)
        {
            return;
        }

        if (callback != null)
        {
            result.onResult -= callback;
        }
        if (progress != null)
        {
            result.onProgress -= progress;
        }
    }
    public void ClearTask()
    {
        if (downloadProcess != null)
        {
            StopCoroutine(downloadProcess.OnDowload());
            downloadProcess.Release();
        }
        downloadProcess = new DownloadProcess();
        StartCoroutine(downloadProcess.OnDowload());
    }


    private void Awake()
    {
        sInstance = this;
    }

    private void Start()
    {
        downloadProcess = new DownloadProcess();
        StartCoroutine(downloadProcess.OnDowload());
    }

    private void OnDestroy()
    {
        if (sInstance == this)
        {
            sInstance = null;
        }
    }

    public class DownloadProcess
    {

        public List<DownloadTask> downloadTasksList = new List<DownloadTask>();

        private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        private WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        private WWW downloadWWW;

        public void Release()
        {
            if (downloadWWW != null)
            {
                downloadWWW.Dispose();
                downloadWWW = null;
            }
        }

        public IEnumerator OnDowload()
        {
            while (true)
            {
                while (downloadTasksList.Count == 0)
                {
                    yield return waitForFixedUpdate;
                }
                DownloadTask request = downloadTasksList[0];
                downloadTasksList.RemoveAt(0);
                Debug.Log("download url:" + request.url);

                string localUrl = string.Format("{0}", Path.GetFileName(request.url));
                localUrl = FileUtils.GetPersistentPath(localUrl);

                downloadWWW = new WWW(File.Exists(localUrl) ? FileUtils.GetWWWPath(localUrl) : request.url);
                Debug.Log("final url:" + downloadWWW.url);
                while (!downloadWWW.isDone)
                {
                    if (request.onProgress != null)
                    {
                        request.onProgress(downloadWWW.progress);
                    }
                    yield return waitForSeconds;
                }
                yield return waitForFixedUpdate;
                if (string.IsNullOrEmpty(downloadWWW.error) && request.url == downloadWWW.url)
                {
                    File.WriteAllBytes(localUrl, downloadWWW.bytes);
                    Debug.Log("local url:" + localUrl);
                }
                if (request.onResult != null)
                {
                    request.onResult(downloadWWW);
                }
                yield return waitForFixedUpdate;
                downloadWWW.Dispose();
                downloadWWW = null;
                request = null;
                Resources.UnloadUnusedAssets();
                GC.Collect();
                yield return waitForFixedUpdate;
            }
        }
    }
}
