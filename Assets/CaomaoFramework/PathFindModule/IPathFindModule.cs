using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CaomaoFramework
{
    public interface IPathFindModule : IModule
    {
        int MaxFindPathPointCount { get; set; }//最大寻找路径点
        bool FullParallel { get; set; }//是否完全的并行化
        int StartFind2DPath(FPVector2 startPos, FPVector2 endPos, Action<NativeList<FPVector2>> callback);
    }
}
