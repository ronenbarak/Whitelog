using System;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
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
}