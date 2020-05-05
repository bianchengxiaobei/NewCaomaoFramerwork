using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Sirenix.OdinInspector;
using System;
using System.IO;
using CaomaoFramework;
using System.Reflection;
/// <summary>
/// 数据转数据（excel=>json,sb等）
/// </summary>
public enum EDataFileType 
{
    None,
    Excel,
    Json,
    ScriptObject,
    BD
}
public class DataToDataOperator
{
    private ICaomaoHeader Header;

    public DataToDataOperator(ICaomaoHeader header) 
    {
        this.Header = header;
        this.ExportFolderPath = CaomaoDataExportGobalConfig.Instance.ExportFolderPath;
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
    [NonSerialized]
    private UnityEngine.Object temp;
    [PropertyOrder(1)]
    [LabelText("检测到配置文件类型")]
    [ShowIf("SourceData")]
    [ReadOnly]  
    public EDataFileType SourceDataFileType;
    [PropertyOrder(2)]
    [OnInspectorGUI]
    public void UpdateType() 
    {
        if (temp == null)
        {
            if (this.SourceData == null)
            {
                return;
            }
            this.temp = this.SourceData;
            this.CheckType();
        }
        else
        {
            if (this.temp != this.SourceData) 
            {
                //检测
                this.CheckType();
                this.temp = this.SourceData;
            }
        }
    }


    [PropertyOrder(3)]
    [InfoBox("存放配置文件导出的文件夹，注意:如果自定义自己的文件夹，需要点击[更新文件目录按钮],否则会失效！")]
    [FolderPath(AbsolutePath =true)]
    [LabelText("导出配置文件目录")]
    [InlineButton("UpdateExportFolderPath", "更新文件目录")]
    public string ExportFolderPath;
    [PropertyOrder(4)]
    [InfoBox("选择需要导出的文件格式（可以多选）")]
    [LabelText("导出Json")]
    [ToggleLeft]
    [DisableIf("SourceDataFileType",EDataFileType.Json)]
    public bool ExportJson;
    [PropertyOrder(5)]
    [LabelText("导出Scriptobject")]
    [ToggleLeft]
    [DisableIf("SourceDataFileType", EDataFileType.ScriptObject)]
    public bool ExportSb;
    [PropertyOrder(6)]
    [LabelText("导出Excel")]
    [ToggleLeft]
    [DisableIf("SourceDataFileType", EDataFileType.Excel)]
    public bool ExportExcel;
    [PropertyOrder(7)]
    [Button("导出配置文件",ButtonSizes.Large)]
    public void ExportData() 
    {
        if (string.IsNullOrEmpty(this.ExportFolderPath))
        {
            EditorUtility.DisplayDialog("导出出错", "请选择导出文件目录", "确定");
            return;
        }
        if (this.ExportJson) 
        {
            this.ExcelToJson();
        }
        if (this.ExportSb) 
        {
            this.ExportScriptObject();
        }
    }



    /// <summary>
    /// 更新导出文件夹
    /// </summary>
    private void UpdateExportFolderPath()
    {
        var temp = CaomaoDataExportGobalConfig.Instance.ExportFolderPath;
        if (this.ExportFolderPath != temp) 
        {
            CaomaoDataExportGobalConfig.Instance.ExportFolderPath = ExportFolderPath;
            EditorUtility.SetDirty(CaomaoDataExportGobalConfig.Instance);
        }
    }
    private void CheckType()
    {
        if (this.SourceData == null)
        {
            this.SourceDataFileType = EDataFileType.None;
        }
        else
        {
            var path = AssetDatabase.GetAssetPath(this.SourceData);
            this.fileName = Path.GetFileNameWithoutExtension(path);
            var extent = Path.GetExtension(path);
            if (extent == ExcelExtension1 || extent == ExcelExtension2)
            {
                this.SourceDataFileType = EDataFileType.Excel;
                this.ExportExcel = false;
            }
            else if (extent == JsonExtension)
            {
                this.SourceDataFileType = EDataFileType.Json;
                this.ExportJson = false;
            }
            else if (extent == SBExtension)
            {
                this.SourceDataFileType = EDataFileType.ScriptObject;
                this.ExportSb = false;
            }
            else 
            {
                this.SourceDataFileType = EDataFileType.None;
            }
        }
    }


    private string fileName = "";
    private Assembly assembly;
    private const string JsonExtension = ".json";
    public const string ExcelExtension1 = ".xlsx";
    public const string ExcelExtension2 = ".xls";
    private const string SBExtension = ".asset";

    private void ExcelToJson() 
    {
        var objs = this.GetExcelDataToClassObject();
        if (objs.Length == 1)
        {
            //只有单个类
            var instance = objs[0];
            try
            {
                var content = JsonUtility.ToJson(instance);
                this.SaveFile(content, this.ExportFolderPath + "/Json", this.fileName, JsonExtension);
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
            }
        }
        else 
        {
            //转成list<T>
            var list = new List<object>();
            list.AddRange(objs);
            try
            {
                var content = JsonUtility.ToJson(list);
                this.SaveFile(content, this.ExportFolderPath + "/Json", this.fileName, JsonExtension);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }  
    }
    [Obsolete("暂时不支持")]
    private void ExportScriptObject() 
    {
        
        //var objs = this.GetExcelDataToClassObject();
        ////创建sb
        //var t = ScriptableObject.CreateInstance<LocalizationData>();
        //t.language = lang;
        //AssetDatabase.CreateAsset(t, $"{this.SBFolderPath}/{sbName}.asset");
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
    }


    private void SaveFile(string content,string filePath,string fileName, string extension)
    {
        if (string.IsNullOrEmpty(content)) 
        {
            Debug.LogError("Content == null");
            return;
        }
        if (Directory.Exists(filePath) == false) 
        {
            Directory.CreateDirectory(filePath);
        }
        var path = $"{filePath}/{fileName}{extension}";
        File.WriteAllText(path, content);
        AssetDatabase.Refresh();   
        //if (!File.Exists(path))
        //{
        //    Debug.LogError("File Not Exit:" + path);
        //    return;
        //}
    }
    /// <summary>
    /// 读取excel文件转成类的实例
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private object[] GetExcelDataToClassObject() 
    {
        if (this.CheckAssembly() == false)
        {
            return null;
        }
        var path = AssetDatabase.GetAssetPath(this.SourceData);
        var excel = CaomaoEditorHelper.ReadExcel(path);
        if (excel == null)
        {
            Debug.LogError("No Excel:" + path);
            return null;
        }
        var convert = new ExcelToClassInstanceConveter(this.assembly);
        convert.Parse(excel.Tables[0]);
        return convert.Instances;
    }
    private bool CheckAssembly()
    {
        if (this.assembly == null)
        {
            this.assembly = Assembly.LoadFrom("./Library/ScriptAssemblies/Assembly-CSharp.dll");
            return this.assembly == null ? false : true;
        }
        return true;
    }
}
