using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core
{
    public class OpenLogScopeTitle : IMessageLogTitle
    {
        public int ParentLogId;
        private string m_message;
        public string Message { get { return m_message; } }

        public OpenLogScopeTitle(string message)
        {
            m_message = message;
        }

        public long Id { get { return ReservedLogTitleIds.Open; }}
    }

    public class CloseLogScopeTitle : ILogTitle
    {
        public long Id { get { return ReservedLogTitleIds.Close; } }
    }
}