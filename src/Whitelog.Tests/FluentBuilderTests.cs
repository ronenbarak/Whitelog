using System;
using NUnit.Framework;
using Whitelog.Core.Configuration.Fluent;
using Whitelog.Core.Configuration.Fluent.StringLayout;
using Whitelog.Interface;
using System.IO;

namespace Whitelog.Tests
{
    [TestFixture]
    public class FluentBuilderTests
    {
        [Test]
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
					.Config(file => file.FilePath(@"{basedir}" + Path.DirectorySeparatorChar + "log.dat")))
                    .CreateLog();
        }
    }
}