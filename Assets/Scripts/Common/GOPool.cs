using NewEngine.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOPool : CService
{
    private static GOPool sInstance = null;
    public static GOPool Instance
    {
        get
        {
            return sInstance;
        }
    }

    public Dictionary<string, Queue<GameObject>> goCahces = new Dictionary<string, Queue<GameObject>>();
    public Dictionary<string, Object> objectCahces = new Dictionary<string, Object>();

    private AssetBundle mainAsset = null;
    public AssetBundle MainAsset
    {
        get
        {
            return mainAsset;
        }
        set
        {
            mainAsset = value;
        }
    }

    public Object LoadRes(string resPath, AssetBundle asset = null)
    {
        if (asset == null && mainAsset == null)
        {
            return loadFromBuildIn(resPath);
        }
        else if (asset != null)
        {
            return loadFromAB(resPath, asset);
        }
        else if (mainAsset != null)
        {
            return loadFromAB(resPath, mainAsset);
        }
        return null;
    }

    public void ReleaseCache()
    {
        foreach (var queue in goCahces.Values)
        {
            while (queue.Count > 0)
            {
                GameObject.Destroy(queue.Dequeue());
            }
        }
        goCahces.Clear();
        objectCahces.Clear();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public GameObject PopGO(string resPath, AssetBundle asset = null)
    {
        Queue<GameObject> caches;
        if (goCahces.TryGetValue(resPath, out caches) && caches.Count > 0)
        {
            return caches.Dequeue();
        }
        else
        {
            Object resObj = LoadRes(resPath + ".prefab", asset);
            GameObject go = resObj == null ? null : GameObject.Instantiate(resObj) as GameObject;
            if (go != null)
            {
                go.name = resPath;
                go.SetActive(true);
            }
            return go;
        }
    }

    public void PushGO(GameObject go)
    {
        go.transform.SetParent(this.transform);
        go.SetActive(false);
        //GameObject.Destroy(go);
        Queue<GameObject> caches;
        if (goCahces.TryGetValue(go.name, out caches))
        {
            caches.Enqueue(go);
        }
        else
        {
            caches = new Queue<GameObject>();
            caches.Enqueue(go);
            goCahces.Add(go.name, caches);
        }
    }
    
    private Object loadFromBuildIn(string resPath)
    {
        Object res;
        if (objectCahces.TryGetValue(resPath, out res))
        {
            return res;
        }
        res = Resources.Load(resPath.Substring(0, resPath.LastIndexOf(".")));
        objectCahces.Add(resPath, res);
        return res;
    }

    private Object loadFromAB(string resPath, AssetBundle asset)
    {
        if (asset == null)
        {
            return null;
        }

        string resFullPath = string.Format("assets/gameres/{0}/{1}", asset.name, resPath).ToLower();

        Object res;
        if (objectCahces.TryGetValue(resFullPath, out res) == false)
        {
#if UNITY_EDITOR
            //res = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(resFullPath);
#else
            //res = asset.LoadAsset(resFullPath);
#endif
            res = asset.LoadAsset(resFullPath);
            if (res == null)
            {
                return loadFromBuildIn(resPath);
            }
            objectCahces.Add(resFullPath, res);
        }

        return res;
    }

    public void ReloadShader(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        for (int idx = 0; idx < renderers.Length; idx++)
        {
            for (int idx2 = 0; idx2 < renderers[idx].sharedMaterials.Length; idx2++)
            {
                if (renderers[idx].sharedMaterials[idx2] != null)
                {
                    Shader shader = Shader.Find(renderers[idx].sharedMaterials[idx2].shader.name);
                    if (shader != null)
                    {
                        renderers[idx].sharedMaterials[idx2].shader = shader;
                    }
                }
            }
        }
    }

    protected override void OnServiceUpdate()
    {

    }

    private void Awake()
    {
        sInstance = this;
    }

    private void Update()
    {
#if USE_SERVICE_UPDATE
#else
        OnServiceUpdate();
#endif
    }

    private void OnDestroy()
    {
        sInstance = null;
        ReleaseCache();
        mainAsset = null;
        AssetBundle.UnloadAllAssetBundles(true);
    }
}
