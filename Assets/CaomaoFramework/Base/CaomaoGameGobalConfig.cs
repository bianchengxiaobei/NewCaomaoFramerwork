using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
namespace CaomaoFramework
{
    public class CaomaoGameGobalConfig : CaomaoRuntimeGlobalSBConfig<CaomaoGameGobalConfig>
    {
        [LabelText("红点配置文件路径（或者Addressable_Label）")]
        public string RedPointTreePath;
        [LabelText("热更新dll")]
        public string HotFixDllName;
        [LabelText("热更新pdb")]
        public string HotFixPdbName;
        [LabelText("新手教程本地PlayerPref的名称")]
        public string LocalPPNewbieHelpName;
        [LabelText("配置文件所在的文件目录")]
        public string ConfigPathDir;
    }
}
