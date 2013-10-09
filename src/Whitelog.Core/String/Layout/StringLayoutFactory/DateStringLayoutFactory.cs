using System;
using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutFactory
{
    public class DateStringLayoutFactory : IStringLayoutFactory
    {
        class DateStringLayoutWriter : IStringLayoutWriter
        {
            public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
            {
                stringBuilder.Append(DateTime.Now);
            }
        }

        public bool CanHandle(string pattern)
        {
            return string.Equals(pattern, "longdate", StringComparison.OrdinalIgnoreCase);
        }

        public IStringLayoutWriter Create(string pattern)
        {
            return new DateStringLayoutWriter();
        }
    }
}