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

}

