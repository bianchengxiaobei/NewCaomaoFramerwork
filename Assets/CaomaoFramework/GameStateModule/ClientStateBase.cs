using System;
namespace CaomaoFramework
{
    public abstract class ClientStateBase
    {
        public abstract void OnEnter();

        public abstract void OnLeave();
    }
}
