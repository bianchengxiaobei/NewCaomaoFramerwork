using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    [Module(true)]
    public class CUIModule : ICUIModule,IModule
    {
        public Dictionary<string, UIBase> m_dicUIs = new Dictionary<string, UIBase>();

        public void Init()
        {
            foreach (var ui in this.m_dicUIs.Values)
            {
                ui.Init();
                if (ui.NotDestroy)
                {
                    ui.PreLoad();
                }
            }
        }

        public void AddUI(string uiName, UIBase ui)
        {
            if (this.m_dicUIs.ContainsKey(uiName))
            {
                return;
            }
            this.m_dicUIs[uiName] = ui;
        }



        public UIBase GetUI(string type)
        {
            UIBase ui = null;
            m_dicUIs.TryGetValue(type, out ui);
            return ui;
        }

        public void Update()
        {
            foreach (var ui in this.m_dicUIs.Values)
            {
                if (ui.Visiable)
                {
                    ui.Update();
                }
            }
        }
    }
}
