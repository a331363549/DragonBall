using UnityEngine;

namespace NewEngine.Framework.UI
{
    public class WUIRootLogic : UILogic
    {
        public Canvas canvas;
        private readonly Vector3 scale = new Vector3(0.01f, 0.01f, 1f);

        public bool IsUIContainsScreenPoint(Vector3 worldPoint)
        {
            Vector2 screenPoint = worldPoint;
            RectTransform[] array = transform.GetComponentsInChildren<RectTransform>();
            for (int idx = 0; idx < array.Length; ++idx)
            {
                if (array[idx].gameObject != canvas.gameObject && RectTransformUtility.RectangleContainsScreenPoint(array[idx], screenPoint, Camera.main))
                {
                    return true;
                }
            }
            return false;
        }

        private void Start()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            transform.localScale = scale;
        }
    }
}
