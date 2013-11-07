using System;
using System.Collections.Generic;
using System.IO;
using Whitelog.Barak.Common.SystemTime;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.ListWriter;
using Whitelog.Core.File;

namespace Whitelog.Core.Loggers.StringAppender.File
{
    public class StringListWriter : IListWriter
    {
        public static readonly Guid Signature  = new Guid("5260911F-DC94-4064-953C-CA239EAC1735");
        private readonly object m_lockObject = new object();
        private Stream m_stream;
        private FileConfiguration m_fileConfiguration;
        public long Length { get { return m_stream.Length; } }
        public DateTime? m_nextArchive;
        private IStreamProvider m_streamProvider;

        public StringListWriter(IStreamProvider streamProvider)
        {
            m_streamProvider = streamProvider;
            m_stream = m_streamProvider.GetStream();
        }

        public byte[] GetListWriterSignature()
        {
            return Signature.ToByteArray();
        }

        public void WriteData(IRawData buffer)
        {
            if (m_streamProvider.ShouldArchive(m_stream.Length, buffer.Length, buffer.DateTime))
            {
                m_stream.Close();
                m_stream.Dispose();
                m_streamProvider.Archive();
                m_stream = m_streamProvider.GetStream();
            }
            
            // we skip the first 4 bytes since it is the length of the string
            m_stream.Write(buffer.Buffer, 4, buffer.Length - 4);
        }

        public void WriteData(IEnumerable<IRawData> buffer)
        {
            foreach (var rawData in buffer)
            {
                if (m_streamProvider.ShouldArchive(m_stream.Length, rawData.Length, rawData.DateTime))
                {
                    m_stream.Close();
                    m_stream.Dispose();
                    m_streamProvider.Archive();
                    m_stream = m_streamProvider.GetStream();
                }

                // we skip the first 4 bytes since it is the length of the string
                m_stream.Write(rawData.Buffer, 4, rawData.Length - 4);
            }
        }

        public object LockObject { get { return m_lockObject; } }
        
        public void Flush()
        {
            m_stream.Flush();
        }
    }
}