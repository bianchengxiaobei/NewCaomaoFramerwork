
namespace CaomaoFramework
{
    public enum SocketState
    {
        State_Closed,
        State_Connecting,
        State_Connected
    }
    public enum ESocketClientType
    {
        TCP,
        KCP,
        P2PKCP,
        Bluetooth
    }
    /// <summary>
    /// Socket事件
    /// </summary>
    public enum NetEvtType
    {
        Event_Connect,
        Event_Closed,
        Event_Receive
    }
    /// <summary>
    /// Socket错误码
    /// </summary>
    public enum NetErrCode
    {
        Net_NoError,
        Net_SysError,
        Net_RecvBuff_Overflow,
        Net_SendBuff_Overflow,
        Net_Unknown_Exception,
        Net_OutTime
    }
}
