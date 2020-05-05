
using System;
namespace CaomaoFramework 
{
    public abstract class NewbieHelpStep : IClassInstance
    {
        public int ID { get; set; }//任务id
        public int MainID { get; set; }//主任务id
        public Action ActionOnFinished { get; set; }//回调

        public bool bCheck { get; set; }//是否需要检测，比如按钮需要某个事件完成之后才能继续

        public virtual void OnFinished()
        {
            //结束回调
            this.ActionOnFinished?.Invoke();
            this.ID = 0;
            this.ActionOnFinished = null;
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
        public virtual void OnAlloc()
        {
            
        }

        public virtual void OnRelease()
        {
            
        }
    }

}

