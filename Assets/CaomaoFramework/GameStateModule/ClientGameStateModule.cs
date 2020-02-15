using System;
using System.Collections.Generic;
namespace CaomaoFramework
{
    /// <summary>
    /// 游戏状态管理器，提供改变游戏状态的接口
    /// </summary>
    [Module(false)]
    public class ClientGameStateModule : IModule, IClientGameStateModule
    {
        private Dictionary<string, ClientStateBase> m_dicClientStates = new Dictionary<string, ClientStateBase>();
        private ClientStateBase m_oCurrentClientState = null;
        private bool m_bScenePrepared = false;
        private bool m_bResourceLoaded = false;
        private ELoadingType m_eCurrentLoadingStyle = ELoadingType.LoadingNormal;
        private Action m_aCallBackWhenChangeFinished = null;
        private Action<ELoadingType,bool> m_aWhenLoadingShowUI = null;
        private Queue<ClientStateChangeArgs> m_qClientNextStateQueue = new Queue<ClientStateChangeArgs>();
        //private ClientGameStateImp m_oClientStateImp = new ClientGameStateImp();
        private string m_sDefalutGameStateName;
        private string m_sCurrentState;
        private string m_sNextGameState;
        private bool m_bIsInChangingState = false;
        /// <summary>
        /// 当前的状态
        /// </summary>
        public string CurrentClientState
        {
            get
            {
                return this.m_sCurrentState;
            }
        }
        /// <summary>
        /// 下个状态
        /// </summary>
        public string ENextGameState
        {
            get
            {
                return this.m_sNextGameState;
            }
        }
        /// <summary>
        /// 是否正在改变状态
        /// </summary>
        public bool IsInChangingState
        {
            get
            {
                return this.m_bIsInChangingState;
            }
        }
        /// <summary>
        /// 改变游戏状态
        /// </summary>
        /// <param name="eGameState"></param>
        public void ChangeGameState(string eGameState)
        {
            this.ChangeGameState(eGameState, ELoadingType.LoadingNormal);
        }
        /// <summary>
        /// 改变游戏状态
        /// </summary>
        /// <param name="eGameState"></param>
        /// <param name="loadingStyle"></param>
        public void ChangeGameState(string eGameState, ELoadingType loadingStyle)
        {
            this.ChangeGameState(eGameState, loadingStyle, null);
        }
        /// <summary>
        /// 改变游戏状态
        /// </summary>
        /// <param name="eGameState"></param>
        /// <param name="loadingStyle"></param>
        /// <param name="callBackWhenChangeFinished"></param>
        public void ChangeGameState(string eGameState, ELoadingType loadingStyle, Action callBackWhenChangeFinished)
        {
            if (this.IsInChangingState)
            {
                ClientStateChangeArgs nextStateArgs = new ClientStateChangeArgs
                {
                    sClientState = eGameState,
                    eLoadingStyle = loadingStyle,
                    aCallBack = callBackWhenChangeFinished
                };
                this.m_qClientNextStateQueue.Enqueue(nextStateArgs);
            }
            else
            {
                if (this.CurrentClientState != eGameState)
                {
                    this.m_sNextGameState = eGameState;
                    this.m_eCurrentLoadingStyle = loadingStyle;
                    this.m_aCallBackWhenChangeFinished = callBackWhenChangeFinished;
                    this.m_bIsInChangingState = true;
                    if (this.CurrentClientState != "Max")
                    {
                        this.SetLoadingVisible(this.m_eCurrentLoadingStyle, true);
                        this.m_oCurrentClientState.OnLeave();
                        CaomaoDriver.ResourceModule.UnloadResource();
                    }
                    this.DoChangeToNewState();
                }
            }
        }
        private void DoChangeToNewState()
        {
            this.m_sCurrentState = this.m_sNextGameState;
            this.m_sNextGameState = "Max";
            this.m_bResourceLoaded = false;
            this.m_bScenePrepared = true;
            //说明是有切换场景的
            if (this.m_eCurrentLoadingStyle == ELoadingType.LoadingChangeScene)
            {
                this.m_bScenePrepared = false;
                CaomaoDriver.SceneLoadModule.RegisterScenePerparedCallback(this.SceneLoadFinished);
            }
            this.m_oCurrentClientState = this.m_dicClientStates[this.CurrentClientState];
            this.m_oCurrentClientState.OnEnter();
            CaomaoDriver.ResourceModule.SetAllLoadFinishedEventHandler(this.ResourceLoadFinished);
        }
        private void ResourceLoadFinished()
        {
            this.m_bResourceLoaded = true;
            this.ClientStateChangeFinished();
        }
        private void SceneLoadFinished()
        {
            this.m_bScenePrepared = true;
            this.ClientStateChangeFinished();
        }
        private void ClientStateChangeFinished()
        {
            if (this.m_bResourceLoaded && this.m_bScenePrepared)
            {
                this.m_bIsInChangingState = false;
                this.SetLoadingVisible(this.m_eCurrentLoadingStyle, false);
                this.m_aCallBackWhenChangeFinished?.Invoke();
                this.m_aCallBackWhenChangeFinished = null;
                this.ChangeGameStateQueue();
            }
        }
        /// <summary>
        /// 显示loading界面回调
        /// </summary>
        /// <param name="eLoadingType"></param>
        /// <param name="bVisible"></param>
        private void SetLoadingVisible(ELoadingType eLoadingType, bool bVisible)
        {
            this.m_aWhenLoadingShowUI?.Invoke(eLoadingType,bVisible);
        }
        public void RegisterLoadingShowUICallback(Action<ELoadingType, bool> callback)
        {
            this.m_aWhenLoadingShowUI = callback;
        }
        /// <summary>
        /// 进入到默认入口
        /// </summary>
        /// <param name="defaultStateName"></param>
        public void EnterDefaultState()
        {
            if (string.IsNullOrEmpty(this.m_sDefalutGameStateName))
            {
                return;
            }
            else
            {
                this.ChangeGameState(this.m_sDefalutGameStateName);
            }
        }
        public void ChangeGameStateQueue()
        {
            if (this.m_qClientNextStateQueue.Count > 0)
            {
                ClientStateChangeArgs stateChangeArgs = this.m_qClientNextStateQueue.Dequeue();
                this.ChangeGameState(stateChangeArgs.sClientState, stateChangeArgs.eLoadingStyle, stateChangeArgs.aCallBack);
            }
        }

        public void Init()
        {
            this.m_sCurrentState = "Max";
        }

        public void Update()
        {

        }

        public void AddClientGameState(string stateName, ClientStateBase state)
        {
            if (this.m_dicClientStates.ContainsKey(stateName))
            {
                return;
            }
            this.m_dicClientStates[stateName] = state;
        }
    }
}
