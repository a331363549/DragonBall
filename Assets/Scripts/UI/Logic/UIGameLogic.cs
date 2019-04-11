using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameLogic : UILogic
{

    /// <summary>
    /// 当前替换的球
    /// </summary>
    ///   [SerializeField]
    public Transform current_ball_pos;
    /// <summary>
    /// 地形圆盘
    /// </summary>
    [SerializeField]
    Transform dx_trans;
    /// <summary>
    /// 发射区位置
    /// </summary>
    [SerializeField]
    public Transform shoot_trans;
    /// <summary>
    /// 下一个球位置
    /// </summary>
    [SerializeField]
    public Transform next_trans;

    Dictionary<string, string> dx_pos = new Dictionary<string, string>();
    List<int> list_shootRate;
    List<int> dx_createRate;

    /// <summary>
    /// 方向
    /// </summary>
    private int direction;
    /// <summary>
    /// 速度
    /// </summary>
    private float speed;
    /// <summary>
    /// 本次消除连击数
    /// </summary>
    int current_combo = 0;
    /// <summary>
    /// 本次消除得分
    /// </summary>
    int current_score = 0;
    /// <summary>
    /// 消除字典
    /// </summary>
    List<List<BallBase>> clear_list = new List<List<BallBase>>();
    UIGameUILogic m_uiLogic;
    // Use this for initialization
    void Start () {
        InitGameMap();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitGameMap()
    {
        MissionConfig cfg = MissionConfig.Instance;
        cfg.Config_mission(MainLogic.Instance.m_CurMission);
        dx_pos = cfg.Config_Dx(cfg.init_ball_id);
        list_shootRate = cfg.Config_RateList(cfg.shoot_ball_id, SheetName.SHOOT_PLAN);
        dx_createRate = cfg.Config_RateList(cfg.create_ball_1, SheetName.SHOOT_PLAN);
        direction = cfg.direction;
        speed = cfg.speed;

        Create_InitBall();
        UIGameUISlide.Open();
        m_uiLogic = FindObjectOfType<UIGameUILogic>();
        CheckClear();
        m_uiLogic.Init(MainLogic.Instance.m_CurMission.ToString());
        Create_NewBall(list_shootRate, shoot_trans, Vector3.zero);
        Create_NewBall(list_shootRate, next_trans, Vector3.zero);
    }

    /// <summary>
    /// 地形初始化
    /// </summary>
    void Create_InitBall()
    {
        Vector2 centerPos = dx_trans.position;
        int radius = 250;
        int angel = 12;
        int index = 360 / angel;
        foreach (var item in dx_pos)
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/GamePab/pos")) as GameObject;
            go.transform.SetParent(dx_trans);
            float x = centerPos.x + radius * Mathf.Cos(index * angel * 3.14f / 180f);
            float y = centerPos.y + radius * Mathf.Sin(index * angel * 3.14f / 180f);
            go.name = "pos" + (30 - index);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(x, y);
            go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 270 + index * angel);
            GameObject ball = Instantiate(Resources.Load("Prefabs/GamePab/ballPab")) as GameObject;
            ball.transform.SetParent(go.transform);
            ball.AddComponent<BallBase>();
            ball.transform.localPosition = new Vector3(0, 75, 0);
            ball.transform.localScale = Vector3.one;
            ball.transform.localEulerAngles = Vector3.zero;
            ball.tag = "pos";
            ball.SendMessage("SetBall", item.Value);
            index -= 1;
        }
    }

    /// <summary>
    /// 检测是否能消除
    /// </summary>
    public void CheckClear()
    {
        for (int i = 0; i < dx_trans.childCount; i++)
        {
            BallBase ball_1;
            BallBase ball_2;
            BallBase ball_3;
            if (i < 28)
            {
                ball_1 = dx_trans.GetChild(i).GetChild(0).GetComponent<BallBase>();
                ball_2 = dx_trans.GetChild(i + 1).GetChild(0).GetComponent<BallBase>();
                ball_3 = dx_trans.GetChild(i + 2).GetChild(0).GetComponent<BallBase>();
            }
            else if (i == 28)
            {
                ball_1 = dx_trans.GetChild(28).GetChild(0).GetComponent<BallBase>();
                ball_2 = dx_trans.GetChild(29).GetChild(0).GetComponent<BallBase>();
                ball_3 = dx_trans.GetChild(0).GetChild(0).GetComponent<BallBase>();
            }
            else if (i == 29)
            {
                ball_1 = dx_trans.GetChild(29).GetChild(0).GetComponent<BallBase>();
                ball_2 = dx_trans.GetChild(0).GetChild(0).GetComponent<BallBase>();
                ball_3 = dx_trans.GetChild(1).GetChild(0).GetComponent<BallBase>();
            }
            else
                return;
            if (ball_1.color == ball_2.color && ball_1.color == ball_3.color)
            {
                List<BallBase> clear_balls = new List<BallBase>();
                clear_balls.Add(ball_1);
                clear_balls.Add(ball_2);
                clear_balls.Add(ball_3);
                int index = 0;
                if (i + 3 > 29)
                    index = i + 3 - 30;
                else
                    index = i + 3;
                while (true)
                {
                    if (index == i)
                        break;
                    if (index > 29)
                        index = 0;
                    BallBase ball = dx_trans.GetChild(index).GetChild(0).GetComponent<BallBase>();
                    if (ball.color == ball_1.color)
                    {
                        clear_balls.Add(ball);
                        index++;
                    }
                    else
                    {
                        clear_list.Add(clear_balls);
                        break;
                    }
                }
            }
        }
        if (clear_list.Count != 0)
            StartCoroutine(ClearAndCreate());
        else
            ShowComboAndScore();
    }


    /// <summary>
    /// 显示分数与连击数
    /// </summary>
    /// <returns></returns>
    private void ShowComboAndScore()
    {
        if (current_combo > 3)
            Debug.Log("七彩球");
        if (current_combo > 1)
            current_score += 10 * current_combo;
        //string info = "good";
        //if (current_score >= MissionConfig._instance.need_good)
        //    info = "good";
        //if (current_score >= MissionConfig._instance.need_greate)
        //    info = "greate";
        //if (current_score >= MissionConfig._instance.need_amazing)
        //    info = "amazing";
        //if (current_score >= MissionConfig._instance.need_excellent)
        //    info = "excellent";
        //if (current_score >= MissionConfig._instance.need_crazy)
        //    info = "crazy";
        //if (current_score >= MissionConfig._instance.need_unbelievable)
        //    info = "unbeliveable";
        if (current_score > 0)
        {
            //UIManager.GetInstance().ShowUIForms(ProConst.INFO_FORM);
            //SendMessage("InfoUIForm", info, current_combo);
        }
        current_combo = 0;
        current_score = 0;
    }

    /// <summary>
    /// 消除并创建新球
    /// </summary>
    /// <returns></returns>
    IEnumerator ClearAndCreate()
    {
        for (int i = 0; i < clear_list.Count; i++)
        {
            //更新目标显示再进行消除
            BallColor color = clear_list[i][0].GetComponent<BallBase>().color;

            for (int j = 0; j < clear_list[i].Count; j++)
            {
                Transform ball_parent = clear_list[i][j].transform.parent;
                Create_NewBall(dx_createRate, ball_parent, new Vector3(0, 75, 0));
            }
            string str_head = clear_list[i][0].transform.parent.name;
            string str_end = clear_list[i][clear_list[i].Count - 1].transform.parent.name;
            int head = int.Parse(System.Text.RegularExpressions.Regex.Replace(str_head, @"[^0-9]+", "")) - 1;
            int end = int.Parse(System.Text.RegularExpressions.Regex.Replace(str_end, @"[^0-9]+", "")) + 1;
            if (head < 0)
                head = 29;
            if (end > 29)
                end = 0;
            Func_Black_Gray(dx_trans.transform.GetChild(head));
            Func_Black_Gray(dx_trans.transform.GetChild(end));
            current_score += getAddScore(clear_list[i].Count);
            Dictionary<BallColor, int> dic_mm = new Dictionary<BallColor, int>();
            dic_mm.Add(color, clear_list[i].Count);
            m_uiLogic.UpdateTargets(dic_mm, current_score.ToString());
            //SendMessage("MainUIForm", "UpdateTargets", dic_mm);
            //SendMessage("MainUIForm", "UpdateScore", current_score);
            Debug.Log("ball_color = " + color + " score = " + current_score + "count = " + clear_list[i].Count + "length = " + clear_list.Count);
        }
        current_combo += clear_list.Count;
        //SendMessage("AudioManager", "Combo", "combo" + current_combo);
        yield return new WaitForSeconds(0.5f);
        clear_list.Clear();
        CheckClear();
    }


    /// <summary>
    /// 黑色球 灰色球处理方法
    /// </summary>
    /// <param name="transform"></param>
    public void Func_Black_Gray(Transform transform)
    {
        BallBase ball = transform.GetComponentInChildren<BallBase>();
        if (ball.color == BallColor.黑色)
            ball.SendMessage("SetBall", "灰色");
        else if (ball.color == BallColor.灰色)
            Create_NewBall(dx_createRate, transform, new Vector3(0, 75, 0));
        else
            return;
    }

    /// <summary>
    /// 消除分数
    /// </summary>
    /// <param name="lenth"></param>
    /// <returns></returns>
    int getAddScore(int lenth)
    {
        int add = 0;
        int n = lenth - 2;
        add = 5 * (n * n - n + 2);  //An = 5(n^2 - n +2) 通项公式
        return add;
    }


    /// <summary>
    /// 生成新球
    /// </summary>
    /// <param name="rateList">概率表</param>
    /// <param name="trans">父物体的transform</param>
    /// list_shootRate  //发射球生成概率表;
    /// dx_createRate   //地形生成概率表;
    /// <returns></returns>
    public void Create_NewBall(List<int> rateList, Transform trans, Vector3 postion)
    {
        int rate = UnityEngine.Random.Range(0, 1000);
        int index = 0;
        for (int i = 0; i < rateList.Count; i++)
        {
            if (rate < rateList[i])
            {
                index = i;
                break;
            }
        }
        //红、蓝、紫、黄、橙、绿、青、黄金球、炸弹、随机、白、黑、灰、宝藏、梦幻球、尖刺
        string name = "";
        switch (index)
        {
            case 0: name = "红色"; break;
            case 1: name = "蓝色"; break;
            case 2: name = "紫色"; break;
            case 3: name = "黄色"; break;
            case 4: name = "橙色"; break;
            case 5: name = "绿色"; break;
            case 6: name = "青色"; break;
            case 7: name = "黄金球"; break;
            case 8: name = "炸弹"; break;
            case 9: name = "随机球"; break;
            case 10: name = "白色"; break;
            case 11: name = "黑色"; break;
            case 12: name = "灰色"; break;
            case 13: name = "宝藏"; break;
            case 14: name = "梦幻球"; break;
            case 15: name = "尖刺"; break;
        }
        GameObject go = null;
        //if (trans.childCount != 0)
        //    go = trans.GetChild(0).gameObject;
        //else
        //    go = Instantiate(ballPab);
        //go.transform.SetParent(trans);
        //if (trans.parent.name == "center")
        //    go.tag = "pos";
        //go.transform.localPosition = postion;
        //go.transform.localScale = Vector3.one;
        //go.transform.localEulerAngles = Vector3.zero;
        //go.SendMessage("SetBall", name);
    }
}
