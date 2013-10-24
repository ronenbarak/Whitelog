using System;
using System.Threading;
using Whitelog.Core.Binary.FileLog;

namespace Whitelog.Core.Binary.Serializer.MemoryBuffer
{
    internal class BufferPool
    {
        class Buffer : IBuffer
        {
            private byte[] m_buffer;
            private int m_length;
            private BufferPool m_bufferPool;
            private RawDataSerializer m_rawDataSerializer = new RawDataSerializer();

            public Buffer(BufferPool bufferPool, int size)
            {
                m_bufferPool = bufferPool;
                m_buffer = new byte[size];
                m_rawDataSerializer.Init(this);
            }

            public void SetLength(int length)
            {
                m_length = length;
            }

            public byte[] IncressSize(int length, int required)
            {
                var buffer = new byte[required];
                System.Buffer.BlockCopy(m_buffer,0,buffer,0,length);
                m_buffer = buffer;
                return m_buffer;
            }

            public int Length
            {
                get { return m_length; }
            }

            byte[] IRawData.Buffer
            {
                get { return m_buffer; }
            }

            public void Dispose()
            {
                m_rawDataSerializer.Reset();
                if (m_bufferPool != null)
                {
                    m_bufferPool.ReleaseBufferToPool(this);
                }
            }

            public ISerializer AttachedSerializer
            {
                get { return m_rawDataSerializer; }
            }
        }

        public BufferPool()
        {
            for (int i = 0; i < PoolSize; i++)
            {
                pool[i] = new Buffer(this, BufferLength);
            }
        }

        const int PoolSize = 20;
        internal const int BufferLength = BufferDefaults.Size;
        private readonly IBuffer[] pool = new IBuffer[PoolSize];

        internal IBuffer GetBuffer()
        {
            IBuffer tmp;
            for (int i = 0; i < pool.Length; i++)
            {
                if ((tmp = Interlocked.Exchange(ref pool[i], null)) != null) return tmp;
            }

            return new Buffer(null,BufferLength);
        }

        internal void ReleaseBufferToPool(IBuffer buffer)
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (Interlocked.CompareExchange(ref pool[i], buffer, null) == null)
                {
                    break; // found a null; swapped it in
                }
            }
        }

    }
}
