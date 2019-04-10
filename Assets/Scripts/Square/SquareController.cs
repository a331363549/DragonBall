using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    private bool moved = false;
    private Vector3 posChanged;
    private Vector3 eulerAngles;
    private float eualerY;

    private void Start()
    {
        WebApi.RequestMusic();
        MainLogic.Instance.GetComponent<AudioSource>().enabled = true;
        gameObject.GetComponent<Renderer>().material.shader = Resources.Load<Shader>("Unlit-InsideVisible");
    }

    // Update is called once per frame
    void Update()
    {
        eulerAngles = UserCamera.Instance.transform.localEulerAngles;
        eulerAngles.y = eualerY;
        UserCamera.Instance.transform.localEulerAngles = eulerAngles;
    }

    private void OnEnable()
    {
        FingerGestures.Instance.TouchBegin += OnTouchBegin;
        FingerGestures.Instance.Touching += OnTouching;
        FingerGestures.Instance.TouchEnd += OnTouchEnd;
    }

    private void OnDisable()
    {
        FingerGestures.Instance.TouchBegin -= OnTouchBegin;
        FingerGestures.Instance.Touching -= OnTouching;
        FingerGestures.Instance.TouchEnd -= OnTouchEnd;
    }


    private const int TriggerLayer = 1 << 11;
    private bool mClickDown = false;
    private float touchStartTime = 0;
    Ray ray;

    private void OnTouchEnd(TouchInfo touchInfo)
    {
        if (UserCamera.Instance == null ||
            mClickDown == false ||
            UserCurState.Instance.EnableMove == false)
        {
            return;
        }
        mClickDown = false;
        ray = UserCamera.Instance.GameCamera.ScreenPointToRay(touchInfo.pos);
        if (Physics.Raycast(ray, out hitInfo, 100f, TriggerLayer))
        {

            StreetTypeTrigger streetType = hitInfo.collider.GetComponent<StreetTypeTrigger>();
            if (streetType != null)
            {
                streetType.OnTrigger();
                return;
            }
            Debug.Log(hitInfo.collider.gameObject.name);
        }
    }

    private float angle;
    private readonly float LimitLine = Mathf.PI / 6 * 2;
    private void OnTouching(TouchInfo touchInfo)
    {
        if (UserCamera.Instance == null ||
            mClickDown == false ||
            UserCurState.Instance.EnableMove == false)
        {
            return;
        }


        //float CameraX = Input.GetAxis("Mouse X");
        //float CameraY = Input.GetAxis("Mouse Y");
        //Vector3 Angle = new Vector3(CameraY, -CameraX, 0);
        //this.transform.eulerAngles -= Angle;

        angle = Mathf.Atan2(Mathf.Abs(touchInfo.totalDelta.y), Mathf.Abs(touchInfo.totalDelta.x));
        if (angle < LimitLine)
        {
            eualerY = eualerY - touchInfo.delta.x * 0.05f;
            return;
        }

        angle = Mathf.Abs(touchInfo.totalDelta.y);
        if (angle > Screen.height * 0.3f || angle < 10)
        {
            return;
        }
    }

    RaycastHit hitInfo;
    private const int UILayer = 1 << 12;
    private void OnTouchBegin(TouchInfo touchInfo)
    {
        if (UserCamera.Instance == null ||
            UserCurState.Instance.EnableMove == false ||
            UIService.Instance.IsUIContainsScreenPoint(touchInfo.pos))
        {
            return;
        }

        mClickDown = true;
        ray = UserCamera.Instance.GameCamera.ScreenPointToRay(touchInfo.pos);
        if (Physics.Raycast(ray, out hitInfo, 100f, UILayer))
        {
            touchStartTime = -1;
        }
        else
        {
            touchStartTime = Time.realtimeSinceStartup;
        }

    }
}
