using UnityEngine;
using System.Collections;
using System.Data;
using TMPro;
/// <summary>
/// 只存字段名，字段类型
/// </summary>
public class CaomaoExcelFieldSimpleData
{
    private string fieldName;
    private string fieldType;
    private string fieldValue;
    private DataSet excel;
    private string realFieldType;
    private ExcelToClassTypeConveter conveter = null;
    public CaomaoExcelFieldSimpleData(string fn, string ft,string fv,DataSet excel,ExcelToClassTypeConveter con)
    {
        this.fieldName = fn;
        this.fieldType = ft;
        this.fieldValue = fv;
        this.excel = excel;
        this.conveter = con;
    }

    public void Parse()
    {
        this.realFieldType = this.GetTypeFromString(this.fieldType);
        if (this.fieldType.StartsWith("list")) 
        {
            this.BList = true;
            //需要转化fieldType
            if (this.fieldType == "list<T>")
            {
                //特殊化需要再次生成一个类
                if (this.excel.HasTable(this.fieldValue, out var table))
                {
                    var className = table.Rows[0][0].ToString();
                    this.realFieldType = $"List<{className}>";
                    var temp = new ExcelToClassTypeConveter(table, conveter.template, conveter.nameSpace, conveter.exprotPath);
                    temp.Parse();
                }
            }
        }        
    }
    private string GetTypeFromString(string type)
    {
        switch (type)
        {
            case "list":
                return "List";
            case "list<int>":
                return "List<int>";
            case "list<float>":
                return "List<float>";
            case "list<long>":
                return "List<long>";
            case "list<bool>":
                return "List<bool>";
            case "list<string>":
                return "List<string>";
            case "list<T>":
                return "List<int>";
            case "array":
            case "array<T>":
                return "int[]";
            default:
                return type;
        }
    }
    public string FieldName
    {
        get => this.fieldName;
    }

    public string FieldType 
    {
        get => this.realFieldType;
    }

    public bool BList
    {
        get;set;
    } = false;
}
