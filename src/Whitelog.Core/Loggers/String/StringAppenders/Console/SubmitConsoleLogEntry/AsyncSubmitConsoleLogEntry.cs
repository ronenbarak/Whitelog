using System.Collections.Generic;

namespace Whitelog.Core.Loggers.String.StringAppenders.Console.SubmitConsoleLogEntry
{
    public class AsyncSubmitConsoleLogEntry : ISubmitConsoleLogEntry
    {
        class ColorLineLog
        {
            public ColorLineLog(string text,ColorLine colorLine)
            {
                Text = text;
                ColorLine = colorLine;
            }

            public string Text;
            public ColorLine ColorLine;
        }
       
        class ConsoleAsync : IAsyncActions<ColorLineLog, ColorLineLog>
        {
            public ColorLineLog Clone(ColorLineLog source)
            {
                return source;
            }

            public void HandleBulk(IEnumerable<ColorLineLog> enumerable)
            {
                foreach (var colorLineLog in enumerable)
                {
                    ConsoleLogEntry.PrintTextLine(colorLineLog.Text, colorLineLog.ColorLine);
                }
            }

            public void BulkEnded()
            {
            }
        }

        private AsyncBulkExecution<ColorLineLog, ColorLineLog> m_asyncBulkExecution;

        public AsyncSubmitConsoleLogEntry()
        {
            m_asyncBulkExecution = new AsyncBulkExecution<ColorLineLog, ColorLineLog>(new ConsoleAsync());
        }

        public void WaitForIdle()
        {
            m_asyncBulkExecution.WaitForIdle();
        }

        public void AddLogEntry(string text, ColorLine colorLine)
        {
            m_asyncBulkExecution.AddEntry(new ColorLineLog(text, colorLine));
        }
    }
}