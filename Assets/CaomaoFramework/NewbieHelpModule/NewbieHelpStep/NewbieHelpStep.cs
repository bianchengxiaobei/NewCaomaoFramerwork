using System;
using System.Collections.Generic;

public abstract class NewbieHelpStep
{
    public int ID { get; set; }//任务id
    public int MainID { get; set; }//主任务id
    public Action ActionOnFinished { get; set; }//回调


    public virtual void OnFinished()
    {
        //结束回调
        this.ActionOnFinished?.Invoke();
    }

    /// <summary>
    /// 加载该步骤引导数据
    /// </summary>
    public virtual void LoadHelpStepData()
    {

    }
    /// <summary>
    /// 进入，注册一些事件，比如按钮这些
    /// </summary>
    public virtual void Enter()
    {
        this.LoadHelpStepData();//加载数据
    } 





    public virtual void Clear()
    {
        this.ID = 0;
        this.ActionOnFinished = null;
    }
}

