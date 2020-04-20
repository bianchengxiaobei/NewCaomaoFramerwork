using System;
using System.Collections.Generic;
//using Newtonsoft.Json;
using System.Text;
namespace CaomaoFramework
{
    public class JsonSnapshootData : ISnapshootData
    {
        public void Deserialize(byte[] data)
        {
            var content = Encoding.UTF8.GetString(data);
            //this = JsonConvert.DeserializeObject(content);
        }

        public byte[] Serialize()
        {
            return null;
        }
    }
}
