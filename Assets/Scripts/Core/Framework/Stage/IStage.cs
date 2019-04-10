using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewEngine.Framework.Stage
{

    public abstract class IStage
    {
        public virtual string StageName
        {
            get
            {
                return GetType().ToString();
            }
        }

        public abstract bool IsOver { get; }

        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        public void Update()
        {
            OnUpdate();
        }

        protected abstract void OnEnter();

        protected abstract void OnExit();

        protected abstract void OnUpdate();

    }
}
