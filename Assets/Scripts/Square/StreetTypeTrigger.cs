using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetTypeTrigger : MonoBehaviour {

    public StreetType type;

    public void OnTrigger()
    {
        MainLogic.Instance.StartCoroutine(MainLogic.Instance.LoadStreet(type));
        Debug.Log(type.ToString());
    }

    public void Update()
    {
        if (UserCamera.Instance.transform == null)
            return;
        transform.LookAt(UserCamera.Instance.transform);
    }
}

public enum StreetType
{
    蜜绣街 = 0,       //蜜绣街
    蜜味巷 = 1,       //蜜味巷
    蜜品居 = 2,       //蜜品居
    蜜享道 = 3,       //蜜享道
    精品街 = 4,       //精品街
    古韵街 = 5,       //古韵街
    广场 = 6,         //广场
}
