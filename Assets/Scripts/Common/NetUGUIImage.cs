using NewEngine.Framework;
using NewEngine.Framework.Service;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class NetUGUIImage : MonoBehaviour
{

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
    public Image image;
    public bool useNativeSize = false;
    public int maxWidth;
    public int maxHeight;

    private void OnEnable()
    {
        if (isCompleted == false)
        {
            string finalRes = resURL;
            if (maxWidth != 0)
            {
                finalRes = string.Format("{0}?imageMogr2/thumbnail/{1}", resURL, maxWidth);
            }
            else if (maxHeight != 0)
            {
                finalRes = string.Format("{0}?imageMogr2/thumbnail/{1}", resURL, maxHeight);
            }
            DownloadService.AddDownloadTask(finalRes, OnDownloadResult);
        }
        else
        {
            //image.color = Color.white;
        }
    }

    private void OnDisable()
    {
        DownloadService.RemoveDownloadTask(resURL, OnDownloadResult);
    }

    private void OnDestroy()
    {
        image.sprite = null;
    }

    private Texture2D srcText = null;
    private void OnDownloadResult(WWW www)
    {
        if (string.IsNullOrEmpty(www.error) == false)
        {
            isCompleted = true;
            Debug.LogError(www.error);
            return;
        }

        if (image == null)
        {
            return;
        }
        Debug.Log("Imgae.Step:0");

        srcText = www.texture;
        AsyncEventDriver.QueueOnMainThread(() =>
        {
            Debug.Log("Imgae.Step:1");
            FreshImage(srcText);
            Debug.Log("Imgae.Step:9");
            srcText = null;
            Debug.Log("Imgae.Step:10");
        });

        //newText = new Texture2D(www.texture.width, www.texture.height);
        //Graphics.ConvertTexture(www.texture, newText);

        //image.sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));

        //if (maxWidth != 0 && maxHeight != 0)
        //{
        //    //newText = new Texture2D(maxWidth, maxHeight);
        //    //Graphics.ConvertTexture(www.texture, newText);
        //    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
        //    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
        //}
        //else if (maxHeight != 0)
        //{
        //    //newText = new Texture2D(www.texture.width * maxHeight / www.texture.height, maxHeight);
        //    //Graphics.ConvertTexture(www.texture, newText);
        //    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
        //    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, www.texture.width * maxHeight / www.texture.height);
        //}
        //else if (maxWidth != 0)
        //{
        //    //newText = new Texture2D(maxWidth, www.texture.height * maxWidth / www.texture.width);
        //    //Graphics.ConvertTexture(www.texture, newText);
        //    //image.sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));
        //    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
        //    image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, www.texture.height * maxWidth / www.texture.width);
        //}
        //else
        //{
        //    image.SetNativeSize();
        //}

        //image.color = Color.white;
    }
    
    private void FreshImage(Texture2D srcText)
    {

        //Texture2D newText = new Texture2D(srcText.width, srcText.height);
        //Graphics.ConvertTexture(srcText, newText);

        Debug.Log("Imgae.Step:2");
        image.sprite = Sprite.Create(srcText, new Rect(0, 0, srcText.width, srcText.height), new Vector2(0.5f, 0.5f));

        Debug.Log("Imgae.Step:3");
        if (maxWidth != 0 && maxHeight != 0)
        {
            //newText = new Texture2D(maxWidth, maxHeight);
            //Graphics.ConvertTexture(www.texture, newText);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
            Debug.Log("Imgae.Step:4");
        }
        else if (maxHeight != 0)
        {
            //newText = new Texture2D(www.texture.width * maxHeight / www.texture.height, maxHeight);
            //Graphics.ConvertTexture(www.texture, newText);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, srcText.width * maxHeight / srcText.height);
            Debug.Log("Imgae.Step:5");
        }
        else if (maxWidth != 0)
        {
            //newText = new Texture2D(maxWidth, www.texture.height * maxWidth / www.texture.width);
            //Graphics.ConvertTexture(www.texture, newText);
            //image.sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, srcText.height * maxWidth / srcText.width);
            Debug.Log("Imgae.Step:6");
        }
        else
        {
            image.SetNativeSize();
            Debug.Log("Imgae.Step:7");
        }

        image.color = Color.white;
        Debug.Log("Imgae.Step:8");
    }
}

