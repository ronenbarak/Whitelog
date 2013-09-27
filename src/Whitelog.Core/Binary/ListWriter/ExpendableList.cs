using System;
using System.Collections.Generic;
using System.IO;
using Whitelog.Core.Binary.FileLog;

namespace Whitelog.Core.Binary.ListWriter
{
    public class ExpendableList : IListWriter
    {
        class ExpendableBuffer : IRawData
        {
            public int Length { get; set; }
            public byte[] Buffer { get; set; }
        }

        public static readonly Guid ID = new Guid("89AA96B3-E9BA-48C9-80CA-CE83BF2AEA70");
        private Stream m_stream;
        private long m_readIndex;
        private ExpendableBuffer m_expendableBuffer;

        public ExpendableList(Stream stream):this(stream,stream.Position)
        {            
        }
        
        public ExpendableList(Stream stream, long startReadPosition)
        {
            m_readIndex = startReadPosition;
            m_stream = stream;
            m_expendableBuffer = new ExpendableBuffer()
                                 {
                                     Buffer = new byte[1024],
                                     Length = 0,
                                 };
        }

        private void MoveToLastPosition()
        {
            m_stream.Position = m_stream.Length;
        }

        public void WriteData(IRawData buffer)
        {
            MoveToLastPosition();
            m_stream.Write(BitConverter.GetBytes(buffer.Length),0,sizeof(int));
            m_stream.Write(buffer.Buffer, 0, buffer.Length);
        }

        public void WriteData(IEnumerable<IRawData> buffers)
        {
            MoveToLastPosition();
            foreach (var buffer in buffers)
            {
                m_stream.Write(BitConverter.GetBytes(buffer.Length), 0, sizeof(int));
                m_stream.Write(buffer.Buffer, 0, buffer.Length);
            }
        }


        public object LockObject
        {
            get { return m_stream; }
        }

        public bool Read(IBufferConsumer bufferConsumer)
        {
            lock (LockObject)
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
            }
            return false;
        }

        public bool ReadAll(IBufferConsumer bufferConsumer)
        {
            lock (LockObject)
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

                byte[] intSizeBuffer = new byte[sizeof (int)];
                bool hasAnyRead = false;
                while (!ended)
                {
                    m_stream.Read(intSizeBuffer, 0, intSizeBuffer.Length);
                    int sizeBuffer = BitConverter.ToInt32(intSizeBuffer, 0);
                    if (sizeBuffer != -1)
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
                    }
                    ended = !(m_readIndex < maxRead);
                }
                return hasAnyRead;
            }
        }

        public void Flush()
        {
            m_stream.Flush();
        }

        public byte[] GetListWriterSignature()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(ID.ToByteArray(), 0, ID.ToByteArray().Length);                
                return ms.ToArray();
            }
        }
    }
}