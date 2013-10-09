using System;
using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutFactory
{
    public class ThreadIdStringLayoutFactory : IStringLayoutFactory
    {
        class ThreadIdStringLayout : IStringLayoutWriter
        {
            public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
            {
                stringBuilder.Append(System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }

        public bool CanHandle(string pattern)
        {
            return string.Equals(pattern, "ThreadId",StringComparison.OrdinalIgnoreCase);
        }

        public IStringLayoutWriter Create(string pattern)
        {
            return new ThreadIdStringLayout();
        }
    }
}