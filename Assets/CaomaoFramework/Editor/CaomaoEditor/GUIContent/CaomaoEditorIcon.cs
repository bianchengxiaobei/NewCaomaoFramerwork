using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;
public static class CaomaoEditorIcon
{
    private static GUIContent m_saveIconBig;
    private static GUIContent m_saveIconSmall;
    private static GUIContent m_qingIcon;
    private static GUIContent m_yinIcon;

    private static GUIContent m_duoYunIcon;

    private static GUIContent m_xiaoYuIcon;
    private static GUIContent m_zhongYuIcon;
    private static GUIContent m_daYuIcon;
    private static GUIContent m_baoYuIcon;
    private static GUIContent m_daBaoYuIcon;
    private static GUIContent m_teDaBaoYuIcon;

    private static GUIContent m_xiaoXueIcon;
    private static GUIContent m_zhongXueIcon;
    private static GUIContent m_daXueIcon;
    private static GUIContent m_baoXueIcon;

    private static GUIContent m_yuJiaXueIcon;
    private static GUIContent m_leiZhenYuIcon;
    private static GUIContent m_leiZhenYuBingBaoIcon;

    private static Texture2D m_logoTexture;
    public static GUIContent SaveGUIContentBig
    {
        get
        {
            if (m_saveIconBig == null)
            {
                m_saveIconBig = EditorGUIUtility.IconContent("winbtn_win_restore@2x");
            }
            return m_saveIconBig;
        }
    }
    public static GUIContent SaveGUIContentSmall
    {
        get
        {
            if (m_saveIconSmall == null)
            {
                m_saveIconSmall = EditorGUIUtility.IconContent("winbtn_win_restore");
            }
            return m_saveIconSmall;
        }
    }
    //天气
    /// <summary>
    /// 多云
    /// </summary>
    public static GUIContent DuoYunGUIContent
    {
        get
        {
            if (m_duoYunIcon == null || m_duoYunIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/DuoYun.png");
                m_duoYunIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_duoYunIcon;
        }     
    }
    /// <summary>
    /// 小雨
    /// </summary>
    public static GUIContent XiaoYuGUIContent
    {
        get
        {
            if (m_xiaoYuIcon == null || m_xiaoYuIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/XiaoYu.png");
                m_xiaoYuIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_xiaoYuIcon;
        }
    }
    /// <summary>
    /// 中雨
    /// </summary>
    public static GUIContent ZhongYuGUIContent
    {
        get
        {
            if (m_zhongYuIcon == null || m_zhongYuIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/ZhongYu.png");
                m_zhongYuIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_zhongYuIcon;
        }
    }
    /// <summary>
    /// 大雨
    /// </summary>
    public static GUIContent DaYuGUIContent
    {
        get
        {
            if (m_daYuIcon == null || m_daYuIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/DaYu.png");
                m_daYuIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_daYuIcon;
        }
    }
    /// <summary>
    /// 爆雨
    /// </summary>
    public static GUIContent BaoYuGUIContent
    {
        get
        {
            if (m_baoYuIcon == null || m_baoYuIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/BaoYu.png");
                m_baoYuIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_baoYuIcon;
        }
    }
    /// <summary>
    /// 大暴雨
    /// </summary>
    public static GUIContent DaBaoYuGUIContent
    {
        get
        {
            if (m_daBaoYuIcon == null || m_daBaoYuIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/DaBaoYu.png");
                m_daBaoYuIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_daBaoYuIcon;
        }
    }
    /// <summary>
    /// 特大暴雨
    /// </summary>
    public static GUIContent TeDaBaoYuGUIContent
    {
        get
        {
            if (m_teDaBaoYuIcon == null || m_teDaBaoYuIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/TeDaBaoYu.png");
                m_teDaBaoYuIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_teDaBaoYuIcon;
        }
    }
    /// <summary>
    /// 晴天
    /// </summary>
    public static GUIContent QingGUIContent
    {
        get
        {
            if (m_qingIcon == null || m_qingIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/Qing.png");
                m_qingIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_qingIcon;
        }
    }
    /// <summary>
    /// 阴天
    /// </summary>
    public static GUIContent YinGUIContent
    {
        get
        {
            if (m_yinIcon == null || m_yinIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/Yin.png");
                m_yinIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_yinIcon;
        }
    }
    /// <summary>
    /// 小雪
    /// </summary>
    public static GUIContent XiaoXueGUIContent
    {
        get
        {
            if (m_xiaoXueIcon == null || m_xiaoXueIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/XiaoXue.png");
                m_xiaoXueIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_xiaoXueIcon;
        }
    }
    /// <summary>
    /// 中雪
    /// </summary>
    public static GUIContent ZhongXueGUIContent
    {
        get
        {
            if (m_zhongXueIcon == null || m_zhongXueIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/ZhongXue.png");
                m_zhongXueIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_zhongXueIcon;
        }
    }
    /// <summary>
    /// 大雪
    /// </summary>
    public static GUIContent DaXueGUIContent
    {
        get
        {
            if (m_daXueIcon == null || m_daXueIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/DaXue.png");
                m_daXueIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_daXueIcon;
        }
    }
    /// <summary>
    /// 暴雪
    /// </summary>
    public static GUIContent BaoXueGUIContent
    {
        get
        {
            if (m_baoXueIcon == null || m_baoXueIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/BaoXue.png");
                m_baoXueIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_baoXueIcon;
        }
    }
    /// <summary>
    /// 雨夹雪
    /// </summary>
    public static GUIContent YuJiaXueGUIContent
    {
        get
        {
            if (m_yuJiaXueIcon == null || m_yuJiaXueIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/YuJiaXue.png");
                m_yuJiaXueIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_yuJiaXueIcon;
        }
    }
    /// <summary>
    /// 雷阵雨
    /// </summary>
    public static GUIContent LeiZhenYuGUIContent
    {
        get
        {
            if (m_leiZhenYuIcon == null || m_leiZhenYuIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/LeiZhenYu.png");
                m_leiZhenYuIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_leiZhenYuIcon;
        }
    }
    /// <summary>
    /// 雷阵雨冰雹
    /// </summary>
    public static GUIContent LeiZhenYuBingBaoGUIContent
    {
        get
        {
            if (m_leiZhenYuBingBaoIcon == null || m_leiZhenYuBingBaoIcon.image == null)
            {
                var png = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CaomaoFramework/Editor/Icons/LeiZhenYuBingBao.png");
                m_leiZhenYuBingBaoIcon = EditorGUIUtility.TrIconContent(png);
            }
            return m_leiZhenYuBingBaoIcon;
        }
    }
    /// <summary>
    /// logo图标
    /// </summary>
    public static Texture2D LogoTexture
    {
        get
        {
            if (m_logoTexture == null)
            {
                m_logoTexture = AssetPreview.GetMiniThumbnail(AssetDatabase.LoadAssetAtPath("Assets/CaomaoFramework/Editor/Icons/CaomaoLogo.png", typeof(Texture2D)));
            }
            return m_logoTexture;
        }
    }
}