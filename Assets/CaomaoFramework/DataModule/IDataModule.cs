using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public interface IDataModule
    {
        void GetDataAsync<T>(Action<T> callback) where T : CaomaoDataBase;
        T GetData<T>() where T : CaomaoDataBase;
        void RegiestErrorCallback(Action<string> error);
        /// <summary>
        /// 同步取得json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        T GetJsonData<T>(string filePath);
    }
}
