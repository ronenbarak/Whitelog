using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutWriters
{
    public class ConstStringLayoutWriter : IStringLayoutWriter
    {
        private readonly string m_stringPattern;

        public ConstStringLayoutWriter(string stringPattern)
        {
            m_stringPattern = stringPattern;
        }

        public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
        {
            stringBuilder.Append(m_stringPattern);
        }
    }
}