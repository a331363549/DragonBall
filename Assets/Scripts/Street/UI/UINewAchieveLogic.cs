using NewEngine.Framework.Service;
using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINewAchieveLogic : UILogic {

    public NetUGUIImage iconImage;
    public Text achieveName;

    //private NewAchieveData achieveData;
    public void InitData(NewAchieveData achieveData)
    {
        //this.achieveData = achieveData;
        achieveName.text = achieveData.c_name;
        iconImage.URL = achieveData.c_has_get_img;
    }


    public void Continue()
    {
        Debug.Log("Accept");
        UIService.Instance.RemoveSlide(this.bindSlide);
    }

    public void GotoAchieve()
    {
        Debug.Log("OnIngore");
        UIService.Instance.RemoveSlide(this.bindSlide);
        Unity2Native.GoToAchieve();
    }
}
