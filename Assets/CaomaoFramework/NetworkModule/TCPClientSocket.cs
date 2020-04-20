//using System;
//using System.Collections.Generic;
//using System.Net.Sockets;
//using System.Net;
//using UnityEngine;
//using System.Threading;
//namespace CaomaoFramework
//{
//    public class TCPClientSocket : IClientSocket
//    {
//        private Socket m_oSocket;
//        private byte[] m_oSendBuff;
//        private byte[] m_oRecvBuff;
//        private int m_nCurrRecvLen;//接收数据长度
        


//        private ManualResetEvent timeoutObject;//看是否连接超时检测
//        private IPacketBreaker m_oBreaker;
        

//        public void CheckBeginSend()
//        {
//            if (this.SocketState == SocketState.State_Connected)
//            {
//                int bufferLen = this.m_oSendBuff.Length;//发送数据缓存的长度
//                int sendLen = this.m_nEndPos + bufferLen - this.m_nStartPos;
//                int realSendLen = sendLen >= bufferLen ? sendLen - bufferLen : sendLen;//实际需要发送的数据长度，主要是判断是否还有数据要发，根据结束指针-开始指针
//                //如果结束比开始小，说明数据溢出了缓存，所以就为num2
//                if (realSendLen == 0)
//                {
//                    this.m_bSending = false;
//                }
//                else
//                {
//                    this.m_bSending = true;
//                    if (this.m_nStartPos + realSendLen >= bufferLen)
//                    {
//                        realSendLen = bufferLen - this.m_nStartPos;//如果数据超出了缓存，截取数据超出部分
//                    }
//                    try
//                    {
//                        this.m_oSocket.BeginSend(this.m_oSendBuff, this.m_nStartPos, realSendLen, SocketFlags.None, this.SendCallBack, this.m_oSocket);
//                    }
//                    catch (Exception ex)
//                    {
//                        Debug.LogError("Exception occured on BeginSend: " + ex.Message);
//                        this.Close();
//                        this.PushClosedEvent(NetErrCode.Net_SysError);
//                    }
//                }
//            }
//        }

//        private void SendCallBack(IAsyncResult ar) 
//        {
//            Socket socket = (Socket)ar.AsyncState;
//            SocketError socketError = SocketError.Success;
//            int sendLen = 0;
//            try
//            {
//                sendLen = socket.EndSend(ar, out socketError);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError("Exception occured on EndSend: " + ex.Message);
//                this.Close();
//                this.PushClosedEvent(NetErrCode.Net_SysError);
//                return;
//            }
//            if (socketError != SocketError.Success)
//            {
//                Debug.LogError("EndSend Error: " + socketError.ToString());
//                this.Close();
//                this.PushClosedEvent(NetErrCode.Net_SysError);
//            }
//            else
//            {
//                this.m_nStartPos = (this.m_nStartPos + sendLen) % this.m_oSendBuff.Length;//下次开始位置指针，也就是start+num（上次发送的长度）
//                this.BeginSendData();
//            }
//        }

//        public override void Close()
//        {
//            if (this.m_oSocket != null)
//            {
//                if (this.SocketState != SocketState.State_Closed)
//                {
//                    this.SocketState = SocketState.State_Closed;
//                    try
//                    {
//                        if (this.m_oSocket.Connected)
//                        {
//                            this.m_oSocket.Shutdown(SocketShutdown.Both);
//                            this.m_oSocket.Close();
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        Debug.LogException(ex);
//                        this.m_oSocket.Close();
//                        this.m_oSocket.Shutdown(SocketShutdown.Both);
//                    }
//                    this.m_oSocket = null;
//                    this.m_oSendBuff = null;
//                    this.m_oRecvBuff = null;
//                }
//            }
//        }

