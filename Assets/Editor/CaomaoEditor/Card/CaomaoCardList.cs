using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.Utilities.Editor;
public class CaomaoCardList<T> where T : CaomaoCardBase
{
    private bool m_bMainCard = false;//是否第一个card会变大
    private string m_sTitle;//cardlist的标题
    private int m_iColNum = 1;
    private GUIContent titleGUIContent = null;
    private GUIStyle titleStyle = null;
    private List<CaomaoCardBase> m_listCards = new List<CaomaoCardBase>();
    private GUIStyle horizontalStyle = null;
    public CaomaoCardList(string title, bool mainCard = false)
    {
        this.m_bMainCard = mainCard;
        if (string.IsNullOrEmpty(title))
        {
            title = "Card List";
            Debug.LogWarning("Card List Title == null");
        }
        this.m_sTitle = title;
        this.titleGUIContent = new GUIContent(this.m_sTitle);  
        this.titleStyle = new GUIStyle(SirenixGUIStyles.SectionHeaderCentered);
        this.titleStyle.fontSize = 17;
        this.titleStyle.margin = new RectOffset(0, 0, 10, 10);
        this.horizontalStyle = new GUIStyle
        {
            padding = new RectOffset(6, 5, 0, 0)
        };

    }
    public void AddCard(CaomaoCardBase card)
    {
        this.m_listCards.Add(card);
        this.m_iColNum = this.m_listCards.Count;
    }
    public void Draw(EditorWindow window)
    {
        //标题
        GUILayout.Label(this.titleGUIContent, this.titleStyle);
        //cards部分
        if (this.m_listCards.Count == 0)
        {
            return;
        }
        var colNum = window.position.width < 470 ? 1 : this.m_iColNum;
        bool flag = false;
        float singleWidth = 0;
        if (this.m_bMainCard)
        {
            singleWidth = window.position.width / (colNum + 1);
        }
        else
        {
            singleWidth = window.position.width / colNum;
        }
        for (int i = 0; i < this.m_listCards.Count; i++)
        {
            if (i % colNum == 0)
            {
                if (i != 0)
                {
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10f);
                }
                Rect rect = EditorGUILayout.BeginHorizontal(this.horizontalStyle);
                flag = true;
            }
            if (this.m_bMainCard && colNum > 1)
            {
                if (i == 0)
                {
                    this.m_listCards[i].Draw(singleWidth * 2);
                }
                else
                {
                    this.m_listCards[i].Draw(singleWidth - 10);
                }
            }
            else
            {
                this.m_listCards[i].Draw(singleWidth - 10f);
            }
            
            if (i % colNum == 0)
            {
                GUILayout.FlexibleSpace();
            }
        }
        if (flag)
        {
            EditorGUILayout.EndHorizontal();
        }
    }

}