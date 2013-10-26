using Whitelog.Core.Binary.FileLog.SubmitLogEntry;
using Whitelog.Core.Filter;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.File
{
    public class FileAppender : IStringAppender
    {
        private readonly IFilter m_filter;
        private FileConfiguration m_configuration;

        private string m_currentFilePath { get; set; }

        public FileAppender(FileConfiguration configuration):this(null,configuration)
        {
        }

        public FileAppender(IFilter filter, FileConfiguration configuration)
        {
            m_filter = filter;
            FileConfiguration m_configuration = Clone(configuration);

        }

        private FileConfiguration Clone(FileConfiguration configuration)
        {
            return new FileConfiguration()
                   {
                       AppendToEnd = configuration.AppendToEnd,
                       ArchiveAboveSize = configuration.ArchiveAboveSize,
                       ArchiveEvery = configuration.ArchiveEvery,
                       ArchiveFilePath = configuration.ArchiveFilePath,
                       ArchiveNumbering = configuration.ArchiveNumbering,
                       FilePath = configuration.FilePath,
                       MaxArchiveFiles = configuration.MaxArchiveFiles,
                       ReplaceFileContentsOnEachWrite = configuration.ReplaceFileContentsOnEachWrite,
                   };
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
