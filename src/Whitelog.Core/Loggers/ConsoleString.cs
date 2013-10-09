using System;
using System.Text;
using Whitelog.Core.String;

namespace Whitelog.Core.Loggers
{
    public class ConsoleLogger : IStringLogger
    {
        private StringLayoutLogger m_stringLayoutLogger;

        public ConsoleLogger()
        {
            m_stringLayoutLogger = new StringLayoutLogger();
        }

        public void AttachToTunnelLog(LogTunnel logTunnel)
        {
            logTunnel.LogEntry += logTunnel_LogEntry;
        }

        public void DetachTunnelLog(LogTunnel logTunnel)
        {
            logTunnel.LogEntry -= logTunnel_LogEntry;
        }

        void logTunnel_LogEntry(Interface.LogEntry entry)
        {
            StringBuilder stringBuilder = new StringBuilder();
            m_stringLayoutLogger.Render(entry, stringBuilder);
            Console.WriteLine(stringBuilder.ToString());
        }

        public void RegisterDefinition(IStringPackageDefinition packageDefinition)
        {
            m_stringLayoutLogger.RegisterDefinition(packageDefinition);
        }
    }
}
