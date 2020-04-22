using System;
using System.Collections.Generic;
namespace CaomaoFramework
{
    [Serializable]
    public class NewbieHelpTipStepData
    {
        public List<NewbieHelpDialogTip> Dialogs = new List<NewbieHelpDialogTip>();//所有对话
    }
    [Serializable]
    public class NewbieHelpDialogTip
    {
        public string DialogContent;//对话内容
        public string CharacterName;//人物名字
        public string CharacterPath;//对话角色资源名称（addressable里面的名称）
    }
}
