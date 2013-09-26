using System;

namespace Whitelog.Interface
{
    public class LogEntry
    {
        public LogEntry(ILogTitle title, object parameter)
        {
            Paramaeter = parameter;
            Title = title;
        }

        public ILogTitle Title;
        public object Paramaeter;

        public DateTime Time;
        public int LogScopeId;
    }
}