using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core
{
    public class LogScopeTitle : StringLogTitle
    {
        public LogScopeTitle(string message):base(message)
        {   
        }

        public override string Title
        {
            get { return "OpenScope"; }
        }
    }

    public class OpenLogScopeTitle : IMessageLogTitle
    {
        public int ParentLogId;
        private string m_message;
        public string Message { get { return m_message; } }

        public OpenLogScopeTitle(string message)
        {
            m_message = message;
        }
    }

    public class CloseLogScopeTitle : ILogTitle
    {
    }
}