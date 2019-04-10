using NewEngine.Framework.Service;
using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMng : MonoBehaviour{

    public static bool IsDay
    {
        get
        {
            int curHours = System.DateTime.Now.TimeOfDay.Hours;
            if (curHours >= 18 || curHours < 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private bool isDay = true;

    public void CheckEnviroment()
    {
        int curHours = System.DateTime.Now.TimeOfDay.Hours;
        Skybox skybox = UserCamera.Instance.GetComponent<Skybox>();
        if (curHours >= 18 || curHours < 6)
        {
            Material mat = GOPool.Instance.LoadRes("Prefabs/Skybox/SkyBox/SkyboxBlueNebula_Material.mat") as Material;
            Material night = new Material(mat);
            night.name = "night";
            skybox.material = night;
            isDay = false;
            QualitySettings.shadows = ShadowQuality.Disable;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.48f, 0.48f, 0.48f);
        }
        else
        {
            Material mat = GOPool.Instance.LoadRes("Prefabs/Skybox/SkyBox/Skybox_Daytime.mat") as Material;
            Material day = new Material(mat);
            day.name = "day";
            skybox.material = day;
            isDay = true;
            QualitySettings.shadows = ShadowQuality.All;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.7f, 0.7f, 0.7f);
        }

        DayLight.Instance.gameObject.SetActive(isDay);
        NightLight.Instance.gameObject.SetActive(!isDay);
        ListenerDriver.DispathMsg<bool>((int)BCListenerId.DayNightSwitch, isDay, true);
    }

    private readonly float Interval = 60;
    private float checkCD = 0;
    public void Update()
    {
        checkCD -= Time.deltaTime;
        if (checkCD < 0)
        {
            CheckEnviroment();
            checkCD += Interval;
        }
    }
}
