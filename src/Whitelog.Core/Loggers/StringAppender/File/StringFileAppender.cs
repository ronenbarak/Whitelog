using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.Filter;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.File
{
    public interface IStringAppenderSubbmiter
    {
        void Submit(string text,DateTime timestamp);
    }

    public class SyncStringFileSubbmiter : IStringAppenderSubbmiter
    {
        private StringFileWriter m_fileWriter;
        private object m_lockObject = new object();
        public SyncStringFileSubbmiter(StringFileWriter fileWriter)
        {
            m_fileWriter = fileWriter;
        }

        public void Submit(string text, DateTime timestamp)
        {
            lock (m_lockObject)
            {
                m_fileWriter.WriteData(new TextEntry(timestamp,text));   
                m_fileWriter.Flush();
            }
        }
    }

    public class AsyncStringFile : IStringAppenderSubbmiter
    {
        private AsyncSubmitEntry<TextEntry, TextEntry> m_asyncSubmitEntry;

        class TextEntryAction : IAsyncActions<TextEntry, TextEntry>
        {
            private StringFileWriter m_fileWriter;

            public TextEntryAction(StringFileWriter fileWriter)
            {
                m_fileWriter = fileWriter;
            }

            public TextEntry Clone(TextEntry source)
            {
                return source;
            }

            public void HandleBulk(IEnumerable<TextEntry> enumerable)
            {
                m_fileWriter.WriteData(enumerable);
            }

            public void BulkEnded()
            {
                m_fileWriter.Flush();
            }
        }

        public AsyncStringFile(StringFileWriter fileWriter)
        {
            m_asyncSubmitEntry = new AsyncSubmitEntry<TextEntry, TextEntry>(new TextEntryAction(fileWriter));
        }

        public void Submit(string text, DateTime timestamp)
        {
            m_asyncSubmitEntry.AddEntry(new TextEntry(timestamp, text));
        }
    }

    public class StringFileAppender : IStringAppender
    {
        private readonly IFilter m_filter;
        private IStringAppenderSubbmiter m_submitEntry;

        public StringFileAppender(IStringAppenderSubbmiter submitEntry, IFilter filter = null)
        {
            m_submitEntry = submitEntry;
            m_filter = filter;
        }

        public bool Filter(LogEntry logEntry)
        {
            if (m_filter != null)
            {
                return m_filter.Filter(logEntry);
            }
            return false;
        }

        public void Append(string value, LogEntry logEntry)
        {
            m_submitEntry.Submit(value,logEntry.Time);
        }
    }
}
