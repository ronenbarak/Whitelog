using System;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Binary.Deserilizer
{
    public class CloneRawData : IRawData
    {
        private byte[] m_newBuffer;

        public CloneRawData(byte[] buffer)
            : this(buffer, buffer.Length)
        {
        }

        public CloneRawData(byte[] buffer, int legnth)
        {
            var newBuffer = new byte[legnth];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, legnth);
            m_newBuffer = newBuffer;
        }

        public CloneRawData(byte[] buffer, int legnth, DateTime dateTime)
        {
            DateTime = dateTime;
            var newBuffer = new byte[legnth];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, legnth);
            m_newBuffer = newBuffer;
        }

        public int Length
        {
            get { return m_newBuffer.Length; }
        }

        public byte[] Buffer
        {
            get { return m_newBuffer; }
        }

        public DateTime DateTime { get; set; }
    }
}