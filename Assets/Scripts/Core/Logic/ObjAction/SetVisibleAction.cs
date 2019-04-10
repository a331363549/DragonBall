using NewEngine.Framework.ObjAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewEngine.Logic.ObjAction
{
    public class SetVisibleAction : CObjAction
    {
        private bool visible;
        public SetVisibleAction InitState(bool visible)
        {
            this.visible = visible;
            return this;
        }

        internal override void OnBind()
        {
        }

        internal override void OnEnter()
        {
            if (owner != null)
            {
                owner.SetActive(visible);
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
