using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace NewEngine.Framework.Service
{

    public class WebRequest : CService
    {

        private const string Method_Get = "get";
        private const string Method_Post = "post";
        private const string Method_Put2Java = "put2Java";
        private const string SaltOfJava = "123456";

        public class Request
        {
            public string apiUrl;
            public string[] args;
            public bool processing;
            public string method;
            public bool md5;
        }

        public delegate void OnMsgProcess(string url, string message);

        public static OnMsgProcess onMsgProcess = WebRequest.OnResponse;
        private static List<Request> requestList = new List<Request>();

        public static void Clear()
        {
            requestList.Clear();
        }

        private void Start()
        {
            CServicesManager.Instance.StartCoroutine(WebRequestUpdate());
        }

        public static void Get(string url, params string[] values)
        {
            Request request = requestList.Find((e) => { return string.Compare(url, e.apiUrl) == 0; });
            if (request == null)
            {
                requestList.Add(new Request() { apiUrl = url, args = values, processing = false, method = Method_Get });
            }
            else if (request.processing == false)
            {
                request.args = values;
            }
        }

        public static void Post(string url, params string[] values)
        {
            Request request = requestList.Find((e) => { return string.Compare(url, e.apiUrl) == 0; });
            if (request == null)
            {
                requestList.Add(new Request() { apiUrl = url, args = values, processing = false, method = Method_Post, md5 = false });
            }
            else if (request.processing == false)
            {
                request.args = values;
            }
        }

        public static void Put2JavaWithMd5(string url, params string[] values)
        {
            Request request = requestList.Find((e) => { return string.Compare(url, e.apiUrl) == 0; });
            if (request == null)
            {
                requestList.Add(new Request() { apiUrl = url, args = values, processing = false, method = Method_Put2Java, md5 = true });
            }
            else if (request.processing == false)
            {
                request.args = values;
            }
        }

        private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        private WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        private IEnumerator WebRequestUpdate()
        {
            while (true)
            {
                while (requestList.Count == 0)
                {
                    yield return waitForFixedUpdate;
                }
                Request request = requestList[0];
                requestList.RemoveAt(0);
                request.processing = true;
                Debug.Log("WebPost:" + request.apiUrl);
                string requestArgs = "";
                UnityWebRequest www = CreateWebRequest(request, out requestArgs);
                Debug.Log(request.apiUrl);
                UnityWebRequestAsyncOperation result = www.SendWebRequest();
                yield return result;
                Debug.Log(request.apiUrl);
                if (result.webRequest.isHttpError || result.webRequest.isNetworkError || result.webRequest.downloadHandler == null)
                {
                    Debug.LogError("Code:" + result.webRequest.responseCode + ", Error" + result.webRequest.error);
                    if (onMsgProcess != null)
                    {
                        onMsgProcess(requestArgs, "");
                    }
                }
                else
                {
                    string jsonString = System.Text.Encoding.UTF8.GetString(result.webRequest.downloadHandler.data);
                    string msg = JsonToUnicode(jsonString);
                    Debug.Log(msg);
                    if (onMsgProcess != null)
                    {
                        onMsgProcess(requestArgs, msg);
                    }
                }
                www.Dispose();
                www = null;
                GC.Collect();
                yield return waitForSeconds;
            }
        }

        private UnityWebRequest CreateWebRequest(Request request, out string requestArgs)
        {
            switch (request.method)
            {
                case Method_Get:
                    return CreateGetRequest(request, out requestArgs);
                case Method_Post:
                    return CreatePostRequest(request, out requestArgs);
                case Method_Put2Java:
                    return CreatePut2JavaRequest(request, out requestArgs);
                default:
                    return CreateGetRequest(request, out requestArgs);
            }
        }

        private UnityWebRequest CreateGetRequest(Request request, out string requestArgs)
        {
            requestArgs = request.apiUrl;
            bool isEmptyArgs = requestArgs.Contains("?") == false;
            for (int idx = 0; request.args != null && idx < request.args.Length; idx += 2)
            {
                //Debug.Log(request.args[idx]);
                //Debug.Log(request.args[idx + 1]);
                requestArgs = string.Format("{0}{1}{2}={3}", requestArgs, isEmptyArgs && idx == 0 ? "?" : "&", request.args[idx], request.args[idx + 1]);
            }
            //Debug.Log("WebPost:" + webApiPath + requestArgs);
            return UnityWebRequest.Get(requestArgs);
        }

        private UnityWebRequest CreatePostRequest(Request request, out string requestArgs)
        {
            WWWForm wwwForm = new WWWForm();
            requestArgs = request.apiUrl;
            for (int idx = 0; request.args != null && idx < request.args.Length; idx += 2)
            {
                wwwForm.AddField(request.args[idx], request.args[idx + 1]);
                requestArgs = string.Format("{0}{1}{2}={3}", requestArgs, idx == 0 ? "?" : "&", request.args[idx], request.args[idx + 1]);
            }
            Debug.Log("WebPost:" + requestArgs);
            return UnityWebRequest.Post(request.apiUrl, wwwForm);
        }

        private UnityWebRequest CreatePut2JavaRequest(Request request, out string requestArgs)
        {
            string input = "";
            requestArgs = request.apiUrl;
            for (int idx = 0; request.args != null && idx < request.args.Length; idx += 2)
            {
                if (request.args[idx + 1] is String)
                {
                    input = string.Format("{0},\"{1}\":\"{2}\"", input, request.args[idx], request.args[idx + 1]);
                }
                else
                {
                    input = string.Format("{0},\"{1}\":{2}", input, request.args[idx], request.args[idx + 1]);
                }
                requestArgs = string.Format("{0}{1},{2}={3}", requestArgs, idx == 0 ? "?" : "&", request.args[idx], request.args[idx + 1]);
            }
            input = input.Substring(1);
            input = "{" + input + "}";
            StringBuilder stringBuilder = new StringBuilder(input);
            stringBuilder.Append(SaltOfJava);
            string postData = stringBuilder.ToString();
            string md5 = Md5Sum(postData);
            Debug.Log("WebPost input:" + postData + ",md5:" + md5);
            UnityWebRequest webrquest = UnityWebRequest.Put(request.apiUrl, input);
            webrquest.SetRequestHeader("Content-Type", "application/json");
            webrquest.SetRequestHeader("md5", md5);
            return webrquest;
        }

        private static void OnResponse(string url, string message)
        {
            //Debug.Log(string.Format("[OnWebResponse]{0}->{1}", url, message));           
        }

        private static string Md5Sum(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));//大  "X2",小"x2"  
            }
            
            return sb.ToString();
        }

        private static string JsonToUnicode(string input)
        {
            input = input.Replace("\\\"", "\"");
            input = input.Replace("\\/", "/");
            string result;
            if (!input.Contains("\\u"))
            {
                result = input;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (input.IndexOf("\\u") > 0)
                {
                    stringBuilder.Append(input.Substring(0, input.IndexOf("\\u")));
                    input = input.Substring(input.IndexOf("\\u"));
                }
                if (!string.IsNullOrEmpty(input))
                {
                    string[] array = input.Split(new string[]{"\\u"}, StringSplitOptions.RemoveEmptyEntries);
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text = array2[i];
                        if (text.Length > 4)
                        {
                            string arg = text.Substring(4);
                            stringBuilder.Append((char)int.Parse(text.Substring(0, 4), NumberStyles.HexNumber) + arg);
                        }
                        else
                        {
                            if (text.Length == 4)
                            {
                                stringBuilder.Append((char)int.Parse(text, NumberStyles.HexNumber));
                            }
                            else
                            {
                                if (text.Length < 4 && text.Length > 0)
                                {
                                    stringBuilder.Append(text);
                                }
                            }
                        }
                    }
                }
                result = stringBuilder.ToString();
            }
            return result;
        }
    }
}

