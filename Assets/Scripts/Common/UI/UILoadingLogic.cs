using NewEngine.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingLogic : UILogic {

    public GameObject modenStreetLoading;
    public GameObject streetLoading;
    public GameObject vrLoading;
    public GameObject transparentLoading;
    public Slider slider;
    public Button btn_back;

    //public Image loadingIcon;
    public Text progress;
    
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainLogic.Instance.Back2Pre();
            Debug.Log("1");
        }
    }

    public void Back()
    {
        MainLogic.Instance.Back2Pre();
    }

    public void Back2App()
    {
        Unity2Native.Back2App();
    }
}
