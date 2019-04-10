using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;
    private float duration = 0;
    // Update is called once per frame
    void Update () {

        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += fpsMeasurePeriod;
        }
        if (m_CurrentFps < 10 && GlobalSetting.Instance.IsLowResolution == false)
        {
            duration += Time.deltaTime;
            if (duration > 5f)
            {
                GlobalSetting.Instance.SetHalfQuality();
                duration = 0;
            }
        }

        if (m_CurrentFps > 15 && GlobalSetting.Instance.IsLowResolution)
        {
            duration += Time.deltaTime;
            if (duration > 5f)
            {
                GlobalSetting.Instance.SetNormalQuality();
                duration = 0;
            }
        }
    }
}
