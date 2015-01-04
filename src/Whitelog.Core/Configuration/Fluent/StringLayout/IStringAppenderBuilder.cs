using Whitelog.Core.Loggers;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    public interface IStringAppenderBuilder
    {
        IStringAppender Build();
    }
}