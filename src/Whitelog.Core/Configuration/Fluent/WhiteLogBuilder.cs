using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Whitelog.Barak.Common.SystemTime;
using Whitelog.Core.Configuration.Fluent.Binary;
using Whitelog.Core.Configuration.Fluent.StringLayout;
using Whitelog.Core.Loggers;

namespace Whitelog.Core.Configuration.Fluent
{
    public enum ExecutionMode
    {
        Async,
        Sync,
    }

    public static class Whilelog
    {
        public static IWhilelogFluentBuilder Configure
        {
            get
            {
                return new WhilelogFluentBuilder();
            }
        }
    }

    public interface ILoggerBuilder
    {
        ILogger Build();
    }

    public interface ILogTimerSetter
    {
        IWhilelogFluentBuilder Custom(ISystemTime systemTime);
        IWhilelogFluentBuilder DateTimeNow { get; }
        IWhilelogFluentBuilder DateTimeUtcNow { get; }
    }

    public interface IWhiteLogLogger
    {
        void AddLogger(ILoggerBuilder logger);
        IWhilelogFluentBuilder Source { get; }
    }
    
    public class WhiteLogLogger : IWhiteLogLogger
    {
        private IWhilelogFluentBuilder m_builder;
        private Action<ILoggerBuilder> m_addLogger;

        public WhiteLogLogger(IWhilelogFluentBuilder builder,Action<ILoggerBuilder> addLogger)
        {
            m_addLogger = addLogger;
            m_builder = builder;
        }

        void IWhiteLogLogger.AddLogger(ILoggerBuilder logger)
        {
            m_addLogger.Invoke(logger);
        }

        IWhilelogFluentBuilder IWhiteLogLogger.Source { get { return m_builder; } }
    }
}
