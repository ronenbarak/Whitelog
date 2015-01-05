using System.Collections.Generic;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Loggers.Binary.SubmitLogEntry
{
    public class SyncSubmitLogEntryFactory : ISubmitLogEntryFactory
    {
        Dictionary<IListWriter, ISubmitEntry<IRawData>> m_asyncFactory = new Dictionary<IListWriter, ISubmitEntry<IRawData>>();
        
        public SyncSubmitLogEntryFactory()
        {
        }

        public ISubmitEntry<IRawData> CreateSubmitLogEntry(IListWriter listWriter)
        {
            ISubmitEntry<IRawData> submitEntry;
            if (!m_asyncFactory.TryGetValue(listWriter, out submitEntry))
            {
                submitEntry = new SyncSubmitEntry(listWriter);
                m_asyncFactory.Add(listWriter, submitEntry);
            }

            return submitEntry;
        }
    }
}