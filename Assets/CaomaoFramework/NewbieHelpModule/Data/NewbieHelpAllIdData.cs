using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
namespace CaomaoFramework
{
    /// <summary>
    /// 新手引导所有的步骤id（串联起来）
    /// </summary>
    [CreateAssetMenu(fileName = "NewbieHelpAllIdData", menuName = "CaomaoFramework/NewbieHelpAllIdData")]
    public class NewbieHelpAllIdData : ScriptableObject
    {
        [LabelText("所有新手引导id")]
        public List<NewbieHelpAllData> AllStepIds = new List<NewbieHelpAllData>();//所有新手引导的id链表
    }

    public enum ENewbieHelpType
    {
        CharacterTip,//任务提示
        ButtonClick,//按钮点击
    }
    [Serializable]
    public class NewbieHelpAllData
    {
        [LabelText("当前新手引导id")]
        public int Id;
        [LabelText("新手引导下个id(-1表示为下个引导结束)")]
        public int NextId = -1;
        [LabelText("新手引导类型")]
        public ENewbieHelpType NewbieType = ENewbieHelpType.CharacterTip;
    }
}


