using System;
using System.Collections.Generic;
using System.IO;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.File;

namespace Whitelog.Core.Binary.ListWriter
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
    
    public class ExpendableListWriter : IListWriter,IDisposable
    {
        public static readonly Guid ID = new Guid("89AA96B3-E9BA-48C9-80CA-CE83BF2AEA70");
        private Stream m_stream;
        private IStreamProvider m_streamProvider;
        private Action<ExpendableListWriter,Stream> m_onNewfileCreated;

        public ExpendableListWriter(IStreamProvider streamProvider, Action<ExpendableListWriter,Stream> onNewfileCreated)
        {
            m_onNewfileCreated = onNewfileCreated;
            m_streamProvider = streamProvider;
            m_stream = m_streamProvider.GetStream();
            onNewfileCreated.Invoke(this,m_stream);
        }

        public void WriteData(IRawData buffer)
        {
            if (m_streamProvider.ShouldArchive(m_stream.Length, buffer.Length, buffer.DateTime))
            {
                m_stream.Close();
                m_stream.Dispose();
                m_streamProvider.Archive();
                m_stream = m_streamProvider.GetStream();
                m_onNewfileCreated.Invoke(this,m_stream);
            }

            m_stream.Write(BitConverter.GetBytes(buffer.Length),0,sizeof(int));
            m_stream.Write(buffer.Buffer, 0, buffer.Length);
        }

        public void WriteData(IEnumerable<IRawData> buffers)
        {
            foreach (var buffer in buffers)
            {
                if (m_streamProvider.ShouldArchive(m_stream.Length, buffer.Length, buffer.DateTime))
                {
                    m_stream.Close();
                    m_stream.Dispose();
                    m_streamProvider.Archive();
                    m_stream = m_streamProvider.GetStream();
                    m_onNewfileCreated.Invoke(this,m_stream);
                }

                m_stream.Write(BitConverter.GetBytes(buffer.Length), 0, sizeof(int));
                m_stream.Write(buffer.Buffer, 0, buffer.Length);
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

        public void Dispose()
        {
            m_stream.Dispose();
        }
    }
}