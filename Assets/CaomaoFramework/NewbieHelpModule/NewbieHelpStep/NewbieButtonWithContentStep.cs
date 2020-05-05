using UnityEngine;
using System.Collections;
namespace CaomaoFramework 
{
    /// <summary>
    /// 带提示的按钮点击
    /// </summary>
    [CInstanceNumber(5)]
    public class NewbieButtonWithContentStep : NewbieButtonNoContentStep
    {
        private NewbieHelpButtonWithContentData m_stepWithContentData;

        public override void LoadHelpStepData()
        {
            base.LoadHelpStepData();
            this.m_stepWithContentData = this.m_stepData as NewbieHelpButtonWithContentData;          
        }

        protected override void ReEnter()
        {
            base.ReEnter();
            //显示提示UI

        }
        public override void OnFinished()
        {
            base.OnFinished();
            ClassPoolModule<NewbieButtonWithContentStep>.Release(this);
        }

    }
}

