using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace CaomaoFramework
{
    [Module(false)]
    public class DataModule : IDataModule, IModule
    {
        private Dictionary<EDataParserType, IDataParser> m_dicDataParsers = new Dictionary<EDataParserType, IDataParser>();
        private Action<string> loadError;
        public void Init()
        {
            this.m_dicDataParsers.Add(EDataParserType.ScriptObject, new ScriptableObjectParser());
        }

        public void RegiestErrorCallback(Action<string> error)
        {
            this.loadError = error;
        }

        public void AddDataParser(EDataParserType type, IDataParser parser)
        {
            if (this.m_dicDataParsers.ContainsKey(type))
            {
                return;
            }
            this.m_dicDataParsers[type] = parser;
        }

        public T GetData<T>()where T : CaomaoDataBase
        {
            T t = Activator.CreateInstance<T>();
            var parser = this.GetParser(t.Parser);
            if (parser != null)
            {
                try
                {
                    parser.Parse<T>(t);
                }
                catch (Exception e)
                {
                    this.loadError?.Invoke(CaomaoDriver.LocalizationModule.GetString(LocalizationConst.ConfigLoadError));
                    Debug.LogException(e);
                }
            }
            return t;
        }

        public void GetDataAsync<T>(Action<T> callback) where T : CaomaoDataBase
        {
            T t = Activator.CreateInstance<T>();
            var parser = this.GetParser(t.Parser);
            if (parser != null)
            {
                parser.ParseAsyn<T>(t, callback,this.LoadError);
            }
            else
            {
                callback?.Invoke(null);
            }
        }

        public void GetJsonDataAsync<T>(string fileName,Action<T> callback)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("JsonFileName == null");
                return;
            }

            CaomaoDriver.ResourceModule.LoadAssetAsync(fileName, (asset) =>
            {
                var textAsset = asset as TextAsset;
                if (textAsset != null)
                {
                    T t = JsonUtility.FromJson<T>(textAsset.text);
                    if (t == null)
                    {
                        Debug.LogError("FromJson == null");
                        return;
                    }
                    callback?.Invoke(t);
                }
            });
        }
        /// <summary>
        /// 取得json的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T GetJsonData<T>(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    Debug.LogError("JsonPathName == null");
                    return default(T);
                }
                var content = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(content))
                {
                    Debug.LogError("File == null");
                    return default(T);
                }
                T t = JsonUtility.FromJson<T>(content);
                if (t == null)
                {
                    Debug.LogError("FromJson == null");
                    return default(T);
                }
                return t;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return default(T);
            }
        }


        private void LoadError()
        {
            this.loadError?.Invoke(CaomaoDriver.LocalizationModule.GetString(LocalizationConst.ConfigLoadError));
        }

        private IDataParser GetParser(EDataParserType type)
        {
            IDataParser result = null;
            this.m_dicDataParsers.TryGetValue(type, out result);
            return result;
        }

        public void Update()
        {
            
        }
    }
}
