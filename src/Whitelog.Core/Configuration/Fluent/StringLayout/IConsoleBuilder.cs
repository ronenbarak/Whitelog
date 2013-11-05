using System;
using Whitelog.Core.Filter;
using Whitelog.Core.Loggers.StringAppender.Console;
using Whitelog.Interface;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    public interface IColorSelection
    {
        IConditionColor None { get; }

        IConditionColor Black       { get; }
        IConditionColor DarkBlue    { get; }
        IConditionColor DarkGreen   { get; }
        IConditionColor DarkCyan    { get; }
        IConditionColor DarkRed     { get; }
        IConditionColor DarkMagenta { get; }
        IConditionColor DarkYellow  { get; }
        IConditionColor Gray        { get; }
        IConditionColor DarkGray    { get; }
        IConditionColor Blue        { get; }
        IConditionColor Green       { get; }
        IConditionColor Cyan        { get; }
        IConditionColor Red         { get; }
        IConditionColor Magenta     { get; }
        IConditionColor Yellow      { get; }
        IConditionColor White       { get; }
    }

    public interface IConditionColor
    {
        IColorSelection Foreground { get; }
        IColorSelection Background { get; }

        IConsoleColorConditions Return { get; }
    }
    public enum LogTitles
    {
        Fatal,
        Error,   
        Warning, 
        Info,    
        Debug,   
        Trace,   
        Open,
        Close,
    }

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

    public interface IFilterBuilder<T>
    {
        T SetCustumFilter(IFilter filter);
        T Return { get; }
    }

    public interface IConsoleBuilder
    {
        IConsoleColors Colors { get; }
        IConsoleBuilder Sync { get; }
        IConsoleBuilder Async { get; }
        
        IFilterBuilder<IConsoleBuilder> Filter { get; }
    }
}