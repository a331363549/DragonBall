using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetTransferTrigger : MonoBehaviour {

    public void OnTrigger()
    {
        MainLogic.Instance.StartCoroutine(MainLogic.Instance.Street2Sight());
    }

    public void OnDestroy()
    {
        StreetManager.Instance.StreetEvtObj = null;
    }
}
