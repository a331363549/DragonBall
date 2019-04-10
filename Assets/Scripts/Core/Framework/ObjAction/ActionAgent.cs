using NewEngine.Framework.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.ObjAction
{
    
    public class ActionAgent
    {
        private GameObject owner = null;
        private IObjAction currentAction = null;
        private IObjAction tailAction = null;
        private bool isStarted = false;

        public void Bind(GameObject go)
        {
            owner = go;
        }

        public void Start()
        {
            if (ActionService.Instance)
            {
                ActionService.Instance.AddAgent(this);
            }
            isStarted = true;
        }

        public void Update()
        {
            if (isStarted && currentAction != null)
            {
                if (currentAction.Update() == false)
                {
                    currentAction.Exit();
                    currentAction = currentAction.Next;
                    if (currentAction == tailAction)
                    {
                        tailAction = null;
                    }
                    if (currentAction != null)
                    {
                        currentAction.Enter();
                    }
                }
            }
        }

        public void Stop()
        {
            isStarted = false;
            if (ActionService.Instance)
            {
                ActionService.Instance.RemoveAgent(this);
            }
        }

        public void Release()
        {
            Stop();
        }

        public void ReplaceAction(IObjAction objAction)
        {
            objAction.Bind(owner);
            if (currentAction != null)
            {
                objAction.Next = currentAction.Next;
                currentAction.Exit();
                currentAction = objAction;
                currentAction.Enter();
            }
            else
            {
                currentAction = objAction;
                currentAction.Enter();
            }
        }

        public void ReplaceNext(IObjAction objAction)
        {
            objAction.Bind(owner);
            if (currentAction == null)
            {
                currentAction = objAction;
                currentAction.Enter();
            }
            else
            {
                currentAction.Next = objAction;
                if (tailAction == null)
                {
                    tailAction = objAction;
                }
            }
        }

        public void InsertNext(IObjAction objAction)
        {
            objAction.Bind(owner);
            if (currentAction == null)
            {
                currentAction = objAction;
                currentAction.Enter();
            }
            else
            {
                objAction.Next = currentAction.Next;
                currentAction.Next = objAction;
                if (tailAction == null)
                {
                    tailAction = objAction;
                }
            }
        }

        public void AddAction(IObjAction objAction)
        {
            objAction.Bind(owner);
            if (currentAction == null)
            {
                currentAction = objAction;
                currentAction.Enter();
            }
            else
            {
                if (tailAction == null)
                {
                    tailAction = objAction;
                    currentAction.Next = tailAction;
                }
                else
                {
                    tailAction.Next = objAction;
                    tailAction = objAction;
                }
            }
        }
    }
}
