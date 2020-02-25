using System;
using System.Collections.Generic;


namespace CaomaoFramework
{
    [Serializable]
    public struct FPVector2 : IEquatable<FPVector2>
    {
        public FP x;
        public FP y;
        public FPVector2(FP x, FP y)
        {
            this.x = x;
            this.y = y;
        }
        public FPVector2(FP value)
        {
            this.x = value;
            this.y = value;
        }

        public static FPVector2 zero = new FPVector2(0, 0);
        public static FPVector2 one = new FPVector2(1, 1);

        public static FPVector2 right = new FPVector2(1, 0);
        public static FPVector2 left = new FPVector2(-1, 0);

        public static FPVector2 up = new FPVector2(0, 1);
        public static FPVector2 down = new FPVector2(0, -1);

        public bool Equals(FPVector2 other)
        {
            return this == other;
        }
        public override bool Equals(object other)
        {
            return other is FPVector2 ? this == (FPVector2)other : false;
        }
        public override int GetHashCode()
        {
            return (int)(x + y);
        }

        public override string ToString()
        {
            return $"({this.x},{this.y})";
        }

        public static FP Distance(FPVector2 value1, FPVector2 value2)
        {
            FP result = DistanceTwo(value1,value2);
            return FP.Sqrt(result);
        }

        public static FP DistanceTwo(FPVector2 value1, FPVector2 value2)
        {
            FP result = (value1.x - value2.x) * (value1.x - value2.x) + 
                (value1.y - value2.y) * (value1.y - value2.y); ;
            return result;
        }


        #region 重载操作数

        public static FPVector2 operator -(FPVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }


        public static bool operator ==(FPVector2 value1, FPVector2 value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(FPVector2 value1, FPVector2 value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static FPVector2 operator +(FPVector2 value1, FPVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }


        public static FPVector2 operator -(FPVector2 value1, FPVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }


        public static FP operator *(FPVector2 value1, FPVector2 value2)
        {
            return value1.x* value2.x + value1.y * value2.y;
        }


        public static FPVector2 operator *(FPVector2 value, FP scaleFactor)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FPVector2 operator *(FP scaleFactor, FPVector2 value)
        {
            value.x *= scaleFactor;
            value.y *= scaleFactor;
            return value;
        }


        public static FPVector2 operator /(FPVector2 value1, FPVector2 value2)
        {
            value1.x /= value2.x;
            value1.y /= value2.y;
            return value1;
        }


        public static FPVector2 operator /(FPVector2 value1, FP divider)
        {
            FP factor = 1 / divider;
            value1.x *= factor;
            value1.y *= factor;
            return value1;
        }

        public static implicit operator FPVector3(FPVector2 v)
        {
            return new FPVector3(v.x, v.y, 0);
        }

        #endregion 重载操作数


    }
}
