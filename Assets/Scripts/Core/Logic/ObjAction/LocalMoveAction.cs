using NewEngine.Framework.ObjAction;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Logic.ObjAction
{
    public class LocalMoveAction : CObjAction
    {

        private List<Vector2> nodeList = new List<Vector2>();
        private float speed = 0.5f;
        private Vector3 curPos = Vector3.zero;
        private Vector3 nextPos = Vector3.zero;
        private float passTime;
        private float needTime;

        public LocalMoveAction Init(List<Vector2> path, float speed)
        {
            nodeList.Clear();
            nodeList.AddRange(path);
            this.speed = speed;
            return this;
        }

        internal override void OnBind()
        {
        }

        internal override void OnEnter()
        {
            if (nodeList.Count == 0)
            {
                return;
            }
            curPos = owner.transform.localPosition;
            nextPos = nodeList[0];
            nextPos.z = owner.transform.localPosition.z;
            nodeList.RemoveAt(0);
            passTime = 0;
            needTime = (nextPos - owner.transform.localPosition).magnitude / speed;
        }

        internal override void OnExit()
        {
        }

        internal override bool OnUpdate()
        {
            if ((passTime += Time.deltaTime) > needTime)
            {
                owner.transform.localPosition = nextPos;
                if (nodeList.Count == 0)
                {
                    return false;
                }
                curPos = nextPos;
                nextPos = nodeList[0];
                nextPos.z = owner.transform.localPosition.z;
                nodeList.RemoveAt(0);
                passTime -= needTime;
                needTime = (nextPos - owner.transform.localPosition).magnitude / speed;
                owner.transform.localPosition = Vector3.Lerp(curPos, nextPos, passTime / needTime);
            }
            else
            {
                owner.transform.localPosition = Vector3.Lerp(curPos, nextPos, passTime / needTime);
            }

            return true;
        }

        private void MoveToNext()
        {
            if ((passTime += Time.deltaTime) > needTime)
            {
                owner.transform.localPosition = nextPos;
                curPos = nextPos;
                nextPos = nodeList[0];
                nextPos.z = owner.transform.localPosition.z;
                nodeList.RemoveAt(0);
                passTime -= needTime;
                needTime = (nextPos - owner.transform.localPosition).magnitude / speed;
                owner.transform.localPosition = Vector3.Lerp(curPos, nextPos, passTime / needTime);
            }
            else
            {
                owner.transform.localPosition = Vector3.Lerp(curPos, nextPos, passTime / needTime);
            }
        }
    }
}
