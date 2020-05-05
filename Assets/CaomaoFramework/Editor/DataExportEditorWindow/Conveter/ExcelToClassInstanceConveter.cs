using UnityEngine;
using System.Collections.Generic;
using System;
using System.Data;
using System.Reflection;
/// <summary>
/// excel转实例
/// </summary>
public class ExcelToClassInstanceConveter
{
    private List<CaomaoExcelFieldData> m_listFieldInfo = new List<CaomaoExcelFieldData>();
    //private object m_instance;
    private Assembly assembly;
    private int m_instanceCount;//实例数量
    private List<object> m_listInstance = new List<object>();
    public ExcelToClassInstanceConveter(Assembly _assembly) 
    {
        this.assembly = _assembly;
    }

    public object[] Instances 
    {
        get => this.m_listInstance.ToArray();
    }
    public void Parse(DataTable table) 
    {
        var rowData = table.Rows;
        var col = table.Columns.Count;
        var row = rowData.Count;
        this.m_instanceCount = row - 3;
        var className =  rowData[0][0].ToString();
        var type = this.GetSafeType(className);
        if (type == null)
        {
            Debug.LogError("Type == null:" + className);
            return;
        }
        this.m_listInstance.Clear();
        for (int j = 0; j < this.m_instanceCount; j++)
        {
            var tempInstance = Activator.CreateInstance(type);
            this.m_listInstance.Add(tempInstance);
        }

        //this.m_instance = Activator.CreateInstance(type);
        for (int i = 0; i < col; i++)
        {
            this.m_listFieldInfo.Clear();
            var data = rowData[1][i];//某一列
            var fieldName = data.ToString();
            if (string.IsNullOrEmpty(fieldName) == false)
            {
                //字段名称
                data = rowData[2][i];//类型
                var fieldType = data.ToString();
                //Debug.Log(fieldType);
                if (string.IsNullOrEmpty(fieldType))
                {
                    Debug.LogError("FieldType == null");
                    continue;
                }
                //接下来数值代表多种实例
                for (int j = 0; j < this.m_instanceCount; j++) 
                {
                    data = rowData[j + 3][i];//数值
                    var fieldValue = data.ToString();
                    //Debug.Log(fieldValue);
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        Debug.LogError("fieldValue == null");
                        continue;
                    }
                    //var tempInstance = Activator.CreateInstance(type);
                    //this.m_listInstance.Add(tempInstance);
                    var tempInstance = this.m_listInstance[j];
                    this.m_listFieldInfo.Add(new CaomaoExcelFieldData(fieldName, fieldType,
                    fieldValue, tempInstance, table.DataSet, this.assembly));
                }
                //开始解析
                foreach (var field in this.m_listFieldInfo)
                {
                    field.Parse();
                }
            }
        }     
    }
    private Type GetSafeType(string className)
    {
        var type = this.assembly.GetType(className);
        if (type == null)
        {
            type = this.assembly.GetType("CaomaoFramework." + className);
            return type;
        }
        return type;
    }
}
