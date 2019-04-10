using NewEngine.Framework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStreetLogic : UILogic {

    public Toggle musicSwitch;
    public Toggle screenMsgSwitch;
    public Button streetChange;
    public Button backBtn;

	// Use this for initialization
	void Start ()
    {
        transform.SetSiblingIndex(0);
        if (MainLogic.Instance.cur_streetType == StreetType.广场)
            streetChange.transform.gameObject.SetActive(false);
        else
            streetChange.transform.gameObject.SetActive(true);
        if (MainLogic.Instance.cur_streetType== StreetType.古韵街)
        {
            streetChange.transform.GetChild(0).gameObject.SetActive(false);
            streetChange.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            streetChange.transform.GetChild(0).gameObject.SetActive(true);
            streetChange.transform.GetChild(1).gameObject.SetActive(false);
        }
        bool isMusicMute = PlayerPrefs.GetInt("MusicIsMute", 0) == 1;
        musicSwitch.isOn = isMusicMute;
        bool isDanMuVisible = PlayerPrefs.GetInt("DanMuVisible", 1) == 1;
        screenMsgSwitch.isOn = !isDanMuVisible;
#if UNITY_IPHONE
        //backBtn.gameObject.SetActive(false);
#endif
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void BroadCastSwitcher(bool val)
    {
        Debug.Log("BroadCastSwitcher:" + val);
        PlayerPrefs.SetInt("DanMuVisible", val ? 0 : 1);
        UIBroadcastSlide.isEnableShow = !val;
        if (val)
        {
            UIBroadcastSlide.Hide();
        }
        else
        {
            UIBroadcastSlide.Show();
        }
    }

    public void MusicSwitcher(bool val)
    {
        PlayerPrefs.SetInt("MusicIsMute", val ? 1 : 0);
        Debug.Log("MusicSwitcher:" + val);
        AudioManager.Instance.MusicMute = val;
    }

    public void Back2App()
    {
        PlayerPrefs.DeleteKey("lastType");
        Unity2Native.Back2App();
    }

    public void StreetChange()
    {
        StreetType type = MainLogic.Instance.cur_streetType;
        if (type == StreetType.广场)
            return;
        if (type != StreetType.古韵街)
        {
            PlayerPrefs.SetInt("lastType", (int)type);
            streetChange.transform.GetChild(0).gameObject.SetActive(false);
            streetChange.transform.GetChild(1).gameObject.SetActive(true);
            MainLogic.Instance.StartCoroutine(MainLogic.Instance.LoadStreet(StreetType.古韵街));
        }
        else
        {
            streetChange.transform.GetChild(0).gameObject.SetActive(true);
            streetChange.transform.GetChild(1).gameObject.SetActive(false);
            int value = PlayerPrefs.GetInt("lastType");
            type = (StreetType)Enum.ToObject(typeof(StreetType), value);
            MainLogic.Instance.StartCoroutine(MainLogic.Instance.LoadStreet(type));
        }
      
    }
}
