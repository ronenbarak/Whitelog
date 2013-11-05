using System;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.StringAppender.Console;
using Whitelog.Core.Loggers.StringAppender.Console.SubmitConsoleLogEntry;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    class ConsoleBuilder : IStringAppenderBuilder, IConsoleBuilder, IConsoleColors
    {
        private IColorSchema m_colorSchema = new DefaultColorSchema();
        private ISubmitConsoleLogEntry m_submitLogEntry;
        public IStringAppender Build()
        {
            ISubmitConsoleLogEntry submit = m_submitLogEntry;
            if (submit == null)
            {
                submit = new SyncSubmitConsoleLogEntry();
            }
            return new ConsoleAppender(submit, m_colorSchema);
        }

        public IConsoleColors Colors { get { return this; } }

        public IConsoleBuilder Sync
        {
            get
            {
                m_submitLogEntry = new SyncSubmitConsoleLogEntry();
                return this;
            }
        }

        public IConsoleBuilder Async
        {
            get
            {
                m_submitLogEntry = new AsyncSubmitConsoleLogEntry();
                return this;
            }
        }

        public IFilterBuilder<IConsoleBuilder> Filter { get; private set; }

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
    }
}