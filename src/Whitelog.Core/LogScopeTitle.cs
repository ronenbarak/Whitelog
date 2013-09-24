using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core
{
    public class LogScopeTitle : CustomStringLogTitle
    {
        public LogScopeTitle(string title):base(title)
        {   
        }
    }

    public class OpenLogScopeTitle : LogScopeTitle
    {
        public int ParentLogId;

        public OpenLogScopeTitle(string title)
            : base(title)
        {
        }
    }

    public class CloseLogScopeTitle : ILogTitle
    {
    }
}