using UnityEngine;
using UnityEditor;
using CaomaoFramework;
using System.IO;
public class CaomaoMenuTool : ScriptableObject
{
    [MenuItem("CaomaoTools/拷贝热更dll")]
    public static void CopyHotfixDll()
    {
        var dllName = CaomaoGameGobalConfig.Instance.HotFixDllName;
        var pbdName = CaomaoGameGobalConfig.Instance.HotFixPdbName;
        var sourceDllFile = $"{CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixSourceFolder}/{dllName}";
        var sourcePbdFile = $"{CaomaoFrameworkGlobalConfig.Instance.CaomaoHotfixSourceFolder}/{pbdName}";
        var targetDllFile = $"{Application.persistentDataPath}/{dllName}";
        var targetPbdFile = $"{Application.persistentDataPath}/{pbdName}";
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
        EditorUtility.DisplayDialog("完成拷贝", "已成功拷贝到目标文件", "确定");
    }
}