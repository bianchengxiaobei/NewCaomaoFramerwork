using System;
using System.Collections.Generic;
using CaomaoFramework;
namespace CaomaoHotFix
{
    public class ClientStateModule_HotFix
    {
        public void Init()
        {
            //添加所有游戏状态
            CaomaoDriver.GameStateModule.AddClientGameState("TestState", new ClientTestState());
        }
    }
}
