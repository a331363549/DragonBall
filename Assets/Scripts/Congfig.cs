using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//红、蓝、紫、黄、橙、绿、青、黄金球、炸弹、随机、白、黑、灰、梦幻球、宝藏
public enum BallColor
{
    //普通球		7
    红色,        //红色
    蓝色,       //蓝色
    紫色,     //紫色球
    黄色,     //黄色
    橙色,     //橙色
    绿色,      //绿色
    青色,      //青色球


    黄金球,       //黄金球
    炸弹,       //炸弹
    随机球,     //随机球

    白色,      //白色球
    黑色,      //黑色	
    灰色,       //灰色

    梦幻球,      //魔幻球
    宝藏球,   //宝藏球
    尖刺,      //三角形

    None,       //分数图标

}
//public enum BallColor
//{
//    //普通球		7
//    Red,        //红色
//    Blue,       //蓝色
//    Purple,     //紫色球
//    Yellow,     //黄色
//    Oringe,     //橙色
//    Green,      //绿色
//    Ching,      //青色球


//    Glod,       //黄金球
//    Boom,       //炸弹
//    Random,     //随机球

//    White,      //白色球
//    Black,      //黑色	
//    Gray,       //灰色

//    Magic,      //魔幻球
//    Precious,   //宝藏球
//    Block,      //三角形

//    None,       //分数图标

//}

public enum BallType
{
    Normal,     //常规
    Tools,      //道具
    Block,      //障碍物
}

public enum LevelType
{
    PASS,       //通过
    NOWING,     //进行中
    LOCKED,     //未解锁
}

public enum SheetName
{
    MISSION,
    INIT_BALL,
    SHOOT_PLAN,
    PAN_PLAN
}

/// <summary>
/// 配置表文件
/// </summary>
public class Sheet_Config
{
    /// 关卡表
    public Dictionary<int, string> mission = new Dictionary<int, string>();
    ///初始化球盘方案表
    public Dictionary<int, string> init_ball = new Dictionary<int, string>();
    /// 发射区球方案表
    public Dictionary<int, string> shoot_plan = new Dictionary<int, string>();
    /// 球盘方案表
    public Dictionary<int, string> pan_plan = new Dictionary<int, string>();
}


public class SourcePath
{
    public const string ball_path = "Sprites/UIGame/ball/";
    public static Dictionary<BallColor, string> cfg_ballname = new Dictionary<BallColor, string>()
    {
        { BallColor.红色,    ball_path + "hongseball"},
        { BallColor.蓝色,   ball_path + "lanseball"},
        { BallColor.紫色, ball_path + "ziseball"},
        { BallColor.黄色, ball_path + "huangseball"},
        { BallColor.橙色, ball_path + "chengseball"},
        { BallColor.绿色,  ball_path + "lvseball"},
        { BallColor.青色,  ball_path + "qingseball"},
        { BallColor.黄金球,   ball_path + "goldball"},
        { BallColor.炸弹,   ball_path + "booms"},
        { BallColor.随机球, ball_path + "suijibiaozhi"},
        { BallColor.白色,  ball_path + "baiseball"},
        { BallColor.黑色,  ball_path + "heiseball"},
        { BallColor.灰色,   ball_path + "huiseball"},
        { BallColor.梦幻球,  ball_path + "menghuanball"},
        { BallColor.宝藏球,   ball_path + "baozangball"},
        { BallColor.尖刺,  ball_path + "jianci 1"},
    };
}

