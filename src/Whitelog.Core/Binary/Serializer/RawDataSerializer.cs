using System;
using System.Text;
using Whitelog.Core.Binary.FileLog;

namespace Whitelog.Core.Binary.Serializer
{
    public class RawDataSerializer : ISerializer
    {
        private static readonly UTF8Encoding Encoding = new UTF8Encoding();
        private int m_ioIndex = 0;
        private byte[] m_buffer;
        private IBuffer m_rawData;
        private int bufferLength = 0;

        public RawDataSerializer(IBuffer buffer)
        {
            Init(buffer);
        }

        public RawDataSerializer()
        {
        }

        public void Init(IBuffer buffer)
        {
            m_rawData = buffer;
            m_buffer = m_rawData.Buffer;
            bufferLength = m_buffer.Length;
            m_ioIndex = 0;
        }

        public void Reset()
        {
            m_ioIndex = 0;
        }

        private void DemandSpace(int required)
        {
            // check for enough space
            if ((bufferLength - m_ioIndex) < required)
            {
                int newLength = bufferLength * 2;
                if (newLength < (bufferLength + required))
                {
                    newLength = bufferLength + required;
                }

                m_buffer = m_rawData.IncressSize(m_ioIndex,newLength);
                bufferLength = m_buffer.Length;
            }
        }

        public void Serialize(int value)
        {
            DemandSpace(sizeof(int));
            m_buffer[m_ioIndex] = (byte)value;
            m_buffer[m_ioIndex + 1] = (byte)(value >> 8);
            m_buffer[m_ioIndex + 2] = (byte)(value >> 16);
            m_buffer[m_ioIndex + 3] = (byte)(value >> 24);
            m_ioIndex += sizeof(int);
        }

        public void SerializeVariant(int value)
        {
            uint svalue = (uint)value;
            DemandSpace(5);
            do
            {
                m_buffer[m_ioIndex++] = (byte)((svalue & 0x7F) | 0x80);
            } while ((svalue >>= 7) != 0);
            m_buffer[m_ioIndex - 1] &= 0x7F;
        }

        public void Serialize(long value)
        {
            DemandSpace(sizeof(long));
            m_buffer[m_ioIndex] = (byte)value;
            m_buffer[m_ioIndex + 1] = (byte)(value >> 8);
            m_buffer[m_ioIndex + 2] = (byte)(value >> 16);
            m_buffer[m_ioIndex + 3] = (byte)(value >> 24);
            m_buffer[m_ioIndex + 4] = (byte)(value >> 32);
            m_buffer[m_ioIndex + 5] = (byte)(value >> 40);
            m_buffer[m_ioIndex + 6] = (byte)(value >> 48);
            m_buffer[m_ioIndex + 7] = (byte)(value >> 56);
            m_ioIndex += sizeof(long);
        }

        public void Serialize(double value)
        {
#if UNSafe
            Serialize(*(long*)&value);
#else
            Serialize(BitConverter.ToInt64(BitConverter.GetBytes(value),0));
#endif
        }

        public void Serialize(bool value)
        {
            DemandSpace(1);
            m_buffer[m_ioIndex] = value ? (byte)1 : (byte)0;
            m_ioIndex++;
        }

        public void Serialize(byte value)
        {
            DemandSpace(1);
            m_buffer[m_ioIndex] = value;
            m_ioIndex++;
        }

        public void Serialize(string value)
        {
            if (value == null)
            {
                Serialize(-1);
            }
            else
            {
                int predicted = (value.Length + 1) * 2;
                DemandSpace(predicted + sizeof(int));
                // We write the string 4 bytes after the index
                int realSize = Encoding.GetBytes(value, 0, value.Length, m_buffer, m_ioIndex + sizeof(int));

                // and than we add the size added at the index location
                m_buffer[m_ioIndex] = (byte)realSize;
                m_buffer[m_ioIndex + 1] = (byte)(realSize >> 8);
                m_buffer[m_ioIndex + 2] = (byte)(realSize >> 16);
                m_buffer[m_ioIndex + 3] = (byte)(realSize >> 24);

                // and advance the index
                m_ioIndex += realSize + sizeof(int);
            }
        }

        public void Serialize(byte[] value,int srcOffset, int count)
        {
            DemandSpace(count);
            System.Buffer.BlockCopy(value, srcOffset, m_buffer, m_ioIndex, count);
            m_ioIndex += count;
        }

        public void Flush()
        {
            m_rawData.SetLength(m_ioIndex);
        }

    }
}
