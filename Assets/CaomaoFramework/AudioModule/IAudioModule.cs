using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public interface IAudioModule 
    {
        float BGMusicVolume { get; set; }
        float UIEffectVolume { get; set; }
        bool IsMuted { get; set; }

        bool EnableBGMusic { get; set; }
        bool EnableEffect { get; set; }
        bool EnableBGMusicFadeOutIn { get; set; }

        void InitVolume();
        bool PlayBGMusic(string bgMusicName);
        bool PlayBGMusic(AudioClip bgMusicClip);
        void PlayAudioSource(AudioSource source, AudioClip clip);
        void StopMusic();
        void FadeOutBGMusic(float duration = 2f);
        void FadeInBGMusic(string musicClipName, float duration = 2f);
    }
}
