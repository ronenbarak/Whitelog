using System.Collections.Generic;
using Whitelog.Core.ListWriter;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.FileLog.SubmitLogEntry
{
    public class SyncSubmitLogEntryFactory : ISubmitLogEntryFactory
    {
        private IBufferAllocator m_rawDataProvider;
        Dictionary<IListWriter, ISubmitLogEntry> m_asyncFactory = new Dictionary<IListWriter, ISubmitLogEntry>();
        public SyncSubmitLogEntryFactory(IBufferAllocator rawDataProvider)
        {
            m_rawDataProvider = rawDataProvider;
        }

        public ISubmitLogEntry CreateSubmitLogEntry(IListWriter listWriter)
        {
            ISubmitLogEntry submitLogEntry;
            if (!m_asyncFactory.TryGetValue(listWriter, out submitLogEntry))
            {
                submitLogEntry = new SyncSubmitLogEntry(listWriter, m_rawDataProvider);
                m_asyncFactory.Add(listWriter, submitLogEntry);
            }

            return submitLogEntry;
        }
    }
}