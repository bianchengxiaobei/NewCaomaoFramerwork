using System;
using System.Collections.Generic;


namespace CaomaoFramework
{
    public class HotFixModule : IHotFixModule, IModule
    {
        private IHotFixModule m_hotfixImp = new ILRuntimeHotFixImp();
        public void Init()
        {
            this.m_hotfixImp.Init();
        }

        public void LoadScript()
        {
            this.m_hotfixImp.LoadScript();
        }

        public void Update()
        {
            this.m_hotfixImp.Update();
        }
    }
}
