using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public class MapNode
    {
        public int NodeIndex;
        public int Penalty;
        public byte ConnectFlags;
        public FPVector3 Position;

        public bool Walkable { get; set; }
        //public bool Connectionable { get; set; }


        public bool HasConnectInDir(int dir)
        {
            return (this.ConnectFlags >> dir & 1) != 0;
        }
        public void SetConnect(int dir, bool value)
        {
            unchecked
            {
                byte canConnectMask = value ? (byte)1 : (byte)(0 << dir);
                byte connectDir = (byte)(this.ConnectFlags & ~(byte)(1 << dir));
                this.ConnectFlags = (byte)(connectDir | canConnectMask);
            }
        }
        public void SetAllConnect(int connect)
        {
            this.SetAllNoConnect();
            unchecked
            {
                this.ConnectFlags = (byte)connect;
            }
        }
        public void SetAllNoConnect()
        {
            unchecked
            {
                this.ConnectFlags = (byte)(this.ConnectFlags & 00000000);
            }
        }
    }
}
