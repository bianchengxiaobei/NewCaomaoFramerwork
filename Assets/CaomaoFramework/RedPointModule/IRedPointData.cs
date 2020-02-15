using System.Collections.Generic;
using System;
namespace CaomaoFramework
{
    public abstract class IRedPointData
    {
        public IRedPointData(string id)
        {
            this.Id = id;
        }
        protected Dictionary<string, IRedPointData> Childs = new Dictionary<string, IRedPointData>();
        protected string Id { get; set; }
        public bool bShow { get; set; }

        private Action m_actionUIEvent;
        public abstract void SetData(string data);
        public abstract string GetData();
        public void AddChild(IRedPointData data)
        {
            if (data != null)
            {
                this.Childs[data.Id] = data;
            }
        }

        public bool GetChild(string id,ref IRedPointData data)
        {
            if (this.Id == id)
            {
                data = this;
                return true;
            }
            return this.Childs.TryGetValue(id, out data);
        }

        public void RegisterUIEvent(Action callback)
        {
            this.m_actionUIEvent = callback;
        }
        public void Callback()
        {
            this.m_actionUIEvent?.Invoke();
        }
    }
}
