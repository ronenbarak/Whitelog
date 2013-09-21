using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.Serializer
{
    public class StreamDeserilizer : IDeserializer, IDisposable
    {
        private static readonly UTF8Encoding Encoding = new UTF8Encoding();
        private byte[] m_buffer;
        private int m_ioIndex = 0;

        public StreamDeserilizer(byte[] buffer)
        {
            m_buffer = buffer;
        }

        public int DeserializeInt()
        {
            return ((int)m_buffer[m_ioIndex++])
                        | (((int)m_buffer[m_ioIndex++]) << 8)
                        | (((int)m_buffer[m_ioIndex++]) << 16)
                        | (((int)m_buffer[m_ioIndex++]) << 24);
        }

        public long DeserializeLong()
        {
            return ((long)m_buffer[m_ioIndex++])
                        | (((long)m_buffer[m_ioIndex++]) << 8)
                        | (((long)m_buffer[m_ioIndex++]) << 16)
                        | (((long)m_buffer[m_ioIndex++]) << 24)
                        | (((long)m_buffer[m_ioIndex++]) << 32)
                        | (((long)m_buffer[m_ioIndex++]) << 40)
                        | (((long)m_buffer[m_ioIndex++]) << 48)
                        | (((long)m_buffer[m_ioIndex++]) << 56);
        }

        public double DeserializeDouble()
        {
            var value = BitConverter.ToDouble(m_buffer, m_ioIndex);
            m_ioIndex += sizeof (double);
            return value;
        }

        public bool DeserializeBool()
        {
            if (m_buffer[m_ioIndex++] == 0)
            {
                return false;
            }
            return true;
        }

        public byte DeserializeByte()
        {
            return m_buffer[m_ioIndex++];
        }

        public string DeserializeString()
        {
            var stringLegnth = DeserializeVariantInt();
            if (stringLegnth == 0)
            {
                return null;
            }
            else
            {
                if (stringLegnth == 1)
                {
                    return string.Empty;
                }
                else
                {
                    var value = string.Intern(Encoding.GetString(m_buffer, m_ioIndex, stringLegnth -1));
                    m_ioIndex += (stringLegnth -1);
                    return value;      
                }
            }
        }

        public byte[] DeserializeByteArray()
        {
            var arrayLegnth = DeserializeInt();
            if (arrayLegnth == 0)
            {
                return new byte[0];
            }

            return DeserializeByteArray(arrayLegnth);
        }

        public byte[] DeserializeByteArray(int length)
        {
            var buffer = new byte[length];
            Buffer.BlockCopy(m_buffer, m_ioIndex, buffer, 0, length);
            m_ioIndex += length;
            return buffer;
        }

        public int DeserializeVariantInt()
        {
            uint value = m_buffer[m_ioIndex++];

            if ((value & 0x80) == 0) return (int)value;
            value &= 0x7F;

            uint chunk = m_buffer[m_ioIndex++];
            value |= (chunk & 0x7F) << 7;
            if ((chunk & 0x80) == 0) return (int)value;

            chunk = m_buffer[m_ioIndex++];
            value |= (chunk & 0x7F) << 14;
            if ((chunk & 0x80) == 0) return (int)value;

            chunk = m_buffer[m_ioIndex++];
            value |= (chunk & 0x7F) << 21;
            if ((chunk & 0x80) == 0) return (int)value;

            chunk = m_buffer[m_ioIndex++];
            value |= chunk << 28; // can only use 4 bits from this chunk
            if ((chunk & 0xF0) == 0) return (int)value;

            throw new CorruptedDataException();
        }

        public void Dispose()
        {
        }
    }
}
