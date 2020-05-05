using UnityEngine;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using Sirenix.OdinInspector.Demos;
using System.IO;
/// <summary>
/// excel转.cs类文件
/// </summary>
public class ExcelToClassTypeConveter
{
    //字段，类型，可能还有private修饰
    //类名，命名空间
    public string nameSpace;
    public string className;
    public List<CaomaoExcelFieldSimpleData> listFieldData = new List<CaomaoExcelFieldSimpleData>();
    private DataTable excelTable;
    public string template;
    private bool bHasList = false;
    public string exprotPath;

    public ExcelToClassTypeConveter(DataTable table,string template,string nameSpace,string exportPath) 
    {
        this.excelTable = table;
        this.template = template;
        this.nameSpace = nameSpace;
        this.exprotPath = exportPath;
    }

    public void Parse() 
    {
        //解析excel
        this.ParseExcel();
        //读取模板,设置变量
        this.ParseTemplate();
    }
    private void ParseExcel() 
    {
        if (this.excelTable == null) 
        {
            return;
        }
        var rowData = this.excelTable.Rows;
        var col = this.excelTable.Columns.Count;
        this.className = rowData[0][0].ToString();
        for (int i = 0; i < col; i++)
        {
            var data = rowData[1][i];//某一列
            var fieldName = data.ToString();
            var fieldType = "";
            var fieldValue = "";
            if (string.IsNullOrEmpty(fieldName) == false)
            {
                //字段名称
                data = rowData[2][i];//类型
                fieldType = data.ToString();
                //Debug.Log(fieldType);
                if (string.IsNullOrEmpty(fieldType))
                {
                    Debug.LogError("FieldType == null");
                    continue;
                }
                if (fieldType.StartsWith("list")) 
                {
                    this.bHasList = true;
                    if (fieldType == "list<T>")
                    {
                        //特殊处理
                        data = rowData[3][i];//数值
                        fieldValue = data.ToString();
                    }
                }
                
                var simpleData = new CaomaoExcelFieldSimpleData(fieldName, fieldType, fieldValue, this.excelTable.DataSet,this);
                this.listFieldData.Add(simpleData);
            }
        }
        //开始解析
        foreach (var field in this.listFieldData)
        {
            field.Parse();
        }
    }
    private void ParseTemplate() 
    {
        CaomaoScriptGenerateModule.Instance.Restart(this.template);
       
        CaomaoScriptGenerateModule.Instance.AddVariable("className",this.className);
        if (string.IsNullOrEmpty(this.nameSpace) == false) 
        {
            CaomaoScriptGenerateModule.Instance.AddVariable("namespace", this.nameSpace);
        }
        List<object[]> paramsValue = new List<object[]>();
        foreach (var field in this.listFieldData) 
        {
            if (field.BList) 
            {
                continue;
            }
            var objs = new object[]
            {
                field.FieldType,
                field.FieldName             
            };
            paramsValue.Add(objs);
        }
        CaomaoScriptGenerateModule.Instance.AddVariable("variables", paramsValue.ToArray());
        var listValue = new List<object[]>();
        if (this.bHasList)
        {           
            foreach (var field in this.listFieldData)
            {
                if (field.BList)
                {
                    var objs = new object[]
                    {
                        field.FieldType,
                        field.FieldName
                    };
                    listValue.Add(objs);
                }
            }
            CaomaoScriptGenerateModule.Instance.AddVariable("arrayVar", listValue.ToArray());
        }
        else 
        {
            CaomaoScriptGenerateModule.Instance.AddVariable("arrayVar",  listValue.ToArray());
        }
        var content = CaomaoScriptGenerateModule.Instance.Parse();
        var path = $"{this.exprotPath}/{this.className}.cs";
        File.WriteAllText(path, content);
    }
}

