using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetStory : MonoBehaviour {
    
    private ShopData shopData;
    //private StreetEvent streetEvt;

    //public void SetData(ShopData data, bool isLeft)
    //{
    //    this.shopData = data;
    //    GameObject streetEvent = GOPool.Instance.PopGO("UIStreetEvent");
    //    if (streetEvent != null)
    //    {
    //        streetEvent.transform.SetParent(transform);
    //        streetEvent.transform.localPosition = Vector3.zero;
    //        streetEvent.transform.rotation = Quaternion.identity;
    //        streetEvent.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
    //        streetEvt = streetEvent.GetComponent<StreetEvent>();
    //        if (streetEvt != null)
    //        {
    //            streetEvt.InitSlide(shopData, isLeft);
    //        }
    //        else
    //        {
    //            GOPool.Instance.PushGO(streetEvent);
    //        }
    //    }
    //}

    public void OnTrigger()
    {
        //UIEventSlide.Show(shopData);
        //enabled = false;
        //gameObject.SetActive(false);
        //shopData.event_info.hasTrigger = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        

    }
}
