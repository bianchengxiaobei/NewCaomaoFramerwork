using System;
using Unity.Collections;

namespace CaomaoFramework
{
    public struct PathBHNode
    {
        public int PathId;
        public uint G;
        public uint F;
    }
    public struct NativeBinaryHeap
    {
        private NativeList<PathBHNode> heap;
        private const int ChildCount = 4;
        public const ushort MaxHeapSize = ushort.MaxValue;
        private int size;
        public NativeBinaryHeap(int capacity)
        {
            this.heap = new NativeList<PathBHNode>(capacity,Allocator.Persistent);
            this.size = 0;
        }

        public int AddNode(PathNode node)
        {
            if (node.PathIndex < 0)
            {
                return -1;
                throw new Exception("Path Error");
            }           
            var newNode = new PathBHNode
            {
                F = node.F,
                G = node.G,
                PathId = node.PathIndex
            };
            if (this.size >= this.heap.Capacity)
            {
                this.heap.Add(newNode);
            }
            this.DecreaseKey(newNode, this.size ++);
            return -1;
        }

        public int RemoveNode()
        {
            var pathId = this.heap[0].PathId;
            this.size--;
            if (this.size == 0)
            {
                return pathId;
            }
            //移除需要重新构建
            var swapNode = this.heap[this.size];
            int parentIndex = 0;
            int swapIndex = 0;
            while (true)
            {
                parentIndex = swapIndex;
                uint swapF = swapNode.F;
                uint swapG = swapNode.G;
                int childIndex = parentIndex * ChildCount + 1;
                if (childIndex <= this.size)
                {
                    var firstIndex = childIndex + 0;
                    var secondIndex = childIndex + 1;
                    var thirdIndex = childIndex + 2;
                    var fourIndex = childIndex + 3;
                    uint firstF = this.heap[firstIndex].F;
                    uint secondF = this.heap[secondIndex].F;
                    uint thirdF = this.heap[thirdIndex].F;
                    uint fourF = this.heap[fourIndex].F;

                    if (firstF < swapF || (firstF == swapF
                        && this.heap[firstIndex].G < swapG))
                    {
                        swapF = firstF;
                        swapG = this.heap[firstIndex].G;
                        swapIndex = firstIndex;
                    }
                    if (secondF < swapF || (secondF == swapF 
                        && this.heap[secondIndex].G < swapG))
                    {
                        swapF = secondF;
                        swapG = this.heap[secondIndex].G;
                        swapIndex = secondIndex;
                    }
                    if (thirdF < swapF || (thirdF == swapF
                      && this.heap[thirdIndex].G < swapG))
                    {
                        swapF = thirdF;
                        swapG = this.heap[thirdIndex].G;
                        swapIndex = thirdIndex;
                    }
                    if (fourF < swapF || (fourF == swapF
                     && this.heap[fourIndex].G < swapG))
                    {
                        swapIndex = fourIndex;
                    }
                }
                if (parentIndex != swapIndex)
                {
                    //将头部更新
                    this.heap[parentIndex] = this.heap[swapIndex];
                }
                else
                {
                    break;
                }
            }

            this.heap[swapIndex] = swapNode;//安排最后一个来填充删除的节点
            return pathId;
        }


        private void DecreaseKey(PathBHNode node,int heapIndex)
        {
            var tempIndex = heapIndex;
            uint nodeF = node.F;
            uint nodeG = node.G;

            while (tempIndex != 0)
            {
                int parentIndex = (tempIndex - 1) / ChildCount;
                uint parentF = this.heap[parentIndex].F;
                if (nodeF < parentF ||
                    (nodeF == parentF && nodeG > this.heap[parentIndex].G))
                {
                    //如果F值比父亲节点小就交换位置
                    this.heap[tempIndex] = this.heap[parentIndex];
                    //this.heap[tempIndex].
                    tempIndex = parentIndex;
                }
                else
                {
                    break;
                }
            }
            this.heap[tempIndex] = node;
        } 
    }
}
