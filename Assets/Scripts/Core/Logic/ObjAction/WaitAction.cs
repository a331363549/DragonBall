using NewEngine.Framework.ObjAction;
using System;
using UnityEngine;

namespace NewEngine.Logic.ObjAction
{
    public class WaitAction : CObjAction
    {
        private float waitingTime;

        public WaitAction InitWaitingTime(float time)
        {
            waitingTime = time;
            return this;
        }

        internal override void OnBind()
        {
        }

        internal override void OnEnter()
        {
        }

        internal override void OnExit()
        {
        }

        internal override bool OnUpdate()
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime <= 0)
            {
                return false;
            }
            return true;
        }
    }
}

