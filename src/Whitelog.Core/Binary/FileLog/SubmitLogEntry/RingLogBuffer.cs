using System;
using Whitelog.Barak.Common.DataStructures.Ring;
using Whitelog.Core.Binary.Serializer;

namespace Whitelog.Core.Binary.FileLog.SubmitLogEntry
{
    internal class RingLogBuffer : IBuffer
    {
        private byte[] m_buffer;
        private int m_length;
        private RingBuffer<RingLogBuffer> m_ring;
        public long m_sequance;
        private readonly RawDataSerializer m_rawDataSerializer = new RawDataSerializer();

        public void SetSequance(long sequance)
        {
            m_sequance = sequance;
            m_length = 0;
        }

        public RingLogBuffer(int size, RingBuffer<RingLogBuffer> ring)
        {
            m_buffer = new byte[size];
            m_ring = ring;
            m_rawDataSerializer.Init(this);
        }

        public void SetLength(int length)
        {
            m_length = length;
        }

        public byte[] IncressSize(int length, int required)
        {
            var buffer = new byte[required];
            System.Buffer.BlockCopy(m_buffer, 0, buffer, 0, length);
            m_length = length;
            m_buffer = buffer;
            return m_buffer;
        }

        public int Length
        {
            get { return m_length; }
        }

        public byte[] Buffer
        {
            get { return m_buffer; }
            set
            {
                throw new NotImplementedException("Not Implemented RingLogBuffer.set_Buffer");
            }
        }

        public void Dispose()
        {
            m_ring.Commit(m_sequance);
        }

        public ISerializer AttachedSerializer
        {
            get { return m_rawDataSerializer; }
        }
    }
}