//        public override bool Connect(string host, int port,int timeout = 4000)
//        {
//            if (this.SocketState != SocketState.State_Closed)
//            {
//                //说明已经连接上，就不用连接了
//                return false;
//            }
//            else
//            {
//                try
//                {
//                    this.SocketState = SocketState.State_Connecting;
//                    this.m_oSocket.BeginConnect(host, port, this.ConnectCallback, this.m_oSocket);
//                    //阻塞当前线程，等待4秒之后关闭socket
//                    if (!this.timeoutObject.WaitOne(4000, false))
//                    {
//                        this.PushConnectEvent(false);
//                        this.PushClosedEvent(NetErrCode.Net_OutTime);
//                        this.Close();
//                        Debug.Log("连接超时!!!!");
//                        return false;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    this.Close();
//                    this.timeoutObject.Set();
//                    Debug.LogException(ex);
//                    return false;
//                }
//            }
//            return true;
//        }
//        /// <summary>
//        /// 异步连接回调
//        /// </summary>
//        /// <param name="ar"></param>
//        private void ConnectCallback(IAsyncResult ar)
//        {
//            Socket socket = (Socket)ar.AsyncState;
//            try
//            {
//                socket.EndConnect(ar);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogException(ex);
//                socket.Close();
//                this.PushClosedEvent(NetErrCode.Net_OutTime);
//            }
//            finally
//            {
//                this.timeoutObject.Set();
//                if (socket.Connected)
//                {
//                    Debug.Log("Connect");
//                    this.PushConnectEvent(true);//将连接成功的消息事件压入队列
//                    this.SocketState = SocketState.State_Connected;//多线程设置状态为已经连接状态
//                    this.GetNetwork().m_dateTimeLastSend = DateTime.Now;
//                    this.BeginReceiveData();//开始异步接受数据
//                }
//                else
//                {
//                    Debug.Log("DisConnect");
//                    this.PushConnectEvent(false);
//                    this.SocketState = SocketState.State_Closed;
//                }
//            }          
//        }
//        /// <summary>
//        /// 开始异步接受消息
//        /// </summary>
//        private void BeginReceiveData()
//        {
//            if (this.SocketState == SocketState.State_Connected)
//            {
//                int overflowLen = this.m_oRecvBuff.Length - this.m_nCurrRecvLen;
//                if (overflowLen == 0)
//                {
//                    //数据溢出
//                    this.Close();
//                    this.PushClosedEvent(NetErrCode.Net_RecvBuff_Overflow);
//                }
//                else
//                {
//                    try
//                    {
//                        this.m_oSocket.BeginReceive(this.m_oRecvBuff, this.m_nCurrRecvLen, overflowLen, SocketFlags.None, this.ReceiveCallback, this.m_oSocket);
//                    }
//                    catch (Exception ex)
//                    {
//                        Debug.Log(ex);
//                        this.Close();
//                        this.PushClosedEvent(NetErrCode.Net_SysError);
//                    }
//                }
//            }
//        }
//        /// <summary>
//        /// 接受消息异步回调
//        /// </summary>
//        /// <param name="ar"></param>
//        private void ReceiveCallback(IAsyncResult ar)
//        {
//            Socket socket = (Socket)ar.AsyncState;
//            SocketError socketError = SocketError.Success;
//            int recvLen = 0;
//            try
//            {
//                recvLen = socket.EndReceive(ar, out socketError);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogException(ex);
//                this.Close();
//                this.PushClosedEvent(NetErrCode.Net_SysError);
//                return;
//            }
//            this.OnReceived(recvLen);
//        }

//        /// <summary>
//        /// 处理收到的数据
//        /// </summary>
//        /// <param name="nLen">数据长度</param>
//        private void OnReceived(int nLen)
//        {
//            if (this.SocketState == SocketState.State_Connected)
//            {
//                if (0 == nLen)
//                {
//                    this.Close();
//                    this.PushClosedEvent(NetErrCode.Net_NoError);
//                }
//                else
//                {
//                    this.m_nCurrRecvLen += nLen;
//                    int index = 0;
//                    while (this.m_nCurrRecvLen > 0)
//                    {
//                        int MsgLen = this.m_oBreaker.BreakPacket(this.m_oRecvBuff, index, this.m_nCurrRecvLen);//黏包处理，返回一个完整数据的长度
//                        if (MsgLen == 0)
//                        {
//                            break;
//                        }
//                        this.WriteData(this.m_oRecvBuff, index, MsgLen);
//                        this.PushReceiveEvent(MsgLen);//将接受事件压入队列中
//                        index += MsgLen;
//                        this.m_nCurrRecvLen -= MsgLen;//因为已经写入buffer管道中
//                    }
//                    if (this.m_nCurrRecvLen == this.m_oRecvBuff.Length)//收到数据溢出缓冲buffer
//                    {
//                        this.Close();
//                        this.PushClosedEvent(NetErrCode.Net_RecvBuff_Overflow);
//                    }
//                    else
//                    {
//                        if (index > 0 && this.m_nCurrRecvLen > 0)//如果收到数据不是个完整的数据，那么会将数据暂存到recvBuffer中，以下次读取
//                        {
//                            Array.Copy(this.m_oRecvBuff, index, this.m_oRecvBuff, 0, this.m_nCurrRecvLen);
//                        }
//                        this.BeginReceiveData();
//                    }
//                }
//            }
//        }

    

//        public bool Init(uint dwSendBuffSize, uint dwRecvBuffSize, IPacketBreaker oBreaker)
//        {
            
//        }

//        public bool Send(byte[] buffer)
//        {
            
//        }

//        public bool Send(byte[] buffer, int start, int length)
//        {
            
//        }

//        public void Set()
//        {
            
//        }

//        public override bool Init(uint dwSendBuffSize, uint dwRecvBuffSize, IPacketBreaker oBreaker)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
