using System;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.IO.LowLevel.Unsafe;
using Unity.Collections;
using System.IO;
using CaomaoFramework;

public class Test : MonoBehaviour
{
    //private ReadHandle readHandle;
    //NativeArray<ReadCommand> cmds;
    private CaomaoGifModule module = new CaomaoGifModule();
    //public unsafe void Start()
    //{
    //    string filePath = Path.Combine(Application.streamingAssetsPath, "myfile.bin");
    //    cmds = new NativeArray<ReadCommand>(1, Allocator.Persistent);
    //    ReadCommand cmd;
    //    cmd.Offset = 0;
    //    cmd.Size = 1024;
    //    cmd.Buffer = (byte*)UnsafeUtility.Malloc(cmd.Size, 16, Allocator.Persistent);
    //    cmds[0] = cmd;
    //    Debug.Log(filePath);
    //    readHandle = AsyncReadManager.Read(filePath, (ReadCommand*)cmds.GetUnsafePtr(), 1);
    //}

    //public unsafe void Update()
    //{
    //    if (readHandle.IsValid() && readHandle.Status != ReadStatus.InProgress)
    //    {
    //        Debug.LogFormat("Read {0}", readHandle.Status == ReadStatus.Complete ? "Successful" : "Failed");
    //        readHandle.Dispose();
    //        UnsafeUtility.Free(cmds[0].Buffer, Allocator.Persistent);
    //        cmds.Dispose();
    //    }
    //}
    //private byte[] a = new byte[256];

    //private void Awake()
    //{
    //    AsyncReadManager.Read();
    //}

    //private async void Start()
    //{

    //    //Debug.Log(data.Length);
    //}

    //private unsafe void Update()
    //{
    //    var job = new JOb1();
    //    job.a = 1;

    //    fixed (byte* temp = this.a)
    //    {
    //        job.b = temp;
    //    }
    //    var handler = job.Schedule();
    //    handler.Complete();
    //} 
    private void Awake()
    {
        module.Init();
    }

    private void Start()
    {
        module.PlayGif("test.gif");
    }

    private void TestAction()
    {
        Debug.Log("TestAction");
    }
}

public unsafe struct JOb1 : IJob
{
    public int a;
    public byte* b;

    public void Execute()
    {
        Debug.Log(a);
    }
}