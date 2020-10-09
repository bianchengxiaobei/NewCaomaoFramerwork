using System;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.IO.LowLevel.Unsafe;
using Unity.Collections;
using System.IO;
using CaomaoFramework;
using System.Runtime.InteropServices;


public class Test : MonoBehaviour
{
    //private ReadHandle readHandle;
    //NativeArray<ReadCommand> cmds;
    //private CaomaoGifModule module = new CaomaoGifModule();
    // public CUIScrollBroadcast a;
    private WordFilterModule module = new WordFilterModule();
    private int a = 4;
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
        //module.Init();
    }

    private unsafe void TestCollectionHelp()
    {
        TestA testA;
        testA.a = 1;
        testA.b = 2;
        TestA[] test = new TestA[2];
        test[0] = testA;
        test[1] = testA;
        var allSize = sizeof(TestA) * test.Length;
        var intPrt = Marshal.AllocHGlobal(allSize);
        Marshal.StructureToPtr<TestA[]>(test, intPrt, true);
        var hash = Test.Hash(intPrt.ToPointer(), allSize);
        Debug.Log(hash);
        //Marshal.Release(intPrt);
    }

    public unsafe static uint Hash(void* pointer, int bytes)
    {
        ulong hash = 5381u;
        while (bytes > 0)
        {
            int num = --bytes;
            ulong c = (ulong)((byte*)pointer)[num];
            Debug.Log("c:"+c);
            hash = (hash << 5) + hash + c;
            Debug.Log("hash:" + hash);
        }
        return (uint)hash;
    }

    private void Start()
    {
        //module.Init();
        //var replace = module.Replace("金三胖3r3r");
        //Debug.Log(replace);
        //this.TestCollectionHelp();
        var bitArray = new UnsafeBitArray(129,Allocator.Temp);
        bitArray.Set(0, true);
        bitArray.Set(1, true);
        bitArray.Set(5, true);
        Debug.Log("Length:"+bitArray.Length);
        for (int i = 0; i < bitArray.Length; i++)
        {
            var value = bitArray.GetBits(i);
            Debug.Log("index:"+i);
            var value2 = bitArray.TestAll(i);
            Debug.Log(value2);
        }
    }

    public struct TestA
    {
        public int a;
        public byte b;
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