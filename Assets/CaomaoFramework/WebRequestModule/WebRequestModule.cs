using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System.Threading.Tasks;

namespace CaomaoFramework
{
    [Module(false)]
    public class WebRequestModule : IWebRequestModule, IModule
    {
        private float m_fProgess = 0;

        public float Progress
        {
            get
            {
                return this.m_fProgess;
            }
        }


        /// <summary>
        /// 下载文本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void DownloadText(string url, Action<string> callback)
        {
            this.m_fProgess = 0;
            CaomaoDriver.Instance.StartCoroutine(CDownloadText(url, callback));
        }
        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void DownloadBytes(string url, Action<byte[]> callback)
        {
            this.m_fProgess = 0;
            CaomaoDriver.Instance.StartCoroutine(CDownloadBytes(url, callback));
        }


        private IEnumerator CDownloadText(string url, Action<string> callback)
        {
            using (UnityWebRequest www = new UnityWebRequest(url))
            {
                this.m_fProgess = www.downloadProgress;
                yield return www.SendWebRequest();
                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(www.error);
                    callback?.Invoke(null);
                    yield break;
                }
                if (www.isDone)
                {
                    var text = www.downloadHandler.text;
                    if (string.IsNullOrEmpty(text))
                    {
                        Debug.LogError("下载文本为空!");
                    }
                    callback?.Invoke(text);
                }
            }
        }

        private IEnumerator CDownloadBytes(string url, Action<byte[]> callback)
        {
            using (UnityWebRequest www = new UnityWebRequest(url))
            {
                this.m_fProgess = www.downloadProgress;
                yield return www.SendWebRequest();
                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(www.error);
                    callback?.Invoke(null);
                    yield break;
                }
                if (www.isDone)
                {
                    var data = www.downloadHandler.data;
                    if (data == null || data.Length == 0)
                    {
                        Debug.LogError("下载数据为空!");
                    }
                    callback?.Invoke(data);
                }
            }
        }

        public async Task<byte[]> LoadLocalBytesNoCallback(string url) 
        {
            try
            {
                using (UnityWebRequest www = UnityWebRequest.Get(url))
                {
                    www.SendWebRequest();
                    while (www.isDone == false) 
                    {
                        await Task.Yield();
                    }
                    return www.downloadHandler.data;
                }
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
                return null;
            }
        }


        public void LoadLocalBytes(string url, Action<byte[]> callback, Action error)
        {
            this.m_fProgess = 0;
            CaomaoDriver.Instance.StartCoroutine(CLoadLocalBytes(url, callback, error));
        }

        public void LoadLocalBytesTest(MonoBehaviour root, string url, Action<byte[]> callback, Action error)
        {
            this.m_fProgess = 0;
            root.StartCoroutine(CLoadLocalBytes(url, callback, error));
        }

        private IEnumerator CLoadLocalBytes(string url, Action<byte[]> callback, Action error)
        {
            //从本地加载dll
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || string.IsNullOrEmpty(www.error) == false)
                {
                    error?.Invoke();
                    yield break;
                }
                else if (www.isDone)
                {
                    callback?.Invoke(www.downloadHandler.data);
                }
            }
        }
        public void Init()
        {
            
        }

        public void Update()
        {
            
        }

        public void LoadLocalText(string url, Action<string> callback, Action error)
        {
            CaomaoDriver.Instance.StartCoroutine(CLoadLocalText(url, callback, error));
        }
        private IEnumerator CLoadLocalText(string url, Action<string> callback, Action error)
        {
            //从本地加载dll
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || string.IsNullOrEmpty(www.error) == false)
                {
                    error?.Invoke();
                    yield break;
                }
                else if (www.isDone)
                {
                    callback?.Invoke(www.downloadHandler.text);
                }
            }
        }
    }
}