using Boo.Lang;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    [Module(false)]
    public class NewbieHelpModule : IModule,INewbieHelpModule
    {
        private CUIHelpMask HelpMask;
        private Transform HelpUIRoot;//引导界面的根节点(存储人物对话界面等)
        private UINewbieGlow NewbieGlow;//引导发光UI
        private int m_networkNewbieHelpId = int.MinValue;
        private bool m_bInit = false;
        private NewbieHelpAllIdData m_allMainData;
        private int m_curNewbieIndex = int.MinValue;
        private Action m_actionOnFinished = null;
        public void Init()
        {          
            //CaomaoDriver.ResourceModule.LoadGameObjectAsync("NewbieHelpModule", (obj) =>
            //{
            //    this.HelpUIRoot = obj.transform.Find("NewbieHelpRoot");
            //    this.HelpMask = obj.transform.Find("HelpMask").GetComponent<CUIHelpMask>();
            //    if (this.HelpMask == null) 
            //    {
            //        Debug.LogError("HelpMask == null");
            //    }
            //    this.HelpUIRoot.gameObject.SetActive(false);
            //    this.m_bInit = true;//初始化完毕
            //});         
        }
        public void CheckNetworkNewbie(int networkId) 
        {
            var ppName = CaomaoGameGobalConfig.Instance.LocalPPNewbieHelpName;
            if (string.IsNullOrEmpty(ppName)) 
            {
                Debug.LogError("PPNewbieName == null");
                return;
            }
            this.m_networkNewbieHelpId = networkId;
            var localNewbieId = PlayerPrefModule.GetInt(ppName);
            if (localNewbieId == 0)
            {
                //说明还没有开始新手教程
                if (networkId != 0) 
                {
                    //说明是客户端自己删了数据，但是服务器已经有新手教程了
                    PlayerPrefs.SetInt(ppName,networkId);
                }
            }
            else
            {
                if (localNewbieId != networkId) 
                {
                    //如果本地的不和服务器下发的一样，就开始新手教程
                    this.StartNewbieHelp(networkId);
                }
            }
        }


        public void SetUIGlowTip(RectTransform center, Vector2? targetSize = null, Vector2? orginSize = null) 
        {
            if (this.NewbieGlow != null) 
            {
                this.NewbieGlow.SetGlow(center, targetSize, orginSize);
            }
        }

        public void StartNewbieHelp(int newbieMainId) 
        {
            CaomaoDriver.UIModule.PreLoadUI<UINewbieHelp>((ui)=>
            {
                if (ui != null) 
                {
                    this.HelpUIRoot = ui.PanelRoot.Find("NewbieHelpRoot");
                    this.HelpMask = ui.CUIHelpMask;
                    this.NewbieGlow = ui.CUIHelpGlow;
                    this.NewbieGlow.SetNoVisiable();
                    ui.PanelRoot.gameObject.SetActive(true);
                    this.m_bInit = true;
                }
                this.InitNewbieHelpData(newbieMainId, () =>
                {
                    if (this.m_allMainData.AllStepIds.Count > 0)
                    {
                        Debug.Log("Start");
                        this.m_networkNewbieHelpId = newbieMainId;
                        this.m_curNewbieIndex = 0;
                        var firstData = this.m_allMainData.AllStepIds[this.m_curNewbieIndex];
                        var firstStep = this.CreateNewbieHelpStep(firstData, newbieMainId);
                        firstStep.Enter();
                    }
                });
            });         
        }

        private NewbieHelpStep CreateNewbieHelpStep(NewbieHelpData data,int mainId)
        {
            NewbieHelpStep result = null;
            switch (data.NewbieType) 
            {
                case ENewbieHelpType.ButtonClickWithContent:
                    ClassPoolModule<NewbieButtonWithContentStep>.Init();
                    result = ClassPoolModule<NewbieButtonWithContentStep>.Alloc();
                    break;
                case ENewbieHelpType.ButtonClickNoContent:
                    ClassPoolModule<NewbieButtonNoContentStep>.Init();
                    result = ClassPoolModule<NewbieButtonNoContentStep>.Alloc();
                    break;
                case ENewbieHelpType.CharacterTip:
                    NewbieHelpTipStep.Init();
                    ClassPoolModule<NewbieHelpTipStep>.Init();
                    result = ClassPoolModule<NewbieHelpTipStep>.Alloc();
                    break;
            }
            if (result != null)
            {
                result.MainID = mainId;
                result.ID = data.Id;
                result.ActionOnFinished = this.NextNewbieStep;
            }     
            return result;
        }
        /// <summary>
        /// 下一个教程
        /// </summary>
        private void NextNewbieStep() 
        {
            this.m_curNewbieIndex++;
            if (this.m_curNewbieIndex < this.m_allMainData.AllStepIds.Count)
            {
                var stepData = this.m_allMainData.AllStepIds[this.m_curNewbieIndex];
                var step = this.CreateNewbieHelpStep(stepData, this.m_networkNewbieHelpId);
                step.Enter();
            }
            else
            {
                //全部已经结束
                this.m_actionOnFinished?.Invoke();
                this.m_curNewbieIndex = int.MinValue;
            }
        }
        /// <summary>
        /// 设置全部结束回调
        /// </summary>
        /// <param name="onFinished"></param>
        public void SetOnFinishedCallback(Action onFinished)
        {
            this.m_actionOnFinished = onFinished;
        }

        public async void InitNewbieHelpData(int newbieMainId,Action loadFinished) 
        {
            //加载配置文件,初始化将要的新手教程（不加载全部）
            this.m_allMainData = null;
            var filePath = $"NewbieHelp_{newbieMainId}";
            this.m_allMainData = await CaomaoDriver.ResourceModule.LoadAssetAsyncNoCallback<NewbieHelpAllIdData>(filePath);
            if (this.m_allMainData == null) 
            {
                Debug.LogError("data == null:" + filePath);
                return;
            }
            loadFinished?.Invoke();
        }

        public void SetVaildArea(RectTransform area) 
        {
            this.HelpMask.SetVaildArea(area);
        }
        /// <summary>
        /// 设置生成的UI到新手引导根节点
        /// </summary>
        /// <param name="uiTransform"></param>
        public Vector3 SetUIToRoot(RectTransform uiTransform,int index = 0)
        {
            if (this.m_bInit == false)
            {
                Debug.LogError("Init == false");
                return Vector3.zero;
            }
            if (uiTransform != null)
            {
                uiTransform.SetParent(this.HelpUIRoot);
                if (index == 0)
                {
                    uiTransform.SetAsFirstSibling();
                }
                else 
                {
                    uiTransform.SetSiblingIndex(index);
                }
                uiTransform.localPosition = Vector3.zero;
                uiTransform.localScale = Vector3.one;
                return uiTransform.position;
            }
            return Vector3.zero;
        }


        public void Update()
        {
            
        }

        public void SetUIGlowTipNoVisiable()
        {
            if (this.NewbieGlow != null) 
            {
                this.NewbieGlow.SetNoVisiable();
            }
        }
    }
}

