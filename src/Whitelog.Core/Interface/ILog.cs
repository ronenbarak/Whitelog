namespace Whitelog.Interface
{
    public interface ILog
    {
        ILogScope CreateScope(string title);
        void Log(LogEntry logEntry);
    }
}
