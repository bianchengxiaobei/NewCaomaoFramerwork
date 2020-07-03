using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using UnityEditor;
[GlobalConfig("Assets/CaomaoFrameWork/BuildWindow/Config")]
public class CaomaoBuildConfig : GlobalConfig<CaomaoBuildConfig>
{
    public BuildTarget buildTarget;
    public BuildTargetGroup buildTargetGroup;
    public List<string> QuDaoList = new List<string>();
    public bool bDevelpment;
    public string ExportApkPath;
}

