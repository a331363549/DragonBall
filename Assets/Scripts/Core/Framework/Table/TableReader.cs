using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Table
{

    public abstract class ITable
    { }

    public class TableReader<T> where T : ITable
    {
        private static List<T> list = null;
        public static List<T> Context
        {
            get
            {
                if (list == null)
                {
                    list = new List<T>();
                    string typeName = typeof(T).ToString().Replace('.', '/');
                    T[] array = TableParser.Parse<T>(typeName, TableFormat.TF_txt);
                    if (array != null)
                    {
                        list.AddRange(array);
                    }
                }
                return list;
            }
        }
    }

}
