using NewEngine.Framework.ObjAction;
using UnityEngine;

namespace NewEngine.Logic.ObjAction
{
    public class BlinkAction : CObjAction
    {
        private Vector3 targetPos;

        public BlinkAction InitTargetPosition(Vector3 pos)
        {
            targetPos = pos;
            return this;
        }

        internal override void OnBind()
        {
        }

        internal override void OnEnter()
        {
            if (owner != null)
            {
                targetPos.z = owner.transform.position.z;
                owner.transform.position = targetPos;
            }
        }

        internal override void OnExit()
        {
        }

        internal override bool OnUpdate()
        {
            return false;
        }
    }
}
