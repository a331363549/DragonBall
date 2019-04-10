using NewEngine.Framework;
using UnityEngine;


namespace NewEngine
{
    public class NewClient : CServicesManager
    {
        protected override void OnInitialize()
        {
            GameObject[] editorOnly = GameObject.FindGameObjectsWithTag("EditorOnly");
            for (int idx = editorOnly.Length - 1; idx >= 0; --idx)
            {
                editorOnly[idx].SetActive(false);
            }
            
            GameObject.DontDestroyOnLoad(gameObject);
            Debug.Log(string.Format("width:{0},height:{1}", Screen.width, Screen.height));
        }

        protected override void OnRelease()
        {
        }

        /// <summary>
        /// 帧驱动
        /// </summary>
        private void Update()
        {
            /// 驱动游戏
            UpdateService();
        }
    }
}

