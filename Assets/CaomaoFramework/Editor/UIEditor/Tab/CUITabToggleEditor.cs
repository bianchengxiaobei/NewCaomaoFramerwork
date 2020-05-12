using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using CaomaoFramework;

[CustomEditor(typeof(CUITabToggle), false)]
public class CUITabToggleEditor : ToggleEditor
{
    private SerializedProperty m_Index;
    private SerializedProperty m_parent;


    protected override void OnEnable()
    {
        base.OnEnable();
        this.m_Index = serializedObject.FindProperty("m_index");
        this.m_parent = serializedObject.FindProperty("m_parent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(this.m_Index);
        EditorGUILayout.PropertyField(this.m_parent);


        GUILayout.Space(20f);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}

