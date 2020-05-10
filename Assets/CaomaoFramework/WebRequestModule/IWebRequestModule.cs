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
        Task<byte[]> LoadLocalBytesNoCallback(string url);
    }
}
