using CaomaoFramework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private LocalizationData data;
    private void Awake()
    {
        this.LoadAD();
    }
    private async void LoadAD()
    {
        data = await Addressables.LoadAssetAsync<LocalizationData>("Localization_中国").Task;
        Debug.Log(data);
    }
    private void OnEnable()
    {
       
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public class A : IModule
{
    public int a;
    public void Init()
    {
        Debug.Log("A:Init");
    }

    public void Update()
    {
        Debug.Log("A");
    }
}
[Serializable]
public class B : IModule
{
    public string b;
    public void Init()
    {
        Debug.Log("B:Init");
    }

    public void Update()
    {
        Debug.Log("B");
    }
}