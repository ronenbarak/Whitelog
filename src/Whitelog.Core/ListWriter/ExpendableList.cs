using System;
using System.Collections.Generic;
using System.IO;
using Whitelog.Core.FileLog;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.ListWriter
{
    public class ExpendableList : IListWriter
    {
        public static readonly Guid ID = new Guid("89AA96B3-E9BA-48C9-80CA-CE83BF2AEA70");
        private Stream m_stream;
        private long m_startPosition;
        private long m_readIndex;
        private IBufferAllocator m_bufferAllocator;

        public ExpendableList(long startPosition, Stream stream,IBufferAllocator bufferAllocator)
        {
            m_bufferAllocator = bufferAllocator;
            m_readIndex = m_startPosition = startPosition;
            m_stream = stream;
            if (m_stream.Length < startPosition)
            {
                m_stream.SetLength(startPosition);
            }
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

        public byte[] Read()
        {
            lock (LockObject)
            {
                IBuffer mainBuffer = m_bufferAllocator.Allocate();
                byte[] intSizeBuffer = new byte[sizeof(int)];
                if (m_readIndex < m_stream.Length)
                {
                    m_stream.Position = m_readIndex;
                    m_stream.Read(intSizeBuffer, 0, intSizeBuffer.Length);
                    int sizeBuffer = BitConverter.ToInt32(intSizeBuffer, 0);
                    if (sizeBuffer != -1)
                    {
                        byte[] dataBuffer = mainBuffer.Buffer;
                        if (mainBuffer.Length >= sizeBuffer)
                        {
                            dataBuffer = new byte[sizeBuffer];
                        }

                        if (m_stream.Read(dataBuffer, 0, sizeBuffer) != sizeBuffer)
                        {
                            throw new CorruptedDataException();
                        }
                        else
                        {
                            m_readIndex = m_stream.Position;
                            return dataBuffer;
                        }
                    }
                }
                return null;
            }
        }

        public void ReadAll(IObjectObserver data)
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

                using (IBuffer mainBuffer = m_bufferAllocator.Allocate())
                {
                    byte[] intSizeBuffer = new byte[sizeof (int)];
                    while (!ended)
                    {
                        m_stream.Read(intSizeBuffer, 0, intSizeBuffer.Length);
                        int sizeBuffer = BitConverter.ToInt32(intSizeBuffer, 0);
                        if (sizeBuffer != -1)
                        {
                            byte[] dataBuffer = mainBuffer.Buffer;
                            if (mainBuffer.Length < sizeBuffer)
                            {
                                dataBuffer = new byte[sizeBuffer];
                            }

                            if (m_stream.Read(dataBuffer, 0, sizeBuffer) != sizeBuffer)
                            {
                                throw new CorruptedDataException();
                            }
                            else
                            {
                                m_readIndex = m_stream.Position;
                                data.Add(dataBuffer);
                            }
                        }
                        ended = !(m_readIndex < maxRead);
                    }
                }
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
                ms.Write(BitConverter.GetBytes(m_startPosition), 0, sizeof(long));
                return ms.ToArray();
            }
        }
    }
}