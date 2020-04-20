using Unity.Mathematics;
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
        public float2 StartPos
        {
            get;
            private set;
        }
        public float2 EndPos
        {
            get;
            private set;
        }

        public Path2D(float2 start, float2 end)
        {
            this.StartPos = start;
            this.EndPos = end;
        }
    }
}
