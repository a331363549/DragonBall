using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIndexLogic : UILogic
{

    public Text m_TxtStar;
    public Text m_CurMission;

    [SerializeField]
    Button m_BtnAD;
    [SerializeField]
    Button m_BtnLianjie;
    [SerializeField]
    Button m_BtnGift;
    [SerializeField]
    Button m_BtnBag;
    [SerializeField]
    Button m_BtnSettings;


    private ScrollRect m_ScrollView;
   


    void Start()
    {
        m_ScrollView = GetComponentInChildren<ScrollRect>();
        Debug.Log(m_ScrollView.name);
        InitMap();

    }

    void InitMap()
    {
        int pages;
        int maxLevel = 100;
        Vector2[] pos_arr = new Vector2[]{
        new Vector2(0,-470),new Vector2(-140,-350),new Vector2(140,-350),
        new Vector2(0,-240),new Vector2(-140,-130),new Vector2(140,-130),
        new Vector2(0,-10),new Vector2(-140,130),new Vector2(140,130),
        new Vector2(0,250),new Vector2(-140,370),new Vector2(140,370),
        new Vector2(0,490)
        };

        int now_level = PlayerPrefs.GetInt("now_level", 1);
        pages = 100 / pos_arr.Length + 1;
        m_ScrollView.content.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(750, m_ScrollView.transform.GetComponent<RectTransform>().rect.height * pages);
        float pos = (m_ScrollView.content.rect.height - m_ScrollView.transform.GetComponent<RectTransform>().rect.height) / 2;
        for (int i = 0; i < pages; i++)
        {
            GameObject p = Instantiate(Resources.Load("Prefabs/PagePab") as GameObject, m_ScrollView.content);
            p.transform.localPosition = new Vector3(0, -pos + 1160 * (i + 4), 0);
            p.transform.localScale = Vector3.one;
            p.name = "page" + (i + 1);
            for (int j = 0; j < pos_arr.Length; j++)
            {
                int index = (i * 13 + j + 1);
                GameObject m = null;

                if (index < now_level)
                {
                    m = Instantiate(Resources.Load("Prefabs/Pass_Level_Ball") as GameObject, p.transform, false);
                    m.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        MainLogic.Instance.m_CurMission = index;
                        UIGameSlide.Open();
                        UIIndexSlide.Close();
                    });
                }
                else if (index == now_level)
                {
                    m = Instantiate(Resources.Load("Prefabs/Now_Level_Ball") as GameObject, p.transform, false);
                    m.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        MainLogic.Instance.m_CurMission = index;
                        UIGameSlide.Open();
                        UIIndexSlide.Close();
                    });
                }
                else if (index > now_level)
                    m = m = Instantiate(Resources.Load("Prefabs/Lock_Level_Ball") as GameObject, p.transform, false);
                else
                    Debug.LogError("Level Error");
                m.name = "level" + index;
                m.transform.GetComponentInChildren<Text>().text = index.ToString();
                m.transform.localPosition = pos_arr[j];
                //int index =  int.Parse(Regex.Replace(m.name,@"[^0-9]+",""));
                if (index >= maxLevel)
                    break;
            }
            // Debug.LogWarning(p.transform.localPosition);
        }
    }
}
