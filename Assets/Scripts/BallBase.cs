using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallBase : MonoBehaviour {

    public BallColor color;
    public BallType type;
    public Sprite sprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBall(string name)
    {

        //红、蓝、紫、黄、橙、绿、青、黄金球、炸弹、随机、白、黑、灰、宝藏、梦幻球、尖刺
        sprite = transform.GetComponent<Image>().sprite;
        color = (BallColor)(Enum.Parse(typeof(BallColor), name));
        string s_path = SourcePath.cfg_ballname[color];
        sprite = Resources.Load<Sprite>(s_path);
        //sprite = Resources.Load(s_path) as Sprite;
        switch (name)
        {
            /******Normal*******/
            case "红色":
                type = BallType.Normal; break;
            case "蓝色":
                type = BallType.Normal; break;
            case "紫色":
                type = BallType.Normal; break;
            case "黄色":
                type = BallType.Normal; break;
            case "橙色":
                type = BallType.Normal; break;
            case "绿色":
                type = BallType.Normal; break;
            case "青色":
                type = BallType.Normal; break;

            case "黄金球":
                type = BallType.Tools; break;
            case "炸弹":
                type = BallType.Tools; break;
            case "随机球":
                type = BallType.Tools; break;
            case "白色":
                type = BallType.Tools; break;
            case "黑色":
                type = BallType.Block; break;
            case "灰色":
                type = BallType.Block; break;
            case "宝藏":
                type = BallType.Tools; break;
            case "梦幻球":
                type = BallType.Tools; break;
            case "尖刺":
                type = BallType.Block; break;

            default: sprite = null; type = 0; break;
        }
        transform.GetComponent<Image>().sprite = sprite;
    }
}
