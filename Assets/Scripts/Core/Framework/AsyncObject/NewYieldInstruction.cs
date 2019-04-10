using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Async
{
    public interface NewAsyncProcess
    {
        bool IsDone();

        bool IsSuccess();

        float CurProcess();
    }

    public class NewYieldInstruction : NewAsyncOperation
    {

        private NewAsyncProcess mProcess;

        public override bool IsDone
        {
            get
            {
                return mProcess != null ? mProcess.IsDone() : true;
            }
        }

        public override bool IsSuccess
        {
            get
            {
                return mProcess != null ? mProcess.IsSuccess() : true;
            }
        }

        public override float Progress
        {
            get
            {
                return mProcess != null ? mProcess.CurProcess() : 0f;
            }
        }

        public NewYieldInstruction(NewAsyncProcess process)
        {
            mProcess = process;
        }
    }
}

