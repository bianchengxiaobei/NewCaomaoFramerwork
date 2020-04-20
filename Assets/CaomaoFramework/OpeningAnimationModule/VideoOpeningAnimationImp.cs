using System;
using UnityEngine;
using UnityEngine.Video;
namespace CaomaoFramework 
{
    public class VideoOpeningAnimationImp : IOpeningAnimationModule
    {
        private CaomaoVideoModule m_videoPlayer;
        //private Action m_actionOnFinished;
        //private Transform m_root;
        public void Awake(Transform root)
        {
            //this.m_root = root;
            this.m_videoPlayer = new CaomaoVideoModule();
            this.m_videoPlayer.Awake(root);
        }
        
        public void SetOnFinishedCallback(Action onFinished)
        {
            this.m_videoPlayer.SetOnFinisedCallback(onFinished);
            //this.m_actionOnFinished = onFinished;
        }

        public void StartPlay()
        {
            this.m_videoPlayer.Play();
        }
    }
}
