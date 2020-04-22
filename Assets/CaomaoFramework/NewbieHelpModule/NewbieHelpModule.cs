using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    [Module(false)]
    public class NewbieHelpModule : IModule,INewbieHelpModule
    {
        private CUIHelpMask HelpMask;
        private Transform HelpUIRoot;//引导界面的根节点
        private bool m_bInit = false;
        public void Init()
        {
            //加载配置文件,初始化将要的新手教程（不加载全部）
            CaomaoDriver.ResourceModule.LoadGameObjectAsync("NewbieHelpModule", (obj) =>
            {
                this.HelpUIRoot = obj.transform.Find("NewbieHelpRoot");
                this.HelpMask = obj.transform.Find("HelpMask").GetComponent<CUIHelpMask>();
                if (this.HelpMask == null) 
                {
                    Debug.LogError("HelpMask == null");
                }
                this.m_bInit = true;//初始化完毕
            });         
        }

        public void SetVaildArea(RectTransform area) 
        {
            this.HelpMask.SetVaildArea(area);
        }
        /// <summary>
        /// 设置生成的UI到新手引导根节点
        /// </summary>
        /// <param name="uiTransform"></param>
        public void SetUIToRoot(Transform uiTransform)
        {
            if (this.m_bInit == false)
            {
                Debug.LogError("Init == false");
                return;
            }
            if (uiTransform != null)
            {
                uiTransform.SetParent(this.HelpUIRoot);
                uiTransform.localPosition = Vector3.zero;
                uiTransform.localScale = Vector3.one;
            }
        }


        public void Update()
        {
            
        }
    }
}

