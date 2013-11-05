using System;
using System.Text;
using Whitelog.Core.String.Layout.StringLayoutWriters;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutFactory
{
    public class ScopeIdStringLayoutFactory : IStringLayoutFactory
    {
        class LogScopeIdStringLayoutWriter : IStringLayoutWriter
        {
            public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
            {
                stringBuilder.Append(logEntry.LogScopeId);
            }
        }

        public bool CanHandle(string pattern)
        {
            return string.Equals(pattern, "ScopeId", StringComparison.OrdinalIgnoreCase);
        }

        public IStringLayoutWriter Create(string pattern)
        {
            return new LogScopeIdStringLayoutWriter();
        }
    }
}