using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IO.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Jobs;
using System.IO;
using CaomaoFramework;
using UnityEngine.Profiling;
public class Test : MonoBehaviour
{
    public Image a;
    public CUIHelpMask mask;
    private void Awake()
    {
        mask.SetArea(a);
    }

    private void Update()
    {
      
    } 
}