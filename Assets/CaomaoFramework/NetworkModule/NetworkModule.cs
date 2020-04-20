using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Net;
namespace CaomaoFramework
{
    public class NetworkModule : IModule, INetworkModule
    {
        private IClientSocket m_oSocket;
        private IPacketBreaker m_oPackageBreaker;
        private IPAddress m_oLocalIPAddress;//本地IP
        private IPAddress m_oTargetIPAddress;//远程目标IP
        private string m_sServerURl;
        private NetworkScriptObjectConfig config;
        private bool m_bTimeout = false;
       
        private bool m_bInit = false;
        private Action m_callbackConncectSuccess;
        private Action m_callbackClose;    

        public SocketState SocketState
        {
            get
            {
                return this.m_oSocket.SocketState;
            }
            set
            {
                this.m_oSocket.SocketState = value;
            }
        }

        public void Init()
        {
            //从配置文件中加载
            if (this.m_bInit == false)
            {
                CaomaoDriver.ResourceModule.LoadAssetAsync("NetworkScriptObjectConfig", (asset) => 
                {
                    this.config = asset as NetworkScriptObjectConfig;
                    if (this.config != null)
                    {
                        this.m_bInit = true;
                    }
                    else
                    {
                        Debug.LogError("NetworkLoadError:NetworkScriptObjectConfig");
                    }
                });
            }
        }

        

        public void ConnectByServerUrl()
        {
            if (string.IsNullOrEmpty(this.m_sServerURl))
            {
                return;
            }
            CaomaoDriver.WebRequestModule.DownloadText(this.m_sServerURl, (text) => 
            {
                if (!string.IsNullOrEmpty(text))
                {
                    //开始解析连接
                    //Gate IP:Port
                    string[] array = text.Split(':');
                    if (array.Length == 2)
                    {
                        this.config.ServerIP = array[0].Trim();//获取IP
                        //Debug.Log("IP:" + this.config.ServerIP);
                        this.config.ServerPort = Convert.ToInt32(array[1]);//获取端口号
                        if (this.Connect() == false)
                        {
                            CaomaoDriver.LocalizationModule.GetString(LocalizationConst.NetworkConnectError);
                            return;
                        }
                    }
                    else
                    {
                        Debug.LogError("URL Content:" + text);
                    }
                }
            });
        }

        public bool Connect()
        {
            return this.Connect(this.config.ServerIP, this.config.ServerPort);
        }

        public bool Connect(string ip,int port)
        {
            this.Close();
            switch (this.config.SocketClientType)
            {
                case ESocketClientType.TCP:
                    //this.m_oSocket = new TCPClientSocket();
                    break;
                case ESocketClientType.KCP:
                    //this.m_oSocket = new KCPClientSocket();
                    break;
                case ESocketClientType.P2PKCP:
                    //this.m_oSocket = new P2PKcpClientSocket();
                    break;
                case ESocketClientType.Bluetooth:
                    throw new Exception("暂时不支持蓝牙通信");
                    break;
            }
            bool result;
            if (!this.m_oSocket.Init(this.config.m_uiSendBuffSize, this.config.m_uiRecvBuffSize, this.m_oPackageBreaker))
            {
                this.m_oSocket = null;
                result = false;
            }
            else
            {
                result = this.m_oSocket.Connect(ip, port);
            }
            return result;
        }

        public void Close()
        {
            if (this.m_oSocket != null)
            {
                this.m_oSocket.Close();
                this.m_oSocket = null;
            }
        }

        public void ConnectTest()
        {
            this.Connect();
        }



        public void Update()
        {
            if (this.m_bInit == false)
            {
                return;
            }
            this.m_oSocket.Update();
        }
    }
}
