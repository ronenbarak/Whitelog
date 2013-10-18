using System;
using System.Collections;
using System.Runtime.InteropServices;
using Whitelog.Core.Filter;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.Console
{
    public class ConsoleAppender : IStringAppender
    {
        private bool m_isConsoleOutputAvaliable;
        private readonly IFilter m_filter;
        private readonly IColorSchema m_colorSchema;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);
        const int STD_OUTPUT_HANDLE = -11;

        public ConsoleAppender()
            : this(false,null,null)
        {
        }

        public ConsoleAppender(bool forceConsoleOutput)
            : this(forceConsoleOutput, null)
        {
        }

        public ConsoleAppender(IFilter filter):this(false,filter,null)
        {
        }

        public ConsoleAppender(bool forceConsoleOutput,IColorSchema colorSchema)
            : this(forceConsoleOutput, null,colorSchema)
        {
        }

        public ConsoleAppender(IFilter filter, IColorSchema colorSchema)
            : this(false, filter, colorSchema)
        {
        }

        public ConsoleAppender(bool forceConsoleOutput, IFilter filter, IColorSchema colorSchema)
        {
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
                System.Console.WriteLine(value);
            }
            else
            {
                ConsoleColor oldBackground = System.Console.BackgroundColor;
                ConsoleColor oldForeground = System.Console.ForegroundColor;

                var colorLine = m_colorSchema.GetColor(logEntry);
                if (colorLine.Background.HasValue)
                {
                    System.Console.BackgroundColor = colorLine.Background.Value;
                }

                if (colorLine.Foreground.HasValue)
                {
                    System.Console.ForegroundColor = colorLine.Foreground.Value;
                }

                System.Console.WriteLine(value);
                
                if (colorLine.Background.HasValue)
                {
                    System.Console.BackgroundColor = oldBackground;
                }

                if (colorLine.Foreground.HasValue)
                {
                    System.Console.ForegroundColor = oldForeground;
                }
            }
            
        }
    }
}
