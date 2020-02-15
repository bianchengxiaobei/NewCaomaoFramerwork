using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    [Module(true)]
    public class TimerModule : IModule, ITimerModule
    {
        public ITimerModule timerImp = new StopWatchTimerImp();
        public static int TimeIdIndex = 0;
        private float time = 0;
        public void Init()
        {
            this.timerImp.Init();
        }

        public void Update()
        {
            this.time += Time.deltaTime;
            if (this.time >= 0.01f)
            {
                this.time = 0;
                this.timerImp.Update();
            }          
        }

        public void Reset()
        {
            TimeIdIndex = 0;
            this.time = 0;
            this.timerImp.Reset();
        }

        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="frameCount"></param>
        /// <param name="callback"></param>
        /// <param name="loop">大于0表示循环，=-1表示不循环</param>
        /// <returns></returns>
        public int AddTimerTask(uint delay, uint frameCount, Action callback, bool loop = false)
        {
            return this.timerImp.AddTimerTask(delay, frameCount, callback, loop);
        }

        public int AddTimerTask<T>(uint delay, uint frameCount, Action<T> callback, T arg, bool loop = false)
        {
            return this.timerImp.AddTimerTask(delay, frameCount, callback,arg, loop);
        }

        public int AddTimerTask<T, V>(uint delay, uint frameCount, Action<T,V> callback, T arg1, V arg2, bool loop = false)
        {
            return this.timerImp.AddTimerTask(delay, frameCount, callback,arg1,arg2 ,loop);
        }

        public void DelTimer(int timerId)
        {
            this.timerImp.DelTimer(timerId);
        }
    }
}
