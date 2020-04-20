using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
namespace CaomaoFramework
{
    [Module(false)]
    public class ResourceModule : IModule,IResourceModule
    {
        private int m_iLoadedAssetsNum;
        private int m_iAllAssetsNum;
        private EAssetType m_eCurAssetType = EAssetType.None;
        private AsyncOperationHandle<GameObject> m_oLoadGameObjectOperation;
        private AsyncOperationHandle<SceneInstance> m_oLoadSceneOperation;
        private AsyncOperationHandle<Object> m_oLoadObjectOperation;
        private Action m_aLoadedFinsihedEventHandler;
        private Queue<ResourceBaseTask> m_queueTasks = new Queue<ResourceBaseTask>();
        private bool m_bIsLoading = false;
        private bool m_bIsInited = false;
        //private float m_fProgressValue = 0;
        /// <summary>
        /// 加载进度
        /// </summary>
        public float ProgressValue
        {
            get;set;
            //{              
                //if (this.m_iAllAssetsNum == 0)
                //{
                //    return 0;
                //}
                //else
                //{
                //    float curP = 0;
                //    if (this.m_eCurAssetType != EAssetType.None)
                //    {
                //        switch (this.m_eCurAssetType)
                //        {
                //            case EAssetType.GameObject:
                //                if (this.m_oLoadGameObjectOperation.IsValid())
                //                {
                //                    curP = this.m_oLoadGameObjectOperation.PercentComplete;
                //                }
                //                break;
                //            case EAssetType.Scene:
                //                if (this.m_oLoadSceneOperation.IsValid())
                //                {
                //                    curP = this.m_oLoadSceneOperation.PercentComplete;
                //                }
                //                break;
                //            case EAssetType.Asset:
                //                if (this.m_oLoadObjectOperation.IsValid())
                //                {
                //                    curP = this.m_oLoadObjectOperation.PercentComplete;
                //                }
                //                break;
                //        }
                //    }
                //    return ((this.m_iLoadedAssetsNum + curP) / this.m_iAllAssetsNum);
                //}  
            //}           
        }
        private void CheckFinishedLoad()
        {
            this.m_iLoadedAssetsNum++;
            if (this.m_iLoadedAssetsNum == this.m_iAllAssetsNum)
            {
                this.m_aLoadedFinsihedEventHandler?.Invoke();
                this.m_aLoadedFinsihedEventHandler = null;
                this.m_bIsLoading = false;
                this.m_iLoadedAssetsNum = 0;
                this.m_iAllAssetsNum = 0;
            }
        }
        /// <summary>
        /// 初始化资源管理器
        /// </summary>
        public void Init()
        {
            this.InitAddressable();            
        }
        private async void InitAddressable()
        {
            var a = await Addressables.InitializeAsync().Task;
            this.m_bIsInited = true;
        }
        //private void SetInit(AsyncOperationHandle<IResourceLocator> obj)
        //{
            
        //}
        public void Clear()
        {
           
        }
        public void AddGameObjectTask(string objPath, AssetLoadFinishedEventHandler<GameObject> callback)
        {
            var task = new ResourceLoadTask<GameObject>();
            task.Path = objPath;
            task.Callback = callback;
            task.AssetType = EAssetType.GameObject;
            this.m_queueTasks.Enqueue(task);
            this.m_iAllAssetsNum++;
            this.m_eCurAssetType = EAssetType.None;
        }
        public void AddAssetTask(string objPath, AssetLoadFinishedEventHandler<Object> callback)
        {
            var task = new ResourceLoadTask<Object>();
            task.Path = objPath;
            task.Callback = callback;
            task.AssetType = EAssetType.Asset;
            this.m_queueTasks.Enqueue(task);
            this.m_iAllAssetsNum++;
            this.m_eCurAssetType = EAssetType.None;
        }
        public void AddSceneTask(string objPath, LoadSceneMode mode, AssetLoadFinishedEventHandler<SceneInstance> callback)
        {
            var task = new SceneResourceTask<SceneInstance>();
            task.Path = objPath;
            task.Callback = callback;
            task.AssetType = EAssetType.Scene;
            task.LoadSceneMode = mode;
            this.m_queueTasks.Enqueue(task);
            this.m_iAllAssetsNum++;
            this.m_eCurAssetType = EAssetType.None;
        }
        public void StartLoad()
        {
            if (this.m_iAllAssetsNum > 0 && this.m_bIsLoading == false)
            {
                this.StartLoadTaskAsset();
            }
        }
        private void StartLoadTaskAsset()
        {
            if (this.m_queueTasks.Count > 0)
            {
                this.ProgressValue = (float)(this.m_iLoadedAssetsNum + 1) / this.m_iAllAssetsNum;
                var task = this.m_queueTasks.Dequeue();
                //Debug.Log(task.Path);
                this.m_eCurAssetType = task.AssetType;
                switch (this.m_eCurAssetType)
                {
                    case EAssetType.GameObject:
                        this.LoadGameObjectAsyn(task.Path, (task as ResourceLoadTask<GameObject>).Callback, true);
                        break;
                    case EAssetType.Asset:
                        var assetTask = task as ResourceLoadTask<Object>;
                        this.LoadAsssetAsync(task.Path, assetTask.Callback, true);
                        break;
                    case EAssetType.Scene:
                        var sceneTask = task as SceneResourceTask<SceneInstance>;
                        this.InnerLoadSceneAsync(sceneTask.Path, sceneTask.Callback, sceneTask.LoadSceneMode,true);
                        break;
                }
            }
        }
        public void LoadGameObjectAsync(string objPath, AssetLoadFinishedEventHandler<GameObject> callback,bool bStart = false)
        {
            //this.m_eCurAssetType = EAssetType.None;
            //this.LoadGameObjectAsyn(objPath, callback,false);
            this.AddGameObjectTask(objPath, callback);
            if (bStart) 
            {
                this.StartLoad();
            }
        }

