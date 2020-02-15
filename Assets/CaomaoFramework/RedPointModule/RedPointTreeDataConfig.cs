using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
namespace CaomaoFramework
{
    [GlobalConfig("Assets/CaomaoFramework/RedPointModule/")]
    public class RedPointTreeDataConfig : GlobalConfig<RedPointTreeDataConfig>
    {
        public RedPointTreeData TreeNode;
    }
    [Serializable]
    public class RedPointTreeData
    {
        [LabelText("红点button的唯一名称")]
        public string Id;
        [LabelText("红点类型(数字或者简单红点)")]
        public ERedPointType RedPointType;
        [LabelText("子红点")]
        public List<RedPointTreeData> Childs = new List<RedPointTreeData>();
    }
}
