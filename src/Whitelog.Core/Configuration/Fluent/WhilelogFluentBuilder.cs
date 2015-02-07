using System.Collections.Generic;
using Whitelog.Barak.Common.SystemTime;
using Whitelog.Barak.SystemDateTime;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Interface;

namespace Whitelog.Core.Configuration.Fluent
{
    public interface IWhilelogFluentBuilder
    {
        ILogTimerSetter Time { get; }
        WhiteLogLogger Logger { get; }
        ILog CreateLog();
    }

    
    class WhilelogFluentBuilder : IWhilelogFluentBuilder, ILogTimerSetter
    {
        private ISystemTime m_systemTime = new SystemDateTime();
        private readonly List<ILoggerBuilder> m_loggers = new List<ILoggerBuilder>();
        private readonly WhiteLogLogger m_logger;

        public WhilelogFluentBuilder()
        {
            m_logger = new WhiteLogLogger(this,builder => m_loggers.Add(builder));
        }

        public ILogTimerSetter Time { get { return this; } }

        public WhiteLogLogger Logger { get { return m_logger; } }

        public ILog CreateLog()
        {
            var logTunnel = new LogTunnel(m_systemTime, LogScopeSyncFactory.Create());

            foreach (var loggerBuilder in m_loggers)
            {                
                var logger = loggerBuilder.Build();
                logger.AttachToTunnelLog(logTunnel);
            }

            return logTunnel;
        }

        public IWhilelogFluentBuilder Custom(ISystemTime systemTime)
        {
            m_systemTime = systemTime;
            return this;
        }

        public IWhilelogFluentBuilder DateTimeNow { get { m_systemTime = new SystemDateTime(); return this; } }
        public IWhilelogFluentBuilder DateTimeUtcNow { get { m_systemTime = new SystemUtcTime(); return this; } }
    }
}