using System;
using System.Collections.Generic;
using UnityEngine;
public static class ColorExtenstion
{
    private static float r;
    private static float g;
    private static float b;
    /// <summary>
    /// 更改Color的alpha值
    /// </summary>
    /// <param name="color"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static Color WithAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    /// <summary>
    /// 将Color灰度化
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Color Grey(float value)
    {
        return new Color(value, value, value, 1);
    }

    //public static string Hex(this Color color)
    //{

    //}


    public static void PushIntColor(int _r, int _g, int _b)
    {
        r = (float)_r / 255;
        g = (float)_g / 255;
        b = (float)_b / 255;
    }
    public static Color PopIntColor()
    {
        return new Color(r, g, b, 1);
    }
}