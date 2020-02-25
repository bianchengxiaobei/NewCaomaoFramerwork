using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaomaoFramework
{
    public class GridMapNode : MapNode
    {
        public int GridIndex
        {
            get;set;
        }
        public int XIndex
        {
            get;set;
        }
        public int ZIndex
        {
            get;set;
        }
        //0000~0011111111,后8位表示是否连接
        public byte ConnectFlags { get; set; }



        public void SetAllConnect(byte _connectionFlags)
        {
            unchecked
            {
                ConnectFlags &= 00000000;//先清除原来的
                ConnectFlags |= _connectionFlags;
            }
        }
        public void SetAllNoConnect()
        {
            unchecked
            {
                ConnectFlags &= 00000000;
            }
        }
    }
}
