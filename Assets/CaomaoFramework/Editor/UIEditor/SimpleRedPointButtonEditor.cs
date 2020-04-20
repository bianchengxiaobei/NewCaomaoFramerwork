using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditor;
using UnityEngine;
using CaomaoFramework;
using Sirenix.Utilities.Editor;
[CustomEditor(typeof(CUISimpleRedPointButton))]
[CanEditMultipleObjects]
public class SimpleRedPointButtonEditor : ButtonEditor
{
    private SerializedProperty redPoint;
    private SerializedProperty id;
    //private SerializedProperty idLayer;
    private GUIContent idcontent;
    private GUIContent redPointContent;
    private ICaomaoHeader header;
   
    protected override void OnEnable()
    {
        base.OnEnable();
        this.id = serializedObject.FindProperty("ID");
        this.redPoint = serializedObject.FindProperty("RedPointImage");
        //this.idLayer = serializedObject.FindProperty("LayerIds");
        this.idcontent = EditorGUIUtility.TrTextContent("红点树节点ID");
        this.redPointContent = EditorGUIUtility.TrTextContent("红点Image");
        this.header = new CaomaoHeader("普通红点按钮(不带数字)");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //var rect = EditorGUI.IndentedRect( EditorGUILayout.GetControlRect(false));
        var rect = EditorGUI.IndentedRect(EditorGUILayout.BeginHorizontal());
        this.header.Draw(rect.width, rect.height,rect.x,rect.y);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10f);
        
        SirenixEditorGUI.Title("红点属性", "", TextAlignment.Left, true);
        EditorGUILayout.PropertyField(this.id, this.idcontent);

        //EditorGUILayout.PropertyField(this.idLayer);

        EditorGUILayout.PropertyField(this.redPoint, this.redPointContent);
        serializedObject.ApplyModifiedProperties();

        
        EditorGUILayout.Space(20f);
        SirenixEditorGUI.Title("UGUI内置Button属性", "", TextAlignment.Left, true);

        base.OnInspectorGUI();
        
        
    }
}
