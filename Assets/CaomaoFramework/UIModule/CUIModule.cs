using System.Collections.Generic;
using UnityEngine;
using System;

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

        public void ShowUI<T>(string defaultName = null) where T : UIBase
        {
            if (defaultName == null)
            {
                var type = typeof(T);
                defaultName = type.FullName;              
            }
            //Debug.Log(defaultName);
            if (m_dicUIs.TryGetValue(defaultName, out var ui))
            {
                ui.Show();
            }
        }

        public void PreLoadUI<T>(Action<T> onFinished = null, string defaultName = null) where T : UIBase
        {
            if (defaultName == null)
            {
                var type = typeof(T);
                defaultName = type.FullName;
            }
            if (m_dicUIs.TryGetValue(defaultName, out var ui))
            {
                ui.PreLoad<T>(onFinished);
            }
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
