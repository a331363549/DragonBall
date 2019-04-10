using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace NewEngine.Framework.Stage
{
    public class StageManager : CService
    {
        private static StageManager sInstance = null;
        public static StageManager Instance { get { return sInstance; } }
        
        private IStage curStage = null;
        public IStage CurStage { get { return curStage; } }

        private IStage nextStage = null;
        public IStage NextStage { get { return nextStage; } set { nextStage = value; } }

        public string preStageName = string.Empty;
        public string PreStageName { get { return preStageName; } }

        public string curStageName = string.Empty;
        public string CurStageName { get { return curStageName; } }

        private void Awake()
        {
            sInstance = this;
        }

        public void ToStage(string stage)
        {
            //string typeName = stage.Substring(stage.LastIndexOf('.') + 1);
            //string assemblyName = stage.Substring(0, stage.LastIndexOf('.'));
            Object obj = Activator.CreateInstance(Type.GetType(stage));
            if (obj is IStage)
            {
                ToStage(obj as IStage);
            }
        }

        public void ToStage(IStage stage)
        {
            if (curStage != null)
            {
                curStage.Exit();
                preStageName = curStage.StageName;
            }

            curStage = stage;
            curStageName = string.Empty;

            if (curStage != null)
            {
                curStage.Enter();
                curStageName = curStage.StageName;
            }
        }
        
        protected override void OnServiceUpdate()
        {
            if (curStage != null)
            {
                curStage.Update();
                if (curStage.IsOver)
                {
                    ToStage(nextStage);
                    nextStage = null;
                }
            }
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

