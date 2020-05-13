using System;
using System.Collections.Generic;
using Unity.Collections;
namespace CaomaoFramework
{
    public struct CaomaoGIFImageData
    {
        public NativeArray<short> m_prefix;
        public NativeArray<byte> m_suffix;
        public NativeArray<byte> m_pixelStack;
        public NativeArray<byte> m_pixels;

        public const int MaxSize = 4096;



        public void Alloctor(int npix)
        {
            this.m_pixels = new NativeArray<byte>(npix, Allocator.TempJob);
            this.m_prefix = new NativeArray<short>(MaxSize, Allocator.TempJob);
            this.m_suffix = new NativeArray<byte>(MaxSize, Allocator.TempJob);
            this.m_pixelStack = new NativeArray<byte>(MaxSize + 1, Allocator.TempJob);
        }

    }
}


