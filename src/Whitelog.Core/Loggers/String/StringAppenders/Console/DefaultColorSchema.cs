using System;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.Loggers.String.StringAppenders.Console
{
    public class DefaultColorSchema : IColorSchema
    {
        public ColorLine Empty { get; set; }
        public ColorLine Error { get; set; }
        public ColorLine Warning { get; set; }
        public ColorLine Debug { get; set; }
        public ColorLine Fatal { get; set; }
        public ColorLine Info { get; set; }
        public ColorLine Trace { get; set; }

        public DefaultColorSchema()
        {
            Empty = new ColorLine(null, null);
            Error = new ColorLine(null, ConsoleColor.Red);
            Warning = new ColorLine(null, ConsoleColor.Yellow);
            Debug = new ColorLine(null, ConsoleColor.White);
            Info = new ColorLine(null, ConsoleColor.Cyan);
            Fatal = new ColorLine(ConsoleColor.Red, null);
            Trace = new ColorLine(null, ConsoleColor.Gray);
        }

        public ColorLine GetColor(LogEntry logEntry)
        {
            switch (logEntry.Title.Id)
            {
                case ReservedLogTitleIds.Fatal:
                    return Fatal;
                case ReservedLogTitleIds.Error:
                    return Error;
                case ReservedLogTitleIds.Warning:
                    return Warning;
                case ReservedLogTitleIds.Debug:
                    return Debug;
                case ReservedLogTitleIds.Info:
                    return Info;
                case ReservedLogTitleIds.Trace:
                    return Trace;
                default:
                    return Empty;
            }
        }
    }
}