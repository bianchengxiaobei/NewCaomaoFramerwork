using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public class FrameTimerImp : ITimerModule
    {
        private KeyedPriorityQueue<int, TimerBase,uint> m_queue;
        private uint m_iTempFrameCount;
        public void Init()
        {
            this.m_queue = new KeyedPriorityQueue<int, TimerBase, uint>();
            this.m_iTempFrameCount = 0;
        }

        public int AddTimerTask(uint delay, uint frameCount, Action callback,bool loop = false)
        {
            var t = ClassPoolModule<TimerData>.Alloc();
            t = this.InitTimer(t,delay, frameCount, loop) as TimerData;
            t.SetCallback(callback);
            return t.TimerId;
        }

        public int AddTimerTask<T>(uint delay, uint frameCount, Action<T> callback,T arg, bool loop = false)
        {
            var t = ClassPoolModule<TimerData<T>>.Alloc();
            t = this.InitTimer(t, delay, frameCount,loop) as TimerData<T>;
            t.SetCallback(callback);
            t.Arg = arg;
            return t.TimerId;
        }

        public int AddTimerTask<T,V>(uint delay, uint frameCount, Action<T,V> callback, T arg1, V arg2,bool loop = false)
        {
            var t = ClassPoolModule<TimerData<T,V>>.Alloc();
            t = this.InitTimer(t, delay, frameCount, loop) as TimerData<T,V>;
            t.SetCallback(callback);
            t.Arg1 = arg1;
            t.Arg2 = arg2;
            return t.TimerId;
        }


        private TimerBase InitTimer(TimerBase timer, uint delay, uint duration, bool loop = false)
        {
            timer.Loop = loop;
            timer.TimerId = TimerModule.TimeIdIndex++;
            timer.Duration = duration;
            timer.NextDuration = this.m_iTempFrameCount + duration + 1 + delay;
            this.m_queue.Enqueue(timer.TimerId,timer,timer.Duration);
            return timer;
        }


        public void Update()
        {
            this.m_iTempFrameCount++;
            while (this.m_queue.Count != 0)
            {
                TimerBase p = this.m_queue.Peek();
                if (this.m_iTempFrameCount < p.NextDuration)
                {
                    break;
                }
                this.m_queue.Dequeue();
                if (p.Loop)
                {
                    //表示循环
                    p.NextDuration = p.Duration + this.m_iTempFrameCount;
                    this.m_queue.Enqueue(p.TimerId, p, p.NextDuration);
                    p.Callback();
                }
                else
                {
                    p.Callback();
                    p.RecyleSelf(p);
                }
            }
        }

        public void Reset()
        {
            this.m_iTempFrameCount = 0;
            while (this.m_queue.Count > 0)
            {
                var q = this.m_queue.Dequeue();
                q.RecyleSelf(q);              
            }
        }

        public void DelTimer(int timerId)
        {
            var q = this.m_queue.Remove(timerId);
            q.RecyleSelf(q);
        }
    }
}
