using UnityEngine;
using CaomaoFramework;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "NetworkScriptObjectConfig", menuName = "CaomaoFramewrok/网络设置配置文件")]
public class NetworkScriptObjectConfig : ScriptableObject
{
    [LabelText("Socket类型")]
    [Tooltip("KCP TCP P2PKCP等类型")]
    public ESocketClientType SocketClientType = ESocketClientType.KCP;
    [LabelText("服务器网关IP")]
    public string ServerIP;
    [LabelText("服务器网关端口")]
    public int ServerPort;
    [LabelText("发送缓冲Buffer大小")]
    public uint m_uiSendBuffSize;
    [LabelText("接收缓冲Buffer大小")]
    public uint m_uiRecvBuffSize;
}