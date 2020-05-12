using System;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// 解析GIF图片数据的接口
/// </summary>
public interface ICaomaoGIFDecoder
{
    void ReadHeader();//读取gif图片头部信息
    void SetStream(Stream stream);
}

