using System;
using Whitelog.Barak.Common.Events;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.PackageDefinitions;
using Whitelog.Core.PackageDefinitions.LogDefinisoins;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.Binary
{
    public class BinaryLogSerilizer : IDisposable
    {
        public class BufferAndSubmiterTuple
        {
            public BufferAndSubmiterTuple(ISubmitEntry<IRawData> entry,IBufferAllocator bufferAllocator)
            {
                SubmitLogEntry = entry;
                BufferAllocator = bufferAllocator;
            }

            public ISubmitEntry<IRawData> SubmitLogEntry { get; private set; }
            public IBufferAllocator BufferAllocator { get; private set; }
        }

        private readonly StreamLogBinaryPackager m_streamLogBinaryPackager;
        private ISubmitEntry<IRawData> m_logEntrySubmiter;
        private ISubmitEntry<IRawData> m_stringCacheSubmiter;
        private ISubmitEntry<IRawData> m_definitionSubmiter;
        private IBufferAllocator m_logEntryBufferAllocator;
        private IBufferAllocator m_stringCacheBufferAllocator;
        private IBufferAllocator m_definitionBufferAllocator;

        public BinaryLogSerilizer(BufferAndSubmiterTuple logentry,BufferAndSubmiterTuple definitions, BufferAndSubmiterTuple stringCache)
        {
            m_logEntrySubmiter = logentry.SubmitLogEntry;
            m_logEntryBufferAllocator = logentry.BufferAllocator;
            m_stringCacheBufferAllocator = stringCache.BufferAllocator;
            m_stringCacheSubmiter = stringCache.SubmitLogEntry;
            m_definitionBufferAllocator = definitions.BufferAllocator;
            m_definitionSubmiter = definitions.SubmitLogEntry;

            m_streamLogBinaryPackager = new StreamLogBinaryPackager();
            m_streamLogBinaryPackager.StringChached += OnStringChached;
            m_streamLogBinaryPackager.PackageRegistered += OnPackageRegistered;

            m_streamLogBinaryPackager.RegisterDefinition(new LogEntryPackageDefinition());
            m_streamLogBinaryPackager.RegisterDefinition(new StringLogTitlePackageDefinition());
            m_streamLogBinaryPackager.RegisterDefinition(new CustomStringLogTitlePackageDefinition());
            m_streamLogBinaryPackager.RegisterDefinition(new OpenLogScopeTitlePackageDefinition());
            m_streamLogBinaryPackager.RegisterDefinition(new PackageDefinition<CloseLogScopeTitle>());
            m_streamLogBinaryPackager.RegisterDefinition(new StringCachePackageDefinition());

            m_streamLogBinaryPackager.RegisterDefinition(new ObjectPackageDefinition());
        }

        public void AttachToTunnelLog(LogTunnel logTunnel)
        {
            logTunnel.LogEntry += aggregateLog_LogEntry;
        }

        public void DetachTunnelLog(LogTunnel logTunnel)
        {
            logTunnel.LogEntry -= aggregateLog_LogEntry;
        }

        private  void aggregateLog_LogEntry(LogEntry logEntry)
        {
            var buffer = m_logEntryBufferAllocator.Allocate();
            try
            {
                buffer.DateTime = logEntry.Time;
                var dataSerializer = buffer.AttachedSerializer;
                m_streamLogBinaryPackager.Pack(logEntry, dataSerializer);
                dataSerializer.Flush();
                m_logEntrySubmiter.AddEntry(buffer);
            }
            finally
            {
                buffer.Dispose();
            }

        }

        private void OnPackageRegistered(object sender, EventArgs<RegisteredPackageDefinition> e)
        {
            var buffer = m_definitionBufferAllocator.Allocate();
            try
            {
                buffer.DateTime = DateTime.MinValue;
                var dataSerializer = buffer.AttachedSerializer;
                m_streamLogBinaryPackager.Pack(e.Data, dataSerializer);
                dataSerializer.Flush();
                m_definitionSubmiter.AddEntry(buffer);
            }
            finally
            {
                buffer.Dispose();
            }
            
        }

        private void OnStringChached(object sender, EventArgs<CacheString> e)
        {
            var buffer = m_stringCacheBufferAllocator.Allocate();
            try
            {
                buffer.DateTime = DateTime.MinValue;
                var dataSerializer = buffer.AttachedSerializer;
                m_streamLogBinaryPackager.Pack(e.Data, dataSerializer);
                dataSerializer.Flush();
                m_stringCacheSubmiter.AddEntry(buffer);
            }
            finally 
            {
                buffer.Dispose();
            }
        }

        public virtual void RegisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            m_streamLogBinaryPackager.RegisterDefinition(packageDefinition);
        }

        /*public virtual void UnregisterDefinition(IBinaryPackageDefinition packageDefinition)
        {
            m_streamLogBinaryPackager.UnregisterDefinition(packageDefinition);
        }*/

        public void Dispose()
        {
        }
    }
}