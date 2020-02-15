using System;
using System.Collections.Generic;
using CaomaoFramework;

namespace CaomaoHotFix
{
    public class UIModule_HotFix
    {
        public void Init()
        {
            CaomaoDriver.UIModule.AddUI(nameof(UITest), new UITest());
        }
    }
}
