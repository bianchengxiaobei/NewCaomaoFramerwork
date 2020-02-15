using System;
using System.Collections.Generic;
using UnityEngine;

namespace CaomaoFramework
{
    public class NumberRedPointData : IRedPointData
    {
        private string m_iNumber;
        public NumberRedPointData(string id) : base(id)
        {
            this.m_iNumber = null;
        }      
        public override void SetData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Debug.LogError("Number == null");
            }
            this.m_iNumber = data;
        }

        public override string GetData()
        {
            return this.m_iNumber;
        }
    }
}
