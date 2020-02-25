using System;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IO.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Jobs;
using System.IO;
using CaomaoFramework;
using UnityEngine.Profiling;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
public class Test : MonoBehaviour
{
    public PathFindModule module;
    private long time;
    private int count = 10;
    private Stopwatch timer = new Stopwatch();
    //private Action<>
    private void Start()
    {
        //module = new PathFindModule();
        //module.Init();

        //module.StartFind2DPath(FPVector2.down, FPVector2.left, (ok) => 
        //{
        //    Debug.Log(ok.Length);
        //    foreach (var a in ok)
        //    {
        //        Debug.Log(a);
        //    }
        //    ok.Dispose();
        //});
        //module.StartFind2DPath(FPVector2.down, FPVector2.left, (ok) =>
        //{
        //    Debug.Log(ok.Length);
        //    foreach (var a in ok)
        //    {
        //        Debug.Log(a);
        //    }
        //    ok.Dispose();
        //});
        var temp = new NativeArray<JobHandle>(this.count, Allocator.Temp);
        var jobs = new NativeArray<int>[this.count];
        for (int i = 0; i < this.count; i++)
        {
            var j = new Job1();
            j.a = new NativeArray<int>(1, Allocator.TempJob);
            jobs[i] = j.a;
            var handler = j.Schedule();
            temp[i] = handler;
        }
        JobHandle.CompleteAll(temp);
        for (int i = 0; i < this.count; i++)
        {
            Debug.Log(jobs[i][0]);
            //jobs[i].Dispose();
        }      
        temp.Dispose();
    }

    private void OK(NativeArray<int> a)
    {

    }
    private void Update()
    {
      
    } 
}
public struct Job1 : IJob
{
    public NativeArray<int> a;

    public void Execute()
    {
        //Debug.Log("111");
        a[0] = 1;
    }
}
public struct Job2 : IJobParallelFor
{
    public NativeArray<int> a;
    public void Execute(int index)
    {
        var temp = a[index];
        a[index] = temp + temp;
    }
}