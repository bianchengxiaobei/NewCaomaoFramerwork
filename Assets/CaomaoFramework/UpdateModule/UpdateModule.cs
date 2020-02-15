using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
namespace CaomaoFramework
{
    /// <summary>
    /// 需要配合打包工具
    /// </summary>
    [Module(true)]
    public class UpdateModule : IUpdateModule, IModule
    {
        private UpdateProgressArgs progressArgs = new UpdateProgressArgs();
        private Action<string> m_actionUpdateFailed;
        private Action m_actionUpdateFinished;
        private Action m_actionOnFirstEnterGame;//第一次进入游戏需要解压stream提示
        private Action m_actionOnStartUpdate;//在开始检测更新的时候更新界面
        private string LocalUpdateVersionFilePath;
        private string m_localVersionContent;
        private UpdateVersionInfo m_oLocalVersionInfo;
        private UpdateVersionInfo m_oRemoteVersionInfo;
        private string LocalRemoteFileTxt;
        private List<string> m_listDownloadRemoteFileName = new List<string>();
        public void Init()
        {
            this.LocalUpdateVersionFilePath = $"{Application.persistentDataPath}/Version.txt";
            this.LocalRemoteFileTxt = $"{Application.persistentDataPath}/RemoteUrl.txt";                 
        }
        public void CheckUpdate()
        {
            //是否有网络
            if (this.HasNetwork())
            {
                this.CheckFirstIntoGame();
                this.m_oLocalVersionInfo = JsonConvert.DeserializeObject<UpdateVersionInfo>(this.m_localVersionContent);
                var url = this.LoadRemoteUrl();
                if (string.IsNullOrEmpty(url) == false)
                {
                    this.DownloadVersion(url, (text) =>
                    {
                        //比对
                        this.m_oRemoteVersionInfo = JsonConvert.DeserializeObject<UpdateVersionInfo>(text);
                        if (this.m_oRemoteVersionInfo.Version != this.m_oLocalVersionInfo.Version)
                        {
                            foreach (var info in this.m_oRemoteVersionInfo.FileMd5)
                            {
                                string localMd5 = "";
                                var fileKey = info.Key;
                                var fileMd5 = info.Value;
                                if (this.m_oLocalVersionInfo.FileMd5.TryGetValue(fileKey, out localMd5))
                                {
                                    if (localMd5 == fileMd5)
                                    {
                                        //不用更新
                                        continue;
                                    }
                                    else
                                    {
                                        this.m_listDownloadRemoteFileName.Add(fileKey);
                                    }
                                }
                                else
                                {
                                    //新增
                                    this.m_listDownloadRemoteFileName.Add(fileKey);
                                }
                            }
                            if (this.m_listDownloadRemoteFileName.Count > 0)
                            {
                                //远程下载，然后解压
                                
                            }
                            else
                            {
                                //不需要更新
                                m_actionUpdateFinished?.Invoke();
                            }
                        }
                        else
                        {
                            //不需要更新
                            m_actionUpdateFinished?.Invoke();
                        }
                    });
                }
            }
            else
            {
                //提示出错
                this.m_actionUpdateFailed?.Invoke
                    (CaomaoDriver.LocalizationModule.GetString(LocalizationConst.UpdateNoNetworkError));
            }
        }
        private bool HasNetwork()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                return true;
            }
            else
            {             
                return false;
            }
        }
        private void CheckFirstIntoGame()
        {
            //看持久化目录是否有version文件，没有的话拷贝stream到持久化
            if (File.Exists(this.LocalUpdateVersionFilePath))
            {
                //读取
                this.m_actionOnStartUpdate?.Invoke();
                this.m_localVersionContent = File.ReadAllText(this.LocalUpdateVersionFilePath);
            }
            else
            {
                //从stream加载zip然后解压到持久化
                //this.m_actionOnFirstEnterGame?.Invoke();
                //if (Directory.Exists(this.ABCacheDir) == false)
                //{
                //    FileAccessModule.DecompressFile(this.StreamZipFile);
                //}
                //this.CheckFirstIntoGame();
                var url = this.LoadRemoteUrl();
                if (string.IsNullOrEmpty(url)  == false)
                {
                    this.DownloadVersion(url, (text) =>
                    {
                        //保存
                        this.m_localVersionContent = text;
                        File.WriteAllText(this.LocalUpdateVersionFilePath, text);
                    });
                }
            }
        }
        private void DownloadVersion(string url,Action<string> callback)
        {
            //从服务器下载，然后保存到这个path
            CaomaoDriver.WebRequestModule.DownloadText(url,callback);
        }
        private string LoadRemoteUrl()
        {
            if (File.Exists(this.LocalRemoteFileTxt))
            {
                var url = File.ReadAllText(this.LocalRemoteFileTxt);
                if (string.IsNullOrEmpty(url))
                {
                    this.m_actionUpdateFailed?.Invoke(CaomaoDriver.LocalizationModule.GetString(LocalizationConst.FileMissError));
                    return null;
                }
                return url;
            }
            else
            {
                this.m_actionUpdateFailed?.Invoke(CaomaoDriver.LocalizationModule.GetString(LocalizationConst.FileMissError));
            }
            return null;
        }
        public void Update()
        {
            
        }
    }
}
