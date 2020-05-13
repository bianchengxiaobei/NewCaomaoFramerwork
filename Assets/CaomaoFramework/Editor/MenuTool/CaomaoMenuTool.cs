using UnityEngine;
using UnityEditor;
using CaomaoFramework;
using System.IO;
using System;
public class CaomaoMenuTool : ScriptableObject
{
    [MenuItem("CaomaoTools/拷贝热更dll")]
    public static void CopyHotfixDll()
    {
        var dllName = CaomaoGameGobalConfig.Instance.HotFixDllName;
        var pbdName = CaomaoGameGobalConfig.Instance.HotFixPdbName;
        if (string.IsNullOrEmpty(dllName) || string.IsNullOrEmpty(pbdName))
        {
            Debug.LogError("CaomaoGameGobalConfig.Instance.HotFixDllName == null || CaomaoGameGobalConfig.Instance.HotFixPdbName");
            EditorUtility.DisplayDialog("名称错误", "请填写dll的名称", "确定");
            return;
        }

        if (string.IsNullOrEmpty(CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixDllFolder) || string.IsNullOrEmpty(CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixSourceFolder))
        {
            Debug.LogError("CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixDllFolder == null || CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixSourceFolder");
            EditorUtility.DisplayDialog("目录错误", "请填写dll的目标目录或者导出目录", "确定");
            return;
        }

        var sourceDllFile = $"{CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixSourceFolder}/{dllName}";
        var sourcePbdFile = $"{CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixSourceFolder}/{pbdName}";
        var targetDllFile = $"{Application.persistentDataPath}/{dllName}";
        var targetPbdFile = $"{Application.persistentDataPath}/{pbdName}";
        try
        {
            if (File.Exists(targetDllFile))
            {
                File.Delete(targetDllFile);
            }
            File.Move(sourceDllFile, targetDllFile);
            if (File.Exists(targetPbdFile))
            {
                File.Delete(targetPbdFile);
            }
            File.Move(sourcePbdFile, targetPbdFile);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            EditorUtility.DisplayDialog("拷贝文件出现错误", e.ToString(), "确定");
            return;
        }     
        EditorUtility.DisplayDialog("完成拷贝", "已成功拷贝到目标文件", "确定");
    }
}