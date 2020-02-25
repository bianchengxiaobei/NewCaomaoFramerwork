using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public class MapNode
    {
        public int NodeIndex;
        public int Penalty;
        //public uint Flags;
        public FPVector3 Position;

        public bool Walkable { get; set; }
        //public bool Connectionable { get; set; }
    }
}
