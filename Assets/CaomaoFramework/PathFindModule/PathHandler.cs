using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
namespace CaomaoFramework
{ 
    public class PathProcess
    {
        private NativeBinaryHeap m_oHeap = new NativeBinaryHeap(128);
        private NativeHashMap<int, PathNode> m_dicAllPathNodes = new NativeHashMap<int, PathNode>();
        private EHeuristicType m_eHeuristiType = EHeuristicType.Euclidean;
        private IMap m_oMap;


        public PathProcess(IMap map)
        {
            this.m_oMap = map;
        }

        public PathNode GetPathNode(int index)
        {
            PathNode result = default(PathNode);
            this.m_dicAllPathNodes.TryGetValue(index, out result);
            return result;
        }

        public PathNode GetPathNode(MapNode node)
        {
            return this.GetPathNode(node.NodeIndex);
        }

        public void CaculatePathNode(MapNode node,MapNode endPos, ref PathNode pathNode)
        {
            pathNode.ParentIndex = -1;
            pathNode.PathIndex = node.NodeIndex;
            pathNode.G = (uint)node.Penalty;
            pathNode.H = this.CalculateH(node, endPos.Position);
        }

        private uint CalculateH(MapNode node,FPVector3 endPos)
        {
            uint result = 0;
            switch (this.m_eHeuristiType)
            {
                case EHeuristicType.Euclidean:
                    result = (uint)(endPos - node.Position).sqrMagnitude;
                    break;
                case EHeuristicType.Manhattan:
                    var x = FPMath.Abs(endPos.x - node.Position.x);
                    var y = FPMath.Abs(endPos.y - node.Position.y);
                    var z = FPMath.Abs(endPos.z - node.Position.z);
                    result = (uint)(x + y + z);
                    break;
                case EHeuristicType.DiagonalManhattan:
                    var p = endPos - node.Position;
                    p.x = FPMath.Abs(p.x);
                    p.y = FPMath.Abs(p.y);
                    p.z = FPMath.Abs(p.z);
                    var diag = FPMath.Min(p.x, p.z);
                    var diag2 = FPMath.Max(p.x, p.z);
                    result = (uint)((14 * diag / 10) + (diag2 - diag) + p.y);
                    break;
            }
            return result;
        }

        public void StartFind(MapNode startNode,MapNode endNode)
        {
            var startPathNode = this.GetPathNode(startNode);
            var endPathNode = this.GetPathNode(endNode);
            this.CaculatePathNode(startNode, endNode, ref startPathNode);
            //然后加入到二叉堆中
            var heap = new NativeBinaryHeap(128);
            var offsets = this.m_oMap.GetNeighbourOffsets();
            for (int i = 0; i < 8; i++)
            {
                if (startNode.HasConnectInDir(i))
                {
                    //如果i方向的Node连通的话
                    var index = startNode.NodeIndex + offsets[i];
                    GridMapNode other = this.m_oMap.GetMapNode(index) as GridMapNode;
                    if (other != null)
                    {

                    }
                }
            }
        }
    }
}
