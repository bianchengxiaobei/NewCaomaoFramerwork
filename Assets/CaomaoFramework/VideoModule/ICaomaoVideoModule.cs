using UnityEngine;
using System;
namespace CaomaoFramework 
{
    public interface ICaomaoVideoModule
    {
        void Awake(Transform root);
        void Play();
        void Stop();
        void Pause();
        void SetOnFinisedCallback(Action onFinished);
    }
}
