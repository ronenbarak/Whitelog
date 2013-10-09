using System;
using System.Text;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.String.Layout.StringLayoutFactory
{
    public class TitleStringLayoutFactory : IStringLayoutFactory
    {
        class TitleStringLayoutWriter : IStringLayoutWriter
        {
            public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
            {
                var stringLogTitle = logEntry.Title as StringLogTitle;
                if (stringLogTitle != null)
                {
                    stringBuilder.Append(stringLogTitle.Title);
                }
                else
                {
                    var openLogScope = logEntry.Title as OpenLogScopeTitle;
                    if (openLogScope != null)
                    {
                        stringBuilder.Append("Open[");
                        stringBuilder.Append(logEntry.LogScopeId);
                        stringBuilder.Append("]");
                    }
                    else
                    {
                        var closeLogScope = logEntry.Title as CloseLogScopeTitle;
                        if (closeLogScope != null)
                        {
                            stringBuilder.Append("Close[");
                            stringBuilder.Append(logEntry.LogScopeId);
                            stringBuilder.Append("]");
                        }
                    }
                }

            }
        }

        public bool CanHandle(string pattern)
        {
            return string.Equals(pattern, "Title", StringComparison.OrdinalIgnoreCase);
        }

        public IStringLayoutWriter Create(string pattern)
        {
            return new TitleStringLayoutWriter();
        }
    }
}