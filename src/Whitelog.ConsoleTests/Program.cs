using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Whitelog.Core;
using Whitelog.Core.Loggers;
using Whitelog.Core.LogScopeSyncImplementation;
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
            LogTunnel logTunnel = new LogTunnel(new SystemDateTime(), new SingleLogPerApplicationScopeSync());

            var consoleLogger = new ConsoleLogger();

            consoleLogger.AttachToTunnelLog(logTunnel);
            consoleLogger.RegisterDefinition(new AllPropertiesPackageDefinition<ComplexData>());

            using (logTunnel.CreateScope("Starting somthing"))
            {
                logTunnel.LogInfo("Test LogEntry {IntValue} {StringValue}", new
                                                                                {
                                                                                    IntValue = 5,
                                                                                    StringValue = "MyValue",
                                                                                    Complex = new ComplexData(){ Complex2 = new ComplexData()},
                                                                                });

                logTunnel.LogInfo("Test LogEntry {*}", new
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
