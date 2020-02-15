using UnityEngine;
using CaomaoFramework;

namespace CaomaoHotFix
{
    public class ClientTestState : ClientStateBase
    {
        public override void OnEnter()
        {
            Debug.Log("Enter");
            CaomaoDriver.UIModule.Init();
            var ui = CaomaoDriver.UIModule.GetUI("UITest");
            ui.Show();
        }
        public override void OnLeave()
        {
            Debug.Log("Leave");
        }
    }
}
