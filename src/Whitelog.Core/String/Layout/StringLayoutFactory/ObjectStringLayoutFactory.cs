using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutFactory
{
    public class ObjectStringLayoutFactory : IStringLayoutFactory
    {
        public class ObjectStringLayoutWriter : IStringLayoutWriter
        {
            public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
            {
                stringRenderer.Render(logEntry.Paramaeter, stringBuilder);
            }
        }

        public bool CanHandle(string pattern)
        {
            return pattern == "*";
        }

        public IStringLayoutWriter Create(string pattern)
        {
            return new ObjectStringLayoutWriter();
        }
    }
}