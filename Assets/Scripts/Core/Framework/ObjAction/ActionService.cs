using NewEngine.Framework.ObjAction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Service
{


    public class ActionService : CService
    {
        private static ActionService sInstance = null;
        internal static ActionService Instance
        {
            get
            {
                return sInstance;
            }
        }

        private List<ActionAgent> agentList = new List<ActionAgent>();

        protected override void OnServiceUpdate()
        {
            for (int idx = 0; idx < agentList.Count; ++idx)
            {
                agentList[idx].Update();
            }
        }

        internal void AddAgent(ActionAgent agent)
        {
            if (agent != null && agentList.Contains(agent) == false)
            {
                agentList.Add(agent);
            }
        }

        internal void RemoveAgent(ActionAgent agent)
        {
            if (agent != null)
            {
                agentList.Remove(agent);
            }
        }

        private void Awake()
        {
            sInstance = this;
        }

        private void OnDestroy()
        {
            sInstance = null;
        }

        private void Update()
        {
#if USE_SERVICE_UPDATE
#else
            OnServiceUpdate();
#endif
        }
    }
}

