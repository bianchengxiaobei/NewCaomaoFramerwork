using System;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;

public class CaomaoGifFrameJob : IJob
{
    public float delay;
    public NativeList<byte> imageData;
    public NativeList<byte> gifData;
    public int position;//读取的位置

    public bool lctFlag = false;
    public int lctSize = 0;

    public void Execute()
    {
         
    }

    private void ReadNextFrame(int position)
    {
        //this.position++;
        //var code = this.gifData[this.position];
        var code = this.ReadByte();
        switch (code)
        {
            case 0x2C:
                this.ReadImage();
                break;
            case 0x21:
                break;
            case 0x3b:
                break;
            case 0x00:
                break;
            default:
                //表示出错
                break;
        }
    }
    private int ReadByte()
    {
        var data = this.gifData[this.position++];
        return data;
    }
    private void ReadImage()
    {
        var imageX = this.ReadShort();
        var imageY = this.ReadShort();
        var imageWidth = this.ReadShort();
        var imageHeight = this.ReadShort();

        var packed = this.ReadByte();




    }

    private int ReadShort()
    {
        var first = this.ReadByte();
        var second = this.ReadByte();
        return first | (second << 8);
    }
}

