using UnityEngine;
using System.Collections;
using System;

namespace CaomaoFramework
{
    public class CaomaoGifModule : IModule
    {
        private Component m_render;//gif渲染组件
        private int m_loop;//循环次数
        private string filePath;
        private const string GIFExtension = ".gif";
        public void Init()
        {
            
        }


        public void PlayGif(string fileName, int loop = -1)
        {
            this.filePath = CaomaoGameGobalConfig.Instance.GIFLoadPathDir + "/" + fileName;
            if (this.filePath.EndsWith(".gif") == false) 
            {
                this.filePath = this.filePath + GIFExtension;
            }
            this.LoadGif(this.OnFinished);
        }

        private async void LoadGif(Action onFinished) 
        {
            var data = await CaomaoDriver.WebRequestModule.LoadLocalBytesNoCallback(this.filePath);
            if (data != null) 
            {

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
