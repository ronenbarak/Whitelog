using System;

namespace Whitelog.Core.Loggers.StringAppender.Console.SubmitConsoleLogEntry
{
    public static class ConsoleLogEntry
    {
        public static void PrintTextLine(string text, ColorLine colorLine)
        {
            ConsoleColor oldBackground = System.Console.BackgroundColor;
            ConsoleColor oldForeground = System.Console.ForegroundColor;
            if (colorLine.Background.HasValue)
            {
                System.Console.BackgroundColor = colorLine.Background.Value;
            }

            if (colorLine.Foreground.HasValue)
            {
                System.Console.ForegroundColor = colorLine.Foreground.Value;
            }

            System.Console.WriteLine(text);

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