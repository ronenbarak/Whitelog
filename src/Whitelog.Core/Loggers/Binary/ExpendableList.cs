using System;
using System.Collections.Generic;
using System.IO;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;

namespace Whitelog.Core.Loggers.Binary
{
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
            return ID.ToByteArray();
        }

        public void Dispose()
        {
            m_stream.Dispose();
        }
    }
}