using NewEngine.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopVRTrigger : MonoBehaviour {

    public string shopUcode = "";
    public string shopName = "";
    public GameObject vip;
    public GameObject normal;
    public bool isVip
    {
        set
        {
            vip.SetActive(value);
            normal.SetActive(!value);
        }
    }

    public void OnTrigger()
    {
       // UserCamera.Instance.ucode = new GUIContent(shopUcode + '\n' + shopName);
        MainLogic.Instance.StartCoroutine(MainLogic.Instance.Street2Shop(shopUcode, shopName));
    }
}
