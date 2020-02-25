using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CaomaoFramework
{
    [Module(true)]
    public class PathFindModule : IPathFindModule
    {
        private IPathFindModule PathfindImp;

        public int MaxFindPathPointCount
        {
            get
            {
                return this.PathfindImp.MaxFindPathPointCount;
            }
            set
            {
                this.PathfindImp.MaxFindPathPointCount = value;
            }
        }

        public bool FullParallel
        {
            get
            {
                return this.PathfindImp.FullParallel;
            }
            set
            {
                this.PathfindImp.FullParallel = value;
            }
        }

        public void Init()
        {
            this.PathfindImp = new JobPathfindImp();
            this.PathfindImp.Init();
        }

        public int StartFind2DPath(FPVector2 startPos, FPVector2 endPos, Action<NativeList<FPVector2>> callback)
        {
            return this.PathfindImp.StartFind2DPath(startPos, endPos, callback);
        }

        public void Update()
        {
            this.PathfindImp.Update();   
        }
    }
}
