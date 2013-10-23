namespace Whitelog.Core.Loggers.StringAppender.Console.SubmitConsoleLogEntry
{
    public class SyncSubmitConsoleLogEntry : ISubmitConsoleLogEntry
    {
        public void WaitForIdle()
        {
        }

        public void AddLogEntry(string text, ColorLine colorLine)
        {
            ConsoleLogEntry.PrintTextLine(text, colorLine);
        }
    }
}