using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
namespace CaomaoFramework
{
    public abstract class ResourceBaseTask
    {
        public string Path;
        public EAssetType AssetType = EAssetType.None;
        public abstract void ReclyeSelf(ResourceBaseTask task);
        public abstract void SetCallback(Delegate callback);
    }

    public class SceneResourceTask<T> : ResourceLoadTask<T>
    {
        public LoadSceneMode LoadSceneMode; 
    }

    public class ResourceLoadTask<T> : ResourceBaseTask
    {
        public AssetLoadFinishedEventHandler<T> Callback;

        public override void ReclyeSelf(ResourceBaseTask task)
        {
            
        }

        public override void SetCallback(Delegate callback)
        {
            this.Callback = callback as AssetLoadFinishedEventHandler<T>;
        }
    }
}

