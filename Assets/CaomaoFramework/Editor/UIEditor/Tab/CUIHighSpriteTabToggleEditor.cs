using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CaomaoFramework;
[CustomEditor(typeof(CUIHighSpriteTabToggle),false)]
public class CUIHighSpriteTabToggleEditor : CUITabToggleEditor
{
    private SerializedProperty m_normalSprite;
    private SerializedProperty m_highSprite;
    private SerializedProperty m_normalColor;
    private SerializedProperty m_highColor;
    private SerializedProperty m_text;

    private GUIContent m_normalSpriteTip;
    private GUIContent m_highSpriteTip;
    private GUIContent m_normalTextTip;
    private GUIContent m_highTextTip;
    private GUIContent m_textTip;

    protected override void OnEnable()
    {
        this.m_normalSprite = serializedObject.FindProperty("sp_normalSprite");
        this.m_highSprite = serializedObject.FindProperty("sp_highSprite");
        this.m_normalColor = serializedObject.FindProperty("textNormalColor");
        this.m_highColor = serializedObject.FindProperty("textHighColor");
        this.m_text = serializedObject.FindProperty("lb_content");
        this.m_normalSpriteTip = new GUIContent("普通Sprite");
        this.m_highSpriteTip = new GUIContent("高亮Sprite");

        this.m_normalTextTip = new GUIContent("普通的Text颜色");
        this.m_highTextTip = new GUIContent("高亮的Text颜色");

        this.m_textTip = new GUIContent("需要改变颜色的Text");

        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(this.m_normalSprite,this.m_normalSpriteTip);
        EditorGUILayout.PropertyField(this.m_highSprite,this.m_highSpriteTip);
        EditorGUILayout.PropertyField(this.m_normalColor,this.m_normalTextTip);
        EditorGUILayout.PropertyField(this.m_highColor,this.m_highTextTip);
        EditorGUILayout.PropertyField(this.m_text,this.m_textTip);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}

