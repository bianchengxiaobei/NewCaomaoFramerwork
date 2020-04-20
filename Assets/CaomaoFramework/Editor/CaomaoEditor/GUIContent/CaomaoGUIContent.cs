using UnityEngine;
using UnityEditor;

public static class CaomaoGUIContent
{
    private static GUIContent m_version;
    private static GUIContent m_developPlatform;
    /// <summary>
    /// 版本信息
    /// </summary>
    public static GUIContent VersionContent
    {
        get
        {
            if (m_version == null)
            {
                m_version = new GUIContent($"Caomao Framework {CaomaoFrameworkGlobalConfig.Instance.Version}");
            }
            return m_version;
        }
    }
    /// <summary>
    /// 开发平台
    /// </summary>
    public static GUIContent DevelopPlatformContent
    {
        get
        {
            if (m_developPlatform == null)
            {
                m_developPlatform = new GUIContent($"开发平台:{CaomaoEditorHelper.GetDevelopPlatform().ToString()}");
            }
            return m_developPlatform;
        }
    }
}