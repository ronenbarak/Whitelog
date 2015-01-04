using System;
using System.Collections.Generic;
using Whitelog.Core.Loggers.StringAppender.Console;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.Configuration.Fluent.StringLayout.Console
{
    class ConsoleColorConditions : IConsoleColorConditions
    {
        private ConditionColorSchema m_conditionColorSchema = new ConditionColorSchema();
        private static readonly Dictionary<LogTitles, Func<LogEntry, bool>> m_titleToFunction = new Dictionary<LogTitles, Func<LogEntry, bool>>()
                                                                         {
                                                                             {LogTitles.Close, entry => entry.Title.Id == ReservedLogTitleIds.Close},
                                                                             {LogTitles.Debug, entry => entry.Title.Id == ReservedLogTitleIds.Debug},
                                                                             {LogTitles.Error, entry => entry.Title.Id == ReservedLogTitleIds.Error},
                                                                             {LogTitles.Fatal, entry => entry.Title.Id == ReservedLogTitleIds.Fatal},
                                                                             {LogTitles.Info, entry => entry.Title.Id == ReservedLogTitleIds.Info},
                                                                             {LogTitles.Open, entry => entry.Title.Id == ReservedLogTitleIds.Open},
                                                                             {LogTitles.Trace, entry => entry.Title.Id == ReservedLogTitleIds.Trace},
                                                                             {LogTitles.Warning, entry => entry.Title.Id == ReservedLogTitleIds.Warning},
                                                                         };
        public IColorSchema Build()
        {
            return m_conditionColorSchema;
        }
        public IConsoleColorConditions Condition(LogTitles logTitle, ConsoleColor foreground)
        {
            m_conditionColorSchema.AddCondition(m_titleToFunction[logTitle],new ColorLine(null,foreground));
            return this;
        }

        public IConsoleColorConditions Condition(LogTitles logTitle, ConsoleColor foreground, ConsoleColor background)
        {
            m_conditionColorSchema.AddCondition(m_titleToFunction[logTitle], new ColorLine(background,foreground));
            return this;
        }

        public IConsoleColorConditions Condition(Func<LogEntry, bool> condition, ConsoleColor foreground)
        {
            m_conditionColorSchema.AddCondition(condition, new ColorLine(null, foreground));
            return this;
        }

        public IConsoleColorConditions Condition(Func<LogEntry, bool> condition, ConsoleColor foreground, ConsoleColor background)
        {
            m_conditionColorSchema.AddCondition(condition, new ColorLine(background,foreground));
            return this;
        }
    }
}