using System;
using System.Collections.Generic;
using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary
{
    public class CloneRawData : IRawData
    {
        private byte[] m_newBuffer;

        public CloneRawData(byte[] buffer)
            : this(buffer, buffer.Length)
        {
        }

        public CloneRawData(byte[] buffer, int legnth)
        {
            var newBuffer = new byte[legnth];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, legnth);
            m_newBuffer = newBuffer;
        }

        public CloneRawData(byte[] buffer, int legnth, DateTime dateTime)
        {
            DateTime = dateTime;
            var newBuffer = new byte[legnth];
            System.Buffer.BlockCopy(buffer, 0, newBuffer, 0, legnth);
            m_newBuffer = newBuffer;
        }

        public int Length
        {
            get { return m_newBuffer.Length; }
        }

        public byte[] Buffer
        {
            get { return m_newBuffer; }
        }

        public DateTime DateTime { get; set; }
    }

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
                asyncSubmitEntry = new AsyncSubmitEntry<IRawData,CloneRawData>(new LogAsyncActions(listWriter));
                m_asyncFactory.Add(listWriter, asyncSubmitEntry);
            }

            return asyncSubmitEntry;
        }
    }
}