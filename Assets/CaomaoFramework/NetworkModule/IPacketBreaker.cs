
namespace CaomaoFramework
{
    public interface IPacketBreaker
    {
        /// <summary>
        /// 黏包处理
        /// </summary>
        /// <param name="data">处理的数据</param>
        /// <param name="index">从哪里开始</param>
        /// <param name="len">处理的长度</param>
        /// <returns>单条数据长度</returns>
        int BreakPacket(byte[] data, int index, int len);
    }
}