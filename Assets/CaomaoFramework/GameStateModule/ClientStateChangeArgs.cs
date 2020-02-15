using System;
namespace CaomaoFramework
{
    public class ClientStateChangeArgs
    {
        public string sClientState;
        public ELoadingType eLoadingStyle;
        public Action aCallBack;
    }
}
