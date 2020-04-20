using System;
using System.Collections.Generic;
//using Newtonsoft.Json;
using System.IO;
using UnityEngine;
namespace CaomaoFramework
{
    public class JsonParser : IDataParser
    {
        public void ParseAsyn<T>(T data,Action<T> callback,Action error) where T : CaomaoDataBase
        {
            var path = $"{CaomaoGameGobalConfig.Instance.ConfigPathDir}/{data.FilePath}";
            CaomaoDriver.WebRequestModule.LoadLocalText(path, (content)=> 
            {
                JsonUtility.FromJsonOverwrite(content, data);
                //JsonConvert.PopulateObject(content,data);
                callback?.Invoke(data);
            }, error);
            
        }
        public void Parse<T>(T data) where T : CaomaoDataBase
        {
            try
            {
                //同步加载
                var path = $"{CaomaoGameGobalConfig.Instance.ConfigPathDir}/{data.FilePath}";
                var content = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(content, data);
                //JsonConvert.PopulateObject(content,data);
            }
            catch
            {
                throw;
            }
        }
    }
}
