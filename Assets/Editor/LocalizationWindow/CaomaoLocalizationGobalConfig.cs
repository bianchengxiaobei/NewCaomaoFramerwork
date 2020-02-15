
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
[CaomaoEditorConfig]
public class CaomaoLocalizationGobalConfig : GlobalConfig<CaomaoLocalizationGobalConfig>
{
    public string LocalizationExcelFolderPath;
    public string LocalizationScriptableObjectPath;
    public string LocalizationTemplatePath;
    public List<CaomaoLocalizationSBConfig> SBConfig = new List<CaomaoLocalizationSBConfig>();

    public void AddConfig(CaomaoLocalizationSBConfig config)
    {
        foreach (var c in this.SBConfig)
        {
            if (c.Language == config.Language)
            {
                return;
            }
        }
        this.SBConfig.Add(config);
    }
}