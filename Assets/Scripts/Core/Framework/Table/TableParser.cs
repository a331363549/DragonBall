using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace NewEngine.Framework.Table
{

    public enum TableFormat
    {
        TF_txt,
        TF_xml,
    }

    public static class TableParser
    {

        public static T[] Parse<T>(string name, TableFormat format)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(name);
            if (textAsset == null)
            {
                Debug.Log("无法加载表格文件：" + name);
                return null;
            }

            T[] dataArr = null;
            if (format == TableFormat.TF_txt)
            {
                dataArr = ParseTxt<T>(textAsset.text);
            }
            else
            {
                dataArr = ParseXml<T>(textAsset.text);
            }

            if (null == dataArr)
            {
                Debug.Log("表格文件解析错误:" + name);
            }
            return dataArr;
        }


        #region txt
        public static T[] ParseTxt<T>(string content)
        {
            if (null == content || string.IsNullOrEmpty(content))
            {
                Debug.Log("表格文件内容为空");
                return null;
            }

            // try parse the table lines.
            // "\r\n" -> "\r", split '\r'
            // 支持excel导出的带分行的文本
            string[] lines = (content.Replace("\r\n", "\r")).Split("\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int lineLen = lines.Length;
            if (lineLen < 3)
            {
                Debug.Log("表格文件行数错误，Line:" + lineLen + "，content:" + content);
                return null;
            }

            // fetch all of the field infos.
            Dictionary<int, FieldInfo> propertyInfos = GetPropertyInfos<T>(lines[1]);

            // parse it one by one.
            int dataLineLen = lineLen - 2;
            T[] array = new T[dataLineLen];
            for (int i = 0; i < dataLineLen; i++)
            {
                if (string.IsNullOrEmpty(lines[i + 2]))
                {
                    continue;
                }
                array[i] = ParseObject<T>(lines[i + 2], i + 2, propertyInfos);
            }

            return array;
        }

        /// <summary>
        /// 将T在memberLine中fieldInfo存入字典中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberLine"></param>
        /// <returns></returns>
        private static Dictionary<int, FieldInfo> GetPropertyInfos<T>(string memberLine)
        {
            Type objType = typeof(T);

            string[] members = memberLine.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, FieldInfo> propertyInfos = new Dictionary<int, FieldInfo>();
            for (int i = 0; i < members.Length; i++)
            {
                FieldInfo fieldInfo = objType.GetField(members[i]);
                if (fieldInfo == null)
                    continue;
                propertyInfos[i] = fieldInfo;
            }

            return propertyInfos;
        }

        /// <summary>
        /// 将data数据分割，根据propertyInfos赋值给obj，返回该T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">一行数据,以table键分割</param>
        /// <param name="idx"></param>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        private static T ParseObject<T>(string data, int idx, Dictionary<int, FieldInfo> propertyInfos)
        {
            T obj = Activator.CreateInstance<T>();
            string[] values = data.Split('\t');
            foreach (KeyValuePair<int, FieldInfo> pair in propertyInfos)
            {
                if (pair.Key >= values.Length)
                    continue;

                string value = values[pair.Key];
                if (string.IsNullOrEmpty(value))
                    continue;

                try
                {
                    ParsePropertyValue(obj, pair.Value, value);
                }
                catch (Exception ex)
                {
                    Debug.LogError(string.Format("ParseError: Row={0} Column={1} Name={2} Want={3} Get={4}",
                        idx + 1,
                        pair.Key + 1,
                        pair.Value.Name,
                        pair.Value.FieldType.Name,
                        value));
                    throw ex;
                }
            }
            return obj;
        }


        #endregion


        #region xml
        public static T[] ParseXml<T>(string content)
        {
            T[] array = new T[0];
            if (null == content || string.IsNullOrEmpty(content))
            {
                return array;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T[]));
                array = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(content))) as T[];
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(content);
            //XmlNode rootNode = xmlDoc.SelectSingleNode(string.Format("ArrayOf{0}", typeof(T).ToString()));
            //rootNode = rootNode != null ? rootNode.SelectSingleNode(typeof(T).ToString()) : null;
            //if (rootNode == null)
            //{
            //    return null;
            //}

            //Type type = typeof(T);
            //FieldInfo[] fields = type.GetFields();
            //XmlNodeList infoNodeList = rootNode.ChildNodes;
            //array = new T[infoNodeList.Count];
            //for (int i = 0; i < infoNodeList.Count; i++)
            //{
            //    array[i] = ParseXmlNode<T>(infoNodeList[i] as XmlElement, fields);
            //}

            return array;
        }

        private static T ParseXmlNode<T>(XmlElement content, FieldInfo[] fields)
        {
            if (null == content)
            {
                return default(T);
            }

            T objData = Activator.CreateInstance<T>();

            foreach (FieldInfo fieldInfo in fields)
            {
                XmlAttribute xmlAttrib = content.GetAttributeNode(fieldInfo.Name);
                if (xmlAttrib != null)
                {
                    string val = xmlAttrib.InnerText;
                    ParsePropertyValue<T>(objData, fieldInfo, val);
                }

                XmlNode xmlNode = content.SelectSingleNode(fieldInfo.Name);
                if (xmlNode != null)
                {
                    string val = xmlNode.InnerText;
                    ParsePropertyValue<T>(objData, fieldInfo, val);
                }
            }

            return objData;
        }

        #endregion


        /// <summary>
        /// 将valueStr转换为对应type的值赋值给obj的fieldInfo上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="valueStr"></param>
        private static void ParsePropertyValue<T>(T obj, FieldInfo fieldInfo, string valueStr)
        {

            System.Object value = valueStr;
            if (fieldInfo.FieldType.IsEnum)
                value = Enum.Parse(fieldInfo.FieldType, valueStr);
            else
            {
                if (fieldInfo.FieldType == typeof(int))
                    value = string.IsNullOrEmpty(valueStr) ? 0 : int.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(uint))
                    value = string.IsNullOrEmpty(valueStr) ? 0 : uint.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(byte))
                    value = string.IsNullOrEmpty(valueStr) ? (byte)0 : byte.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(float))
                    value = string.IsNullOrEmpty(valueStr) ? 0 : float.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(double))
                    value = string.IsNullOrEmpty(valueStr) ? 0 : double.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(bool))
                    value = string.IsNullOrEmpty(valueStr) ? false : bool.Parse(valueStr);
                //else if (fieldInfo.FieldType == typeof(SmartInt))
                //    value = new SmartInt(string.IsNullOrEmpty(valueStr) ? 0 : int.Parse(valueStr));
                else
                {
                    if (valueStr.Contains("\"\""))
                        valueStr = valueStr.Replace("\"\"", "\"");

                    // process the excel string.
                    if (valueStr.Length > 2 && valueStr[0] == '\"' && valueStr[valueStr.Length - 1] == '\"')
                        valueStr = valueStr.Substring(1, valueStr.Length - 2);

                    value = valueStr;
                }
            }

            if (value == null)
                return;

            fieldInfo.SetValue(obj, value);
        }
    }
}

