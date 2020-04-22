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
using BatteryStatus = UnityEngine.BatteryStatus;

public class Test : MonoBehaviour
{
    public Text a;
    //public CUIHelpMask mask;
    private void Awake()
    {
        
    }

    private void Start()
    {
        var tex = a.cachedTextGenerator;
        var characount = tex.characterCount;
        var count = tex.characters.Count;
        Debug.Log(characount);
        Debug.Log(tex.characters.Count);
    }

    private void Update()
    {
      
    } 
}