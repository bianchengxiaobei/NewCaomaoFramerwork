using System;

namespace CaomaoFramework
{
    public interface ITimerModule
    {
        void Init();
        void Update();
        void Reset();

        int AddTimerTask(uint delay, uint duration, Action callback, bool loop = false);
        int AddTimerTask<T>(uint delay, uint duration, Action<T> callback, T arg, bool loop = false);
        int AddTimerTask<T, V>(uint delay, uint duration, Action<T,V> callback, T arg1, V arg2, bool loop = false);

        void DelTimer(int timerId);
    }
}
