using System.Collections.Generic;
using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary.FileLog.SubmitLogEntry
{
    public class SyncSubmitLogEntryFactory : ISubmitLogEntryFactory
    {
        Dictionary<IListWriter, ISubmitLogEntry> m_asyncFactory = new Dictionary<IListWriter, ISubmitLogEntry>();
        
        public SyncSubmitLogEntryFactory()
        {
        }

        public ISubmitLogEntry CreateSubmitLogEntry(IListWriter listWriter)
        {
            ISubmitLogEntry submitLogEntry;
            if (!m_asyncFactory.TryGetValue(listWriter, out submitLogEntry))
            {
                submitLogEntry = new SyncSubmitLogEntry(listWriter);
                m_asyncFactory.Add(listWriter, submitLogEntry);
            }

            return submitLogEntry;
        }
    }
}