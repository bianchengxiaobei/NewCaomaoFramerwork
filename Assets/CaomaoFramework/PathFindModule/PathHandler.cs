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

        public void CaculatePathNode(MapNode node,ref PathNode pathNode)
        {

        }

        public void StartFind(MapNode startNode,MapNode endNode)
        {
            var startPathNode = this.GetPathNode(startNode);
            var endPathNode = this.GetPathNode(endNode);

        }
    }
}
