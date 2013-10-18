using Whitelog.Core.Filter;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.File
{
    public class FileAppender : IStringAppender
    {
        private readonly IFilter m_filter;

        public FileAppender(FileConfiguration configuration):this(null,configuration)
        {
        }

        public FileAppender(IFilter filter, FileConfiguration configuration)
        {
            m_filter = filter;

            // Lets create the file.
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

        }
    }
}
