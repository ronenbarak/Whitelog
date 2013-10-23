using System;
using System.Collections;
using System.Runtime.InteropServices;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers.StringAppender.Console.SubmitConsoleLogEntry;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.Console
{
    public class ConsoleAppender : IStringAppender
    {
        private bool m_isConsoleOutputAvaliable;
        private readonly IFilter m_filter;
        private readonly IColorSchema m_colorSchema;
        private ISubmitConsoleLogEntry m_consoleLogEntrySubmitter;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
        const int STD_OUTPUT_HANDLE = -11;

        public ConsoleAppender(ISubmitConsoleLogEntry consoleLogEntrySubmitter)
            : this(consoleLogEntrySubmitter, false, null, null)
        {
        }

        public ConsoleAppender(ISubmitConsoleLogEntry consoleLogEntrySubmitter,bool forceConsoleOutput)
            : this(consoleLogEntrySubmitter, forceConsoleOutput, null)
        {
        }

        public ConsoleAppender(ISubmitConsoleLogEntry consoleLogEntrySubmitter, IColorSchema colorSchema)
            : this(consoleLogEntrySubmitter, false, null, colorSchema)
        {
        }

        public ConsoleAppender(ISubmitConsoleLogEntry consoleLogEntrySubmitter, IFilter filter)
            : this(consoleLogEntrySubmitter, false, filter, null)
        {
        }

        public ConsoleAppender(ISubmitConsoleLogEntry consoleLogEntrySubmitter,bool forceConsoleOutput,IColorSchema colorSchema)
            : this(consoleLogEntrySubmitter, forceConsoleOutput, null, colorSchema)
        {
        }

        public ConsoleAppender(ISubmitConsoleLogEntry consoleLogEntrySubmitter, IFilter filter, IColorSchema colorSchema)
            : this(consoleLogEntrySubmitter, false, filter, colorSchema)
        {
        }

        public ConsoleAppender(ISubmitConsoleLogEntry consoleLogEntrySubmitter, bool forceConsoleOutput, IFilter filter, IColorSchema colorSchema)
        {
            m_consoleLogEntrySubmitter = consoleLogEntrySubmitter;
            m_colorSchema = colorSchema;
            m_filter = filter;
            if (forceConsoleOutput)
            {
                m_isConsoleOutputAvaliable = true;
            }
            else
            {
                IntPtr iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                if (iStdOut == IntPtr.Zero)
                {
                    m_isConsoleOutputAvaliable = false;
                }
                else
                {
                    m_isConsoleOutputAvaliable = true;
                }   
            }
        }


        public bool Filter(LogEntry logEntry)
        {
            if (!m_isConsoleOutputAvaliable)
            {
                return true;
            }

            if (m_filter != null)
            {
                return m_filter.Filter(logEntry);
            }

            return false;
        }

        public void Append(string value,LogEntry logEntry)
        {
            if (m_colorSchema == null)
            {
                m_consoleLogEntrySubmitter.AddLogEntry(value,ColorLine.Empty);
            }
            else
            {

                var colorLine = m_colorSchema.GetColor(logEntry);
                m_consoleLogEntrySubmitter.AddLogEntry(value,colorLine);
            }   
        }
    }
}
