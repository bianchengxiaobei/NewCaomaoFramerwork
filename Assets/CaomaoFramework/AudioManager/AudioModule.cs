using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    [Module(false)]
    public class AudioModule : IAudioModule, IModule
    {
        private float m_bgMusicVolume = 1;
        public float BGMusicVolume
        {
            get
            {
                return this.m_bgMusicVolume;
            }
            set
            {
                this.m_bgMusicVolume = value;
                if (this.m_oBGMusicSource != null)
                {
                    this.m_oBGMusicSource.volume = this.m_bgMusicVolume;
                }
            }
        }
        public float UIEffectVolume { get; set; } = 1;

        public bool IsMuted { get; set; }

        public bool EnableBGMusic { get; set; }
        public bool EnableEffect { get; set; }

        public bool EnableBGMusicFadeOutIn { get; set; } = true;



        private string m_sCurPlayingBGMusicName;
        private AudioClip m_oCurPlayingBGMusicClip;

        //private bool m_bFadeOutStart = false;
        private float m_fFadeDuration = 0;
        //private float m_tempFadeOutTime = 0;
        private int m_fadeOutTimerId = 0;
        private int m_fadeInTimerId = 0;
        private float m_fFadeOutTempVolume;
        private float m_fFadeInTempVolume;

        private GameObject AudioListener;
        private AudioSource m_oBGMusicSource;
        private AudioSource m_oUIEffectSource;

        public void Init()
        {
            //创建一个Object，然后添加AudioSource
            if (this.AudioListener != null)
            {
                return;
            }
            this.AudioListener = new GameObject("AudioListener");
            GameObject.DontDestroyOnLoad(this.AudioListener);
            this.m_oBGMusicSource = this.AudioListener.AddComponent<AudioSource>();
            this.m_oUIEffectSource = this.AudioListener.AddComponent<AudioSource>();
            //初始化音量
            this.InitVolume();
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="bgMusicName"></param>
        public void PlayBGMusic(string bgMusicName)
        {
            if (this.m_oBGMusicSource == null)
            {
                this.m_oBGMusicSource = this.AudioListener.AddComponent<AudioSource>();
            }
            else
            {
                if (this.m_sCurPlayingBGMusicName != bgMusicName)
                {
                    CaomaoDriver.ResourceModule.LoadAsset(bgMusicName, this.LoadBGMusicFinished);
                    this.m_sCurPlayingBGMusicName = bgMusicName;
                }
                else if (this.m_oBGMusicSource.isPlaying == false)
                {
                    this.m_oBGMusicSource.Play();
                }
            }
        }
        public void PlayBGMusic(AudioClip bgMusicClip)
        {
            if (this.m_oBGMusicSource == null)
            {
                this.m_oBGMusicSource = this.AudioListener.AddComponent<AudioSource>();
            }
            else
            {
                if (this.m_oCurPlayingBGMusicClip != bgMusicClip && this.m_sCurPlayingBGMusicName != bgMusicClip.name)
                {
                    this.m_sCurPlayingBGMusicName = bgMusicClip.name;                   
                    this.LoadBGMusicFinished(bgMusicClip);
                }
                else if (this.m_oBGMusicSource.isPlaying == false)
                {
                    this.m_oBGMusicSource.Play();
                }
            }
        }

        public void StopMusic()
        {
            if (this.m_oBGMusicSource == null)
            {
                this.m_oBGMusicSource = this.AudioListener.AddComponent<AudioSource>();
            }
            this.m_oBGMusicSource.Stop();
        }

        public void FadeInBGMusic(string musicClipName,float duration = 2f)
        {
            if (this.EnableBGMusicFadeOutIn == false)
            {
                return;
            }
            if (this.m_oBGMusicSource == null)
            {
                this.m_oBGMusicSource = this.AudioListener.AddComponent<AudioSource>();
            }      
            this.m_fFadeDuration = duration > 0 ? duration * 0.01f : 0.02f;
            this.m_fFadeInTempVolume = this.BGMusicVolume;
            this.BGMusicVolume = 0;
            this.PlayBGMusic(musicClipName);
            //然后替换音乐
            this.m_fadeInTimerId = CaomaoDriver.TimerModule.AddTimerTask<string>(0, 200, this.FadeInUpdate,musicClipName,true);
        }

        private void FadeInUpdate(string musicName)
        {
            float v = 0;
            this.BGMusicVolume = Mathf.SmoothDamp(this.BGMusicVolume, this.m_fFadeInTempVolume + 1f, ref v,this.m_fFadeDuration);
            if (this.BGMusicVolume >= this.m_fFadeInTempVolume)
            {
                this.BGMusicVolume = this.m_fFadeInTempVolume;
                CaomaoDriver.TimerModule.DelTimer(this.m_fadeInTimerId);
            }
        }

        public void FadeOutBGMusic(float duration = 2f)
        {
            if (this.EnableBGMusicFadeOutIn == false)
            {
                return;
            }
            this.m_fFadeDuration = duration > 0 ? duration * 0.01f : 0.02f;
            this.m_fFadeOutTempVolume = this.BGMusicVolume;
            this.m_fadeOutTimerId = CaomaoDriver.TimerModule.AddTimerTask(0,200,this.FadeOutUpdate,true);
        }
        private void FadeOutUpdate()
        {
            float v = 0;
            this.BGMusicVolume = Mathf.SmoothDamp(this.BGMusicVolume, -1, ref v, this.m_fFadeDuration);
            if (this.BGMusicVolume <= 0)
            {
                this.BGMusicVolume = this.m_fFadeOutTempVolume;
                this.StopMusic();
                CaomaoDriver.TimerModule.DelTimer(this.m_fadeOutTimerId);
            }
        }
        private void LoadBGMusicFinished(UnityEngine.Object asset)
        {
            var audioClip = asset as AudioClip;
            if (audioClip != null)
            {
                this.m_oCurPlayingBGMusicClip = audioClip;
                this.m_oBGMusicSource.clip = audioClip;
                this.m_oBGMusicSource.Play();
            }
            else
            {
                Debug.LogError("AudioClip == null:"+ this.m_sCurPlayingBGMusicName);
            }
        }

        public void InitVolume()
        {

        }


        public void Update()
        {
            //if (this.m_bFadeOutStart == false)
            //{
            //    return;
            //}
            
        }
    }
}
