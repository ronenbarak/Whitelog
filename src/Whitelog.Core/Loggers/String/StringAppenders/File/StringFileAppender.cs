using Whitelog.Core.Filter;
using Whitelog.Core.Loggers.String.StringAppenders.File.Submitter;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.String.StringAppenders.File
{
    public class StringFileAppender : IStringAppender
    {
        private readonly IFilter m_filter;
        private IStringAppenderSubmitter m_submitEntry;

        public StringFileAppender(IStringAppenderSubmitter submitEntry, IFilter filter = null)
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
