using UnityEngine;

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace NewEngine.Framework.Table
{

    public static class TableSaver
    {

        public static bool Save<T>(T[] tableItems, string tablePath, TableFormat fmt)
        {
            if (fmt == TableFormat.TF_txt)
            {
                return SaveTxt<T>(tablePath, tableItems);
            }
            else
            {
                return SaveXml<T>(tablePath, tableItems);
            }
        }


        public static bool SaveXml<T>(string tablePath, T[] tableItems)
        {
            List<T> Root = new List<T>();
            Root.AddRange(tableItems);
            Stream stream = null;
            try
            {
                stream = File.Open(tablePath, FileMode.Create);
                XmlWriterSettings xms = new XmlWriterSettings();
                xms.Indent = true;
                xms.Encoding = System.Text.Encoding.UTF8;
                XmlWriter xmlWriter = XmlWriter.Create(stream, xms);
                XmlSerializer serializer = new XmlSerializer(tableItems.GetType());
                serializer.Serialize(xmlWriter, tableItems);
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("{0} Save Failed : {1}", DateTime.Now.ToString("HH:mm:ss"), tablePath));
                Debug.Log(string.Format("{0} Save Failed : {1}", DateTime.Now.ToString("HH:mm:ss"), e.StackTrace));
                return false;
            }
            finally
            {
                if (null != stream)
                {
                    stream.Close();
                }
            }

            Debug.Log(string.Format("{0} Save Success : {1}", DateTime.Now.ToString("HH:mm:ss"), tablePath));
            return true;
        }

        public static bool SaveTxt<T>(string tablePath, T[] tableItems)
        {
            if (null == tableItems)
            {
                return false;
            }
            //string Path.Combine(Application.dataPath, tableName);
            //Logger.Log(newFileName);
            int index = 0;
            int idx = 0;
            try
            {
                FileStream newFile = new FileStream(tablePath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(newFile, Encoding.Unicode);

                Type typeObj = typeof(T);
                FieldInfo[] fieldArr = typeObj.GetFields();
                string content = string.Empty;
                for (index = 0; index < fieldArr.Length; ++index)
                {
                    if (0 == index)
                    {
                        content = fieldArr[index].Name;
                    }
                    else
                    {
                        content = string.Format("{0}\t{1}", content, fieldArr[index].Name);
                    }
                }
                sw.WriteLine(content);
                sw.WriteLine(content);
                for (idx = 0; idx < tableItems.Length; ++idx)
                {
                    object valObj;
                    content = string.Empty;
                    for (index = 0; index < fieldArr.Length; ++index)
                    {
                        valObj = fieldArr[index].GetValue(tableItems[idx]);
                        if (valObj == null)
                        {
                            if (fieldArr[index].FieldType == typeof(int))
                                valObj = 0;
                            else if (fieldArr[index].FieldType == typeof(byte))
                                valObj = 0;
                            //else if (fieldArr[index].FieldType == typeof(SmartInt))
                            //    valObj = 0;
                            else if (fieldArr[index].FieldType == typeof(float))
                                valObj = 0;
                            else if (fieldArr[index].FieldType == typeof(double))
                                valObj = 0;
                            else if (fieldArr[index].FieldType == typeof(bool))
                                valObj = false;
                            else if (fieldArr[index].FieldType == typeof(Enum))
                                valObj = 0;
                            else
                                valObj = string.Empty;
                        }
                        if (0 == index)
                        {
                            content = valObj.ToString();
                        }
                        else
                        {
                            content = string.Format("{0}\t{1}", content, valObj.ToString());
                        }
                    }
                    if (string.IsNullOrEmpty(content))
                    {
                        continue;
                    }
                    sw.WriteLine(content);
                }
                sw.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return false;
            }
        }



    }
}

