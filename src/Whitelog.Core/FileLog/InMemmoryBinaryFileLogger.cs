using System;
using System.IO;
using Whitelog.Interface;
using Whitelog.Core.ListWriter;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.FileLog
{
    public class InMemmoryBinaryFileLogger : IBinaryLogger
    {
        public static readonly Guid ID = new Guid("5BC9AE06-B6FD-4147-82A2-6E9F06140BCF");
        private BaseFileLog m_baseFileLog;
        private MemoryStream m_stream;

        public Stream Stream
        {
            get { return m_stream; }
        }

        public InMemmoryBinaryFileLogger(ISubmitLogEntryFactory submitLogEntryFactory)
        {
            m_stream = new MemoryStream();

            var logData = new ExpendableList(m_stream.Length, m_stream,  BufferPoolFactory.Instance.CreateBufferAllocator());
            var logEntry = submitLogEntryFactory.CreateSubmitLogEntry(logData);
            var stringCache = submitLogEntryFactory.CreateSubmitLogEntry(logData);
            var definition = submitLogEntryFactory.CreateSubmitLogEntry(logData);

            m_stream.Position = 0;
            m_stream.Write(ID.ToByteArray(), 0, ID.ToByteArray().Length);
            var logDataSign = logData.GetListWriterSignature();
            m_stream.Write(logDataSign, 0, logDataSign.Length);

            m_baseFileLog = new BaseFileLog(new BaseFileLog.BufferAndSubmiterTuple(logEntry, BufferPoolFactory.Instance.CreateBufferAllocator()),
                                            new BaseFileLog.BufferAndSubmiterTuple(stringCache, BufferPoolFactory.Instance.CreateBufferAllocator()),
                                            new BaseFileLog.BufferAndSubmiterTuple(definition, BufferPoolFactory.Instance.CreateBufferAllocator()));
        }

        public void AttachToTunnelLog(LogTunnel logTunnel)
        {
            m_baseFileLog.AttachToTunnelLog(logTunnel);
        }

        public void DetachTunnelLog(LogTunnel logTunnel)
        {
            m_baseFileLog.DetachTunnelLog(logTunnel);
        }

        public void RegisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            m_baseFileLog.RegisterDefinition(packageDefinition);
        }

        public void Dispose()
        {
            m_baseFileLog.Dispose();
            m_stream.Dispose();
        }
    }
}