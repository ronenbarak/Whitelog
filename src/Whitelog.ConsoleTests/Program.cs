using System;
using Whitelog.Core.Configuration.Fluent;
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

        public class ComplexDataDerived :ComplexData
        {
            public ComplexDataDerived()
            {
                ExtraProp = 2;
            }
            public int ExtraProp { get; set; }
        }

        static void Main(string[] args)
        {
            ILog logTunnel = Whilelog.Configure
                                .Time.DateTimeNow
                                .Logger.BinaryFile(binary => binary.Buffer.ThreadStatic                                    
                                                                       .Mode.Async
                                                                       .File(x => x.ArchiveEvery.Size(1024*1024*10))
                                                       )
                                .Logger.String(layout => layout.SetLayout("${longdate} ${title} ${message}")
                                                                   .Extensions.All
                                                                   .Filter.Exclude(LogTitles.Close)
                                                                   .OutputTo.Sync.ColorConsole(console => console.Filter.Exclude(LogTitles.Open)
                                                                                                                 .Colors.Conditions(conditions => conditions.Condition(LogTitles.Info, ConsoleColor.DarkGreen)
                                                                                                                        .Condition(LogTitles.Warning, ConsoleColor.DarkMagenta, ConsoleColor.White)))
                                                                   .OutputTo.Async.File(file=>file.Path("Log.txt").ArchiveEvery.Hour))
                                .CreateLog();

            WriteSomeLogs(logTunnel);
            
            Console.WriteLine();

            // just as good
            ILog logTunnel2 = Whilelog.Configure.Logger.String(p=>p.OutputTo.Sync.ColorConsole()).CreateLog();

            WriteSomeLogs(logTunnel2);

            // Let write an array

            logTunnel2.Info("This is an Array: {*}", new
            {
                MyArray = new []{1,2}
            });

            Console.ReadLine();
        }

        private static void WriteSomeLogs(ILog logTunnel)
        {
            using (logTunnel.CreateScope("Starting somthing"))
            {
                logTunnel.Info("Test LogEntry {IntValue} {StringValue}", new
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

                logTunnel.Warning("My Warning");

                logTunnel.Log("Custom", "Non spesific log title");

                logTunnel.Error("Test LogEntry {*}", new
                                                        {
                                                            IntValue = 5,
                                                            StringValue = "MyValue",
                                                            Complex = new ComplexData()
                                                                      {
                                                                          //ExtraProp = 2,
                                                                          Complex2 = new ComplexDataDerived(),
                                                                      }
                                                        });

            }
        }
    }
}
