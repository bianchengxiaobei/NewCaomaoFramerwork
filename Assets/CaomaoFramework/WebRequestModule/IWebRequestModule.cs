using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CaomaoFramework
{
    public interface IWebRequestModule
    {
        float Progress { get; }
        void DownloadText(string url, Action<string> callback);
        void DownloadBytes(string url, Action<byte[]> callback);
        void LoadLocalBytes(string url, Action<byte[]> callback, Action error);
        void LoadLocalBytesTest(MonoBehaviour root, string url, Action<byte[]> callback, Action error);
        void LoadLocalText(string url, Action<string> callback, Action error);
        /// <summary>
        /// 加载本地数据无回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        Task<byte[]> LoadLocalBytesNoCallback(string url,Action<string> onError);
        /// <summary>
        /// 下载文本无回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        Task<string> DownloadTextNoCallback(string url, Action<string> onError);
        /// <summary>
        /// 下载数据无回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        Task<byte[]> DownloadBytesNoCallback(string url, Action<string> onError);
        /// <summary>
        /// 加载本地文本无回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>

        Task<string> LoadLocalTextNoCallback(string url, Action<string> onError);

    }
}
