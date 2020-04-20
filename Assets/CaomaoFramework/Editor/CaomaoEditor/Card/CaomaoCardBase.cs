using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
public class CaomaoCardBase
{
    private GUIStyle cardStylePadding;
    private GUIStyle cardStyle;
    public CaomaoCardBase()
    {
        this.cardStylePadding = new GUIStyle();
        this.cardStylePadding.padding = new RectOffset(15, 15, 15, 15);
        this.cardStylePadding.stretchHeight = false;

        this.cardStyle = new GUIStyle("sv_iconselector_labelselection");
        this.cardStyle.padding = new RectOffset(15, 15, 15, 15);
        this.cardStyle.margin = new RectOffset(0, 0, 0, 0);
        this.cardStyle.stretchHeight = false;
    }
    public void Draw(float cardWidth)
    {
        Rect cardBoxRect = EditorGUILayout.BeginVertical(this.cardStylePadding,
                        GUILayoutOptions.Width(cardWidth));
        if (Event.current.type == EventType.Repaint)
        {
            GUIHelper.PushColor(new Color(1f, 1f, 1f, EditorGUIUtility.isProSkin ? 0.25f : 0.45f), false);
            cardStyle.Draw(cardBoxRect, GUIContent.none, 0);
            GUIHelper.PopColor();
        }
        this.DrawCard();
        SirenixEditorGUI.DrawVerticalLineSeperator(cardBoxRect.x,cardBoxRect.y,cardBoxRect.height);
        EditorGUILayout.EndVertical();
    }
    public virtual void DrawCard()
    {

    }
}