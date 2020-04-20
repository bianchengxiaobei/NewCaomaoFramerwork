using UnityEngine;
using System;
[CreateAssetMenu(fileName = "MoblieNotifySBConfig", menuName = "CaomaoFramewrok/MoblieNotifySBConfig")]
public class MoblieNotifySBConfig : ScriptableObject
{
    public string ChannelName;
    public string ChannelId;
    public string Description;
    public EImportance Importance;
    public bool CanBypassDnd;
    public bool CanShowBadge;
    public bool EnableLights;
    public bool EnableVibration;
    public ELockScreenVisibility LockScreenVisibility;
}
public enum EImportance 
{
    None = 0,
    Low = 2,
    Default = 3,
    High = 4
}
public enum ELockScreenVisibility 
{
    Private = -1000,
    Secret = -1,
    Public = 1
}
