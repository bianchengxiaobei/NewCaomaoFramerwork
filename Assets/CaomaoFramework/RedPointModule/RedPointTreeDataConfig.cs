using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace CaomaoFramework
{
    public class RedPointTreeDataConfig : CaomaoRuntimeGlobalSBConfig<RedPointTreeDataConfig>
    {
        public RedPointTreeData TreeNode;
        //private static RedPointTreeDataConfig m_instance;
        //public static RedPointTreeDataConfig Instance
        //{
        //    get
        //    {
        //        if (m_instance == null)
        //        {
        //            m_instance = Resources.Load<RedPointTreeDataConfig>("RedPointTreeDataConfig");
        //        }
        //        return m_instance;
        //    }
        //}
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
