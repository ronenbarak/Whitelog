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

        public static void LogDebug(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new DebugLogTitle(message), paramaeter));
        }

        public static void LogError(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new ErrorLogTitle(message), paramaeter));
        }

        public static void LogFatal(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new FatalLogTitle(message), paramaeter));
        }

        public static void LogWarning(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new WarningLogTitle(message), paramaeter));
        }

        public static void LogInfo(this ILog log, string message, object paramaeter = null)
        {
            log.Log(new LogEntry(new InfoLogTitle(message), paramaeter));
        }
    }
}