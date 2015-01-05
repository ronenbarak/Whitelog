using System;
using System.Collections.Generic;

namespace Whitelog.Core.Loggers.String.StringAppenders.File.Submitter
{
    public class AsyncStringFile : IStringAppenderSubmitter
    {
        private AsyncBulkExecution<TextEntry, TextEntry> m_asyncBulkExecution;

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
            m_asyncBulkExecution = new AsyncBulkExecution<TextEntry, TextEntry>(new TextEntryAction(fileWriter));
        }

        public void Submit(string text, DateTime timestamp)
        {
            m_asyncBulkExecution.AddEntry(new TextEntry(timestamp, text));
        }
    }
}