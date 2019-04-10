using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMeshImage : MonoBehaviour {


    public string URL
    {
        get { return resURL; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (string.Compare(resURL, value) == 0)
            {
                return;
            }
            resURL = value;
            isCompleted = false;
            enabled = false;
            enabled = true;
        }
    }
    public string resURL;
    public bool isCompleted = true;
    public bool useNativeSize = false;
    public MeshRenderer meshRenderer;

    private void OnEnable()
    {
        if (isCompleted == false)
        {
            StartCoroutine(DownloadRes());
            meshRenderer.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(DownloadRes());
    }

    private IEnumerator DownloadRes()
    {
        WWW www = new WWW(resURL);
        yield return www;
        if (string.IsNullOrEmpty(www.error) == false)
        {
            isCompleted = true;
            Debug.LogError(www.error);
            www.Dispose();
            yield break;
        }

        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
            Material newMat = new Material(meshRenderer.sharedMaterial);
            newMat.SetTexture("_MainTex", www.texture);
            meshRenderer.material = newMat;
        }
        www.Dispose();
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //if (sr != null)
        //{
        //    sr.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
        //    sr.color = Color.white;
        //}
    }
}
