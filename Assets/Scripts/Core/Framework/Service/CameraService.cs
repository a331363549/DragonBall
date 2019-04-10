using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Service
{
    public class CameraService : CService
    {

        protected override void OnServiceUpdate()
        {
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

