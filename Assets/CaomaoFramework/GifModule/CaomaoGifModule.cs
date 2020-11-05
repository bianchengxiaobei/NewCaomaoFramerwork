using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Unity.Jobs;
using Unity.Collections;
namespace CaomaoFramework
{
    public class CaomaoGifModule : IModule
    {
        private Component m_render;//gif渲染组件
        private int m_loop;//循环次数
        private string filePath;
        private const string GIFExtension = ".gif";
        private Texture2D GifTexture;
        private ICaomaoGIFDecoder m_gifDecoder;
        private bool m_bIsDecoding = false;//是否正在解析
        public void Init()
        {
            this.m_gifDecoder = new CaomaoGIFDecoder();
        }


        public void PlayGif(string fileName, int loop = -1)
        {
            if (this.m_gifDecoder == null)
            {
                Debug.LogError("GIF Decoder == null");
                return;
            }
            if (this.m_bIsDecoding)
            {
                //如果正在解析，得先直接暂停这个job，然后重新开始解析新的

            }
            this.filePath = CaomaoGameGobalConfig.Instance.GIFLoadPathDir + "/" + fileName;
            if (this.filePath.EndsWith(".gif") == false) 
            {
                this.filePath = this.filePath + GIFExtension;
            }
            this.LoadGif(this.OnFinished);
        }

        private async void LoadGif(Action onFinished) 
        {
            Debug.Log(this.filePath);
            var data = await new WebRequestModule().LoadLocalBytesNoCallback(this.filePath,null);
            if (data != null && data.Length > 0) 
            {
                //开启线程或者job线程加载gif数据
                this.m_gifDecoder.Init(new MemoryStream(data));
                this.m_gifDecoder.Decode();
            }
        }
        private JobHandle lastJobHandler;
        private void OnFinished() 
        {
            
        }

        private void CreateTaregetImageTexture()
        {
            if (this.GifTexture != null && this.GifTexture.width == this.m_gifDecoder.Width 
                && this.GifTexture.height == this.m_gifDecoder.Height)
            {
                return;
            }
            if (this.GifTexture == null || this.m_gifDecoder.Width == 0 || this.m_gifDecoder.Height == 0)
            {
                this.GifTexture = Texture2D.blackTexture;
                return;
            }
            if (this.GifTexture != null && this.GifTexture.hideFlags == HideFlags.HideAndDontSave)
            {
                GameObject.Destroy(this.GifTexture);
            }
            this.GifTexture = new Texture2D(this.m_gifDecoder.Width, this.m_gifDecoder.Height, TextureFormat.RGBA32, false);
            this.GifTexture.hideFlags = HideFlags.HideAndDontSave;
        }



        public void Update()
        {
            //开始播放如果解析完成的话

        }
    }

}
