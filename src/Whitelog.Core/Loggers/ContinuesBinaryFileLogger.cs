using System;
using System.IO;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.ListWriter;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;

namespace Whitelog.Core.Loggers
{
    public class ContinuesBinaryFileLogger : IBinaryLogger
    {
        public static readonly Guid ID = new Guid("CD06DD66-1C1B-4AB3-B061-D7A965620120");

        private readonly BinaryLogSerilizer m_binaryLogSerilizer;
        private ISubmitEntry<IRawData> m_logEntry;
        private ExpendableListWriter m_listWriter;

        public ContinuesBinaryFileLogger(IStreamProvider streamProvider, ISubmitLogEntryFactory submitLogEntryFactory, IBufferAllocatorFactory bufferAllocatorFactory)
        {
            m_listWriter = new ExpendableListWriter(streamProvider, OnNewfileCreated);
            m_logEntry = submitLogEntryFactory.CreateSubmitLogEntry(m_listWriter);
            var stringCache = submitLogEntryFactory.CreateSubmitLogEntry(m_listWriter);
            var definition = submitLogEntryFactory.CreateSubmitLogEntry(m_listWriter);

            m_binaryLogSerilizer = new BinaryLogSerilizer(new BinaryLogSerilizer.BufferAndSubmiterTuple(m_logEntry, bufferAllocatorFactory.CreateBufferAllocator()),
                                            new BinaryLogSerilizer.BufferAndSubmiterTuple(stringCache, bufferAllocatorFactory.CreateBufferAllocator()),
                                            new BinaryLogSerilizer.BufferAndSubmiterTuple(definition, bufferAllocatorFactory.CreateBufferAllocator()));
        }

        private static void OnNewfileCreated(ExpendableListWriter expendableListWriter, Stream stream)
        {
            bool isNewFile = stream.Position == 0;
            if (isNewFile)
            {
                stream.Write(ID.ToByteArray(), 0, ID.ToByteArray().Length);
                var logDataSign = expendableListWriter.GetListWriterSignature();
                stream.Write(logDataSign, 0, logDataSign.Length);
            }
        }

        public void WaitForIdle()
        {
            m_logEntry.WaitForIdle();
        }

        public void AttachToTunnelLog(LogTunnel logTunnel)
        {
            m_binaryLogSerilizer.AttachToTunnelLog(logTunnel);
        }

        public void DetachTunnelLog(LogTunnel logTunnel)
        {
            m_binaryLogSerilizer.DetachTunnelLog(logTunnel);
        }

        public void RegisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            m_binaryLogSerilizer.RegisterDefinition(packageDefinition);
        }

        /*public void UnregisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            m_binaryLogSerilizer.UnregisterDefinition(packageDefinition);
        }*/

        public void Dispose()
        {
            m_binaryLogSerilizer.Dispose();            
            m_listWriter.Dispose();
        }
    }
}