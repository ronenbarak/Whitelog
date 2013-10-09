using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout.StringLayoutWriters
{
    public class NullStringLayoutWriter : IStringLayoutWriter
    {
        public void Render(StringBuilder stringBuilder, IStringRenderer stringRenderer, LogEntry logEntry)
        {
        }
    }
}