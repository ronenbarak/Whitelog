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
            get { return "ScopeTitle"; }
        }
    }

    public class OpenLogScopeTitle : LogScopeTitle
    {
        public int ParentLogId;

        public OpenLogScopeTitle(string message)
            : base(message)
        {
        }
    }

    public class CloseLogScopeTitle : ILogTitle
    {
    }
}