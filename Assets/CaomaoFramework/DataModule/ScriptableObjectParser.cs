using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public class ScriptableObjectParser : IDataParser
    {
        public void Parse<T>(T data) where T : CaomaoDataBase
        {
           //var temp = data as CaomaoSBDataBase<V>; 
           // temp.SB = 
        }


        public void ParseAsyn<T>(T data, Action<T> callback, Action error) where T : CaomaoDataBase
        {
            var temp = data as CaomaoSBDataBase;
            CaomaoDriver.ResourceModule.LoadAssetAsync(temp.FilePath, (asset) =>
            {
                temp.SB = asset as ScriptableObject;
                callback?.Invoke(data);
            });
        }
    }
}
