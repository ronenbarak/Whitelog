using System;

namespace Whitelog.Core.Loggers.String.StringAppenders.File.Submitter
{
    public class SyncStringFileSubmitter : IStringAppenderSubmitter
    {
        private StringFileWriter m_fileWriter;
        private object m_lockObject = new object();
        public SyncStringFileSubmitter(StringFileWriter fileWriter)
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
}