using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public abstract class TimerBase:IClassInstance
    {
        public int TimerId { get; set; }
        public uint Duration { get; set; }

        public uint NextDuration { get; set; }

        public bool Loop { get; set; } = false;//不循环

        public abstract void Callback();

        public abstract void RecyleSelf(TimerBase data);

        public void OnAlloc()
        {
            
        }

        public virtual void OnRelease()
        {
            this.Loop = false;
            this.NextDuration = 0;
            this.TimerId = -1;
            this.Duration = 0;
        }

        public abstract void SetCallback(Delegate callback);
    }
    [CInstanceNumber(2)]
    public class TimerData : TimerBase
    {
        private Action callback;

        public override void Callback()
        {
            this.callback?.Invoke();
        }

        public override void RecyleSelf(TimerBase data)
        {
            var d = data as TimerData;
            ClassPoolModule<TimerData>.Release(ref d);
        }

        public override void SetCallback(Delegate callback)
        {
            this.callback = callback as Action;
        }
        public override void OnRelease()
        {
            base.OnRelease();
            this.callback = null;
        }
    }

    [CInstanceNumber(2)]
    public class TimerData<T> : TimerBase
    {
        private Action<T> callback;

        public T Arg
        {
            get;set;
        }

        public void SetParam(T arg)
        {
            this.Arg = arg;
        }

        public override void Callback()
        {
            this.callback?.Invoke(this.Arg);
        }

        public override void SetCallback(Delegate callback)
        {
            this.callback = callback as Action<T>;
        }

        public override void RecyleSelf(TimerBase data)
        {
            var d = data as TimerData<T>;
            ClassPoolModule<TimerData<T>>.Release(ref d);
        }
        public override void OnRelease()
        {
            base.OnRelease();
            this.callback = null;
        }
    }
    [CInstanceNumber(2)]
    public class TimerData<T, V> : TimerBase
    {
        private Action<T,V> callback;

        public T Arg1
        {
            get; set;
        }

        public V Arg2
        {
            get;set;
        }

        public void SetParam(T arg1,V arg2)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;
        }

        public override void Callback()
        {
            this.callback?.Invoke(this.Arg1,this.Arg2);
        }

        public override void SetCallback(Delegate callback)
        {
            this.callback = callback as Action<T,V>;
        }

        public override void RecyleSelf(TimerBase data)
        {
            var d = data as TimerData<T,V>;
            ClassPoolModule<TimerData<T,V>>.Release(ref d);
        }
        public override void OnRelease()
        {
            base.OnRelease();
            this.callback = null;
        }
    }

}
