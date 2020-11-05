using System;
using System.Collections.Generic;
using Unity.Collections;
/// <summary>
/// 存储每帧的图像数据
/// </summary>
public struct CaomaoGifFrame
{
    public NativeArray<byte> imageData;//每帧的图像数据
    public float delay;//每帧的时间延迟
}

