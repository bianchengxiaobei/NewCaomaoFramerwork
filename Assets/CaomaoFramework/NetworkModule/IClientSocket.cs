using UnityEngine;
using System.Collections.Concurrent;
using System;

namespace CaomaoFramework
{
    public abstract class IClientSocket
    {
        
        protected object locker = new object();
        private SocketState m_oState = SocketState.State_Closed;

        protected volatile int m_nStartPos;
        protected volatile int m_nEndPos;
        protected bool m_bSending;

        private ConcurrentQueue<NetEvent> m_queueEvent = new ConcurrentQueue<NetEvent>();
        private RecycleBufferPipeline m_oBufferPipline;



        public abstract bool Init(uint dwSendBuffSize, uint dwRecvBuffSize, IPacketBreaker oBreaker);
        public abstract bool Connect(string host, int port, int timeout = 4000);
        public SocketState SocketState
        {
            get
            {
                //加锁
                lock (this.locker)
                {
                    return this.m_oState;
                }
            }
            set
            {
                //加锁
                lock (this.locker)
                {
                    this.m_oState = value;
                }
            }
        }

        public bool CanUpdate { get; set; } = true;
        public abstract void Close();
        public abstract void CheckBeginSend();
        public abstract bool Send(byte[] buffer);
        public abstract bool Send(byte[] buffer, int start, int length);
        public abstract void Set();


        public void PushConnectEvent(bool bSuccess)
        {
            var evt = ClassPoolModule<NetEvent>.Alloc();
            evt.m_nEvtType = NetEvtType.Event_Connect;
            evt.m_nErrCode = bSuccess ? NetErrCode.Net_NoError : NetErrCode.Net_SysError;
            m_queueEvent.Enqueue(evt);
            this.CanUpdate = bSuccess;
        }

        public void PushReceiveEvent(int dataLen)
        {
            NetEvent evt = ClassPoolModule<NetEvent>.Alloc();
            evt.m_nEvtType = NetEvtType.Event_Receive;
            evt.m_dataLen = dataLen;
            m_queueEvent.Enqueue(evt);
        }
        /// <summary>
        /// 关闭事件压入队列
        /// </summary>
        /// <param name="nErrCode">关闭错误码</param>
        public void PushClosedEvent(NetErrCode nErrCode)
        {
            NetEvent evt = ClassPoolModule<NetEvent>.Alloc();
            evt.m_nEvtType = NetEvtType.Event_Closed;
            evt.m_nErrCode = nErrCode;
            m_queueEvent.Enqueue(evt);
        }
        /// <summary>
        /// 将数据写入管道
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        public void WriteData(byte[] data, int offset, int len)
        {
            lock (this.m_oBufferPipline)
            {
                this.m_oBufferPipline.Write(data, offset, len);
            }
        }
        /// <summary>
        /// 将数据写入管道
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(byte[] data)
        {
            lock (this.m_oBufferPipline)
            {
                this.m_oBufferPipline.Write(data, 0, data.Length);
            }
        }


        public void Update()
        {
            if (this.CanUpdate == false)
            {
                return;
            }
        }
    }
}
