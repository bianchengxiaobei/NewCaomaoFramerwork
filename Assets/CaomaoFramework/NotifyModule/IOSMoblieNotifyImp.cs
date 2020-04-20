#if UNITY_IOS
using UnityEngine;
using System.Collections;
using Unity.Notifications.iOS;
namespace CaomaoFramework 
{
    public class IOSMoblieNotifyImp : IMobileNotifyModule
    {
        private bool m_bInit = false;
        public void Init()
        {
            CaomaoDriver.Instance.StartCoroutine(this.RequestAuthorization());
        }

        IEnumerator RequestAuthorization()
        {
            using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                };
                this.m_bInit = true;
                //string res = "\n RequestAuthorization: \n";
                //res += "\n finished: " + req.IsFinished;
                //res += "\n granted :  " + req.Granted;
                //res += "\n error:  " + req.Error;
                //res += "\n deviceToken:  " + req.DeviceToken;
                //Debug.Log(res);
            }
        }


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
}

#endif