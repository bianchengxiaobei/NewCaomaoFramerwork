using CaomaoFramework;
using UnityEngine;

public class PCNotifyImp : IMobileNotifyModule
{
    public bool AddNotifyChannel(string channelId, string channelName, string des)
    {
        return true;
    }

    public bool AddNotifyChannel(MoblieNotifySBConfig config)
    {
        return true;
    }

    public bool CancelNotify(int notifyId)
    {
        return true;
    }

    public void Init()
    {
        
    }

    public void SendNotify(string title, string content, int min)
    {
        
    }

    public void SendNotify(string title, string content, int min, int sec)
    {
        
    }

    public void Update()
    {
        
    }
}