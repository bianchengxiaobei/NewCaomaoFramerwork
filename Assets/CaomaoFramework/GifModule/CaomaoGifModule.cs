using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CaomaoFramework
{
    public class CaomaoGifModule : IModule
    {
        private Component m_render;//gif渲染组件
        private int m_loop;//循环次数
        private string filePath;
        private const string GIFExtension = ".gif";

        private ICaomaoGIFDecoder m_gifDecoder;

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
                this.m_gifDecoder.SetStream(new MemoryStream(data));
                this.m_gifDecoder.ReadHeader();
            }
        }

        private void OnFinished() 
        {

        }


        public void Update()
        {
            
        }
    }

}
