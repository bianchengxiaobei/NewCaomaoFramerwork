using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
[CaomaoEditorConfig]
public class CaomaoFrameworkGlobalConfig : GlobalConfig<CaomaoFrameworkGlobalConfig>
{
    public string Version;
    [FilePath]
    public string CaomaoGUIStyleSBPath;
    [FolderPath(AbsolutePath = true)]
    public string CaomaoHotfixDllFolder;//热更dll执行目录
    [FolderPath(AbsolutePath = true)]
    public string CaomaoHotfixSourceFolder;//热更编译出来的目录
}