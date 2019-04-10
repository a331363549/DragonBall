using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputProc : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        UserCurState.Instance.TargetAngle = 0;
    }

    private bool moved = false;
    private Vector3 posChanged;
    private Vector3 eulerAngles;
    // Update is called once per frame
    void Update ()
    {
        eulerAngles = UserCamera.Instance.transform.localEulerAngles;
        eulerAngles.y = UserCurState.Instance.TargetAngle;
        UserCamera.Instance.transform.localEulerAngles = eulerAngles;
        if (moved)
        {
            moved = false;
            if (MainLogic.Instance.cur_streetType == StreetType.古韵街)
                StreetManager.Instance.Update(posChanged * 0.5f);
            else
                StreetManager.Instance.Update(posChanged * 1.2f);
        }
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
        if (touchStartTime > 0 &&
            Time.realtimeSinceStartup - touchStartTime < 0.5f &&
            touchInfo.totalDelta.sqrMagnitude < 5)
        {
            ray = UserCamera.Instance.GameCamera.ScreenPointToRay(touchInfo.pos);
            if (Physics.Raycast(ray, out hitInfo, 100f, TriggerLayer))
            {
                StreetStory stroyTrigger = hitInfo.collider.GetComponent<StreetStory>();
                if (stroyTrigger != null)
                {
                    stroyTrigger.OnTrigger();
                    return;
                }

                ShopHomeTrigger homeTrigger = hitInfo.collider.GetComponent<ShopHomeTrigger>();
                if (homeTrigger != null)
                {
                    homeTrigger.OnTrigger();
                    return;
                }

                ShopVRTrigger shopVRTrigger = hitInfo.collider.GetComponent<ShopVRTrigger>();
                if (shopVRTrigger != null)
                {
                    shopVRTrigger.OnTrigger();
                    return;
                }

                StreetTransferTrigger streetTransfer = hitInfo.collider.GetComponent<StreetTransferTrigger>();
                if (streetTransfer != null)
                {
                    streetTransfer.OnTrigger();
                    return;
                }

                ModenTransferTrigger viewTransfer = hitInfo.collider.GetComponent<ModenTransferTrigger>();
                if (viewTransfer != null)
                {
                    viewTransfer.OnTrigger();
                    return;
                }



                Debug.Log(hitInfo.collider.gameObject.name);
            }
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

        angle = Mathf.Atan2(Mathf.Abs(touchInfo.totalDelta.y), Mathf.Abs(touchInfo.totalDelta.x));
        if (angle < LimitLine)
        {
            UserCurState.Instance.TargetAngle = UserCurState.Instance.TargetAngle - touchInfo.delta.x * 0.05f;
            return;
        }

        angle = Mathf.Abs(touchInfo.totalDelta.y);
        if (angle > Screen.height * 0.3f || angle < 10)
        {
            return;
        }

        float curMove = 0;
        UserCurState.Instance.UpdateMoveDist(touchInfo.delta.y, out curMove);
        posChanged = Vector3.back * curMove;
        moved = true;
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
