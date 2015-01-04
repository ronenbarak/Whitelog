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
            ILog log = Whilelog.Configure
                    .Time.DateTimeNow
                    .Logger.String(x => x.SetLayout("${longdate} ${Title} ${Message}")
                                  .Extensions.All
                                  .OutputTo.Sync.ColorConsole(c => c.Filter.Exclude(LogTitles.Trace)
                                                                    .Colors.Conditions(con => con
                                                                                        .Condition(entry => entry.Paramaeter == null, ConsoleColor.Gray)
                                                                                        .Condition(LogTitles.Debug,ConsoleColor.Blue)
                                                                                        .Condition(LogTitles.Info, ConsoleColor.DarkYellow,ConsoleColor.Green)))
                                  .OutputTo.Async.File(b => b.Append
                                                             .ArchiveEvery.Hour
                                                             .Path("Log.txt")))
                    .Logger.BinaryFile(b => b.Buffer.ThreadStatic
                                             .Mode.Async
                                             .File(builder => builder.ArchiveEvery.Size(1024 * 1024 * 10)
                                                                                    .ArchivePath("{basedir}" + Path.DirectorySeparatorChar + "Archive.log")
                                                                                    .MaxArchiveFiles(10)
                                                                                    .Path("BinaryFile {DateTime}.log")))
                    .Logger.Json()
                    .CreateLog();
        }
    }
}