using UnityEngine;
using System.Collections;
using System;

namespace CaomaoFramework 
{
    public class CaomaoVideoModule : ICaomaoVideoModule
    {
        private ICaomaoVideoModule m_videoImp;

        public void Awake(Transform root) 
        {
            this.m_videoImp = new VideoPlayerImp();
            this.m_videoImp.Awake(root);
        }

        public void Pause()
        {
            this.m_videoImp.Pause();
        }

        public void Play()
        {
            this.m_videoImp.Play();
        }

        public void SetOnFinisedCallback(Action onFinished)
        {
            this.m_videoImp.SetOnFinisedCallback(onFinished);
        }

        public void Stop()
        {
            this.m_videoImp.Stop();
        }
    }
}

