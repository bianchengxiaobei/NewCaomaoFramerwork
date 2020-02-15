using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public class CaomaoDriver : MonoBehaviour
    {
        public static IClientGameStateModule GameStateModule;
        public static IResourceModule ResourceModule;
        public static ISceneLoadModule SceneLoadModule;
        public static IWordFilterModule WordFilterModule;
        public static ILocalizationModule LocalizationModule;
        public static IWebRequestModule WebRequestModule;
        public static IRedPointModule RedPointModule;
        public static ICUIModule UIModule;
        public static IHotFixModule HotFixModule;
        public static ITimerModule TimerModule;
        public static IAudioModule AudioModule;


        public static CaomaoGameGobalConfig GlobalConfig;
        private GameModules modules = new GameModules();
        public static CaomaoDriver Instance = null;
        public static Transform UIRoot = null;

        //public SDKPlatformManager sdkManager = SDKPlatformManager.Instance;

        //public UIManager uiManager = UIManager.Instance;


        //private void Awake()
        //{
        //    Application.targetFrameRate = targetFrameRate;
        //    UnityMonoDriver.s_instance = this;
        //    if (base.transform.parent != null)
        //    {
        //        DontDestroyOnLoad(base.transform.parent);
        //    }
        //    InvokeRepeating("Tick",0f, 0.01f);
        //    resourceManager.Init(GameObject.Find("ResourceManager").GetComponent<GameResourceManager>());
        //    uiManager.Init();
        //}
        //private void Start()
        //{
        //    sdkManager.Init();
        //    sdkManager.Install();
        //    AudioManagerBase.Instance.Init();
        //    clientGameStateManager.Init();
        //    clientGameStateManager.EnterDefaultState();
        //}
        //private void Update()
        //{
        //    sdkManager.Update();
        //    resourceManager.Update();
        //    AudioManagerBase.Instance.Update();
        //    uiManager.Update(Time.deltaTime);
        //}
        //private void Tick()
        //{
        //    //StoryManager.singleton.Tick();
        //    TimerManager.Tick();
        //    FrameTimerManager.Tick();
        //}
        //private void OnApplicationFocus(bool focus)
        //{
        //    if (focus)
        //    {
        //        if (GameControllerBase.thePlayer != null)
        //        {
        //            GameControllerBase.thePlayer.m_skillManager.Compensation(prePay);
        //            TimerManager.AddTimer(1000, 0, () => { prePay = 0; });
        //        }
        //    }
        //}
        //private float prePause = 0;
        //private float prePay = 0;
        //private void OnApplicationPause(bool pause)
        //{
        //    if (pause)
        //    {
        //        prePause = Time.realtimeSinceStartup;
        //    }
        //    else
        //    {
        //        float cur = Time.realtimeSinceStartup;
        //        GameControllerBase.StartTime = cur;
        //        float pay = cur - prePause;
        //        if (GameControllerBase.thePlayer != null)
        //        {
        //            GameControllerBase.thePlayer.m_skillManager.Compensation(-prePay);
        //            prePay = 0;
        //            GameControllerBase.thePlayer.m_skillManager.Compensation(pay);
        //        }
        //    }
        //}
        private void Awake()
        {
            CaomaoDriver.Instance = this;
            DontDestroyOnLoad(this.gameObject);
            UIRoot = GameObject.FindGameObjectWithTag("UIRoot").transform;
            DontDestroyOnLoad(UIRoot.gameObject);
            UIModule = this.CreateModule<CUIModule>();
            GameStateModule = this.CreateModule<ClientGameStateModule>();
            ResourceModule = this.CreateModule<ResourceModule>();
            SceneLoadModule = this.CreateModule<SceneLoadModule>();
            WordFilterModule = this.CreateModule<WordFilterModule>();
            LocalizationModule = this.CreateModule<LocalizationModule>();
            WebRequestModule = this.CreateModule<WebRequestModule>();
            RedPointModule = this.CreateModule<RedPointModule>();
            HotFixModule = this.CreateModule<HotFixModule>();
            TimerModule = this.CreateModule<TimerModule>();
            AudioModule = this.CreateModule<AudioModule>();
            this.modules.Awake();
        }
        private void Start()
        {
            HotFixModule.LoadScript();
        }
        private T CreateModule<T>() where T :class,IModule
        {
            return this.modules.AddModule<T>();
        }
        private void Update()
        {
            this.modules.Update();
            //Debug.Log(CaomaoDriver.ResourceModule.ProgressValue + ":"+Time.frameCount);
        }
    }
}
