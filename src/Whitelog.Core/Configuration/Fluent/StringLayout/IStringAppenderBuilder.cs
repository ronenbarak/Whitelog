using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.String;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    public interface IStringAppenderBuilder
    {
        IStringAppender Build();
    }
}