        public GameObject LoadGameObject(string objPath)
        {
            if (this.m_bIsInited == false)
            {
                return null;
            }
            throw new Exception("暂时不支持同步加载，请使用同步加载");
            var op = Addressables.InstantiateAsync(objPath);
            if (op.IsDone == false)
            {
                throw new Exception("暂时不支持同步加载");
            }
            if (op.Result == null)
            {
                throw new Exception("暂时不支持同步加载");
            }
            return op.Result;
        }

        private async void LoadGameObjectAsyn(string objPath, AssetLoadFinishedEventHandler<GameObject> callback,bool bCheck = true)
        {
            try
            {              
                this.m_oLoadGameObjectOperation = Addressables.InstantiateAsync(objPath,Vector3.zero,Quaternion.identity);
                var obj = await this.m_oLoadGameObjectOperation.Task;
                Debug.Log(objPath);
                if (bCheck)
                {
                    this.CheckFinishedLoad();
                    this.StartLoadTaskAsset();
                }               
                callback?.Invoke(obj);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        public void LoadSceneAsync(string sceneName,AssetLoadFinishedEventHandler<SceneInstance> callback,LoadSceneMode mode,bool bStart = false)
        {
            //this.m_eCurAssetType = EAssetType.Scene;
            //this.LoadSceneAsync(sceneName, callback,addtive,false);  
            this.AddSceneTask(sceneName,mode,callback);
            if (bStart)
            {
                this.StartLoad();
            }
        }
        private async void InnerLoadSceneAsync(string sceneName, AssetLoadFinishedEventHandler<SceneInstance> callback,LoadSceneMode mode,bool bCheck = true)
        {
            try
            {
                this.m_oLoadSceneOperation = Addressables.LoadSceneAsync(sceneName,mode);
                var scene = await this.m_oLoadSceneOperation.Task;
                if (bCheck)
                {
                    this.CheckFinishedLoad();
                    this.StartLoadTaskAsset();
                }               
                callback?.Invoke(scene);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        public void LoadAssetAsync(string assetName, AssetLoadFinishedEventHandler<Object> callback,bool bStart = false)
        {
            this.AddAssetTask(assetName, callback);
            if (bStart) 
            {
                this.StartLoad();
            }
        }

        //public void LoadAsset<T>(string assetName, AssetLoadFinishedEventHandler<T> callback)where T:Object
        //{
        //    //this.m_eCurAssetType = EAssetType.Asset;
        //    //this.LoadAsssetAsync<T>(assetName, callback,false);
        //    this.AddAssetTask<T>(assetName, callback);
        //}
        //private async void LoadAsssetAsync<T>(string assetName, AssetLoadFinishedEventHandler<T> callback,bool bCheck = true) where T : Object
        //{
        //    try
        //    {
        //        this.m_oLoadObjectOperation = Addressables.LoadAssetAsync<Object>(assetName);
        //        var obj = await this.m_oLoadObjectOperation.Task as T;
        //        if (bCheck)
        //        {
        //            this.CheckFinishedLoad();
        //            this.StartLoadTaskAsset();
        //        }              
        //        callback?.Invoke(obj);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogException(e);
        //    }
        //}
        private async void LoadAsssetAsync(string assetName, AssetLoadFinishedEventHandler<Object> callback,bool bCheck = true)
        {
            try
            {
                this.m_oLoadObjectOperation = Addressables.LoadAssetAsync<Object>(assetName);
                var obj = await this.m_oLoadObjectOperation.Task;
                if (bCheck)
                {
                    this.CheckFinishedLoad();
                    this.StartLoadTaskAsset();
                }             
                callback?.Invoke(obj);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        public void Update()
        {
            
        }

        public void SetAllLoadFinishedEventHandler(Action eventHandler)
        {
            this.m_aLoadedFinsihedEventHandler = eventHandler;
        }

        public void UnloadResource()
        {
            Resources.UnloadUnusedAssets();
        }
        public void UnloadResource(Object asset)
        {
            Addressables.Release(asset);
        }     
    }
}
