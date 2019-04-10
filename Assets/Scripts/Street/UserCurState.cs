using NewEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCurState : Singleton<UserCurState> {
    
    public bool EnableMove { get; set; }

    private float targetAngle = 0f;
    public float TargetAngle
    {
        get
        {
            return targetAngle;
        }
        set
        {
            targetAngle = value;
            if (targetAngle > 180)
            {
                targetAngle -= 360;
            }
            else if (targetAngle < -180)
            {
                targetAngle += 360;
            }
            if (targetAngle > 30)
            {
                targetAngle = 30;
            }
            else if (targetAngle < -30)
            {
                targetAngle = -30;
            }
        }
    }
    public float moveSpd = 0f;
    public float enableBackDist = 0;
    public float maxBackDist = 0;

    public void UpdateMoveDist(float changedDist, out float moveDist)
    {
        if (changedDist < 0)
        {
            float curMove = changedDist * moveSpd;
            if (enableBackDist < maxBackDist)
            {
                enableBackDist -= curMove;
                if (enableBackDist > maxBackDist)
                {
                    enableBackDist = maxBackDist;
                }
            }
            moveDist = curMove;
        }
        else if (changedDist > 0 && enableBackDist > 0)
        {
            float curMove = changedDist * moveSpd;
            if (curMove < enableBackDist)
            {
                enableBackDist -= curMove;
            }
            else
            {
                curMove = enableBackDist;
                enableBackDist = -0.0001f;
            }
            moveDist = curMove;
        }
        else
        {
            moveDist = 0;
        }
    }
}
