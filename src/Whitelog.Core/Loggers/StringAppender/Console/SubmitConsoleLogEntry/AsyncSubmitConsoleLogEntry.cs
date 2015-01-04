using System.Collections.Generic;

namespace Whitelog.Core.Loggers.StringAppender.Console.SubmitConsoleLogEntry
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

        private AsyncSubmitEntry<ColorLineLog, ColorLineLog> m_asyncSubmitEntry;

        public AsyncSubmitConsoleLogEntry()
        {
            m_asyncSubmitEntry = new AsyncSubmitEntry<ColorLineLog, ColorLineLog>(new ConsoleAsync());
        }

        public void WaitForIdle()
        {
            m_asyncSubmitEntry.WaitForIdle();
        }

        public void AddLogEntry(string text, ColorLine colorLine)
        {
            m_asyncSubmitEntry.AddEntry(new ColorLineLog(text, colorLine));
        }
    }
}