namespace Whitelog.Core.Loggers.String.StringAppenders.Console.SubmitConsoleLogEntry
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