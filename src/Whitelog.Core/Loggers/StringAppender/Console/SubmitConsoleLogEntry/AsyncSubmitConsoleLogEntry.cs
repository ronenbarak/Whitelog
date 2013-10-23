using System.Threading;
using Whitelog.Barak.Common.DataStructures.BulkCollector;

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

        private QueuedBulkCollector<ColorLineLog> m_queuedBulk = new QueuedBulkCollector<ColorLineLog>();
        private int m_pendingBulksToCollect = 0;
        private int m_useCounter = 0;

        public void WaitForIdle()
        {
            SpinWait spinWait = new SpinWait();
            while (m_pendingBulksToCollect != 0 || m_useCounter != 0)
            {
                spinWait.SpinOnce();
            }
        }

        private void PrintQueue(object state)
        {
            Interlocked.Increment(ref m_useCounter); // This is thread safe without the Interlocked.Increment but its looks better this way

            bool bContinue = true;
            while (bContinue)
            {
                var logEntries = m_queuedBulk.GetBulk();
                foreach (var colorLineLog in logEntries)
                {
                    ConsoleLogEntry.PrintTextLine(colorLineLog.Text, colorLineLog.ColorLine);
                }
                bContinue = Interlocked.Decrement(ref m_pendingBulksToCollect) != 0;
            }

            Interlocked.Decrement(ref m_useCounter); // This line should be thread safe.
        }

        public void AddLogEntry(string text, ColorLine colorLine)
        {
            bool isFirstInQueue = false;
            m_queuedBulk.Add(new ColorLineLog(text, colorLine), out isFirstInQueue);
            if (isFirstInQueue)
            {
                int threadCount = Interlocked.Increment(ref m_pendingBulksToCollect);
                if (threadCount == 1)
                {
                    // the application might terminate befor the thread run and we will lose logs
                    // for that resean we have WaitForIdle to insure the log has written into console.
                    ThreadPool.QueueUserWorkItem(PrintQueue);
                }
            }
        }
    }
}