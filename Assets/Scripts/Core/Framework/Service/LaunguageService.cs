using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Service
{
    public class LanguageService : CService
    {
        private static Dictionary<string, string> languageDic;
        public static void SetLang(Dictionary<string, string> dic)
        {
            languageDic = dic;
        }

        public static string GetLang(string key)
        {
            string val = key;
            if (languageDic == null || languageDic.TryGetValue(key, out val) == false)
            {
                return key;
            }
            return val;
        }
    }
}

