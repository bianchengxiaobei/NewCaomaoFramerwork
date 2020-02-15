using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CaomaoFramework
{
    [Module(false)]
    public class SceneLoadModule : IModule,ISceneLoadModule
    {
        private AsyncOperation m_oAsyncOperation;
        private Action m_actionOnLoadSceneFinish = null;//加载场景完成的委托
        private Action m_aScenePerparedAction = null;
        private bool m_bLoadSceneFinished = false;//是否加载完成

        public float ProgressValue
        {
            get
            {
                if (this.m_oAsyncOperation != null)
                {
                    if (this.m_oAsyncOperation.isDone)
                    {
                        return 1f;
                    }
                    return this.m_oAsyncOperation.progress;
                }
                return 1f;
            }
        }

        public void Init()
        {
            
        }
        public void EnterScene(string sceneName)
        {
            if (SceneManager.GetActiveScene().name.Equals(sceneName))
            {
                Debug.LogWarning("重复加载:" + sceneName);
                return;
            }
            else
            {
                CaomaoDriver.Instance.StartCoroutine(LoadScene(sceneName));
            }
        }
        private IEnumerator LoadScene(string strSceneName)
        {
            this.m_oAsyncOperation = SceneManager.LoadSceneAsync(strSceneName);
            yield return this.m_oAsyncOperation;
            if (this.m_oAsyncOperation.isDone)
            {
                this.m_actionOnLoadSceneFinish?.Invoke();
                this.m_actionOnLoadSceneFinish = null;
                this.OnLoadSceneFinish();
            }        
        }
        /// <summary>
        /// 注册加载场景完成之后的委托
        /// </summary>
        /// <param name="actionCallback"></param>
        public void RegisterLoadSceneFinishCallback(Action actionCallback)
        {
            this.m_actionOnLoadSceneFinish = actionCallback;
        }
        public void RegisterScenePerparedCallback(Action actionCallback)
        {
            this.m_aScenePerparedAction = actionCallback;
        }
        private void OnLoadSceneFinish()
        {
            this.m_bLoadSceneFinished = true;
            if (this.m_bLoadSceneFinished)
            {
                CaomaoDriver.Instance.StartCoroutine(this.OnScenePrepared());
            }                  
        }
        private IEnumerator OnScenePrepared()
        {
            yield return new WaitForSeconds(0.01f);
            this.m_aScenePerparedAction?.Invoke();
        }

        public void Update()
        {
            
        }
    }
}
