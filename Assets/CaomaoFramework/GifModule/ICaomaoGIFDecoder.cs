using System;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// 解析GIF图片数据的接口
/// </summary>
public interface ICaomaoGIFDecoder
{
    void Init(Stream stream);//初始化nativeArray等集合数据
    //void ReadHeader();//读取gif图片头部信息
    void Decode();
    int Width { get; }
    int Height { get; }
}

