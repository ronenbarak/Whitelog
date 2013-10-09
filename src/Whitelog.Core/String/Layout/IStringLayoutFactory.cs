using System.Text;
using Whitelog.Interface;

namespace Whitelog.Core.String.Layout
{
    public interface IStringLayoutFactory
    {
        bool CanHandle(string pattern);
        IStringLayoutWriter Create(string pattern);
    }

    public interface IStringLayoutWriter
    {
        void Render(StringBuilder stringBuilder,IStringRenderer stringRenderer, LogEntry logEntry);
    }
}