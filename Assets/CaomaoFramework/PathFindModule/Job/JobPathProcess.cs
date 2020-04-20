using System;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;
namespace CaomaoFramework
{
    //[BurstCompile]
    public struct Job2DPathParallelProcess : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Path2D> ABPath;
        public NativeMultiHashMap<int,float2>.ParallelWriter Paths;
        public void Execute(int index)
        {
            try
            {
                var a = index * 2;
                this.Paths.Add(index, float2.zero * (a + 1));
                this.Paths.Add(index, float2.zero * (a + 2));
            }
            catch (Exception e)
            {
                //太多了，直接结束
                Debug.LogException(e);
                return;
            }
        }
    }
    //[BurstCompile]
    public struct Job2DPathIJobProcess : IJob
    {
        [ReadOnly]
        public Path2D ABPath;
        public NativeList<float2> Paths;
        public void Execute()
        {
            
        }
    }
}
