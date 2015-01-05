using System;
using System.IO;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Binary.Deserilizer.Reader
{
    public class ExpendableListReader : IListReader
    {
        class ExpendableBuffer : IRawData
        {
            public int Length { get; set; }
            public byte[] Buffer { get; set; }
            public DateTime DateTime { get; set; }
        }
        private ExpendableBuffer m_expendableBuffer;

        private long m_readIndex;
        private Stream m_stream;

        public ExpendableListReader(Stream stream):this (stream,stream.Position)
        {
            
        }
        public ExpendableListReader(Stream stream, long startReadPosition)
        {
            m_readIndex = startReadPosition;
            m_stream = stream;
            m_expendableBuffer = new ExpendableBuffer()
            {
                Buffer = new byte[1024],
                Length = 0,
            };
        }

        public bool Read(IBufferConsumer bufferConsumer)
        {
            byte[] intSizeBuffer = new byte[sizeof(int)];
            if (m_readIndex < m_stream.Length)
            {
                m_stream.Position = m_readIndex;
                m_stream.Read(intSizeBuffer, 0, intSizeBuffer.Length);
                int sizeBuffer = BitConverter.ToInt32(intSizeBuffer, 0);
                if (sizeBuffer != -1)
                {
                    if (m_expendableBuffer.Buffer.Length >= sizeBuffer)
                    {
                        m_expendableBuffer.Buffer = new byte[sizeBuffer];
                    }

                    if (m_stream.Read(m_expendableBuffer.Buffer, 0, sizeBuffer) != sizeBuffer)
                    {
                        throw new CorruptedDataException();
                    }
                    else
                    {
                        m_readIndex = m_stream.Position;
                        m_expendableBuffer.Length = sizeBuffer;
                        bufferConsumer.Consume(m_expendableBuffer);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ReadAll(IBufferConsumer bufferConsumer)
        {
            var maxRead = m_stream.Length;
            bool ended = false;

            if (m_readIndex < maxRead)
            {
                m_stream.Position = m_readIndex;
            }
            else
            {
                ended = true;
            }

            byte[] intSizeBuffer = new byte[sizeof(int)];
            bool hasAnyRead = false;
            while (!ended)
            {
                if (m_stream.Read(intSizeBuffer, 0, intSizeBuffer.Length) != sizeof (int))
                {
                    ended = true;
                }
                else
                {

                    int sizeBuffer = BitConverter.ToInt32(intSizeBuffer, 0);
                    if (sizeBuffer == 0)
                    {
                        // this is for debug only 
                        m_stream.Seek(-4, SeekOrigin.Current);
                        ended = true;
                    }
                    else
                    {
                        if (m_expendableBuffer.Buffer.Length < sizeBuffer)
                        {
                            m_expendableBuffer.Buffer = new byte[sizeBuffer];
                        }

                        if (m_stream.Read(m_expendableBuffer.Buffer, 0, sizeBuffer) != sizeBuffer)
                        {
                            throw new CorruptedDataException();
                        }
                        else
                        {
                            m_readIndex = m_stream.Position;
                            m_expendableBuffer.Length = sizeBuffer;
                            bufferConsumer.Consume(m_expendableBuffer);
                            hasAnyRead = true;
                        }
                        ended = !(m_readIndex < maxRead);
                    }
                }
            }
            return hasAnyRead;
        }
    }
}