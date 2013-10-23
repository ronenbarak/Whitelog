using System;
using Whitelog.Interface;

namespace Whitelog.Core.Loggers.StringAppender.Console
{
    /// <summary>
    /// This class is immutable and can be safly transfer between threads 
    /// </summary>
    public class ColorLine
    {
        public static readonly ColorLine Empty = new ColorLine(null, null);
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