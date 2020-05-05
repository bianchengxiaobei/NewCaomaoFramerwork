using UnityEngine;
using System;
using System.Data;
using System.Reflection;
/// <summary>
/// 自定义泛型List
/// </summary>
public class CaomaoGenericListDataParse : ICaomaoDataParse
{
    private DataSet m_excel;
    private Assembly m_assembly;
    public CaomaoGenericListDataParse(DataSet excel,Assembly assem)
    {
        this.m_excel = excel;
        this.m_assembly = assem;
    }
    public void Parse(object obj, string fieldName, string fieldValue)
    {
        if (obj == null)
        {
            Debug.LogError("obj == null");
            return;
        }
        if (this.m_excel == null || string.IsNullOrEmpty(fieldValue)) 
        {
            Debug.LogError("execl == null");
            return;
        }
        if (this.m_excel.HasTable(fieldValue,out var excelTable) == false)
        {
            Debug.LogError("noTable:"+fieldValue);
            return;
        }
        var conveter = new ExcelToClassInstanceConveter(this.m_assembly);
        conveter.Parse(excelTable);
        var inss = conveter.Instances;//得到的是类型的实例
        try
        {
            var type = obj.GetType();
            var fieldInfo = type.GetField(fieldName);//需要加入到list里面
            var genericType = fieldInfo.GetCaomaoListGenericType();
            var listObj = Activator.CreateInstance(fieldInfo.FieldType);
            var listAddMethod = fieldInfo.FieldType.GetMethod("Add");
            //有点奇怪，不能用inss直接传参
            foreach (var ins in inss) 
            {
                listAddMethod.Invoke(listObj, new object[] { ins });
            }           
            fieldInfo.SetValue(obj, listObj);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
