using System;
using System.Collections.Generic;


namespace CaomaoFramework
{
    public interface ICGestureModule
    {
        void Init();
        void Update();

        void AddGesture(IGestureActionCallbackBase callback);
        void RemoveGesture(IGestureActionCallbackBase callback);
    }
}
