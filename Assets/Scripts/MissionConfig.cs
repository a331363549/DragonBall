using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionConfig : MonoBehaviour {


    private static MissionConfig sInstance;
    public static MissionConfig Instance
    {
        get
        {
            if (sInstance == null)
                sInstance = new MissionConfig();
            return sInstance;
        }
    }



    #region 配置表参数
    /****关卡配置表参数*****/
    public int mission_id;            //关卡id
    public int mission_mode;          //关卡模式（1.一龙一转盘2.一龙两转盘3.两龙一转盘）
    public bool isDouble = false;     //两龙珠子是否一致（仅在关卡模式选择3时有效）
    public int direction;             //转盘转动方向（1.向左2.向右3.左右循环4.右左循环）
    public float speed = 1.0f;        //初始速度
    public int add_speed = 0;         //加速度（每转一圈生效，填0就是匀速）
    public bool isStop = false;       //（每转一圈）是否停顿加速度时，填写停顿则停顿时加速清零，再次转动时重新加速）
    public float stopTime = 10000;    //停顿时间（ms）
    public int type = 1;              //关卡类型(1记步2计时）
    public int value = 10;            //对应数值
    public int init_ball_id;          //初始球球方案ID
    public int create_ball_1;         //球球生成方案1
    public int create_ball_2;         //球球生成方案2
    public int shoot_ball_id;         //发射区球球方案ID
    public string[] targets;          // 关卡目标
    public int starsFull;               // 满星分数要求
    // 分数类型（good ,greate,amazing,excellent,crazy,unbelievable）
    string[] scoreType = new string[] { "good", "greate", "amazing", "excellent", "crazy", "unbelievable" };
    public Dictionary<string, int> socreTypeDic = new Dictionary<string, int>();     

    public int star1;                 //1星数值要求
    public int star2;                 //2星数值要求
    public int star3;                 //3星数值要求
    public int need_good;             //good所需数值
    public int need_greate;           //greate所需数值
    public int need_amazing;          //amazing所需数值
    public int need_excellent;        //Excellent所需数值
    public int need_crazy;            //crazy所需数值
    public int need_unbelievable;     //unbelievable所需数值
    #endregion


    public Dictionary<string, string> dx_pos = new Dictionary<string, string>();
    public List<int> list_shootRate = new List<int>();
    public List<int> dx_createRate = new List<int>();
    public List<int> rateList = new List<int>();
    private void Awake()
    {
        //sInstance = this;
        mission_id = PlayerPrefs.GetInt("missionNum", 1);
        Config_mission(mission_id);

        //dx_pos = Config_Dx(init_ball_id);
        //list_shootRate = Config_RateList(shoot_ball_id, SheetName.SHOOT_PLAN);
        //dx_createRate = Config_RateList(create_ball_1, SheetName.PAN_PLAN);
    }

    /// <summary>
    /// 获取关卡表配置文件
    /// </summary>
    /// <param name="id">当前关卡数</param>
    public void Config_mission(int id)
    {
        string mission = GetConfigData(id, SheetName.MISSION);
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(mission);
        mission_mode = int.Parse(data["mission_mode"]);
        isDouble = bool.Parse(data["isDouble"].ToLower());
        direction = int.Parse(data["direction"]);
        speed = int.Parse(data["speed"]);
        add_speed = int.Parse(data["add_speed"]);
        isStop = bool.Parse(data["isStop"].ToLower());
        stopTime = int.Parse(data["stopTime"]);
        type = int.Parse(data["type"]);
        value = int.Parse(data["value"]);
        init_ball_id = int.Parse(data["init_ball_id"]);
        create_ball_1 = int.Parse(data["create_ball_1"]);
        create_ball_2 = int.Parse(data["create_ball_2"]);
        shoot_ball_id = int.Parse(data["shoot_ball_id"]);
        //if (data["target_1"] != "")
        //    targets.Add(data["target_1"]);
        //if (data["target_2"] != "")
        //    targets.Add(data["target_2"]);
        //if (data["target_3"] != "")
        //    targets.Add(data["target_3"]);
        targets = data["targets"].Split('|');
        starsFull = int.Parse(data["starsFull"]);
        for (int i = 0; i < scoreType.Length; i++)
        {
            socreTypeDic[scoreType[i]] = int.Parse(data["socreType"].Split('|')[i]);
        }

        //star1 = int.Parse(data["star_1"]);
        //star2 = int.Parse(data["star_2"]);
        //star3 = int.Parse(data["star_3"]);
        //need_good = int.Parse(data["need_good"]);
        //need_greate = int.Parse(data["need_greate"]);
        //need_amazing = int.Parse(data["need_amazing"]);
        //need_excellent = int.Parse(data["need_excellent"]);
        //need_crazy = int.Parse(data["need_crazy"]);
        //need_unbelievable = int.Parse(data["need_unbelievable"]);
    }

    /// <summary>
    /// 获取地形配置文件
    /// </summary>
    /// <param name="id">地形id</param>
    /// <returns dx_ball="dx_ball">初始化地形的球的颜色</returns>
    public Dictionary<string,string> Config_Dx(int id)
    {
        dx_pos.Clear();
        string data_ball = GetConfigData(id, SheetName.INIT_BALL);
        dx_pos = JsonConvert.DeserializeObject<Dictionary<string, string>>(data_ball);
        return dx_pos;
    }


  
    /// <summary>
    /// 获取发射表配置文件
    /// </summary>
    /// <param name="id">发射表id</param>
    /// <returns>发射列表、地形生成球的概率</returns>
    public List<int> Config_RateList(int id,SheetName sheet)
    {
        rateList.Clear();
        string data = GetConfigData(id, sheet);
        Dictionary<string, string> dic_ball = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        List<int> tmp = new List<int>();
        //红、蓝、紫、黄、橙、绿、青、黄金球、炸弹、随机、白、黑、灰、梦幻、宝藏
        foreach (var p in dic_ball)
        {
            if (p.Key == "desc")
                continue;
            tmp.Add(int.Parse(p.Value));
        }
        for (int i = 0; i < tmp.Count; i++)
        {
            if (tmp[i] == 0)
                rateList.Add(0);
            else
            {
                int rate = 0;
                for (int k = 0; k <= i; k++)
                    rate += tmp[k];
                rateList.Add(rate);
            }
        }
        return rateList;
    }


    /// <summary>
    ///  获取配置表数据
    /// </summary>
    public string GetConfigData(int id, SheetName name)
    {
        string sheet_name = "";
        switch (name)
        {
            case SheetName.MISSION: sheet_name = "mission"; break;
            case SheetName.INIT_BALL: sheet_name = "init_ball"; break;
            case SheetName.SHOOT_PLAN: sheet_name = "shoot_plan"; break;
            case SheetName.PAN_PLAN: sheet_name = "pan_plan"; break;
        }
        TextAsset asset = Resources.Load(sheet_name) as TextAsset;

        if (!asset)  //读不到就退出此方法
            return null;

        string strdata = asset.text;

        Dictionary<int, string> dic = JsonConvert.DeserializeObject<Dictionary<int, string>>(strdata);
        return dic[id];

    }
}
