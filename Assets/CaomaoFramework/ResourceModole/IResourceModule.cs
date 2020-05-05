using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;
using System.Threading.Tasks;

namespace CaomaoFramework
{
    /// <summary>
    /// 资源请求加载完成之后的委托
    /// </summary>
    /// <param name="assetRequest"></param>
    public delegate void AssetLoadFinishedEventHandler<T>(T assetData);
    public interface IResourceModule
    {
        float ProgressValue
        {
            get;
        }

        void Clear();

        void LoadGameObjectAsync(string objPath, AssetLoadFinishedEventHandler<GameObject> callback,bool bStart = false);
        void LoadAssetAsync(string assetName, AssetLoadFinishedEventHandler<Object> callback,bool bStart = false);

        //void LoadAsset<T>(string assetName, AssetLoadFinishedEventHandler<T> callback) where T : Object;
        void LoadSceneAsync(string sceneName, AssetLoadFinishedEventHandler<SceneInstance> callback,LoadSceneMode mode,bool bStart = false);
        [Obsolete("暂时不支持同步加载")]
        GameObject LoadGameObject(string objPath);
        Task<T> LoadAssetAsyncNoCallback<T>(string assetName) where T : Object;

        Task<GameObject> LoadGameObjectAsyncNoCallback(string assetName);

        void AddGameObjectTask(string objPath, AssetLoadFinishedEventHandler<GameObject> callback);
        void AddAssetTask(string objPath, AssetLoadFinishedEventHandler<Object> callback);
        void AddSceneTask(string objPath, LoadSceneMode mode, AssetLoadFinishedEventHandler<SceneInstance> callback);
        void StartLoad();
        void SetAllLoadFinishedEventHandler(Action eventHandler);
        /// <summary>
        /// 卸载资源
        /// </summary>
        void UnloadResource();
        /// <summary>
        /// 卸载单个资源
        /// </summary>
        /// <param name="asset"></param>
        void UnloadResource(Object asset);
    }
    public enum EAssetType
    {
        None,
        GameObject,
        Scene,
        Asset
    }
}
