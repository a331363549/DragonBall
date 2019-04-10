
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Service
{
    public class WorldCamera2D : CService
    {
        public static WorldCamera2D sInstance = null;

        protected override void OnServiceUpdate()
        {
            if (worldCamera2D == null)
            {
                return;
            }
            if (worldCamera2D.transform.parent != null)
            {
                worldCamera2D.transform.parent.localScale = scale;
            }
            worldCamera2D.orthographicSize = CameraSize;
            worldCamera2D.aspect = Screen.width * 1f / Screen.height;
        }

        public float CameraSize
        {
            get;set;
        }

        private Camera worldCamera2D = null;
        private readonly Vector3 scale = new Vector3(0.01f, 0.01f, 1f);

        private void Awake()
        {
            sInstance = this;
            CameraSize = 9.6f;
            if (worldCamera2D == null)
            {
                if (Camera.main != null)
                {
                    GameObject.Destroy(Camera.main.gameObject);
                }

                Object obj = Resources.Load("New/Framework/WorldCamera2D");
                GameObject camera2D = obj == null ? null : GameObject.Instantiate(obj) as GameObject;
                if (camera2D == null)
                {
                    camera2D = new GameObject("WorldCamera2D");
                    camera2D.transform.position = Vector3.zero;
                    camera2D.transform.rotation = Quaternion.identity;
                    camera2D.transform.localScale = scale;
                    GameObject cameraObj = new GameObject("Camera");
                    cameraObj.transform.parent = camera2D.transform;
                    cameraObj.transform.localPosition = Vector3.zero;
                    cameraObj.transform.localRotation = Quaternion.identity;
                    cameraObj.transform.localScale = Vector3.one;
                    worldCamera2D = cameraObj.AddComponent<Camera>();
                    camera2D.transform.parent = null;
                    GameObject.DontDestroyOnLoad(camera2D);
                }
                else
                {
                    camera2D.name = "WorldCamera2D";
                    worldCamera2D = camera2D.GetComponentInChildren<Camera>();
                    camera2D.transform.parent = null;
                    GameObject.DontDestroyOnLoad(camera2D);
                }

                worldCamera2D.tag = "MainCamera";
                worldCamera2D.nearClipPlane = -100;
                worldCamera2D.farClipPlane = 100;
                worldCamera2D.orthographic = true;
                worldCamera2D.orthographicSize = Screen.height / 200;
            }
        }

        private void Update()
        {
#if USE_SERVICE_UPDATE
#else
            OnServiceUpdate();
#endif
        }

        private void OnDestroy()
        {
            sInstance = null;
        }
    }
}


