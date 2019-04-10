using NewEngine.Framework.ObjAction;
using UnityEngine;

namespace NewEngine.Logic.ObjAction
{
    public class LocalBlinkAction : CObjAction
    {
        private Vector3 targetPos;

        public LocalBlinkAction InitTargetPosition(Vector3 pos)
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
                targetPos.z = owner.transform.localPosition.z;
                owner.transform.localPosition = targetPos;
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
