using System;
using System.Collections.Generic;

public abstract class NewbieHelpStep
{
    public int ID { get; set; }//任务id
    //public string Content { get; set; }//内容

    public Action ActionOnFinished { get; set; }//回调


    public virtual void OnFinished()
    {
        //结束回调
        this.ActionOnFinished?.Invoke();
    }

    public abstract void Enter();//进入，注册一些事件，比如按钮这些


    public virtual void Clear()
    {
        this.ID = 0;
        this.ActionOnFinished = null;
    }
}

