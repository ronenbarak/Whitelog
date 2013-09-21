using System;

namespace Whitelog.Interface
{
    public class LogEntry
    {
        public LogEntry(ILogTitle title, object[] parameters)
        {
            if (parameters != null && parameters.Length != 0)
            {
                Paramaeters = parameters;
            }
            Title = title;
        }

        public ILogTitle Title;
        public object[] Paramaeters;

        public DateTime Time;
        public int LogScopeId;
    }
}