﻿using Whitelog.Interface.LogTitles;

namespace Whitelog.Interface
{
    public static class LogExtenstions
    {
        public static void Log(this ILog log, ILogTitle title, params object[] paramaeters)
        {
            log.Log(new LogEntry(title,paramaeters));
        }

        public static void Log(this ILog log, string title, params object[] paramaeters)
        {
            log.Log(new LogEntry(new CustomStringLogTitle(title), paramaeters));
        }

        public static void LogError(this ILog log, string title, params object[] paramaeters)
        {
            log.Log(new LogEntry(new ErrorLogTitle(title), paramaeters));
        }

        public static void LogFatal(this ILog log, string title, params object[] paramaeters)
        {
            log.Log(new LogEntry(new FatalLogTitle(title), paramaeters));
        }

        public static void LogWarning(this ILog log, string title, params object[] paramaeters)
        {
            log.Log(new LogEntry(new WarningLogTitle(title), paramaeters));
        }

        public static void LogInfo(this ILog log, string title, params object[] paramaeters)
        {
            log.Log(new LogEntry(new InfoLogTitle(title), paramaeters));
        }
    }
}