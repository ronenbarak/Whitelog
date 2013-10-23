using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.StringAppender.Console;
using Whitelog.Interface;

namespace Whitelog.Sample
{
    public class ProxyConsoleAppander : IStringAppender
    {
        private RichTextBox m_richTextBox;
        private Dictionary<ConsoleColor, Color> m_colors;
        private IColorSchema m_colorSchema;
        private readonly IFilter m_filter;

        public ProxyConsoleAppander(RichTextBox richTextBox, IColorSchema colorSchema,IFilter filter = null)
        {
            m_colorSchema = colorSchema;
            m_filter = filter;
            m_richTextBox = richTextBox;

            m_colors = new Dictionary<ConsoleColor, Color>()
                       {
                           {ConsoleColor.Black, Color.Black},
                           {ConsoleColor.DarkBlue, Color.DarkBlue},
                           {ConsoleColor.DarkGreen, Color.DarkGreen},
                           {ConsoleColor.DarkCyan, Color.DarkCyan},
                           {ConsoleColor.DarkRed, Color.DarkRed},
                           {ConsoleColor.DarkMagenta, Color.DarkMagenta},
                           {ConsoleColor.Gray, Color.Gray},
                           {ConsoleColor.DarkGray, Color.DarkGray},
                           {ConsoleColor.Blue, Color.Blue},
                           {ConsoleColor.Green, Color.Green},
                           {ConsoleColor.Cyan, Color.Cyan},
                           {ConsoleColor.Red, Color.Red},
                           {ConsoleColor.Magenta, Color.Magenta},
                           {ConsoleColor.Yellow, Color.Yellow},
                           {ConsoleColor.DarkYellow, Color.Orange},
                           {ConsoleColor.White, Color.White},
                       };
        }

        public bool Filter(LogEntry logEntry)
        {
            if (m_filter != null)
            {
                return m_filter.Filter(logEntry);
            }

            return false;
        }

        public void Append(string value, LogEntry logEntry)
        {
            if (m_colorSchema == null)
            {
                m_richTextBox.Invoke((Action)(() =>
                                     {
                                         m_richTextBox.AppendText(value);
                                         m_richTextBox.AppendText(Environment.NewLine);
                                     }));
            }
            else
            {
                m_richTextBox.Invoke((Action)(() =>
                {
                    var colorLine = m_colorSchema.GetColor(logEntry);
                    var oldBackcolor = m_richTextBox.SelectionBackColor;
                    var oldColor = m_richTextBox.SelectionColor;

                    if (colorLine.Background.HasValue)
                    {
                        m_richTextBox.SelectionBackColor = m_colors[colorLine.Background.Value];
                    }

                    if (colorLine.Foreground.HasValue)
                    {
                        m_richTextBox.SelectionColor = m_colors[colorLine.Foreground.Value];
                    }

                    m_richTextBox.AppendText(value);
                    m_richTextBox.AppendText(Environment.NewLine);

                    m_richTextBox.SelectionBackColor = oldBackcolor;
                    m_richTextBox.SelectionColor = oldColor;
                }));
            }
        }
    }
}