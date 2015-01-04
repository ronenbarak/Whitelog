using System;
using System.Security.Cryptography.X509Certificates;
using Whitelog.Core.Configuration.Fluent.StringLayout.File;
using Whitelog.Core.Loggers;
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

    public class StringOutputer : IStringOutputer
    {
        private IStringLayoutBuilder m_layoutBuilder;
        private Action<IStringAppenderBuilder> m_addLogger;
        private ExecutionMode m_executionMode;

        public StringOutputer(IStringLayoutBuilder layoutBuilder,Action<IStringAppenderBuilder> addLogger,ExecutionMode executionMode)
        {
            m_executionMode = executionMode;
            m_addLogger = addLogger;
            m_layoutBuilder = layoutBuilder;
        }

        public ExecutionMode ExecutionMode { get { return m_executionMode; } }
        IStringLayoutBuilder IStringOutputer.Source { get { return m_layoutBuilder; } }

        public void AddLogger(IStringAppenderBuilder logger)
        {
            m_addLogger.Invoke(logger);
        }
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