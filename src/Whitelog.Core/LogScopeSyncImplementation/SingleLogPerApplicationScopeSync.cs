using System;
using Whitelog.Interface;

namespace Whitelog.Core.LogScopeSyncImplementation
{
    public class SingleLogPerApplicationScopeSync : ILogScopeSyncImplementation
    {
        [ThreadStatic] 
        private static LogScope S_LogScope;

        public LogScope LogScope
        {
            get { return S_LogScope; }
            set { S_LogScope = value; }
        }

        public int GetScopeId()
        {
            return LogScope == null ? 0 : LogScope.LogScopeId;
        }
    }
}