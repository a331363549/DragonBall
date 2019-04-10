using NewEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WebApiCheckRedPacketData
{
    public string code;
    public string msg;
    public ShopRedPacketData data;
}

[Serializable]
public class ShopRedPacketData
{
    public string target_ucode_m;
}


[Serializable]
public class WebApiReturnStoreListData
{
    public string code;
    public string msg;
    public StoreListData data;
}


[Serializable]
public class StoreListData
{
    public int count;
    public int page;
    public WelcomeGift welcome_gift;
    public ShopData[] list;
}

[Serializable]
public class WelcomeGift
{
    public int c_id;    //120
    public string c_index_pic; // "http://a.png"
    public string c_index_words;    // "一分钱"
    public string c_left_button;    // "交给警察叔叔"
    public string c_left_data;      // 12
    public string c_left_go_type;   // "GET_ACHIEVE"
    public string c_pic;            // "http://b.png"
    public string c_right_button;   // "装口袋"
    public string c_right_data;     // "0.1"
    public string c_right_go_type;  // "GET_RED"
    public string c_title;          // "在马路上捡到一分钱"
    public string c_words;          // "一分钱"
}

[Serializable]
public class ShopDataList
{
    public ShopData[] list;
}

[Serializable]
public class ShopData
{
    public StoreInfo shop_info;
    public EventInfo event_info;
    public ActivityInfo active_info;
    public string buildBodyId;
}

[Serializable]
public class ActivityInfo
{
    public string[] is_has_active;
    public string ad_id;
}

    [Serializable]
public class StoreInfo
{
    public int c_id;
    public int industry_id;     // 11
    public string c_name;   // "牛B店铺0"
    public string c_ucode;    // "牛B店铺0"
    public string c_is_vip_vr;
    public string c_vr_url;     // http://tanling.welcomest.com/Source/Panorama_0001.unity3d
}

public enum ShopEventType
{
    BENEFIT,
    ENTERTAINMENT,
    PROMOTION,
}

[Serializable]
public class EventInfo
{
    public int c_id;    //120
    public string c_type;      //
    public string c_index_pic; // "http://a.png"
    public string c_index_words;    // "一分钱"
    public string c_left_button;    // "交给警察叔叔"
    public string c_left_data;      // 12
    public string c_left_go_type;   // "GET_ACHIEVE"
    public string c_pic;            // "http://b.png"
    public string c_right_button;   // "装口袋"
    public string c_right_data;     // "0.1"
    public string c_right_go_type;  // "GET_RED"
    public string c_title;          // "在马路上捡到一分钱"
    public string c_words;          // "一分钱"
    public bool hasTrigger = false;
}

public class StoreData : Singleton<StoreData> {

    public Queue<ShopData> shopDataQueue = new Queue<ShopData>();
}
