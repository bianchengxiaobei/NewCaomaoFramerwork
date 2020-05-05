using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

public static class CaomaoGUIStyle
{
    private static GUIStyle m_headerStyle;
    private static GUIStyle m_graphBgStyle;
    private static GUIStyleSB GUIStyleSB;
    private static GUIStyle m_nodeHeadStyle;
    private static OdinMenuStyle m_defaultMenuStyle;
    /// <summary>
    /// 头部Style
    /// </summary>
    public static GUIStyle HeaderStyle
    {
        get
        {
            if (m_headerStyle == null)
            {
                m_headerStyle = new GUIStyle(SirenixGUIStyles.SectionHeader);
                m_headerStyle.fontSize = 23;
                m_headerStyle.padding = new RectOffset(20, 20, 0, 0);
                m_headerStyle.alignment = TextAnchor.MiddleLeft;
            }
            return m_headerStyle;
        }
    }

    public static GUIStyle GraphBGStyle
    {
        get
        {
            if (m_graphBgStyle == null)
            {
                //从Ssript里面加载
                if (GUIStyleSB == null)
                {
                    GUIStyleSB = AssetDatabase.LoadAssetAtPath<GUIStyleSB>(CaomaoFrameworkGlobalConfig.Instance.CaomaoGUIStyleSBPath);
                }
                m_graphBgStyle = GUIStyleSB.GraphBGStyle;
            }
            return m_graphBgStyle;
        }
    }

    public static GUIStyle NodeHeadStyle
    {
        get
        {
            if (m_nodeHeadStyle == null)
            {
                //从Ssript里面加载
                if (GUIStyleSB == null)
                {
                    GUIStyleSB = AssetDatabase.LoadAssetAtPath<GUIStyleSB>(CaomaoFrameworkGlobalConfig.Instance.CaomaoGUIStyleSBPath);
                }
                m_nodeHeadStyle = GUIStyleSB.NodeHeadStyle;
            }
            return m_nodeHeadStyle;
        }
    }
    /// <summary>
    /// 左侧菜单的默认style
    /// </summary>
    public static OdinMenuStyle DefaultMenuStyle 
    {
        get 
        {
            if (m_defaultMenuStyle == null) 
            {
                m_defaultMenuStyle = new OdinMenuStyle
                {
                    BorderPadding = 0f,
                    AlignTriangleLeft = true,
                    TriangleSize = 16f,
                    TrianglePadding = 0f,
                    Offset = 20f,
                    Height = 23,
                    IconPadding = 0f,
                    BorderAlpha = 0.323f
                };
            }
            return m_defaultMenuStyle;
        }
    }
}