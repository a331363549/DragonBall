using UnityEngine;

public class StreetCtrl : MonoBehaviour {


    private void Start()
    {
        if (MainLogic.Instance.GetComponent<AudioSource>().clip == null)        {
            WebApi.RequestMusic();
            MainLogic.Instance.GetComponent<AudioSource>().enabled = true;
        }
        UserCamera.Instance.GameCamera.fieldOfView = 85;
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        //WebApi.RequestCityViews();
    }

    private const float SCREEN_MSG_REQUEST_CD = 10f;
    private float lastRequestTime = 0;
    private void Update()
    {
        if (NewMsgData.Instance.MsgQueue.Count == 0)
        {
            if (Time.timeSinceLevelLoad - lastRequestTime > SCREEN_MSG_REQUEST_CD)
            {
                WebApi.RequestNewMsg();
                lastRequestTime = Time.timeSinceLevelLoad;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIPageSlide.Show();
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
        if (pause)
        {
            UserCamera.Instance.SaveCamTrans();
            GOPool.Instance.ReleaseCache();
            UIStreetSlide.Close();
            AudioManager.Instance.PauseMusic();
            StreetManager.Instance.ToBack();
        }
        else
        {
            UserCamera.Instance.ResetCamTrans();
            UIStreetSlide.Open();
            AudioManager.Instance.PlayMusic(null);
            StreetManager.Instance.ToFront();
        }
    }
}
