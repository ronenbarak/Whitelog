using Whitelog.Core.Loggers;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    interface IStringAppenderBuilder
    {
        IStringAppender Build();
    }
}