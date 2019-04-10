using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreetEvent : MonoBehaviour {
    
    public Image bgImg;
    public Text desc;
    public Animation popAnim;
    private ShopData shopData;

    private float width = 0;
    private float speed = 100;
    private bool isLeft = true;
    private Vector3 textPos = Vector3.zero;

    public void InitSlide(ShopData shopData, bool isLeft)
    {
        this.isLeft = isLeft;
        this.shopData = shopData;
        Vector3 scale = isLeft ? Vector3.one : new Vector3(-1, 1, 1);
        width = shopData.event_info.c_title.Length * desc.fontSize + desc.fontSize;
        RectTransform descRedt = desc.GetComponent<RectTransform>();
        descRedt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        descRedt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        desc.text = shopData.event_info.c_title;

        textPos = desc.transform.localPosition;
        if (width > 476)
        {
            textPos.x = -238 * scale.x;
        }
        else
        {
            textPos.x = 0;
        }
        desc.transform.localPosition = textPos;


        bgImg.transform.localScale = scale;
        desc.transform.localScale = scale;
        gameObject.SetActive(false);
    }

    public void HideCompleted()
    {
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        popAnim.Play("streetEvtHide");
    }

    public void Pop()
    {
        gameObject.SetActive(true);
        transform.eulerAngles = Vector3.zero;
        if (isLeft)
        {
            popAnim.Play("streetEvtLeftPop");
        }
        else
        {
            popAnim.Play("streetEvtRightPop");
        }
    }

    public void OnConfirm()
    {
        //if (string.IsNullOrEmpty(UserData.Instance.UserInfo.userId))
        //{
        //    Unity2Native.GoToLogin();
        //    WebApi.SendUserInfo();
        //    return;
        //}
        gameObject.SetActive(false);
        Debug.Log("Accept");
        //WebApi.SendEventResult(this.shopData.event_info.c_id.ToString(), "left");
        if (this.shopData.event_info.c_left_go_type == "INTO_SHOP")
        {
            MainLogic.Instance.SaveSteetData();
            Unity2Native.OpenStorePageByCode(shopData.shop_info.c_ucode, "0");
        }
        //else if (this.shopData.event_info.c_left_go_type == "GET_RED")
        //{
        //    WebApi.CheckRedPacketInfo(shopData.shop_info.c_ucode);
        //}
    }

    private void Update()
    {
        if (width < 476)
        {
            return;
        }

        textPos = desc.transform.localPosition;
        if (isLeft)
        {
            textPos.x -= speed * Time.deltaTime;
            if (textPos.x + width / 2 < -238)
            {
                textPos.x += (width + 476);
            }
        }
        else
        {
            textPos.x += speed * Time.deltaTime;
            if (textPos.x - width / 2 > 238)
            {
                textPos.x -= (width + 476);
            }
        }

        desc.transform.localPosition = textPos;
    }


    private void OnDestroy()
    {
        StreetManager.Instance.StreetEvtObj = null;
    }

}
