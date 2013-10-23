namespace Whitelog.Core.Loggers.StringAppender.Console.SubmitConsoleLogEntry
{
    public interface ISubmitConsoleLogEntry
    {
        void WaitForIdle();
        void AddLogEntry(string text,ColorLine colorLine);
    }
}