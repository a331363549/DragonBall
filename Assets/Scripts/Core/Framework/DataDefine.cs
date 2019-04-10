using NewEngine.Framework.Table;
using System;

namespace NewEngine.Framework
{

    public class Language : ITable
    {
        public string id;
        public string dest;
    }

    [Serializable]
    public class Msg
    {
        public int msgId = -1;
        public int code = -1;
        public string msg = "";
        public string msgType = "";
    }
}
