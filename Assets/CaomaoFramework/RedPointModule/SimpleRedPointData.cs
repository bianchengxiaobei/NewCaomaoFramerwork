using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public class SimpleRedPointData : IRedPointData
    {
        public SimpleRedPointData(string id) : base(id)
        {

        }


        public override string GetData()
        {
            return null;
        }

        public override void SetData(string data)
        {
            
        }
    }
}
