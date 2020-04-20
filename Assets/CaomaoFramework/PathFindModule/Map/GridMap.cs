using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
namespace CaomaoFramework
{
    public class GridMap : IMap
    {
        private MapSerializeData m_oData;
        private bool m_bInit = false; 
        private MapNode[] m_listNodes;
        private Transform m_Transform;



        private readonly int2[] neighbourOffsets = new int2[8];
        public void Init()
        {
            this.Serialize("");//初始化Data
        }




        public void Scan()
        {
            this.m_listNodes = new MapNode[this.m_oData.Width * this.m_oData.Height];
            for (int i = 0; i < this.m_oData.Height; i++)
            {
                for (int j = 0; j < this.m_oData.Width; j++)
                {
                    var index = this.CaculateIndex(j,i);
                    var node = new GridMapNode();
                    node.GridIndex = index;
                    node.XIndex = j;
                    node.ZIndex = i;
                    this.CalculateNodePro(node);
                }
            }
        }

        public void StartFindPath()
        {

        }

        private void InitPath()
        {
            
            
        }


        private int CaculateIndex(int xIndex,int zIndex)
        {
            return zIndex * this.m_oData.Width + xIndex;
        }

        private void CalculateNodePro(GridMapNode node)
        {
            if (node == null)
            {
                Debug.LogError("MapNode == null");
                return;
            }
            //node.Position = (FPVector3)(this.m_Transform.position + new Vector3(node.XIndex + 0.5f,0,node.ZIndex + 0.5f));
            node.Walkable = this.m_oData.Walkable[node.GridIndex];

        }
        private void CaculateConnection(GridMapNode node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Walkable == false)
            {
                node.SetAllNoConnect();
                return;
            }
            if (this.m_oData.neighboursType == ENeighboursType.Eight ||
                this.m_oData.neighboursType == ENeighboursType.Four)
            {
                byte fourDirConnectFlags = 0;
                for (int i = 0; i < 4; i++)
                {
                    var offset = this.neighbourOffsets[i];
                    int nx = node.XIndex + offset.x;
                    int nz = node.ZIndex + offset.y;
                    if (nx >= 0 && nz >= 0 && 
                        nx < this.m_oData.Width && nz < this.m_oData.Height)
                    {
                        var nNode = this.m_listNodes[this.CaculateIndex(nx, nz)] as GridMapNode;
                        if (nNode != null)
                        {
                            if (this.CheckCanConnection(nNode, node))
                            {
                                fourDirConnectFlags |= (byte)(1 << i);
                                //0001
                                //0011....1111
                            }
                        }
                    }
                }
                if (this.m_oData.neighboursType == ENeighboursType.Eight)
                {
                    byte diagConns = 0;//斜边方向是否有连接
                    for (int i = 0; i < 4; i++)
                    {
                        var offset = this.neighbourOffsets[i];
                        int nx = node.XIndex + offset.x;
                        int nz = node.ZIndex + offset.y;
                        if (nx >= 0 && nz >= 0 &&
                            nx < this.m_oData.Width && nz < this.m_oData.Height)
                        {
                            var nNode = this.m_listNodes[this.CaculateIndex(nx, nz)] as GridMapNode;
                            if (nNode != null)
                            {
                                if (this.CheckCanConnection(nNode, node))
                                {
                                    fourDirConnectFlags |= (byte)(1 << i);
                                    //0001
                                    //0011....1111
                                }
                            }
                        }
                    }
                }
            }

        }

        private bool CheckCanConnection(GridMapNode node1,GridMapNode node2)
        {
            if (node1.Walkable == false || node2.Walkable == false)
            {
                return false;
            }
            return true;
        }

        public void Serialize(string data)
        {
            
        }

        public void Serialize(byte[] data)
        {
            
        }

        public int[] GetNeighbourOffsets()
        {
            throw new NotImplementedException();
        }

        public uint[] GetNeighbourCosts()
        {
            throw new NotImplementedException();
        }

        public MapNode GetMapNode(int nodeIndex)
        {
            throw new NotImplementedException();
        }
    }
}
