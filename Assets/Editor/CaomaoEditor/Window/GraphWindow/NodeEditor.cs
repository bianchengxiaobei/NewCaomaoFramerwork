using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public abstract partial class Node
{
    public Rect m_pos;
    private GUIContent headherGUIContent;

    public virtual void Init()
    {
        if (this.headherGUIContent == null)
        {
            if (string.IsNullOrEmpty(this.HeaderName) == false)
            {
                //EditorGUIUtility.SetIconSize(Vector2.one * 14);
                var text = $"<color=#{ColorUtility.ToHtmlStringRGB(this.HeaderColor)}>{this.HeaderName}</color>";
                if (this.HeaderNameIcon == null)
                {
                    this.headherGUIContent = new GUIContent(text);
                }
                else
                {
                    this.headherGUIContent = new GUIContent(text, this.HeaderNameIcon);
                }
            }
        }
    }

    public void Draw()
    {
      
       
    }

    public virtual void DrawWindow(float zoom)
    {
        this.m_pos = GUILayout.Window(this.Id,this.m_pos,this.DrawWindowCallback, GUIContent.none);
    }

    private void DrawWindowCallback(int id)
    {
        Debug.Log(id);
    }
    private void DrawHeader()
    {
        if (this.headherGUIContent != null)
        {
            EditorGUIUtility.SetIconSize(Vector2.one * 14f);
            var height = 0;
            //GUILayout.Label()
        }
    }
}

