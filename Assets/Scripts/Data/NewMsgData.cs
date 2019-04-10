using NewEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMsg
{
    public string msg;
    public int msgType;
}

public class NewMsgData : Singleton<NewMsgData>
{
    private Queue<NewMsg> msgQueue = new Queue<NewMsg>();
    public Queue<NewMsg> MsgQueue
    {
        get
        {
            return msgQueue;
        }
        set
        {
            msgQueue = value;
        }
    }
}

[Serializable]
public class WebNewMsgData
{
    public string code;
    public string msg;
    public NewBroadcastMsg data;
}

[Serializable]
public class NewBroadcastMsg
{
    public NewMsgDic list;
}

[Serializable]
public class NewMsgDic
{
    public string[] red;
    public string[] coupon;
    public string[] street_visit;
}
