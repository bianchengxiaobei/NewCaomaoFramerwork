using System;
using System.Collections;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
namespace CaomaoFramework
{
    public class ILRuntimeHotFixImp : IHotFixModule
    {
        public AppDomain appdomain;
        private bool m_bLoaded = false;
        private Action<string> m_actionError;
        private byte[] dll;
        private IType m_hotFixEnter;
        private IMethod m_hotFixUpdateMethod;
        private MemoryStream dllStream;
        private MemoryStream pdbStream;
        public void Init()
        {
            if (this.appdomain == null)
            {
                this.appdomain = new AppDomain();
            }
        }

        public void LoadScript()
        {
            var dllPath = $"{Application.persistentDataPath}/{CaomaoGameGobalConfig.Instance.HotFixDllName}";
            CaomaoDriver.WebRequestModule.LoadLocalBytes(dllPath, this.InitDll, this.LoadDllError);
        }

        public void RegisterErrorCallback(Action<string> onError)
        {
            this.m_actionError = onError;
        }

        public void OnScriptLoadedInitialized()
        {
            this.m_bLoaded = true;
            this.InitILRuntime();
            this.m_hotFixEnter = this.appdomain.LoadedTypes["CaomaoHotFix.HotFixDriver"];
            var initMethod = this.m_hotFixEnter.GetMethod("Init", 0);
            this.appdomain.Invoke(initMethod, null, null);
            this.m_hotFixUpdateMethod = this.m_hotFixEnter.GetMethod("Update", 0);
            //更新完成后进入热更dll的代码执行
            CaomaoDriver.UIModule.AddUI(nameof(UIRedPoint), new UIRedPoint());
            CaomaoDriver.UIModule.GetUI(nameof(UIRedPoint)).Show();




            //CaomaoDriver.GameStateModule.ChangeGameState("TestState", ELoadingType.LoadingNormal, null);
            //CaomaoDriver.UIModule.GetUI("UITest").PreLoad();
            //CaomaoDriver.TimerModule.AddTimerTask(0, 1000,this.Test,false);
            //CaomaoDriver.AudioModule.PlayBGMusic("Login_Music");
            //CaomaoDriver.DataModule.GetDataAsyn<TestData>((asset) =>
            //{
            //    var sb = asset.SB as LocalizationData;
            //    Debug.Log(sb.language);
            //});
            //var obj = CaomaoDriver.ResourceModule.LoadGameObject("UITest");
            //Debug.Log(obj == null);
            //CaomaoDriver.ResourceModule.AddGameObjectTask("TestLoad", (obj) =>
            //{
            //    //Debug.Log("111");
            //});       
            //CaomaoDriver.ResourceModule.AddGameObjectTask("UITest", (obj) =>
            //{
            //    //Debug.Log("333");
            //});
            //CaomaoDriver.ResourceModule.AddSceneTask("Test", (obj) =>
            //{
            //    Debug.Log("333");
            //});
            //CaomaoDriver.ResourceModule.StartLoad();
        }
        private void Test()
        {
            //CaomaoDriver.AudioModule.FadeInBGMusic("Login_Music",2);
        }
        private void InitILRuntime()
        {
            this.appdomain.RegisterCrossBindingAdaptor(new UIBaseApadater());
            this.appdomain.RegisterCrossBindingAdaptor(new ClientStateBaseAdapter());
            this.appdomain.RegisterCrossBindingAdaptor(new TestApadater());
            this.appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });
        }
        public void Update()
        {
            if (this.m_bLoaded == false)
            {
                return;
            }
            //更新热更代码
            this.appdomain.Invoke(this.m_hotFixUpdateMethod, null, null);
        }

        private void InitDll(byte[] data)
        {
            this.dll = data;
            var pdbPath = $"{Application.persistentDataPath}/{CaomaoGameGobalConfig.Instance.HotFixPdbName}";
            CaomaoDriver.WebRequestModule.LoadLocalBytes(pdbPath, this.InitPdb, this.LoadDllError);
        }
        private void InitPdb(byte[] pdb)
        {
            this.dllStream = new MemoryStream(dll);
            this.pdbStream = new MemoryStream(pdb);
            appdomain.LoadAssembly(this.dllStream, this.pdbStream, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            this.OnScriptLoadedInitialized();

        }
        private void LoadDllError()
        {
            Debug.LogError("ILRuntime Load Error");
            this.m_actionError?.Invoke(CaomaoDriver.LocalizationModule.GetString(LocalizationConst.HotFixError));
        }
    }
}
