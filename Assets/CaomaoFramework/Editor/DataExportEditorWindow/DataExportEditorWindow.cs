using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

public class DataExportEditorWindow : OdinMenuEditorWindow
{
    public static DataExportEditorWindow Window;
    private DataToDataOperator m_dataToDataOperator;
    private DataToClassOperator m_dataToClassOperator;
    private CaomaoHeader Header;
    [MenuItem("CaomaoTools/配置文件转换窗口")]
    private static void OpenWindow() 
    {
        Window = GetWindow<DataExportEditorWindow>();
        Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }


    protected override void Initialize()
    {
        base.Initialize();
        this.Header = new CaomaoHeader("数据配置导出");
        this.m_dataToDataOperator = new DataToDataOperator(this.Header);
        this.m_dataToClassOperator = new DataToClassOperator(this.Header);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.DefaultMenuStyle = CaomaoGUIStyle.DefaultMenuStyle;
        tree.Config.DrawSearchToolbar = true;
        tree.AddObjectAtPath("配置文件转换配置文件",this.m_dataToDataOperator);
        tree.AddObjectAtPath("配置文件转Class", this.m_dataToClassOperator);
        return tree;
    }

}