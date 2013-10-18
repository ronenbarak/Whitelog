using System;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.Console
{
    public class ColorLine
    {
        public ColorLine(ConsoleColor? background, ConsoleColor? foreground)
        {
            Background = background;
            Foreground = foreground;
        }

        public ConsoleColor? Background { get; private set; }
        public ConsoleColor? Foreground { get; private set; }
    }

    public interface IColorSchema
    {
        ColorLine GetColor(LogEntry logEntry);
    }
}