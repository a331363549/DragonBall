using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageSlide : MonoBehaviour {
    
    public static void ShowMessage(string message)
    {
        Unity2Native.ShowMessage(message);
    }
}
