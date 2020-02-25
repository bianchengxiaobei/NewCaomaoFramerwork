using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace CaomaoFramework
{
    public interface IMap
    {
        void Serialize(string data);
        void Serialize(byte[] data);
    }
}
