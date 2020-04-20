using System;
using System.Collections.Generic;


namespace CaomaoFramework
{
    public struct PathNode
    {
        public int ParentIndex;
        public int PathIndex;
        public uint G
        {
            get;set;
        }
        public uint H
        {
            get;set;
        }
        public uint F
        {
            get
            {
                return this.G + this.H;
            }
        }
    }
}
