using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public interface IDataModule
    {
        void GetDataAsync<T>(Action<T> callback) where T : CaomaoDataBase;
        T GetData<T>() where T : CaomaoDataBase;
        void RegiestErrorCallback(Action<string> error);
    }
}
