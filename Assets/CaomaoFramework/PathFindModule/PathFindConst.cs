using System;
using System.Collections.Generic;


namespace CaomaoFramework
{
    public static class PathFindConst
    {
        public const uint FlagsWalkableMask = 1;
    }
    public enum EHeuristicType
    {
        Manhattan,//曼哈顿
        DiagonalManhattan,//斜角曼哈顿
        Euclidean,//欧几里得
        None,
    }
}
