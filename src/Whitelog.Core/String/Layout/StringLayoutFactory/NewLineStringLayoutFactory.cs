using System;
using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutFactory
{
    public class NewLineStringLayoutFactory : IStringLayoutFactory
    {
        class NewLineStringLayoutWriter : IStringLayoutWriter
        {
            public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
            {
                stringBuilder.AppendLine();
            }
        }

        public bool CanHandle(string pattern)
        {
            return string.Equals(pattern, "newline", StringComparison.OrdinalIgnoreCase);
        }

        public IStringLayoutWriter Create(string pattern)
        {
            return new NewLineStringLayoutWriter();
        }
    }
}