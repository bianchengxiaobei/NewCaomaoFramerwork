using System;
using System.IO;
using System.Text;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
namespace CaomaoFramework
{
    /// <summary>
    /// GIF图片数据解析
    /// </summary>
    public class CaomaoGIFDecoder : ICaomaoGIFDecoder
    {
        private MemoryStream stream;
        private byte[] m_readCache = new byte[256];//读取的缓存区
                                                   //private int[] localColorTable = null;
        private bool m_bHasError = false;//是否解析的过程中出现错误

        private bool m_golbalColorTableFlag = false;
        private int m_golbalColorTabSize = 0;
        private int m_bgIndex = 0;
        private int m_bgColor = 0;

        private int m_pixelAspectRatio = 0;

        private long m_imageDataPosition = 0;//image的Stream的读取位置


        public int Width => this.GifWidth[0];//图像宽度
        public int Height => this.GifHeigth[0];//图像高度

        public NativeList<int> Position;//读取的位置
        public NativeList<bool> AllFrameFinished;//是否所有的帧已经完成
        public NativeList<int> AllFrameCount;//所有帧的数量
        public NativeList<CaomaoGifFrame> AlreadyFrames;//已经读取的帧的数据
        //全局数据    
        public NativeList<byte> GifStreamData;//Gif的流数据
        public NativeList<int> GifHeigth;//全局的高度
        public NativeList<int> GifWidth;//全局的宽度
        public NativeList<int> GobalBgColor;//全局背景颜色
        public NativeList<int> LastDispose;//上一帧的数据
        public NativeList<int> LastImageX;
        public NativeList<int> LastImageY;
        public NativeList<int> LastImageWidth;
        public NativeList<int> LastImageHeigth;
        public NativeList<int> LastBgColor;

        private const int MaxJobCount = 300;
        private const int MaxFrameCount = 20;//默认初始化collection的大小


        public void Init(Stream stream)
        {
            this.SetStream(stream);
            this.Position = new NativeList<int>(1, Allocator.Persistent);
            this.AllFrameFinished = new NativeList<bool>(1, Allocator.Persistent);
            this.AllFrameCount = new NativeList<int>(1, Allocator.Persistent);
            this.AlreadyFrames = new NativeList<CaomaoGifFrame>(MaxFrameCount, Allocator.Persistent);
            this.GifStreamData = new NativeList<byte>((int)stream.Length, Allocator.Persistent);
            this.GifHeigth = new NativeList<int>(1, Allocator.Persistent);
            this.GifWidth = new NativeList<int>(1, Allocator.Persistent);
            this.LastDispose = new NativeList<int>(1, Allocator.Persistent);
            this.LastImageX = new NativeList<int>(1,Allocator.Persistent);
            this.LastImageY = new NativeList<int>(1,Allocator.Persistent);
            this.LastImageWidth = new NativeList<int>(1, Allocator.Persistent);
            this.LastImageHeigth = new NativeList<int>(1, Allocator.Persistent);
            this.LastBgColor = new NativeList<int>(1, Allocator.Persistent);
        }


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
                this.CreateGlobalColorTable(this.m_golbalColorTabSize, out this.GobalBgColor);
                this.m_bgColor = this.GobalBgColor[this.m_bgIndex];
            }
            this.m_imageDataPosition = this.stream.Position;
            Debug.Log("数据读取位置:" + this.m_imageDataPosition);
            this.Position[0] = (int)this.m_imageDataPosition;
        }
        private JobHandle lastJobHandler;
        /// <summary>
        /// 解析gif数据
        /// </summary>
        public void Decode()
        {
            this.ReadHeader();
            //然后new个job开始解析每一帧的image的数据
            for (int i = 0; i < MaxJobCount; i++)
            {
                CaomaoGifFrameJob job = new CaomaoGifFrameJob();
                //传递Position进去
                job.Position = this.Position;
                job.AllFrameCount = this.AllFrameCount;
                job.AllFrameFinished = this.AllFrameFinished;
                job.AlreadyFrames = this.AlreadyFrames;
                job.GifStreamData = this.GifStreamData;
                job.GifHeigth = this.GifHeigth;
                job.GifWidth = this.GifWidth;
                job.LastDispose = this.LastDispose;
                job.LastImageX = this.LastImageX;
                job.LastImageY = this.LastImageY;
                job.LastImageWidth = this.LastImageWidth;
                job.LastImageHeigth = this.LastImageHeigth;
                job.LastBgColor = this.LastBgColor;
                job.GobalBgColor = this.GobalBgColor;
                if (i == 0)
                {
                    this.lastJobHandler = job.Schedule();
                }
                else
                {
                    this.lastJobHandler = job.Schedule(this.lastJobHandler);
                }
            }
            this.lastJobHandler.Complete();
            Debug.Log("完成解析");

        }


        private void CreateGlobalColorTable(int ncolors, out NativeList<int> table)
        {
            var nbytes = 3 * ncolors;
            table = new NativeList<int>(256, Allocator.Persistent);
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
                //tab = new int[256]; // max size to avoid bounds checks
                var i = 0;
                var j = 0;
                while (i < ncolors)
                {
                    uint r = (c[j++]);
                    var g = (c[j++]) & (uint)0xff;
                    var b = (c[j++]) & (uint)0xff;
                    table[i++] = (int)(0xff000000 | (b << 16) | (g << 8) | r);
                }
            }
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
            this.GifWidth[0] = this.ReadShort();
            this.GifHeigth[0] = this.ReadShort();
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
            this.AllFrameCount[0] = 0;
            this.AllFrameFinished[0] = false;
            this.GobalBgColor.Dispose();
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
}


