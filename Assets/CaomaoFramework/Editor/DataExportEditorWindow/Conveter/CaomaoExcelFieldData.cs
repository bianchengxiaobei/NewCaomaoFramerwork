using UnityEngine;
using System.Collections;
using Unity.Entities;
using UnityEditor.Build.Pipeline.Tasks;
using System.Data;
using System.Reflection;

public enum EFieldType
{
    Custom,//自定义
    Int,
    String,
    Float,
    Long,
    List,
    Array,
    Bool,
}

public class CaomaoExcelFieldData 
{
    public string fieldName;//字段名称
    public EFieldType eFieldType;//字段类型
    public string fieldValue;//字段值（如果是另外类型，用表名）
    public object instance;

    private DataSet excel;
    private Assembly assembly;



    private ICaomaoDataParse m_dataParse;

    public CaomaoExcelFieldData(string name, string type, string fieldValue, object ins, DataSet excel, Assembly assembly)
    {
        this.fieldName = name;
        this.instance = ins;
        this.excel = excel;
        this.assembly = assembly;
        this.eFieldType = this.GetTypeFromString(type);
        this.fieldValue = fieldValue;      
    }


    public void SetParse(ICaomaoDataParse parse) 
    {
        this.m_dataParse = parse;
    }

    private EFieldType GetTypeFromString(string type) 
    {
        switch (type) 
        {
            case "int":
                this.m_dataParse = new CaomaoIntDataParse();
                return EFieldType.Int;
            case "string":
                this.m_dataParse = new CaomaoStringDataParse();
                return EFieldType.String;
            case "float":
                this.m_dataParse = new CaomaoFloatDataParse();
                return EFieldType.Float;
            case "long":
                this.m_dataParse = new CaomaoLongDataParse();
                return EFieldType.Long;
            case "bool":
                this.m_dataParse = new CaomaoBoolDataParse();
                return EFieldType.Bool;
            case "list":
            case "list<int>":
            case "list<float>":
            case "list<long>":
            case "list<bool>":
            case "list<string>":
                this.m_dataParse = new CaomaoListDataParse();
                return EFieldType.List;
            case "list<T>":
                this.m_dataParse = new CaomaoGenericListDataParse(this.excel,this.assembly);
                return EFieldType.List;
            case "array":
            case "array<T>":
                return EFieldType.Array;
            default:
                return EFieldType.Custom;
        }
    }


    public void Parse() 
    {
        if (this.m_dataParse != null) 
        {
            this.m_dataParse.Parse(this.instance,this.fieldName,this.fieldValue);
        }
    }
}
