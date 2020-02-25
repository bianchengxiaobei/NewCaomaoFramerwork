using System;
using System.Collections.Generic;


namespace CaomaoFramework
{
    public interface IDataParser
    {
        void Parse<T>(T data) where T : CaomaoDataBase;
        void ParseAsyn<T>(T data, Action<T> callback, Action error) where T : CaomaoDataBase;
    }
}
