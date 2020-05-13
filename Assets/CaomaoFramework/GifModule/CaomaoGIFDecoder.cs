using System;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// GIF图片数据解析
/// </summary>
public class CaomaoGIFDecoder : ICaomaoGIFDecoder
{
    private MemoryStream stream;
    private byte[] m_readCache = new byte[256];//读取的缓存区
    private int m_frameCount = 0;
    private bool m_bFinished = false;
    private int[] golbalColorTable = null;
    private int[] localColorTable = null;
    private bool m_bHasError = false;//是否解析的过程中出现错误

    private int m_width;
    private int m_height;
    private bool m_golbalColorTableFlag = false;
    private int m_golbalColorTabSize = 0;
    private int m_bgIndex = 0;
    private int m_bgColor = 0;

    private int m_pixelAspectRatio = 0;

    private long m_imageDataPosition = 0;//image的Stream的读取位置





    /// <summary>
    /// 解析GIF的头部信息
    /// </summary>
    public void ReadHeader()
    {
        var id = this.ReadString(6);//前面6个字符是GIF89a（GIF署名）
        if (id.StartsWith("GIF") == false)
        {
            //说明不是GIF格式的
            this.m_bHasError = true;
        }
        this.ReadLsd();
        if (this.m_golbalColorTableFlag)
        {
            Debug.Log("GCTSize:" + this.m_golbalColorTabSize);
            this.golbalColorTable = this.CreateGlobalColorTable(this.m_golbalColorTabSize);
            this.m_bgColor = this.golbalColorTable[this.m_bgIndex];
        }
        this.m_imageDataPosition = this.stream.Position;
        Debug.Log("数据读取位置:"+this.m_imageDataPosition);
    }

    private int[] CreateGlobalColorTable(int ncolors)
    {
        var nbytes = 3 * ncolors;
        int[] tab = null;
        var c = new byte[nbytes];
        var n = 0;
        try
        {
            n = this.stream.Read(c, 0, c.Length);
        }
        catch (IOException)
        {
        }
        if (n < nbytes)
        {
            this.m_bHasError = true;
        }
        else
        {
            tab = new int[256]; // max size to avoid bounds checks
            var i = 0;
            var j = 0;
            while (i < ncolors)
            {
                uint r = (c[j++]);
                var g = (c[j++]) & (uint)0xff;
                var b = (c[j++]) & (uint)0xff;
                tab[i++] = (int)(0xff000000 | (b << 16) | (g << 8) | r);
            }
        }
        return tab;
    }


    /// <summary>
    /// 逻辑屏幕标识符（Logical Screen Descriptor）
    /// 8bit Width
    /// 8bit Heigth
    /// 1bit m 3bit cr 1bit s 3bit pixel
    /// 8bit 背景色
    /// 8bit Pixel Aspect Radio
    /// </summary>
    private void ReadLsd()
    {
        this.m_width = this.ReadShort();
        this.m_height = this.ReadShort();
        var packed = this.ReadByte();
        this.m_golbalColorTableFlag = (packed & 0x80) != 0; // 1   : global color table flag
                                                            // 2-4 : color resolution
                                                            // 5   : gct sort flag
        Debug.Log(this.m_golbalColorTableFlag);
        this.m_golbalColorTableFlag = (packed >> 7) > 0;
        Debug.Log(this.m_golbalColorTableFlag);
        //7 -> 0111
        this.m_golbalColorTabSize = 2 << (packed & 7); // 6-8 : gct size
        this.m_bgIndex = this.ReadByte();
        this.m_pixelAspectRatio = this.ReadByte();
    }



    public void SetStream(Stream stream)
    {
        if (this.stream == null)
        {
            this.stream = stream as MemoryStream;
        }
    }



    private void Reset()
    {
        this.m_frameCount = 0;
        this.m_bFinished = false;
        this.golbalColorTable = null;
        this.localColorTable = null;
    }



    /// <summary>
    /// 从流里面读取一个char
    /// </summary>
    /// <returns></returns>
    private char ReadChar()
    {
        try
        {
            var curChar = this.stream.ReadByte();
            return (char)curChar;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return char.MinValue;
        }
    }
    private int ReadByte()
    {
        try
        {
            var curByte = this.stream.ReadByte();
            return curByte;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return 0;
        }
    }



    /// <summary>
    /// 读取一个16bit的数据
    /// </summary>
    /// <returns></returns>
    private int ReadShort()
    {
        return ReadByte() | (ReadByte() << 8);
    }




    private string ReadString(int stringLen)
    {
        //this.sb.Clear();
        try
        {
            var readCount = this.stream.Read(this.m_readCache, (int)this.stream.Position, stringLen);
            //从cache里面转成字符串     
            var content = Encoding.UTF8.GetString(this.m_readCache, 0, readCount);
            return content;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }     
    }


}

