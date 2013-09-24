using Whitelog.Interface.LogTitles;

namespace Whitelog.Interface
{
    public static class LogExtenstions
    {
        public static void Log(this ILog log, ILogTitle title, params object[] paramaeters)
        {
            log.Log(new LogEntry(title,paramaeters));
        }

        public static void Log(this ILog log,string title, string message, params object[] paramaeters)
        {
            log.Log(new LogEntry(new CustomStringLogTitle(title, message), paramaeters));
        }

        public static void LogDebug(this ILog log, string message, params object[] paramaeters)
        {
            log.Log(new LogEntry(new DebugLogTitle(message), paramaeters));
        }

        public static void LogError(this ILog log, string message, params object[] paramaeters)
        {
            log.Log(new LogEntry(new ErrorLogTitle(message), paramaeters));
        }

        public static void LogFatal(this ILog log, string message, params object[] paramaeters)
        {
            log.Log(new LogEntry(new FatalLogTitle(message), paramaeters));
        }

        public static void LogWarning(this ILog log, string message, params object[] paramaeters)
        {
            log.Log(new LogEntry(new WarningLogTitle(message), paramaeters));
        }

        public static void LogInfo(this ILog log, string message, params object[] paramaeters)
        {
            log.Log(new LogEntry(new InfoLogTitle(message), paramaeters));
        }
    }
}