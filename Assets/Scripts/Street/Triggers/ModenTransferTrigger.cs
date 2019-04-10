using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModenTransferTrigger : MonoBehaviour
{

    public void OnTrigger()
    {
        UITriggerSlide.Show();
    }

    public void OnDestroy()
    {
        StreetManager.Instance.StreetEvtObj = null;
    }
}
