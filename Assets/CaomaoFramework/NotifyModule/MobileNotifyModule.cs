using UnityEngine;
using System.Collections;
namespace CaomaoFramework 
{
    [Module(false)]
    public class MobileNotifyModule : IMobileNotifyModule
    {
        private IMobileNotifyModule m_oNotifyImp;
        public void Init()
        {
            if (this.m_oNotifyImp == null) 
            {
#if UNITY_ANDROID
                this.m_oNotifyImp = new AndroidMobileNotifyImp();
#elif UNITY_IOS
                this.m_oNotifyImp = new IOSMoblieNotifyImp();
#elif UNITY_EDITOR || UNITY_PLAYER
                this.m_oNotifyImp = new PCNotifyImp();
#endif
            }
            this.m_oNotifyImp.Init();
        }

        public void AddNotifyChannel(string channelId, string channelName, string des) 
        {
            if (this.m_oNotifyImp != null) 
            {
                this.m_oNotifyImp.AddNotifyChannel(channelId, channelName, des);
            }
        }

        public void SendNotify(string title, string content, int min) 
        {
            if (this.m_oNotifyImp != null) 
            {
                this.m_oNotifyImp.SendNotify(title, content, min);
            }
        }

        public void SendNotify(string title, string content, int min, int sec)
        {
            if (this.m_oNotifyImp != null)
            {
                this.m_oNotifyImp.SendNotify(title, content, min,sec);
            }
        }


        public void Update()
        {
            this.m_oNotifyImp.Update();
        }

        bool IMobileNotifyModule.AddNotifyChannel(string channelId, string channelName, string des)
        {
            throw new System.NotImplementedException();
        }

        public bool AddNotifyChannel(MoblieNotifySBConfig config)
        {
            throw new System.NotImplementedException();
        }

        public bool CancelNotify(int notifyId)
        {
            throw new System.NotImplementedException();
        }
    }

}
