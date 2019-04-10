using NewEngine.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class DebugOption : NewEngine.Utils.Singleton<DebugOption>
{
    public bool BuildRealMode { get; set; }
    public bool LightVisible { get; set; }
    public bool StoreVisible { get; set; }
    public bool FaceVisible { get; set; }
    public bool FaceItemVisible { get; set; }
    public bool FlagVisible { get; set; }
    public bool LanterVisible { get; set; }

    public DebugOption()
    {
        BuildRealMode = true;
        LightVisible = true;
        StoreVisible = true;
        FaceVisible = true;
        FaceItemVisible = true;
        FlagVisible = true;
        LanterVisible = true;
    }
}

public class DebugSetting : MonoBehaviour {

    public Text debugInfo = null;
    public Slider slider;
    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;
    const string display = "{0} FPS";

    public void OpenVR()
    {
        gameObject.SetActive(false);
        Client.Instance.StartCoroutine(vrOn());
    }

    public void OnValChange(float val)
    {
    }

    private IEnumerator vrOn()
    {
        //yield return SceneManager.LoadSceneAsync("empty");
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        XRSettings.LoadDeviceByName(XRSettings.supportedDevices[1]);
        yield return new WaitUntil(() => XRSettings.loadedDeviceName == XRSettings.supportedDevices[1]);
        yield return new WaitForFixedUpdate();
        XRSettings.enabled = true;
        //yield return SceneManager.LoadSceneAsync("lowpolyStreet");
    }

    //private string[] msgArr = new string[] { "恭喜***获得礼包", "恭喜***获得100元优惠券"};
    public void SendBroadcast()
    {
        Debug.Log("RequestUserInfo");
        ContentCfg content = new ContentCfg()
        {
            code = "",
            isRoad = "1"
        };
        Unity2Native.Instance.SendMessage("SetContent", JsonUtility.ToJson(content));
        //NativeAgent.Instance.BroadcastNewMsg(msgArr[Random.Range(0, 2)]);
    }

    public void BodyMode(bool visible)
    {
        DebugOption.Instance.BuildRealMode = visible;
        FreshStreet();
        Debug.Log("BodyMode:" + visible);
    }

    public void LightVisible(bool visible)
    {
        DebugOption.Instance.LightVisible = visible;
        FreshStreet();
        Debug.Log("LightVisible:" + visible);
    }

    public void StoreVisible(bool visible)
    {
        DebugOption.Instance.StoreVisible = visible;
        FreshStreet();
        Debug.Log("StoreVisible:" + visible);
    }

    public void FaceVisible(bool visible)
    {
        DebugOption.Instance.FaceVisible = visible;
        FreshStreet();
        Debug.Log("FaceVisible:" + visible);
    }

    public void FaceItemVisible(bool visible)
    {
        DebugOption.Instance.FaceItemVisible = visible;
        FreshStreet();
        Debug.Log("FaceItemVisible:" + visible);
    }

    public void BuildFlagVisible(bool visible)
    {
        DebugOption.Instance.FlagVisible = visible;
        FreshStreet();
        Debug.Log("BuildFlagVisible:" + visible);
    }

    public void BuildLanternVisible(bool visible)
    {
        DebugOption.Instance.LanterVisible = visible;
        FreshStreet();
        Debug.Log("BuildLanternVisible:" + visible);
    }

    private void FreshStreet()
    {
        //if (StreetView.Instance)
        //{
        //    for (int i = 0; i < StreetView.Instance.mainStreets.Length; i++)
        //    {
        //        StreetView.Instance.mainStreets[i].Fresh(false);
        //    }
        //}
    }

    // Use this for initialization
    void Start ()
    {
        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        for (int i = 0; i < XRSettings.supportedDevices.Length; i++)
        {
            Debug.Log(XRSettings.supportedDevices[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {

        // measure average frames per second
        //m_FpsAccumulator++;
        //if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        //{
        //    m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
        //    m_FpsAccumulator = 0;
        //    m_FpsNextPeriod += fpsMeasurePeriod;
        //}

        //debugInfo.text = "FPS: " + string.Format(display, m_CurrentFps);
        //debugInfo.text += "\nsupportsGyroscope:" + SystemInfo.supportsGyroscope;
        //debugInfo.text += "\ngyroscopeAttitude:" + Input.gyro.attitude;
        //debugInfo.text += "\nOrientation:" + Screen.orientation;
    }
}
