using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using CaomaoFramework;

public class Test : MonoBehaviour
{
    private WebRequestModule a;
    private void Awake()
    {
        a = new WebRequestModule();
    }

    private async void Start()
    {
        var dllPath = $"{Application.persistentDataPath}/{CaomaoGameGobalConfig.Instance.HotFixDllName}";
        //Debug.Log(dllPath);
        Debug.Log("start:" + Time.frameCount);
        var data = await a.LoadLocalBytesNoCallback(dllPath);
        Debug.Log("end:"+Time.frameCount);
        Debug.Log("data:" + data.Length);
        Debug.Log("start1:" + Time.frameCount);
        a.LoadLocalBytesTest(this,dllPath, (temp) => 
        {
            Debug.Log("end1:" + Time.frameCount);
            Debug.Log("data1:" + temp.Length);
        }, null);
        //Debug.Log(data.Length);
    }

    private void Update()
    {
        
    } 
}