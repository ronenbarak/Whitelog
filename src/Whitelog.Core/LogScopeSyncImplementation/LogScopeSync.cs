using System;
using System.Linq;
using System.Reflection;
using Whitelog.Interface;

namespace Whitelog.Core.LogScopeSyncImplementation
{
    // This is a major hack and will be needed to be fixed some how....
    class LogScopeSync<T> : ILogScopeSyncImplementation
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