using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
namespace CaomaoFramework
{
    public interface IPathFindModule : IModule
    {
        int MaxFindPathPointCount { get; set; }//最大寻找路径点
        bool FullParallel { get; set; }//是否完全的并行化
        int StartFind2DPath(float2 startPos, float2 endPos, Action<NativeList<float2>> callback);
    }
}
