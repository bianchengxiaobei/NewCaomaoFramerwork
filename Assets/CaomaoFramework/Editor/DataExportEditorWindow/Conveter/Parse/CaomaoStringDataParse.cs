using UnityEngine;
using System.Collections;
using System;
public class CaomaoStringDataParse : ICaomaoDataParse
{
    public void Parse(object obj, string fieldName, string fieldValue)
    {
        if (obj == null)
        {
            Debug.LogError("obj == null");
            return;
        }
        try
        {
            var type = obj.GetType();
            var fieldInfo = type.GetField(fieldName);
            fieldInfo.SetValue(obj, fieldValue);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
