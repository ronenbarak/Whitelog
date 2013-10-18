using Whitelog.Core.Filter;
using Whitelog.Interface;
using Whitelog.Barak.Common.SystemTime;

namespace Whitelog.Core
{
    public class LogTunnel : ILog
    {
        public delegate void LogEntrySubmitted(LogEntry entry);
        
        public event LogEntrySubmitted LogEntry;

        private readonly ISystemTime m_systemTime;
        private readonly ILogScopeSyncImplementation m_logScopeSyncImplementation;

        public LogTunnel(ISystemTime systemTime,ILogScopeSyncImplementation logScopeSyncImplementation)
        {
            m_logScopeSyncImplementation = logScopeSyncImplementation;
            m_systemTime = systemTime;
        }

        public void Log(ILogTitle title, object paramaeter)
        {
            Log(new LogEntry(title, paramaeter));
        }

        public void Log(LogEntry logEntry)
        {
            logEntry.Time = m_systemTime.Now();
            logEntry.LogScopeId = m_logScopeSyncImplementation.GetScopeId();

            var temp = LogEntry;
            if (temp != null)
            {
                temp.Invoke(logEntry);
            }              
        }

        public void LogWithNotTimeSet(LogEntry logEntry)
        {
            logEntry.LogScopeId = m_logScopeSyncImplementation.GetScopeId();
            
            var temp = LogEntry;
            if (temp != null)
            {
                temp.Invoke(logEntry);
            }
        }

        public ILogScope CreateScope(string title)
        {
            return new LogScope(this, title, m_logScopeSyncImplementation);
        }
    }
}