using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework
{

    public class CService : MonoBehaviour
    {
        public void UpdateService()
        {
#if USE_SERVICE_UPDATE
            OnServiceUpdate();
#else
#endif
        }


        protected virtual void OnServiceUpdate()
        {

        }
    }

    public abstract class CServicesManager : MonoBehaviour
    {
        private static CServicesManager sInstance = null;
        public static CServicesManager Instance { get { return sInstance; } }

        private List<CService> servicesList = new List<CService>();
        private bool initialize = false;

        public T RegisterService<T>() where T : CService
        {
            if (initialize == false)
            {
                Debug.LogError("CServicesManager is NULL");
                return null;
            }
            T service = gameObject.GetComponent<T>();
            if (service == null)
            {
                service = gameObject.AddComponent<T>();
                servicesList.Add(service);
            }
            return service;
        }

        public void UnregisterService<T>() where T : CService
        {
            if (initialize == false)
            {
                Debug.LogError("CServicesManager is NULL");
                return;
            }
            T service = gameObject.GetComponent<T>();
            if (service == null)
            {
                return;
            }
            servicesList.Remove(service);
            GameObject.Destroy(service);
        }

        public T FindService<T>() where T : CService
        {
            if (initialize == false)
            {
                Debug.LogError("CServicesManager is NULL");
                return null;
            }
            return gameObject.GetComponent<T>();
        }


        public void UpdateService()
        {
            if (initialize == false)
            {
                Debug.LogError("CServicesManager is NULL");
                return;
            }
#if USE_SERVICE_UPDATE
            for (int idx = 0; idx < servicesList.Count; ++idx)
            {
                servicesList[idx].UpdateService();
            }
#else
#endif
        }

        protected abstract void OnInitialize();

        protected abstract void OnRelease();

        private void Awake()
        {
            initialize = true;
            sInstance = this;
            OnInitialize();
        }

        private void OnDestroy()
        {
            initialize = false;
            OnRelease();
        }
    }
}
