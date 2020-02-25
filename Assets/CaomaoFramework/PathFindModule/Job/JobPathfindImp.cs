using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
namespace CaomaoFramework
{
    public class JobPathfindImp : IPathFindModule
    {
        private IMap m_oMap;
        private List<Path2D> m_list2dPaths = new List<Path2D>();
        private List<Action<NativeList<FPVector2>>> m_callbacks = new List<Action<NativeList<FPVector2>>>();
        private int m_listIndex = 0;

        public int MaxFindPathPointCount
        {
            get; set;
        } = 10;
        public bool FullParallel
        {
            get; set;
        } = false;

        public void Init()
        {
            
        }

        public int StartFind2DPath(FPVector2 startPos, FPVector2 endPos,Action<NativeList<FPVector2>> callback)
        {
            if (callback == null)
            {
                Debug.LogError("PathCallback == null");
                throw new Exception("寻路异常");
            }
            //构建一个path，然后在job里面计算，得出路径传给nativearray
            var path = new Path2D(startPos, endPos);
            this.m_list2dPaths.Add(path);
            this.m_callbacks.Add(callback);
            return this.m_listIndex++;
        }

        public void Update()
        {
            if (this.m_listIndex > 0)
            {
                if (this.FullParallel)
                {
                    var paths = new NativeArray<Path2D>(this.m_listIndex, Allocator.TempJob);
                    var targetPaths = new NativeMultiHashMap<int, FPVector2>(this.m_listIndex * this.MaxFindPathPointCount, Allocator.TempJob);
                    for (int i = 0; i < this.m_listIndex; i++)
                    {
                        var path = this.m_list2dPaths[i];
                        paths[i] = path;
                    }
                    var job = new Job2DPathParallelProcess();
                    job.ABPath = paths;
                    job.Paths = targetPaths.AsParallelWriter();
                    var handle = job.Schedule(this.m_listIndex, 32);
                    handle.Complete();
                    //然后传递数据给主线程
                    for (int i = 0; i < this.m_listIndex; i++)
                    {
                        var callback = this.m_callbacks[i];
                        var array = new NativeList<FPVector2>(Allocator.Persistent);
                        foreach (var temp in targetPaths.GetValuesForKey(i))
                        {
                            array.Add(temp);
                        }
                        callback?.Invoke(array);
                    }
                    this.m_listIndex = 0;
                    this.m_list2dPaths.Clear();
                    paths.Dispose();
                    targetPaths.Dispose();
                }
                else
                {
                    var allJobs = new NativeArray<JobHandle>(this.m_listIndex, Allocator.Temp);
                    var data = new NativeList<FPVector2>[this.m_listIndex];
                    for (int i = 0; i < this.m_listIndex; i++)
                    {
                        var path = this.m_list2dPaths[i];
                        var job = new Job2DPathIJobProcess();
                        job.ABPath = path;
                        job.Paths = new NativeList<FPVector2>(5,Allocator.TempJob);
                        data[i] = job.Paths;
                        allJobs[i] = job.Schedule();
                    }
                    JobHandle.CompleteAll(allJobs);
                    for (int i = 0; i < this.m_listIndex; i++)
                    {
                        var d = data[i];
                        var callback = this.m_callbacks[i];
                        callback?.Invoke(d);
                    }
                    allJobs.Dispose();
                }
            }
        }
    }
}
