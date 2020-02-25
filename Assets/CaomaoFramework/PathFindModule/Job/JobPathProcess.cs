using System;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
namespace CaomaoFramework
{
    public struct Job2DPathParallelProcess : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Path2D> ABPath;
        public NativeMultiHashMap<int,FPVector2>.ParallelWriter Paths;
        public void Execute(int index)
        {
            try
            {
                var a = index * 2;
                this.Paths.Add(index, FPVector2.one * (a + 1));
                this.Paths.Add(index, FPVector2.one * (a + 2));
            }
            catch (Exception e)
            {
                //太多了，直接结束
                Debug.LogException(e);
                return;
            }
        }
    }

    public struct Job2DPathIJobProcess : IJob
    {
        [ReadOnly]
        public Path2D ABPath;
        public NativeList<FPVector2> Paths;
        public void Execute()
        {
            
        }
    }
}
