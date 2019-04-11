using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameUILogic : UILogic
{

    public Button m_BtnRefresh;
    public Button m_BtnClear;
    public Button m_BtnPause;
    public Text m_missionNum;
    public GameObject m_time;
    public Text m_score;
    public Slider m_slide;
    public GameObject m_targets;
    public GameObject m_stars;


    public void Init(string str)
    {
        m_missionNum.text = "关卡：" + str;
        m_BtnRefresh.onClick.AddListener(OnRefreshHandler);
        m_BtnClear.onClick.AddListener(OnClearHandler);
        m_BtnPause.onClick.AddListener(OnPauseHandler);
    }

    public void UpdateTargets(Dictionary<BallColor, int> dic,string score)
    {
        m_score.text = score;
    }

    //public void UpdateScore(string)
    //{

    //}

    private void OnRefreshHandler()
    {
        Debug.Log("OnRefreshHandler");
    }
    private void OnClearHandler()
    {
        Debug.Log("OnClearHandler");
    }
    private void OnPauseHandler()
    {
        Debug.Log("OnPauseHandler");
    }
}
