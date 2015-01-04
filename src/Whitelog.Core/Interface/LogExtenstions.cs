using Whitelog.Interface.LogTitles;

namespace Whitelog.Interface
{
    public static class LogExtenstions
    {
        public static void Log(this ILog log, ILogTitle title, object paramaeter = null)
        {
            log.Log(new LogEntry(title,paramaeter));
        }

        public static void Log(this ILog log, string title, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new CustomStringLogTitle(title, message), paramaeter));
        }

        public static void Debug(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new DebugLogTitle(message), paramaeter));
        }

        public static void Error(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new ErrorLogTitle(message), paramaeter));
        }

        public static void Fatal(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new FatalLogTitle(message), paramaeter));
        }

        public static void Warning(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new WarningLogTitle(message), paramaeter));
        }

        public static void Info(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new InfoLogTitle(message), paramaeter));
        }
    }
}