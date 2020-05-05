using UnityEngine;
using System.Collections;
using UnityEditor;
using Sirenix.OdinInspector;
using ICSharpCode.NRefactory.Ast;
using System.IO;
/// <summary>
/// 数据转类（excel生成class类）
/// </summary>
public class DataToClassOperator
{
    private ICaomaoHeader Header;

    public DataToClassOperator(ICaomaoHeader header) 
    {
        this.Header = header;
        this.TemplateNoNamespaceFilePath = CaomaoDataExportGobalConfig.Instance.NoNamespaceTemplate;
        this.TemplateWithNamespaceFilePath = CaomaoDataExportGobalConfig.Instance.WithNamespaceTemplate;
        this.ExportFolderPath = CaomaoDataExportGobalConfig.Instance.ExportClassPath;
    }

    [PropertyOrder(-1)]
    [OnInspectorGUI]
    public void DrawHeader()
    {
        if (this.Header == null) 
        {
            return;
        }
        var rect = EditorGUI.IndentedRect(EditorGUILayout.BeginHorizontal());
        this.Header.Draw(rect.width, rect.height, rect.x, rect.y);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10f);
    }

    [InfoBox("选择需要转化的配置文件")]
    [AssetSelector]
    [PropertyOrder(0)]
    public UnityEngine.Object SourceData;//源配置文件

    [PropertyOrder(1)]
    [InfoBox("存放配置文件导出的文件夹，注意:如果自定义自己的文件夹，需要点击[更新文件目录按钮],否则会失效！")]
    [FolderPath(AbsolutePath = true)]
    [LabelText("导出配置文件目录")]
    [InlineButton("UpdateExportFolderPath", "更新导出文件目录")]
    [Space(10f)]
    public string ExportFolderPath;
    [InfoBox("存放本地化Template的文件夹，注意:如果自定义自己的文件夹，需要点击[更新本地化Template文件目录按钮],否则会失效！")]
    [FilePath]
    [LabelText("不带Namespace的Template文件目录")]
    [InlineButton("UpdateNoNamespcaeTemplateFilePath", "更新不带命名空间Template文件目录")]
    [PropertyOrder(2)]
    [Space(10f)]
    public string TemplateNoNamespaceFilePath;
    [InfoBox("存放本地化Template的文件夹，注意:如果自定义自己的文件夹，需要点击[更新本地化Template文件目录按钮],否则会失效！")]
    [FilePath]
    [LabelText("带Namespace的Template文件目录")]
    [InlineButton("UpdateWithNamespaceTemplateFilePath", "更新有命名空间Template文件目录")]
    [PropertyOrder(3)]
    [Space(10f)]
    public string TemplateWithNamespaceFilePath;

    [LabelText("是否使用命名空间")]
    [PropertyOrder(4)]
    [ToggleLeft]
    [Space(20f)]
    public bool useNamespace = true;

    [LabelText("命名空间")]
    [PropertyOrder(5)]
    [ReadOnly]
    [ShowIf("useNamespace")]
    public string nameSpace = "CaomaoFramework";

    [PropertyOrder(6)]
    [Button("导出.cs类", ButtonSizes.Large)]
    public void ExportData()
    {
        if (string.IsNullOrEmpty(this.ExportFolderPath))
        {
            EditorUtility.DisplayDialog("导出出错", "请选择导出文件目录", "确定");
            return;
        }
        var path = AssetDatabase.GetAssetPath(this.SourceData);
        if (path.EndsWith(DataToDataOperator.ExcelExtension1)
            || path.EndsWith(DataToDataOperator.ExcelExtension2))
        {
            //说明是excel文件
            var excel = CaomaoEditorHelper.ReadExcel(path);
            if (excel == null)
            {
                Debug.LogError("No Excel:" + path);
                return;
            }
            var conveter = new ExcelToClassTypeConveter(excel.Tables[0], this.LoadTemplate(),this.nameSpace,this.ExportFolderPath);
            conveter.Parse();
            AssetDatabase.Refresh();
        }
        else
        {
            EditorUtility.DisplayDialog("文件格式不支持", "请选择Excel文件作为配置文件", "确定");
            return;
        }
    }

    private static string noNamespaceTempalteContent;
    private static string withNamespaceTemplateContent;
    public string LoadTemplate() 
    {
        if (useNamespace)
        {
            if (string.IsNullOrEmpty(withNamespaceTemplateContent)) 
            {
                withNamespaceTemplateContent = AssetDatabase.LoadAssetAtPath<TextAsset>(this.TemplateWithNamespaceFilePath).text;
            }         
            return withNamespaceTemplateContent;
        }
        else 
        {
            if (string.IsNullOrEmpty(noNamespaceTempalteContent)) 
            {
                noNamespaceTempalteContent = AssetDatabase.LoadAssetAtPath<TextAsset>(this.TemplateNoNamespaceFilePath).text;
            }           
            return noNamespaceTempalteContent;
        }      
    }

    /// <summary>
    /// 更新导出文件夹
    /// </summary>
    private void UpdateExportFolderPath()
    {
        var temp = CaomaoDataExportGobalConfig.Instance.ExportClassPath;
        if (this.ExportFolderPath != temp)
        {
            CaomaoDataExportGobalConfig.Instance.ExportClassPath = this.ExportFolderPath;
            EditorUtility.SetDirty(CaomaoDataExportGobalConfig.Instance);
        }
    }
    private void UpdateWithNamespaceTemplateFilePath() 
    {
        var temp = CaomaoDataExportGobalConfig.Instance.WithNamespaceTemplate;
        if (this.TemplateWithNamespaceFilePath != temp)
        {
            CaomaoDataExportGobalConfig.Instance.WithNamespaceTemplate = TemplateWithNamespaceFilePath;
            EditorUtility.SetDirty(CaomaoDataExportGobalConfig.Instance);
        }
    }
    private void UpdateNoNamespcaeTemplateFilePath() 
    {
        var temp = CaomaoDataExportGobalConfig.Instance.NoNamespaceTemplate;
        if (this.TemplateNoNamespaceFilePath != temp)
        {
            CaomaoDataExportGobalConfig.Instance.NoNamespaceTemplate = TemplateNoNamespaceFilePath;
            EditorUtility.SetDirty(CaomaoDataExportGobalConfig.Instance);
        }
    }
}
