using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Whitelog.Barak.Common.SystemTime;
using Whitelog.Barak.SystemDateTime;
using Whitelog.Core.Configuration.Fluent.StringLayout;
using Whitelog.Core.Loggers;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Interface;

namespace Whitelog.Core.Configuration.Fluent
{
    public interface IFile<T>
    {
        IFile<T> Name(string path);
        T Return { get; }
    }

    public enum Buffers
    {
        /// <summary>
        /// Ring buffer is async
        /// </summary>
        RingBuffer,  
        ThreadStatic,
        BufferPool,
        MemoryAllocation,  
    }
    public interface IBinaryBuilder
    {
        IBinaryBuilder Buffer(Buffers buffers);
        IBinaryBuilder Sync { get; }
        IBinaryBuilder Async { get; }

        IFile<IBinaryBuilder> File { get; } 
    }
    
    public enum SystemTime
    {
        DateTimeNow,
        DateTimeUtcNow,
    }

    public static class Whilelog
    {
        public static WhilelogFluentBuilder FluentConfiguration
        {
            get
            {
                return new WhilelogFluentBuilder();
            }
        }
    }

    interface ILoggerBuilder
    {
        ILogger Build();
    }

    public class WhilelogFluentBuilder
    {
        private ISystemTime m_systemTime = null;
        private List<ILoggerBuilder> m_loggers = new List<ILoggerBuilder>();

        internal WhilelogFluentBuilder()
        {
            
        }

        public WhilelogFluentBuilder Time(ISystemTime systemTime)
        {
            m_systemTime = systemTime;
            return this;
        }

        public WhilelogFluentBuilder Time(SystemTime systemTime)
        {
            switch (systemTime)
            {
                case SystemTime.DateTimeNow:
                    m_systemTime = new SystemDateTime();
                    break;
                case SystemTime.DateTimeUtcNow:
                    m_systemTime = new SystemUtcTime();
                    break;
            }
            return this;
        }


        public WhilelogFluentBuilder StringLayout(Func<IStringLayoutBuilder, object> stringLayout)
        {
            var stringLayoutBuilder = new StringLayoutBuilder();
            stringLayout.Invoke(stringLayoutBuilder);
            m_loggers.Add(stringLayoutBuilder);
            return this;
        }

        public WhilelogFluentBuilder Binary(Func<IBinaryBuilder, object> binary)
        {
            return this;
        }

        public ILog CreateLog()
        {
            ISystemTime systemTime = m_systemTime;
            if (systemTime == null)
            {
                systemTime = new SystemDateTime();
            }
            var logTunnel = new LogTunnel(systemTime, LogScopeSyncFactory.Create());

            foreach (var loggerBuilder in m_loggers)
            {
                var logger = loggerBuilder.Build();
                logger.AttachToTunnelLog(logTunnel);
            }

            return logTunnel;
        }
    }
}
