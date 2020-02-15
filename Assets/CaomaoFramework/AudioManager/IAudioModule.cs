using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public interface IAudioModule 
    {
        void InitVolume();
        void PlayBGMusic(string bgMusicName);
        void PlayBGMusic(AudioClip bgMusicClip);
        void StopMusic();
        void FadeOutBGMusic(float duration = 2f);
        void FadeInBGMusic(string musicClipName, float duration = 2f);
    }
}
