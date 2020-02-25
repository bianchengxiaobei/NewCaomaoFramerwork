using System;
using System.Collections.Generic;
using CaomaoFramework;
namespace CaomaoHotFix
{
    public class HotFixDriver
    {
        public static UIModule_HotFix UIModule = new UIModule_HotFix();
        public static ClientStateModule_HotFix ClientStateModule = new ClientStateModule_HotFix();
        public static void Init()
        {
            //UIModule.Init();
            //ClientStateModule.Init();
            ITestA baseT = new TestBase();
            baseT.Start();
            ITestA childT = new TestB();
            childT.Start();
        }

        public static void Update()
        {
            
        }
    }
}

