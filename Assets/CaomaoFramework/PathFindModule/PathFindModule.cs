using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
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

        public int StartFind2DPath(float2 startPos, float2 endPos, Action<NativeList<float2>> callback)
        {
            return this.PathfindImp.StartFind2DPath(startPos, endPos, callback);
        }

        public void Update()
        {
            this.PathfindImp.Update();   
        }
    }
}
