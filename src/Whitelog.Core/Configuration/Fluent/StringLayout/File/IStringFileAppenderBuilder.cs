using System;

namespace Whitelog.Core.Configuration.Fluent.StringLayout.File
{
    public interface IStringFileAppenderBuilder
    {
        IStringFileAppenderBuilder Config(Func<IFileConfigurationBuilder, object> file);
        IFilterBuilder<IStringFileAppenderBuilder> Filter { get; }

        IStringFileAppenderBuilder ExecutionMode(ExecutionMode executionMode);
        IStringFileAppenderBuilder Buffer(Buffers buffers);
    }
}