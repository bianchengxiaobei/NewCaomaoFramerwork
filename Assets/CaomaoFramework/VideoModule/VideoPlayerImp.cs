using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using System;

namespace CaomaoFramework
{
    public class VideoPlayerImp : ICaomaoVideoModule
    {
        private VideoPlayer m_player;
        private Action m_actionFinished;
        public void Awake(Transform root) 
        {
            this.m_player = root.GetComponent<VideoPlayer>();
            if (!this.m_player)
            {
                Debug.LogError("最好在本层级放VideoPlayer");
                this.m_player = root.GetComponentInChildren<VideoPlayer>();
            }
            this.m_player.playOnAwake = false;
            this.m_player.aspectRatio = VideoAspectRatio.Stretch;
        }


        public void Pause()
        {
            this.m_player.Pause();
        }

        public void Play()
        {
            this.m_player.Play();
        }

        public void SetOnFinisedCallback(Action onFinished)
        {
            this.m_actionFinished = onFinished;
            this.m_player.loopPointReached += this.OnFinished;
        }

        private void OnFinished(VideoPlayer player) 
        {
            Debug.Log("Finished");
            this.m_actionFinished?.Invoke();
        }


        public void Stop()
        {
            this.m_player.Stop();
        }
    }
}

