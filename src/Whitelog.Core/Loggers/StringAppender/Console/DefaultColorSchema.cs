using System;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.Loggers.StringAppender.Console
{
    public class DefaultColorSchema : IColorSchema
    {
        private readonly ColorLine m_empty;
        private readonly ColorLine m_error;
        private readonly ColorLine m_warning;
        private readonly ColorLine m_debug;
        private readonly ColorLine m_fatal;
        private readonly ColorLine m_info;

        public DefaultColorSchema()
        {
            m_empty = new ColorLine(null, null);
            m_error = new ColorLine(null, ConsoleColor.Red);
            m_warning = new ColorLine(null, ConsoleColor.Yellow);
            m_debug = new ColorLine(null, ConsoleColor.Gray);
            m_info = new ColorLine(null, ConsoleColor.Cyan);
            m_fatal = new ColorLine(ConsoleColor.Red, null);
        }

        public ColorLine GetColor(LogEntry logEntry)
        {
            switch (logEntry.Title.Id)
            {
                case ReservedLogTitleIds.Fatal:
                    return m_fatal;
                case ReservedLogTitleIds.Error:
                    return m_error;
                case ReservedLogTitleIds.Warning:
                    return m_warning;
                case ReservedLogTitleIds.Debug:
                    return m_debug;
                case ReservedLogTitleIds.Info:
                    return m_info;
                default:
                    return m_empty;
            }
        }
    }
}