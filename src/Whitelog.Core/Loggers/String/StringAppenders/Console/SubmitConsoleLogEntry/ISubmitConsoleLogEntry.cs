namespace Whitelog.Core.Loggers.String.StringAppenders.Console.SubmitConsoleLogEntry
{
    public interface ISubmitConsoleLogEntry
    {
        void WaitForIdle();
        void AddLogEntry(string text,ColorLine colorLine);
    }
}