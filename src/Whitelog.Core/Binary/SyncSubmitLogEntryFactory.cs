using System.Collections.Generic;
using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary
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