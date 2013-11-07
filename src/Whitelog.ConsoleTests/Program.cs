using System;
using Whitelog.Barak.SystemDateTime;
using Whitelog.Core.Binary.FileLog.SubmitLogEntry;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.Configuration.Fluent;
using Whitelog.Core.File;
using Whitelog.Core.Loggers.StringAppender.File;
using Whitelog.Core.PackageDefinitions;
using Whitelog.Interface;

namespace Whitelog.ConsoleTests
{
    class Program
    {
        public class ComplexData
        {
            public ComplexData()
            {
                Prop1 = 6;
                Prop2 = "ComplexValue";
            }

            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
            public ComplexData Complex2 { get; set; }
        }

        static void Main(string[] args)
        {
            var stringWriter = new StringListWriter(new FileStreamProvider(new FileConfiguration()));
            ILog logTunnel = Whilelog.FluentConfiguration
                        .StringLayout(builder => builder.SetLayout("${longdate} ${title} ${message}")
                                                        .Extensions(extensions => extensions.All)
                                                        .Filter.Exclude(LogTitles.Close)
                                                        .Define(new AllPropertiesPackageDefinition<ComplexData>())
                              .Appenders

                              .Custom(new StringFileAppender(ThreadStaticBufferFactory.Instance.CreateBufferAllocator(stringWriter), new SyncSubmitLogEntry(stringWriter)))
                                .Console(consoleBuilder => consoleBuilder.Sync
                                                                                 .Filter
                                                                                 .Exclude(LogTitles.Open)
                                                                                 .Colors.Conditions(conditions => conditions.Condition(LogTitles.Info,ConsoleColor.DarkGreen)
                                                                                                                            .Condition(LogTitles.Warning, ConsoleColor.DarkMagenta,ConsoleColor.White))))
                .CreateLog();

            WriteSomeLogs(logTunnel);
            
            Console.WriteLine();

            // just as good
            ILog logTunnel2 = Whilelog.FluentConfiguration
                        .StringLayout(builder => builder.Appenders.Console())
                        .CreateLog();

            WriteSomeLogs(logTunnel2);
        }

        private static void WriteSomeLogs(ILog logTunnel)
        {
            using (logTunnel.CreateScope("Starting somthing"))
            {
                logTunnel.LogInfo("Test LogEntry {IntValue} {StringValue}", new
                                                                            {
                                                                                IntValue = 5,
                                                                                StringValue = "MyValue",
                                                                                Complex =
                                                                                    new ComplexData()
                                                                                    {
                                                                                        Complex2 =
                                                                                            new ComplexData()
                                                                                    },
                                                                            });

                logTunnel.LogWarning("My Warning");
                logTunnel.Log("Custom", "Non spesific log title");

                logTunnel.LogError("Test LogEntry {*}", new
                                                        {
                                                            IntValue = 5,
                                                            StringValue = "MyValue",
                                                            Complex = new ComplexData()
                                                                      {
                                                                          Complex2 = new ComplexData()
                                                                                     {
                                                                                         Prop1 = 6,
                                                                                     }
                                                                      },
                                                        });
            }
        }
    }
}
