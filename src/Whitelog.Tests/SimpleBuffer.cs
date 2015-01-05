using System;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Tests
{
    public class SimpleBuffer : IBuffer
    {
        private byte[] m_buffer;
        private int m_length;
        private readonly RawDataSerializer m_rawDataSerializer = new RawDataSerializer();

        public SimpleBuffer(int size = 1024)
        {
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
            System.Buffer.BlockCopy(m_buffer, 0, buffer, 0, length);
            m_length = length;
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

        public DateTime DateTime { get; set; }

        public void Dispose()
        {
            m_length = 0;
            m_rawDataSerializer.Reset();
        }

        public ISerializer AttachedSerializer
        {
            get { return m_rawDataSerializer; }
        }
    }
}