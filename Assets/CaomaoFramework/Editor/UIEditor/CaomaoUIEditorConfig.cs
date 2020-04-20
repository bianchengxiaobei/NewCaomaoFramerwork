using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
[GlobalConfig("Assets/CaomaoFramework/Editor/UIEditor")]
public class CaomaoUIEditorConfig : GlobalConfig<CaomaoUIEditorConfig>
{
    [LabelText("创建Button在Hierarchy的名称")]
    public string buttonName;
    [LabelText("创建Text在Hierarchy的名称")]
    public string textName;
    [LabelText("Text默认用的字体")]
    public Font textFont;
}