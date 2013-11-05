using Whitelog.Core.Configuration.Fluent;
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
            ILog logTunnel = Whilelog.FluentConfiguration
                        .StringLayout(builder => builder.SetLayout("${longdate} ${title} ${message}")
                                                        .Extensions(extensions => extensions.All)
                                                        .Define(new AllPropertiesPackageDefinition<ComplexData>())
                              .Appenders.Console(consoleBuilder => consoleBuilder.Sync.Colors.Default))
                .CreateLog();

            WriteSomeLogs(logTunnel);

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
