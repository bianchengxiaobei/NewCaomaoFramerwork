using UnityEngine;
using System.Collections;
using System;

namespace CaomaoFramework 
{
    /// <summary>
    /// 带提示的按钮点击
    /// </summary>
    [CInstanceNumber(5)]
    public class NewbieButtonWithContentStep : NewbieButtonNoContentStep
    {
        private NewbieHelpButtonWithContentData m_stepWithContentData;
        private static CUINewbieButtonTip UITip;
        public override void LoadHelpStepData()
        {
            if (this.m_stepData == null) 
            {
                var filePath = $"{Application.persistentDataPath}/Newbie/NewbieHelp_{this.MainID}_{this.ID}.json";//远程端持久化目录下载 + this.id（归到下载更新里面）
                this.m_stepWithContentData = CaomaoDriver.DataModule.GetJsonData<NewbieHelpButtonWithContentData>(filePath);
                this.m_stepData = this.m_stepWithContentData;
            }        
        }

        protected override void ReEnter()
        {
            base.ReEnter();
            //显示提示UI
            //Debug.Log(this.m_stepWithContentData.ButtonPath);
            //Debug.Log(this.m_stepWithContentData.Content);
            if (UITip == null)
            {
                this.InitUITip(this.SetTipContent);
            }
            else 
            {
                this.SetTipContent();
            }
        }
        private void SetTipContent() 
        {
            UITip.SetTip(this.m_stepWithContentData.Content, new Vector2(0,-51f), true);
        }
        private async void InitUITip(Action callback) 
        {
            var obj = await CaomaoDriver.ResourceModule.LoadGameObjectAsyncNoCallback("NewbieButtonTip");
            if (obj != null) 
            {
                UITip = obj.GetComponent<CUINewbieButtonTip>();
                if (UITip == null) 
                {
                    Debug.LogError("UITip == null");
                    return;
                }
                var rectTransorm = UITip.GetComponent<RectTransform>();
                CaomaoDriver.NewbieHelpModule.SetUIToRoot(rectTransorm,0);
                UITip.SetVisiable(false);
                callback?.Invoke();
            }
        }


        public override void OnFinished()
        {
            base.OnFinished();
            UITip.SetVisiable(false);
            ClassPoolModule<NewbieButtonWithContentStep>.Release(this);
        }
    }
}

