using System;
namespace CaomaoFramework

{
    [AttributeUsage(AttributeTargets.Class)]
    public class CInstanceNumber : Attribute
    {
        public int Num { get; private set; }
        public CInstanceNumber(int num)
        {
            this.Num = num;
        }
    }
}
