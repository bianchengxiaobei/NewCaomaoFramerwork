using System;
using System.Collections.Generic;
namespace CaomaoFramework
{
    public enum EGestureActionCallbackState
    {
        Possible = 1,
        Began = 2,
        Executing = 3,
        Ended = 4,
        EndPending = 5,
        Failed = 6
    }
}
