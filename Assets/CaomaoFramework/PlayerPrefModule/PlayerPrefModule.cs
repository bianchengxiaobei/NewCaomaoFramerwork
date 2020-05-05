using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// PlayerPref扩展类
/// </summary>
public static class PlayerPrefModule
{
    public static List<int> GetIntList(string key)
    {
        var content = GetStringKey(key);
        if (string.IsNullOrEmpty(content))
        {
            return null;
        }
        var list = new List<int>();
        try
        {
            var sq = content.Split(';');
            foreach (var s in sq)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    list.Add(Convert.ToInt32(s));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
        return list;
    }
    public static List<float> GetFloatList(string key)
    {
        var content = GetStringKey(key);
        if (string.IsNullOrEmpty(content))
        {
            return null;
        }
        var list = new List<float>();
        try
        {
            var sq = content.Split(';');
            foreach (var s in sq)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    list.Add(Convert.ToSingle(s));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
        return list;
    }
    private static string GetStringKey(string key)
    {
        var content = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(content))
        {
            return null;
        }
        return content;
    }
    public static string GetString(string key,string defaultValue = "")
    {
        return PlayerPrefs.GetString(key,defaultValue);
    }
    public static float GetFloat(string key,float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(key,defaultValue);
    }
    public static int GetInt(string key, int defalutValue = 0)
    {
        return PlayerPrefs.GetInt(key,defalutValue);
    }
    public static bool GetBool(string key,bool defalutValue = false)
    {
        return PlayerPrefs.GetInt(key,0) == 0 ? false : true;
    }



    ////////////////////////////////////////////////SetValue

    public static void SetListValue<T>(string key, List<T> value) 
    {
        var content = ListToString<T>(value);
        if (!string.IsNullOrEmpty(content))
        {
            PlayerPrefs.SetString(key, content);
        }
        else
        {
            Debug.LogError("IntListString == null");
        }
    }

    //public static void SetIntList(string key,List<int> value) 
    //{
    //    var content = ListToString<int>(value);
    //    if (!string.IsNullOrEmpty(content))
    //    {
    //        PlayerPrefs.SetString(key, content);
    //    }
    //    else 
    //    {
    //        Debug.LogError("IntListString == null");
    //    }
    //}
    //public static void SetFloatList(string key, List<float> value)
    //{
    //    var content = ListToString<float>(value);
    //    if (!string.IsNullOrEmpty(content))
    //    {
    //        PlayerPrefs.SetString(key, content);
    //    }
    //    else
    //    {
    //        Debug.LogError("IntListString == null");
    //    }
    //}

    private static string ListToString<T>(List<T> list) 
    {
        if (list.Count > 5)
        {
            var sb = new StringBuilder();
           
            foreach (var l in list)
            {
                sb.Append(l.ToString());
                sb.Append(";");
            }
            return sb.ToString();
        }
        else 
        {
            var s = "";
            foreach (var l in list)
            {
                s += l.ToString() + ";";
            }
            return s;
        }     
    }





    //private static T GetValue<T>(string content) where T : IComparable
    //{
    //    var typeName = typeof(T).Name;
    //    switch (typeName)
    //    {
    //        case "Int32":
    //            return (T)Convert.ToInt32(content);
    //        case "Single":
    //            break;
    //        case "String":
    //            break;
    //        case "Double":
    //            break;
    //    }
    //}
}
