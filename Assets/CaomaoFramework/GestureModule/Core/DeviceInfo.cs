using System;
using System.Collections.Generic;
using UnityEngine;
public static class DeviceInfo
{
    private static float unitMultiplier = 0;
    private static float oneOverUnitMultiplier = 0;

    public static float UnitMultiplier
    {
        get
        {
            return unitMultiplier;
        }
        set
        {
            value = Mathf.Max(0.00001f, value);
            unitMultiplier = value;
            oneOverUnitMultiplier = 1 / unitMultiplier;
        }
    }
    /// <summary>
    /// 单位转像素
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static float UnitToPixels(float unit)
    {
        return unit * UnitMultiplier;
    }
    /// <summary>
    /// 像素转成unit
    /// </summary>
    /// <param name="pixels"></param>
    /// <returns></returns>
    public static float PixelsToUnit(float pixels)
    {
        return pixels * oneOverUnitMultiplier;
    }

    public static Vector2 PixelsToUnit(Vector2 pixelsVector)
    {
        Vector2 result = Vector2.zero;
        result.x = pixelsVector.x * oneOverUnitMultiplier;
        result.y = pixelsVector.y * oneOverUnitMultiplier;
        return result;
    }

}

