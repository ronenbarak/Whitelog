using System;
using System.IO;
using Whitelog.Interface;
using Whitelog.Core.ListWriter;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.FileLog
{
    public class ContinuesBinaryFileLogger : IBinaryLogger
    {
        public static readonly Guid ID = new Guid("CD06DD66-1C1B-4AB3-B061-D7A965620120");

        private readonly BaseFileLog m_baseFileLog;
        private FileStream m_stream;
        private ISubmitLogEntry m_logEntry;

        public ContinuesBinaryFileLogger(string filePath,ISubmitLogEntryFactory submitLogEntryFactory,IBufferAllocatorFactory bufferAllocatorFactory)
        {
            m_stream = System.IO.File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            bool isNewFile = m_stream.Length == 0;

            var logData = new ExpendableList(m_stream.Length, m_stream, BufferPoolFactory.Instance.CreateBufferAllocator());
            m_logEntry = submitLogEntryFactory.CreateSubmitLogEntry(logData);
            var stringCache = submitLogEntryFactory.CreateSubmitLogEntry(logData);
            var definition = submitLogEntryFactory.CreateSubmitLogEntry(logData);
            if (isNewFile)
            {
                m_stream.Position = 0;
                m_stream.Write(ID.ToByteArray(), 0, ID.ToByteArray().Length);
                var logDataSign = logData.GetListWriterSignature();
                m_stream.Write(logDataSign, 0, logDataSign.Length);
            }

            m_baseFileLog = new BaseFileLog(new BaseFileLog.BufferAndSubmiterTuple(m_logEntry, bufferAllocatorFactory.CreateBufferAllocator()),
                                            new BaseFileLog.BufferAndSubmiterTuple(stringCache, bufferAllocatorFactory.CreateBufferAllocator()),
                                            new BaseFileLog.BufferAndSubmiterTuple(definition, bufferAllocatorFactory.CreateBufferAllocator()));
        }
        
        public void WaitForIdle()
        {
            m_logEntry.WaitForIdle();   
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

        /*public void UnregisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            m_baseFileLog.UnregisterDefinition(packageDefinition);
        }*/

        public void Dispose()
        {
            m_baseFileLog.Dispose();
            m_stream.Dispose();
        }
    }
}