using UnityEngine;
using System.Collections;
using System;
using Boo.Lang;
using Sirenix.Utilities;
/// <summary>
/// 普通类型list（不是自定义泛型）
/// </summary>
public class CaomaoListDataParse : ICaomaoDataParse
{
    public void Parse(object obj, string fieldName, string fieldValue)
    {
        if (obj == null)
        {
            Debug.LogError("obj == null");
            return;
        }
        //如果只有一个值
        if (fieldValue.Contains(",") == false) 
        {
            //说明不是list
            Debug.LogWarning("No List:" + fieldValue);
            //return;
        }
        try
        {
            var type = obj.GetType();
            var fieldInfo = type.GetField(fieldName);
            var values = fieldValue.Split(',');
            var genericType = fieldInfo.GetCaomaoListGenericType();
            var listObj = Activator.CreateInstance(fieldInfo.FieldType);
            var listAddMethod = fieldInfo.FieldType.GetMethod("Add");
            foreach (var tempValue in values)
            {
                if (string.IsNullOrEmpty(tempValue))
                {
                    continue;
                }
                var value = this.GetValueByGenericType(genericType, tempValue);
                listAddMethod.Invoke(listObj,value);
            }
            fieldInfo.SetValue(obj, listObj);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    private object[] GetValueByGenericType(Type type,string value) 
    {
        var result = new object[1];
        switch(type.Name)
        {
            case "Int32":
                result[0] = int.Parse(value);
                break;
            case "Single":
                result[0] = float.Parse(value);
                break;
            case "Int64":
                result[0] = long.Parse(value);
                break;
            case "String":
                result[0] = value;
                break;
            case "Boolean":
                result[0] = bool.Parse(value);
                break;
        }
        return result;
    }
}
