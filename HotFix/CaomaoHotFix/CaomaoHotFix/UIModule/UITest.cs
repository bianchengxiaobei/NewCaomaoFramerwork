using System;
using System.Collections.Generic;
using CaomaoFramework;
using UnityEngine.UI;
using UnityEngine;
namespace CaomaoHotFix
{
    public class UITest : UIBase
    {
        public Button bt_test;

        public UITest()
        {
            this.m_sResName = "UITest";
            this.m_eHideType = EUIHideType.Active;
            this.m_bNotDestroy = false;
        }

        public override void Init()
        {
            
        }

        private void OnClickTestButton()
        {
            Debug.Log("测试Button是否执行");
        }

        public override void OnDisable()
        {
            
        }

        public override void OnEnable()
        {
                       
        }

        protected override void InitGraphicComponet()
        {
            this.bt_test = this.m_oRoot.Find("bt_test").GetComponent<Button>();
            this.bt_test.onClick.AddListener(this.OnClickTestButton);
        }

        protected override void OnAddListener()
        {
            
        }

        protected override void OnRemoveListener()
        {
            
        }

        protected override void RealseGraphicComponet()
        {
            
        }
        public override void Update()
        {
            
        }
    }
}
