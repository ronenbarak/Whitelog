using System;
using Whitelog.Core.PackageDefinitions;
using Whitelog.Core.String;
using Whitelog.Core.String.Layout;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    public interface ILayoutExtensions<T>
    {
        T All { get; }
        
        T Astrix { get; }
        T Title { get; }
        T LongDate { get; }
        T ThreadId { get; }

        T Custom(IStringLayoutFactory layoutFactory);
    }

    public interface IStringOutputTo
    {
        StringOutputer Async { get; }
        StringOutputer Sync { get; }
    }

    public interface IStringOutputer
    {
        ExecutionMode ExecutionMode { get; }
        IStringLayoutBuilder Source { get; }
        void AddLogger(IStringAppenderBuilder consoleBuilder);
    }

    public interface IStringLayoutBuilder
    {
        IFilterBuilder<IStringLayoutBuilder> Filter { get; }
        IStringLayoutBuilder SetLayout(string layout);
        ILayoutExtensions<IStringLayoutBuilder> Extensions { get; }
        IStringLayoutBuilder Map<T>(Func<PackageDefinition<T>, object> define);
        IStringLayoutBuilder Map(IStringPackageDefinition definition);

        IStringOutputTo OutputTo { get; }
    }
}