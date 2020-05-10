using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
namespace CaomaoFramework 
{
    /// <summary>
    /// 按钮点击步骤
    /// </summary>
    /// 可能按钮不在当前界面,延迟到下一个去查找
    [CInstanceNumber(5)]
    public class NewbieButtonNoContentStep : NewbieHelpStep
    {
        protected NewbieHelpButtonNoContentData m_stepData;//数据
        protected Button bt_activeButton; 
        private bool m_bInitEvent = false;
        public override void Enter()
        {
            base.Enter();
            //首先判断是否有button，没有就延时，然后每次点击的时候检测一遍
            this.CheckConditionAndReEnter();
        }

        public override void OnFinished()
        {
            //Debug.Log("fwerwOnfishjed");
            CaomaoDriver.NewbieHelpModule.SetUIGlowTipNoVisiable();
            this.RemoveButtonListener();
            this.bt_activeButton = null;
            base.OnFinished();
            if (!(this is NewbieButtonWithContentStep)) 
            {
                ClassPoolModule<NewbieButtonNoContentStep>.Release(this);
            }         
        }
        public override void LoadHelpStepData()
        {
            
        }

        private void AddButtonListener() 
        {
            if (this.m_bInitEvent) 
            {
                return;
            }
            this.m_bInitEvent = true;
            this.bt_activeButton.onClick.AddListener(this.OnFinished);
        }
        private void RemoveButtonListener() 
        {
            if (this.m_bInitEvent)
            {
                this.bt_activeButton.onClick.RemoveListener(this.OnFinished);
            }
        }

        public bool CheckConditionAndReEnter() 
        {
            if (this.FindActiveButton(out this.bt_activeButton)) 
            {
                this.AddButtonListener();
                CaomaoDriver.NewbieHelpModule.SetVaildArea(this.bt_activeButton.image.rectTransform);
                CaomaoDriver.NewbieHelpModule.SetUIGlowTip(this.bt_activeButton.image.rectTransform,
                    new Vector2(180,53),new Vector2(360,106));//这里应该填表
                this.bCheck = false;
                this.ReEnter();
                return true;
            }
            this.bCheck = true;
            return false;
        }

        protected virtual void ReEnter()
        {

        }

        private bool FindActiveButton(out Button btn) 
        {
            if (this.m_stepData != null && string.IsNullOrEmpty(this.m_stepData.ButtonPath) == false) 
            {
                btn = CaomaoDriver.UIRoot.Find(this.m_stepData.ButtonPath).GetComponent<Button>();
                if (btn == null) 
                {
                    return false;
                }
                return btn.IsActive();
            }
            btn = null;
            return false;
        }
    }
}

