using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHomeTrigger : MonoBehaviour {

    [HideInInspector]
    public string targetId;

    public void OnTrigger()
    {
        MainLogic.Instance.SaveSteetData();
        Unity2Native.OpenStorePageByCode(targetId, "0");
    }
}
