using System.Threading;
using Whitelog.Interface;

namespace Whitelog.Core.LogScopeSyncImplementation
{
    public class MultiLogPerApplicationScopeSync : ILogScopeSyncImplementation
    {
        private ThreadLocal<LogScope> m_logScopeData = new ThreadLocal<LogScope>();

        public void SetScope(LogScope logScope)
        {
            m_logScopeData.Value = logScope;
        }

        public int GetScopeId()
        {
            var logscope = m_logScopeData.Value;
            return logscope == null ? 0 : logscope.LogScopeId;
        }

        public LogScope LogScope
        {
            get { return m_logScopeData.Value; }
            set { m_logScopeData.Value = value; }
        }
    }
}