using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class UITriggerLogic : UILogic, IBeginDragHandler, IEndDragHandler
{

    [SerializeField]
    private Transform streets;
    public Sprite[] icon;

    [SerializeField]
    private ScrollRect rect;                        //滑动组件  
    private float targethorizontal = 0;             //滑动的起始坐标  
    private bool isDrag = false;                    //是否拖拽结束  
    private List<float> posList = new List<float>();            //求出每页的临界角，页索引从0开始  
    private int currentPageIndex = -1;
    public Action<int> OnPageChanged;

    private bool stopMove = true;
    public float smooting = 4;      //滑动速度  
    public float sensitivity = 0;
    private float startTime;

    private float startDragHorizontal;

    public void Init()
    {
        int idx = 0;
        foreach (StreetType p in Enum.GetValues(typeof(StreetType)))
        {
            if (p == StreetType.广场 || p == StreetType.古韵街 || p == MainLogic.Instance.cur_streetType)
                continue;
            Transform street = streets.GetChild(idx);
            street.GetComponentInChildren<Text>().text = p.ToString();
            street.GetComponent<Image>().sprite = icon[Convert.ToInt32(p)];
            street.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                StreetButtonHandler(p);
            });
            idx += 1;
        }
    }

    public void StreetButtonHandler(StreetType type)
    {
        Close();
        MainLogic.Instance.StartCoroutine(MainLogic.Instance.LoadStreet(type));
    }

    void Awake()
    {
        Init();
        for (int i = 0; i < CityViewsData.Instance.viewsDataList.Count; i++)
        {
            GameObject go = new GameObject("sight" + i);
            go.transform.parent = rect.content;
            //StartCoroutine(DownLoadTexture(CityViewsData.Instance.viewsDataList[i].pic, go.GetComponent<Image>()));
            string name = Path.GetFileNameWithoutExtension(CityViewsData.Instance.viewsDataList[i].pic);
            go.AddComponent<Image>().sprite = Texture2Sprite(name);
            string res_id = CityViewsData.Instance.viewsDataList[i].source;
            go.AddComponent<Button>().onClick.AddListener(delegate() { ViewTrigger(res_id); });
           
        }
        rect.content.sizeDelta = new Vector2(320 * rect.content.childCount + 100, rect.content.rect.height);
        float horizontalLength = rect.content.rect.width - GetComponent<RectTransform>().rect.width;
        float verticalLength = rect.content.rect.height - GetComponent<RectTransform>().rect.height;

    }

    void ViewTrigger(string source)
    {
        MainLogic.Instance.StartCoroutine(MainLogic.Instance.Street2Sight(source));
        Debug.Log(source);
    }


    void Update()
    {
        if (!isDrag && !stopMove)
        {
            startTime += Time.deltaTime;
            float t = startTime * smooting;
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
            if (t >= 1)
                stopMove = true;
        }
    }


    private void SetPageIndex(int index)
    {
        if (currentPageIndex != index)
        {
            currentPageIndex = index;
            if (OnPageChanged != null)
                OnPageChanged(index);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        startDragHorizontal = rect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float posX = rect.horizontalNormalizedPosition;
        posX += ((posX - startDragHorizontal) * sensitivity);
        posX = posX < 1 ? posX : 1;
        posX = posX > 0 ? posX : 0;
        int index = 0;
        float offset = Mathf.Abs(posList[index] - posX);
        for (int i = 1; i < posList.Count; i++)
        {
            float temp = Mathf.Abs(posList[i] - posX);
            if (temp < offset)
            {
                index = i;
                offset = temp;
            }
        }
        SetPageIndex(index);

        targethorizontal = posList[index]; //设置当前坐标，更新函数进行插值  
        isDrag = false;
        startTime = 0;
        stopMove = false;
    }

    public void Close()
    {
        UITriggerSlide.Close();
    }

    IEnumerator DownLoadTexture(string url, Image pic)
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.isDone && www.error == null)
        {
            Texture2D img = www.texture;
            byte[] bytes = img.EncodeToPNG();
            //File.WriteAllBytes(filePath, bytes);

            Texture2D t2d = new Texture2D(1920, 1080);
            t2d.LoadImage(bytes);
            Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), Vector2.zero);
            pic.sprite = sprite;
        }
    }

    private Sprite Texture2Sprite(string name)
    {
        FileStream files = new FileStream(Application.persistentDataPath + string.Format("/{0}.jpg", name), FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
      
        Texture2D t2d = new Texture2D(1920, 1080);
        t2d.LoadImage(imgByte);
        Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), Vector2.zero);
        return sprite;
    }
}
