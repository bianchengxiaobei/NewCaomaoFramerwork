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
        [LabelText("主引导Id")]
        public int MainId;
        [LabelText("该MainId下所有新手引导")]
        public List<NewbieHelpData> AllStepIds = new List<NewbieHelpData>();//所有新手引导的id链表
    }

    public enum ENewbieHelpType
    {
        CharacterTip,//任务提示
        ButtonClickWithContent,//按钮点击带提示
        ButtonClickNoContent,//按钮点击不带提示
    }
    [Serializable]
    public class NewbieHelpData
    {
        [LabelText("当前新手引导id")]
        public int Id;
        [LabelText("新手引导下个id(-1表示为下个引导结束)")]
        public int NextId = -1;
        [LabelText("新手引导类型")]
        public ENewbieHelpType NewbieType = ENewbieHelpType.CharacterTip;
    }
}


