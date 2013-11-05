using System;
using System.Threading;
using Whitelog.Interface;

namespace Whitelog.Core
{
    public class LogScope : ILogScope
    {
        private static int m_logScopeIdCount = 0;
        private readonly int m_longScopeScopeId;
        private bool m_isDisposed = false;

        protected readonly LogTunnel m_Log;
        private LogScope m_parent;
        private ILogScopeSyncImplementation m_logScopeSyncImplementation;

        public LogScope(LogTunnel log, ILogScopeSyncImplementation logScopeSyncImplementation)
            : this(log, new OpenLogScopeTitle(string.Empty), logScopeSyncImplementation)
        {
            
        }

        public LogScope(LogTunnel log, string title, ILogScopeSyncImplementation logScopeSyncImplementation)
            : this(log, new OpenLogScopeTitle(title), logScopeSyncImplementation)
        {
            
        }

        protected LogScope(LogTunnel log, OpenLogScopeTitle title, ILogScopeSyncImplementation logScopeSyncImplementation)
        {
            m_Log = log;
            title.ParentLogId = logScopeSyncImplementation.GetScopeId();
            m_logScopeSyncImplementation = logScopeSyncImplementation;
            m_parent = logScopeSyncImplementation.LogScope;
            m_longScopeScopeId = Interlocked.Increment(ref m_logScopeIdCount);
            logScopeSyncImplementation.LogScope = this;

            m_Log.Log(title);
        }

        public int LogScopeId
        {
            get { return m_longScopeScopeId; }
        }

        protected virtual void OnDispose()
        {
            m_Log.Log(new CloseLogScopeTitle());
        }

        public virtual void Dispose()
        {
            if (!m_isDisposed)
            {
                m_isDisposed = true;

                OnDispose();

                m_logScopeSyncImplementation.LogScope = m_parent;
            }      
        }
    }
}