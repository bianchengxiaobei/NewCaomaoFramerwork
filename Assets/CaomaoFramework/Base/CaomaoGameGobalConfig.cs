using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
namespace CaomaoFramework
{
    [GlobalConfig("Assets/CaomaoFramework/Base/")]
    public class CaomaoGameGobalConfig : GlobalConfig<CaomaoGameGobalConfig>
    {
        [LabelText("红点配置文件路径（或者Addressable_Label）")]
        public string RedPointTreePath;
        [LabelText("热更新dll")]
        public string HotFixDllName;
        [LabelText("热更新pdb")]
        public string HotFixPdbName;
    }
}
