using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.ObjAction
{
    public interface IObjAction
    {
        IObjAction Next { get; set; }
        void Bind(GameObject go);
        void Enter();
        void Exit();
        bool Update();
        
    }

    public abstract class CObjAction : IObjAction
    {
        protected GameObject owner = null;
        private bool bActive = true;

        private Action onCompleted;

        public IObjAction Next
        {
            get; set;
        }

        public CObjAction InitOnCompleted(Action onCompleted)
        {
            this.onCompleted = onCompleted;
            return this;
        }

        public void Bind(GameObject go)
        {
            owner = go;
            OnBind();
        }

        internal abstract void OnBind();

        public void Enter()
        {
            bActive = false;
            OnEnter();
        }

        internal abstract void OnEnter();

        public void Exit()
        {
            bActive = true;
            OnExit();
        }

        internal abstract void OnExit();

        public bool Update()
        {
            if (bActive || owner == null)
            {
                return false;
            }
            if (OnUpdate() == false)
            {
                if (onCompleted != null)
                {
                    onCompleted();
                }
                bActive = false;
                return false;
            }
            return true;
        }

        internal abstract bool OnUpdate();
    }

}

