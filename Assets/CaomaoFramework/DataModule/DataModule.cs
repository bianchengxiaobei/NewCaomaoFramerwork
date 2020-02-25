using System;
using System.Collections.Generic;
using UnityEngine;
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
