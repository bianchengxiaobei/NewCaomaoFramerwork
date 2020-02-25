using System;

namespace CaomaoFramework
{
    public interface IWebRequestModule
    {
        float Progress { get; }
        void DownloadText(string url, Action<string> callback);
        void DownloadBytes(string url, Action<byte[]> callback);
        void LoadLocalBytes(string url, Action<byte[]> callback, Action error);
        void LoadLocalText(string url, Action<string> callback, Action error);
    }
}
