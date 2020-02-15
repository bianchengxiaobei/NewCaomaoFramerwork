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
}