using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public class MapSerializeData
    {
        public int Width;
        public int Height;
        public FP AspectRatio = 1f;
        public FP IsometricAngle = 45;
        public FPVector3 Rotation;
        public FPVector3 Center;
        public FP NodeSize = 1f;
        public FP MaxClimb = 0.4f;
        public FP maxSlope = 90;
        public ENeighboursType neighboursType = ENeighboursType.Eight;
        public FPVector2 Size { get; set; }
        public bool[] Walkable;
    }
    public enum ENeighboursType
    {
        Four,
        Six,
        Eight
    }
}
