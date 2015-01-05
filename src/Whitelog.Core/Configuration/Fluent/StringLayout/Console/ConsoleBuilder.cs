using System;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.String;
using Whitelog.Core.Loggers.String.StringAppenders.Console;
using Whitelog.Core.Loggers.String.StringAppenders.Console.SubmitConsoleLogEntry;

namespace Whitelog.Core.Configuration.Fluent.StringLayout.Console
{
    class ConsoleBuilder : IStringAppenderBuilder, IConsoleBuilder, IConsoleColors, IFilterBuilder<IConsoleBuilder>
    {
        private IColorSchema m_colorSchema = new DefaultColorSchema();
        private ISubmitConsoleLogEntry m_submitLogEntry;
        private FilterBuilder<ConsoleBuilder> m_filterBuilder;
        
        public ConsoleBuilder(ExecutionMode executionMode)
        {
            m_filterBuilder = new FilterBuilder<ConsoleBuilder>(this);
            if (executionMode == ExecutionMode.Sync)
            {
                m_submitLogEntry = new SyncSubmitConsoleLogEntry();
            }
            else
            {
                m_submitLogEntry = new AsyncSubmitConsoleLogEntry();
            }
        }

        public IStringAppender Build()
        {
            var filter = m_filterBuilder.Build();
            return new ConsoleAppender(m_submitLogEntry, filter, m_colorSchema);
        }

        public IConsoleColors Colors { get { return this; } }

        public IFilterBuilder<IConsoleBuilder> Filter { get { return this; } }

        public IConsoleBuilder None { get { m_colorSchema = null; return this; } }

        public IConsoleBuilder Default { get { m_colorSchema = new DefaultColorSchema(); return this;} }
        public IConsoleBuilder Conditions(Func<IConsoleColorConditions, object> condition)
        {
            var consoleColorConditions = new ConsoleColorConditions();
            condition.Invoke(consoleColorConditions);
            m_colorSchema = consoleColorConditions.Build();
            return this;
        }

        public IConsoleBuilder Custom(IColorSchema colorSchema)
        {
            m_colorSchema = colorSchema;
            return this;
        }

        #region Filter
        
        public IConsoleBuilder Include(params LogTitles[] logTitles)
        {
            return m_filterBuilder.Include(logTitles);
        }

        public IConsoleBuilder Exclude(params LogTitles[] logTitles)
        {
            return m_filterBuilder.Exclude(logTitles);
        }

        public IConsoleBuilder Custom(IFilter filter)
        {
            return m_filterBuilder.Custom(filter);
        }

        #endregion
    }
}