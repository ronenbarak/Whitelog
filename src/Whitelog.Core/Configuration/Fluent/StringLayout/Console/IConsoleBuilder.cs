﻿using System;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers.StringAppender.Console;
using Whitelog.Interface;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    public interface IConsoleColorConditions
    {
        IConsoleColorConditions Condition(LogTitles logTitle, ConsoleColor foreground);
        IConsoleColorConditions Condition(LogTitles logTitle, ConsoleColor foreground,ConsoleColor background);
        IConsoleColorConditions Condition(Func<LogEntry, bool> condition, ConsoleColor foreground);
        IConsoleColorConditions Condition(Func<LogEntry,bool> condition, ConsoleColor foreground, ConsoleColor background);
    }

    public interface IConsoleColors
    {
        IConsoleBuilder None { get; }
        IConsoleBuilder Default { get; }
        IConsoleBuilder Conditions(Func<IConsoleColorConditions,object> condition);
        
        IConsoleBuilder Custom(IColorSchema colorSchema);
    }


    public interface IConsoleBuilder
    {
        IConsoleColors Colors { get; }
        IConsoleBuilder Sync { get; }
        IConsoleBuilder Async { get; }

        IFilterBuilder<IConsoleBuilder> Filter { get; }
    }
}