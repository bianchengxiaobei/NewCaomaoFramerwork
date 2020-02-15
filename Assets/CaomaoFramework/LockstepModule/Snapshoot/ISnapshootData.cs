using System;
using System.Collections.Generic;
namespace CaomaoFramework
{
    public interface ISnapshootData
    {
        //int SnapshootId { get; }
        byte[] Serialize();
        void Deserialize(byte[] data);
    }
}
