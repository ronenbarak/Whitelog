using System.Collections.Generic;
using Whitelog.Core.ListWriter;

namespace Whitelog.Core.FileLog.SubmitLogEntry
{
    public class AsyncSubmitLogEntryFactory : ISubmitLogEntryFactory
    {
        Dictionary<IListWriter, ISubmitLogEntry> m_asyncFactory = new Dictionary<IListWriter, ISubmitLogEntry>();

        public ISubmitLogEntry CreateSubmitLogEntry(IListWriter listWriter)
        {
            ISubmitLogEntry asyncSubmitLogEntry;
            if (!m_asyncFactory.TryGetValue(listWriter,out asyncSubmitLogEntry))
            {
                asyncSubmitLogEntry = new AsyncSubmitLogEntry(listWriter);
                m_asyncFactory.Add(listWriter, asyncSubmitLogEntry);
            }

            return asyncSubmitLogEntry;
        }
    }
}