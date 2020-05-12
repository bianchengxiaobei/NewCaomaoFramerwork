#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using Unity.Notifications.Android;
namespace CaomaoFramework
{
    public class AndroidMobileNotifyImp : IMobileNotifyModule
    {
        public string CurChannelId = "";
        public List<int> ProcessNotifyList = new List<int>();//正在处理的通知
        private Action<int> receiveCallback = null;
        private bool m_bInit = false;
        public void Init()
        {
            //加载配置文件，然后加入Channel
            CaomaoDriver.DataModule.GetDataAsync<MobileNotifyData>((asset)=> 
            {
                if (asset != null) 
                {
                    var config = asset.SB as MoblieNotifySBConfig;
                    if (config != null) 
                    {                       
                        if (this.AddNotifyChannel(config)) 
                        {
                            //初始化
                            this.m_bInit = false;
                        }
                    }
                }
            });
        }
        public bool AddNotifyChannel(MoblieNotifySBConfig config)
        {
            if (config == null) 
            {
                return false;
            }
            var channel = new AndroidNotificationChannel()
            {
                Id = config.ChannelId,
                Name = config.ChannelName,
                Importance = (Importance)config.Importance,
                Description = config.Description,
                CanBypassDnd = config.CanBypassDnd,
                CanShowBadge = config.CanShowBadge,
                EnableLights = config.EnableLights,
                EnableVibration = config.EnableVibration,
                LockScreenVisibility = (LockScreenVisibility)config.LockScreenVisibility
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
            this.CurChannelId = config.ChannelId;
            return true;
        }
        public bool AddNotifyChannel(string channelId, string channelName, string des)
        {
            if (string.IsNullOrEmpty(channelId))
            {
                return false;
            }
            var channel = new AndroidNotificationChannel()
            {
                Id = channelId,
                Name = channelName,
                Importance = Importance.High,
                Description = des,
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
            this.CurChannelId = channelId;
            return true;
        }

        public void SendNotify(string title, string content, int min = 1)
        {
            var notification = this.CreateNotify(title, content);
            if (min <= 0)
            {
                notification.FireTime = DateTime.Now.AddSeconds(1);
            }
            else
            {
                notification.FireTime = DateTime.Now.AddMinutes(min);
            }
            var id = AndroidNotificationCenter.SendNotification(notification, this.CurChannelId);
            this.ProcessNotifyList.Add(id);
        }

        public void SendNotify(string title, string content, int min = 1, int sec = 0)
        {
            var notification = this.CreateNotify(title, content);
            if (min <= 0 && sec <= 0) 
            {
                notification.FireTime = DateTime.Now.AddSeconds(0).AddMinutes(1);
            }
            else
            {
                notification.FireTime = DateTime.Now.AddSeconds(sec).AddMinutes(min);
            }           
            var id = AndroidNotificationCenter.SendNotification(notification, this.CurChannelId);
            this.ProcessNotifyList.Add(id);
        }

        public bool CancelNotify(int notifyId)
        {
            var state = AndroidNotificationCenter.CheckScheduledNotificationStatus(notifyId);
            if (state == NotificationStatus.Delivered || state == NotificationStatus.Scheduled)
            {
                AndroidNotificationCenter.CancelNotification(notifyId);
                return true;
            }
            return false;
        }


        public void RegisterReceiveNotify(Action<int> callback)
        {
            AndroidNotificationCenter.OnNotificationReceived += this.GetNotifyHandler;
            this.receiveCallback = callback;
        }

        private void GetNotifyHandler(AndroidNotificationIntentData data)
        {
            if (this.receiveCallback != null)
            {
                this.receiveCallback(data.Id);
            }
            this.ProcessNotifyList.Remove(data.Id);
        }


        private AndroidNotification CreateNotify(string title, string content)
        {
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = content;
            notification.LargeIcon = "ln";
            return notification;
        }

        public void Update()
        {
            
        }

       
    }
}

#endif