using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whitelog.Core.Configuration.Fluent;
using Whitelog.Core.Configuration.Fluent.StringLayout;
using Whitelog.Interface;

namespace Whitelog.Tests
{
    [TestClass]
    public class FluentBuilderTests
    {
        [TestMethod]
        public void BuildLogFromNothing()
        {
            ILog log = Whilelog.FluentConfiguration
                    .Time(SystemTime.DateTimeNow)
                    .StringLayout(x => x.SetLayout("${longdate} ${Title} ${Message}")
                                        .Extensions(extensions => extensions.All)
                        .Appenders
                            .Console(console=>console.Async
                                .Colors.Conditions(con => con
                                    .Condition(entry => entry.Paramaeter == null, ConsoleColor.Gray)
                                    .Condition(LogTitles.Debug,ConsoleColor.Blue)
                                    .Condition(LogTitles.Info, ConsoleColor.DarkYellow,ConsoleColor.Green))))
                    .Binary(x =>x.ExecutionMode(ExecutionMode.Async)
                                 .Buffer(Buffers.ThreadStatic)
                                 .Config(file => file.FilePath(@"{basedir}\log.dat")))
                    .CreateLog();
        }
    }
}