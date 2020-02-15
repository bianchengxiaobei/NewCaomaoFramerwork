using System;
using System.Collections.Generic;


namespace CaomaoFramework
{ 
    public interface IHotFixModule
    {
        void Init();
        void Update();

        void LoadScript();
    }
}
