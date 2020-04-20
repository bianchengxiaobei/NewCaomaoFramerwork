using System;
using System.Collections.Generic;
using UnityEngine;
public class RecycleBufferPipeline
{
    private LinkedList<RecycleBuffer> m_pieces = new LinkedList<RecycleBuffer>();
    private LinkedList<RecycleBuffer> m_abandonPiecesCache = new LinkedList<RecycleBuffer>();
    /// <summary>
    /// 全部数据长度
    /// </summary>
    private int m_nDataSize = 0;
    /// <summary>
    ///当前存在的buffer数量限制
    /// </summary>
    private int m_nMinPieceNum;
    /// <summary>
    /// 缓存buffer的大小
    /// </summary>
    private int m_nPieceSize;
    public RecycleBufferPipeline()
        : this(4096, 10)
    {
    }
    public RecycleBufferPipeline(int pieceSize, int minPieceNum)
    {
        this.m_nPieceSize = pieceSize;
        this.m_nMinPieceNum = minPieceNum;
        this.m_pieces.AddLast(new RecycleBuffer(this.m_nPieceSize));
    }
    /// <summary>
    /// 读取数据，如果超出buffer就添加这个已经读取的数据到abandonpiecesCache里面
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    public void Read(byte[] buffer, int offset, int len)
    {
        if ((len > 0 && offset + len <= buffer.Length && len <= this.m_nDataSize) == false)
        {
            Debug.LogError("Read Error");
            return;
        }
        this.m_nDataSize -= len;
        while (len > 0)
        {
            RecycleBuffer value = this.m_pieces.First.Value;
            if (value.IsReadOver)
            {
                LinkedListNode<RecycleBuffer> first = this.m_pieces.First;
                this.m_pieces.RemoveFirst();
                if (this.m_pieces.Count + this.m_abandonPiecesCache.Count < this.m_nMinPieceNum)
                {
                    this.m_abandonPiecesCache.AddLast(first);
                }
            }
            else
            {
                int num = value.Read(buffer, offset, len);
                offset += num;
                len -= num;
            }
        }
    }
    /// <summary>
    /// 写入数据，如果数据溢出，就
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="len"></param>
    public void Write(byte[] buffer, int offset, int len)
    {
        if (len <= 0 || offset + len > buffer.Length)
        {
            Debug.LogError("Write Error");
            return;
        }
        this.m_nDataSize += len;
        while (len > 0)
        {
            RecycleBuffer pieceBuffer = this.m_pieces.Last.Value;
            if (pieceBuffer.IsWriteOver)
            {
                if (this.m_abandonPiecesCache.Count > 0)
                {
                    LinkedListNode<RecycleBuffer> first = this.m_abandonPiecesCache.First;
                    this.m_abandonPiecesCache.RemoveFirst();
                    this.m_pieces.AddLast(first);
                    pieceBuffer = first.Value;
                }
                else
                {
                    pieceBuffer = new RecycleBuffer(this.m_nPieceSize);
                    this.m_pieces.AddLast(pieceBuffer);
                }
            }
            int num = pieceBuffer.Write(buffer, offset, len);
            offset += num;
            len -= num;
        }
    }
}