using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Data;

public static class CaomaoExtensionHelper
{
    public static Type GetCaomaoListGenericType(this FieldInfo fieldInfo) 
    {
        if (fieldInfo != null) 
        {
            var param = fieldInfo.FieldType.GetGenericArguments();
            if (param.Length > 0) 
            {
                return param[0];
            }
        }
        return null;
    }

    public static bool HasTable(this DataSet excel, string tableName,out DataTable table) 
    {
        table = excel.Tables[tableName];
        return table != null;
    }
}
