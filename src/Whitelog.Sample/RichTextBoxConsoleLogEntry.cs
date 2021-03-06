﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers;
using Whitelog.Core.Loggers.String.StringAppenders.Console;
using Whitelog.Core.Loggers.String.StringAppenders.Console.SubmitConsoleLogEntry;
using Whitelog.Interface;

namespace Whitelog.Sample
{
    public class RichTextBoxConsoleLogEntry : ISubmitConsoleLogEntry
    {
        private RichTextBox m_richTextBox;
        private Dictionary<ConsoleColor, Color> m_colors;

        public RichTextBoxConsoleLogEntry(RichTextBox richTextBox)
        {
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

        public void WaitForIdle()
        {
        }

        public void AddLogEntry(string text, ColorLine colorLine)
        {
            m_richTextBox.Invoke((Action)(() =>
                                          {
                                              var lastPosition = m_richTextBox.TextLength;
                                              m_richTextBox.AppendText(text);
                                              m_richTextBox.AppendText(Environment.NewLine);
                                              m_richTextBox.SelectionStart = lastPosition;
                                              m_richTextBox.SelectionLength = m_richTextBox.TextLength - lastPosition;
                                              if (colorLine.Background.HasValue)
                                              {
                                                  m_richTextBox.SelectionBackColor = m_colors[colorLine.Background.Value];
                                              }

                                              if (colorLine.Foreground.HasValue)
                                              {
                                                  m_richTextBox.SelectionColor = m_colors[colorLine.Foreground.Value];
                                              }
                                              m_richTextBox.SelectionStart = m_richTextBox.TextLength;
                                              m_richTextBox.SelectionLength = 0;
                                          }));
        }
    }
}