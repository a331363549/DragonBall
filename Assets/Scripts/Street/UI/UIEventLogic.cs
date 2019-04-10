using NewEngine.Framework.Service;
using NewEngine.Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIEventLogic : UILogic {

    public NetUGUIImage eventImg;
    public Text acceptTxt;
    public Text ingoreTxt;
    public Text titleTxt;
    public Text descTxt;

    private ShopData shopData;
    public void InitEvent(ShopData shopData)
    {
        this.shopData = shopData;
        acceptTxt.text = shopData.event_info.c_left_button;
        ingoreTxt.text = shopData.event_info.c_right_button;
        titleTxt.text = shopData.event_info.c_title;
        descTxt.text = shopData.event_info.c_words;
        eventImg.URL = shopData.event_info.c_index_pic;
    }

    public void OnAccept()
    {
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId))
        {
            Debug.Log("Accept 2 login");
            Unity2Native.GoToLogin();
            return;
        }
        Debug.Log("Accept");
        WebApi.SendEventResult(this.shopData.event_info.c_id.ToString(), "left");
        UIService.Instance.RemoveSlide(this.bindSlide);
        if (this.shopData.event_info.c_left_go_type == "INTO_SHOP")
        {
            Unity2Native.OpenStorePageByCode(shopData.shop_info.c_ucode.ToString(), "0");
        }
    }

    public void OnIngore()
    {
        if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId))
        {
            Debug.Log("OnIngore 2 login");
            Unity2Native.GoToLogin();
            return;
        }
        Debug.Log("OnIngore");
        WebApi.SendEventResult(this.shopData.event_info.c_id.ToString(), "right");
        UIService.Instance.RemoveSlide(this.bindSlide);
        if (this.shopData.event_info.c_right_go_type == "INTO_SHOP")
        {
            Unity2Native.OpenStorePageByCode(shopData.shop_info.c_ucode.ToString(), "0");
        }
    }

    public void OnClose()
    {
        Debug.Log("OnClose");
        //WebApi.SendEventResult(this.eventInfo.c_id.ToString(), "right");
        UIService.Instance.RemoveSlide(this.bindSlide);
    }
}
