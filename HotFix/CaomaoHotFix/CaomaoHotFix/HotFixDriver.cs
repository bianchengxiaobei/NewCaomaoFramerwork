using System;
using System.Collections.Generic;
namespace CaomaoHotFix
{
    public class HotFixDriver
    {
        public static UIModule_HotFix UIModule = new UIModule_HotFix();
        public static ClientStateModule_HotFix ClientStateModule = new ClientStateModule_HotFix();
        public static void Init()
        {
            UIModule.Init();
            ClientStateModule.Init();
        }

        public static void Update()
        {
            
        }
    }
}

