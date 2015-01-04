using System;
using System.IO;
using Whitelog.Core.Configuration.Fluent.Binary;
using Whitelog.Core.Configuration.Fluent.StringLayout;
using Whitelog.Core.Configuration.Fluent.StringLayout.Console;
using Whitelog.Core.Configuration.Fluent.StringLayout.File;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;

namespace Whitelog.Core.Configuration.Fluent
{
    public static class BinaryLoggerExtension
    {
        public static IWhilelogFluentBuilder BinaryFile(this WhiteLogLogger logLogger, Func<IBinaryBuilder, object> s)
        {
            BinaryBuilder binaryBuilder = new BinaryBuilder();
            s.Invoke(binaryBuilder);
            ((IWhiteLogLogger)logLogger).AddLogger(binaryBuilder);

            return ((IWhiteLogLogger)logLogger).Source;
        }
    }

    public static class JsonLoggerExtension
    {
        public static IWhilelogFluentBuilder Json(this WhiteLogLogger logLogger)
        {
            return ((IWhiteLogLogger)logLogger).Source;
        }
    }

    public static class StringLoggerExtension
    {
        public static IWhilelogFluentBuilder String(this WhiteLogLogger logLogger, Func<IStringLayoutBuilder, object> s)
        {
            StringLayoutBuilder stringLayoutBuilder = new StringLayoutBuilder();
            s.Invoke(stringLayoutBuilder);
            ((IWhiteLogLogger)logLogger).AddLogger(stringLayoutBuilder);

            return ((IWhiteLogLogger) logLogger).Source;
        }

        public static IStringLayoutBuilder ColorConsole(this StringOutputer stringOutputer, Func<IConsoleBuilder, object> s)
        {
            ConsoleBuilder consoleBuilder = new ConsoleBuilder(((IStringOutputer)stringOutputer).ExecutionMode);
            s.Invoke(consoleBuilder);
            ((IStringOutputer) stringOutputer).AddLogger(consoleBuilder);
            return ((IStringOutputer)stringOutputer).Source;
        }

        public static IStringLayoutBuilder ColorConsole(this StringOutputer stringOutputer)
        {
            ((IStringOutputer)stringOutputer).AddLogger(new ConsoleBuilder(((IStringOutputer)stringOutputer).ExecutionMode));
            return ((IStringOutputer)stringOutputer).Source;
        }

        public static IStringLayoutBuilder File(this StringOutputer stringOutputer, Func<IFileConfigurationBuilder, object> s)
        {
            FileConfigurationBuilder fileConfigurationBuilder = new FileConfigurationBuilder(string.Format(@"{{basepath}}{0}Log.txt", Path.DirectorySeparatorChar));
            s.Invoke(fileConfigurationBuilder);
            var sfab = new StringFileAppenderBuilder(((IStringOutputer)stringOutputer).ExecutionMode, fileConfigurationBuilder);
            ((IStringOutputer)stringOutputer).AddLogger(sfab);
            return ((IStringOutputer)stringOutputer).Source;
        }

        public static IStringLayoutBuilder File(this StringOutputer stringOutputer)
        {
            ((IStringOutputer)stringOutputer).AddLogger(new StringFileAppenderBuilder(((IStringOutputer)stringOutputer).ExecutionMode, new FileConfigurationBuilder(string.Format(@"{{basepath}}{0}Log.txt", Path.DirectorySeparatorChar))));
            return ((IStringOutputer)stringOutputer).Source;
        }
    }
}