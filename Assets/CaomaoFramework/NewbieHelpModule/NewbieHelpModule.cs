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
        private bool m_bInit = false;
        public void Init()
        {
            //加载配置文件,初始化将要的新手教程（不加载全部）
            CaomaoDriver.ResourceModule.LoadGameObjectAsync("NewbieHelpModule", (obj) =>
            {
                this.HelpMask = obj.transform.Find("HelpMask").GetComponent<CUIHelpMask>();
                if (this.HelpMask == null) 
                {
                    Debug.LogError("HelpMask == null");
                }
            });         
        }

        public void SetVaildArea(RectTransform area) 
        {
            this.HelpMask.SetVaildArea(area);
        }


        public void Update()
        {
            
        }
    }
}

