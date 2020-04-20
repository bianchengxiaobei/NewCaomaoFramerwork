
namespace CaomaoFramework
{
    [CInstanceNumber(16)]
    public class NetEvent : IClassInstance
    {
        /// <summary>
        /// Socket事件类型
        /// </summary>
        public NetEvtType m_nEvtType;
        //public byte[] m_oBuffer;
        /// <summary>
        /// Socket错误码
        /// </summary>
        public NetErrCode m_nErrCode;
        public int m_dataLen;
        public void OnAlloc()
        {

        }
        public void OnRelease()
        {
            //this.m_oBuffer = null;
        }
    }
}