using NewEngine.Framework.ObjAction;
using System;
using UnityEngine;

namespace NewEngine.Logic.ObjAction
{
    public class AnimatorAction : CObjAction
    {
        private string name;
        private object val;
        public AnimatorAction InitContidition(string name, object val)
        {
            this.name = name;
            this.val = val;
            return this;
        }

        internal override void OnBind()
        {
        }

        internal override void OnEnter()
        {
            if (owner == null)
            {
                Exit();
            }
            Animator animator = owner.GetComponent<Animator>();
            if (val is int)
            {
                animator.SetInteger(name, (int)val);
            }
            else if (val is bool)
            {
                animator.SetBool(name, (bool)val);
            }
            else if (val is float)
            {
                animator.SetFloat(name, (float)val);
            }
            Exit();
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
