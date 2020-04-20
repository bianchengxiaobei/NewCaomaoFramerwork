using System;
using Sirenix.OdinInspector;

[Serializable]
public class CaomaoLocalizationSBConfig
{
    [TableColumnWidth(60)]
    [LabelText("语言")]
    public string Language;
    [LabelText("SB文件名字")]
    public string ScriptableObjectName;

    public CaomaoLocalizationSBConfig(string l, string n)
    {
        this.Language = l;
        this.ScriptableObjectName = n;
    }
}

