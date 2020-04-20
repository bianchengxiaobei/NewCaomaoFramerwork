using UnityEngine;
using System.Collections;
namespace CaomaoFramework 
{
    public interface IMobileNotifyModule : IModule
    {
        bool AddNotifyChannel(string channelId, string channelName, string des);
        bool AddNotifyChannel(MoblieNotifySBConfig config);
        void SendNotify(string title, string content, int min);
        void SendNotify(string title, string content, int min, int sec);
        bool CancelNotify(int notifyId);
    }
}
