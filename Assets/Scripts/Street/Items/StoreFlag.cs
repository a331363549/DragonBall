using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreFlag : MonoBehaviour {
    
    public ShopHomeTrigger shopHomeTrigger;
    public Text nameFg;
    public Text nameBg;
    public int length;
    public Vector2 textSize;
    private StoreInfo storeInfo;

    public void SetStoreInfo(StoreInfo storeInfo)
    {
        this.storeInfo = storeInfo;
        string storeName = storeInfo == null ? "" : storeInfo.c_name;
        string newStr = "";
        if (string.IsNullOrEmpty(storeName) == false)
        {
            for (int idx = 0; idx < (length == 0 || length >= storeName.Length ? storeName.Length : length); idx++)
            {
                newStr += storeName[idx].ToString() + "\n";
            }
        }
        nameFg.text = newStr;
        nameBg.text = newStr;
        if (this.storeInfo != null)
        {
            shopHomeTrigger.targetId = this.storeInfo.c_ucode;
            shopHomeTrigger.gameObject.SetActive(true);
        }
        else
        {
            shopHomeTrigger.gameObject.SetActive(false);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
