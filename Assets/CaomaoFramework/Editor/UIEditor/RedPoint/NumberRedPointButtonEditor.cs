using UnityEditor.UI;
using UnityEditor;
using UnityEngine;
using CaomaoFramework;
using Sirenix.Utilities.Editor;
[CustomEditor(typeof(CUINumberRedPointButton))]
[CanEditMultipleObjects]
public class NumberRedPointButtonEditor : ButtonEditor
{
    private SerializedProperty redPoint;
    private SerializedProperty id;
    private SerializedProperty number;
    private GUIContent idcontent;
    private GUIContent redPointContent;
    private GUIContent numberContent;
    private ICaomaoHeader header;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.id = serializedObject.FindProperty("ID");
        this.redPoint = serializedObject.FindProperty("RedPointImage");
        this.number = serializedObject.FindProperty("lb_number");
        this.idcontent = EditorGUIUtility.TrTextContent("红点树节点ID");
        this.redPointContent = EditorGUIUtility.TrTextContent("红点Image");
        this.numberContent = EditorGUIUtility.TrTextContent("数字Text");
        this.header = new CaomaoHeader("带数字红点按钮");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var rect = EditorGUI.IndentedRect(EditorGUILayout.BeginHorizontal());
        this.header.Draw(rect.width, rect.height, rect.x, rect.y);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10f);

        SirenixEditorGUI.Title("红点属性", "", TextAlignment.Left, true);
        EditorGUILayout.PropertyField(this.id, this.idcontent);

        EditorGUILayout.PropertyField(this.number, this.numberContent);

        EditorGUILayout.PropertyField(this.redPoint, this.redPointContent);

        EditorGUILayout.Space(20f);
        SirenixEditorGUI.Title("UGUI内置Button属性", "", TextAlignment.Left, true);

        base.OnInspectorGUI();
    }
}
