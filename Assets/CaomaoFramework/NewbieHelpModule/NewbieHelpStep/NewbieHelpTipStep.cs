using CaomaoFramework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    /// <summary>
    /// 人物提示界面（点击关闭进入下一个新手引导）
    /// </summary>
    [CInstanceNumber(3)]
    public class NewbieHelpTipStep : NewbieHelpStep
    {
        //public List<NewbieHelpDialogTip> Tips { get; set; }//所有对话
        private int m_iCurIndex = 0;//当前对话所在的索引
        private static CUIHelpTip UITipObj;
        private NewbieHelpTipStepData StepData;
        private static bool m_bStaticInit = false;
        private static Action m_actionInitFinished;
        public static void Init()
        {
            if (m_bStaticInit) 
            {
                return;
            }
            CaomaoDriver.ResourceModule.LoadGameObjectAsync("NewbieHelpTip", (obj) =>
            {
                UITipObj = obj.GetComponent<CUIHelpTip>();
                if (UITipObj == null)
                {
                    Debug.LogError("UITip == null");
                    return;
                }
                var rectTransform = UITipObj.GetComponent<RectTransform>();
                //设置父亲节点，让他可以点击
                CaomaoDriver.NewbieHelpModule.SetUIToRoot(rectTransform);
                UITipObj.SetVisiable(false);
                m_actionInitFinished?.Invoke();
                m_bStaticInit = true;               
            },true);
        }
        public override void Enter()
        {
            base.Enter();
            //实例化对话。然后设置mask为最上层
            if (UITipObj != null)
            {
                Debug.Log("Enter");
                UITipObj.AddButtonListener(this.NextDialogTip);
                //设置新手提示图片的更改
                UITipObj.SetNewbieHelpStepData(this.StepData, this.m_iCurIndex);
                //显示并且能点击
                UITipObj.SetVisiable(true);
            }
            else
            {
                Debug.LogError("UITip没有初始化成功!");
                //Init();
                m_actionInitFinished = this.Enter;
            }
        }
        public override void LoadHelpStepData()
        {
            if (this.StepData == null)
            {
                var filePath = $"{Application.persistentDataPath}/Newbie/NewbieHelp_{this.MainID}_{this.ID}.json";//远程端持久化目录下载 + this.id（归到下载更新里面）
                this.StepData = CaomaoDriver.DataModule.GetJsonData<NewbieHelpTipStepData>(filePath);
                if (this.StepData != default(NewbieHelpTipStepData))
                {
                    //说明读取成功
                    Debug.Log("加载成功");
                }
            }          
        }

        /// <summary>
        /// 下一个对话，如果没有就直接结束
        /// </summary>
        public void NextDialogTip()
        {
            this.m_iCurIndex++;
            if (this.m_iCurIndex >= this.StepData.Dialogs.Count)
            {
                //说明结束了
                this.OnFinished();
                return;
            }
            //设置新手提示图片的更改
            UITipObj.SetNewbieHelpStepData(this.StepData, this.m_iCurIndex);
        }
        /// <summary>
        /// 清除对话资源
        /// </summary>
        public override void OnFinished()
        {
            this.m_iCurIndex = 0;
            //this.Tips.Clear();
            base.OnFinished();
        }     
    }
}