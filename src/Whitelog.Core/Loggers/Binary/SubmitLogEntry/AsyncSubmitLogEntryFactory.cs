using System.Collections.Generic;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Deserilizer;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Loggers.Binary.SubmitLogEntry
{
    public class AsyncSubmitLogEntryFactory : ISubmitLogEntryFactory
    {
        class LogAsyncActions : IAsyncActions<IRawData, CloneRawData>
        {
            private IListWriter m_listWriter;

            public LogAsyncActions(IListWriter listWriter)
            {
                m_listWriter = listWriter;
            }

            public CloneRawData Clone(IRawData source)
            {
                return new CloneRawData(source.Buffer,source.Length,source.DateTime);
            }

            public void HandleBulk(IEnumerable<CloneRawData> enumerable)
            {
                m_listWriter.WriteData(enumerable);
            }

            public void BulkEnded()
            {
                m_listWriter.Flush();
            }
        }

        Dictionary<IListWriter, ISubmitEntry<IRawData>> m_asyncFactory = new Dictionary<IListWriter, ISubmitEntry<IRawData>>();

        public ISubmitEntry<IRawData> CreateSubmitLogEntry(IListWriter listWriter)
        {
            ISubmitEntry<IRawData> asyncSubmitEntry;
            if (!m_asyncFactory.TryGetValue(listWriter,out asyncSubmitEntry))
            {
                asyncSubmitEntry = new AsyncBulkExecution<IRawData,CloneRawData>(new LogAsyncActions(listWriter));
                m_asyncFactory.Add(listWriter, asyncSubmitEntry);
            }

            return asyncSubmitEntry;
        }
    }
}