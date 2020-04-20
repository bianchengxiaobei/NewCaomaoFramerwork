using System;
using System.IO;
using System.Data;
using Excel;
using UnityEngine;
public enum EDevPlatformType
{
    Windows,
    Linux,
    Mac
}
public enum EGUIEventButtonType
{
    Left = 0,
    Right = 1,
    Center = 2
}
public class CaomaoEditorHelper
{
    public static EDevPlatformType GetDevelopPlatform()
    {
        var platform = Environment.OSVersion.Platform;
        if (platform != PlatformID.Unix)
        {
            if (platform != PlatformID.MacOSX)
            {
                return EDevPlatformType.Windows;
            }
            return EDevPlatformType.Mac;
        }
        else
        {
            //Linux或者是Mac
            if (Directory.Exists("/Applications") & Directory.Exists("/System") & Directory.Exists("/Users") & Directory.Exists("/Volumes"))
            {
                return EDevPlatformType.Mac;
            }
            return EDevPlatformType.Linux;
        }
    }
    public static DataRowCollection ReadExcel(string filePath,ref int col,ref int row)
    {
        if (File.Exists(filePath))
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            //Tables[0] 下标0表示excel文件中第一张表的数据
            col = result.Tables[0].Columns.Count;
            row = result.Tables[0].Rows.Count;
            return result.Tables[0].Rows;
        }
        Debug.LogError("不存在Excel文件:" + filePath);
        return null;
    }
    public static DataRowCollection ReadExcel(string filePath)
    {
        FileStream stream = null;
        try
        {
            if (File.Exists(filePath))
            {
                stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                DataSet result = excelReader.AsDataSet();
                return result.Tables[0].Rows;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);          
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
        Debug.LogError("不存在Excel文件:" + filePath);
        return null;
    }
}