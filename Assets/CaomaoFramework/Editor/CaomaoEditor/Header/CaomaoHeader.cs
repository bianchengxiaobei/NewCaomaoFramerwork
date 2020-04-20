using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
public class CaomaoHeader : ICaomaoHeader
{
    public GUIContent logoContent;

    public CaomaoHeader(string title)
    {
        logoContent = new GUIContent($" {title}", CaomaoEditorIcon.LogoTexture);
    }

    public Rect Draw(float width, float height, float x = 0, float y = 0)
    {
        Rect allRect = EditorGUILayout.BeginVertical();
        if (EditorGUIUtility.isProSkin)
        {
            EditorGUI.DrawRect(new Rect(x, allRect.yMax, width, height), SirenixGUIStyles.DarkEditorBackground);
        }
        else
        {
            EditorGUI.DrawRect(new Rect(x, y, width, allRect.yMax), SirenixGUIStyles.BoxBackgroundColor);
        }
        Rect rect = GUILayoutUtility.GetRect(0f, 70f);
        GUI.Label(rect.AlignCenterY(45f), this.logoContent,
            CaomaoGUIStyle.HeaderStyle);
        float versionWidth = SirenixGUIStyles.CenteredGreyMiniLabel.CalcSize(CaomaoGUIContent.VersionContent).x;
        float developWidth = SirenixGUIStyles.CenteredGreyMiniLabel.CalcSize(CaomaoGUIContent.DevelopPlatformContent).x;
        var maxWidth = Mathf.Max(versionWidth, developWidth);
        Rect rightRect = rect.AlignRight(maxWidth + 10f);
        rightRect.x -= 10f;
        rightRect.y += 8f;
        rightRect.height = 17f;
        if (Event.current.type == EventType.Repaint)
        {
            GUI.Label(rightRect, CaomaoGUIContent.VersionContent, SirenixGUIStyles.CenteredGreyMiniLabel);
        }
        rightRect.y += 15f;
        if (Event.current.type == EventType.Repaint)
        {
            GUI.Label(rightRect, CaomaoGUIContent.DevelopPlatformContent, SirenixGUIStyles.CenteredGreyMiniLabel);
        }
        rightRect.y += rightRect.height + 4f;
        if (GUI.Button(rightRect, "版本信息", SirenixGUIStyles.MiniButton))
        {
            Application.OpenURL("https://odininspector.com/patch-notes");
        }
        SirenixEditorGUI.DrawHorizontalLineSeperator(rect.x, rect.y, rect.width, 0.5f);
        SirenixEditorGUI.DrawHorizontalLineSeperator(rect.x, rect.yMax + y, rect.width, 0.5f);
        this.DrawOhterHeader();
        EditorGUILayout.EndHorizontal();
        return rect;
    }
    public virtual void DrawOhterHeader()
    {

    }
}