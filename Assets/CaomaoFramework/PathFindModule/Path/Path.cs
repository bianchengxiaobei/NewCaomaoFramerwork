using System;
using System.Collections.Generic;
namespace CaomaoFramework
{
    public struct PathBase
    {
        public int PathId
        {
            get;
            private set;
        }
    }
    public struct Path2D
    {
        public FPVector2 StartPos
        {
            get;
            private set;
        }
        public FPVector2 EndPos
        {
            get;
            private set;
        }

        public Path2D(FPVector2 start, FPVector2 end)
        {
            this.StartPos = start;
            this.EndPos = end;
        }
    }
}
