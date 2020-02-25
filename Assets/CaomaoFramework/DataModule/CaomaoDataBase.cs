using System;
using System.Collections.Generic;
using UnityEngine;

namespace CaomaoFramework
{
    public abstract class CaomaoDataBase
    {
        public abstract EDataParserType Parser { get; }
        public abstract string FilePath { get; }

        public EDataLoadType LoadType { get; set; }
    }

    //public abstract class CaomaoSBDataBase<T> : CaomaoDataBase where T : ScriptableObject
    //{
    //    public T SB;
    //}
    public abstract class CaomaoSBDataBase: CaomaoDataBase
    {
        public ScriptableObject SB;
    }
    public enum EDataLoadType
    {
        PreLoad,
        UsedLoad,
    }
}
