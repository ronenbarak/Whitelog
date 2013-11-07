using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whitelog.Barak.SystemDateTime;
using Whitelog.Core;
using Whitelog.Core.Binary.FileLog.SubmitLogEntry;
using Whitelog.Core.Binary.Reader;
using Whitelog.Core.Binary.Reader.ExpendableList;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Tests
{
    [TestClass]
    public class RingBufferLogTests
    {
        [TestMethod]
        public void SimpleLogMessageWork()
        {
            var log = new LogTunnel(new SystemDateTime(), LogScopeSyncFactory.Create());

            using (var ringBufferLogFactory = new RingSubmitLogEntryFactory(RingConsumeOption.SpinWait, 100))
            {
                var memoryStream = new MemoryStream();
                var m_cfl = new ContinuesBinaryFileLogger(new InMemoryStreamProvider(memoryStream), ringBufferLogFactory, ringBufferLogFactory);
                m_cfl.AttachToTunnelLog(log);
                ringBufferLogFactory.WaitForIdle();
                var readerFactory = new WhitelogBinaryReaderFactory();
                readerFactory.RegisterReaderFactory(new ExpandableLogReaderFactory());

                log.LogInfo("SomeInfo");
                ringBufferLogFactory.WaitForIdle();
            
                var m_testConsumer = new TestConsumer();
                var m_logReader = readerFactory.GetLogReader(memoryStream, m_testConsumer);
                Assert.IsTrue(m_logReader.TryRead());
                var logEntries = m_testConsumer.Logs();
            

                Assert.AreEqual(1, logEntries.Count());
                logEntries.ElementAt(0).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, 0)
                    .ValidatePropertyLogEntry<LogEntry, object>(x => x.Paramaeter, null)
                    .ValidateLogEntry<LogEntry, InfoLogTitle>(x => x.Title,
                                                                x =>
                                                                x.ValidatePropertyLogEntry<InfoLogTitle, string>(
                                                                    p => p.Message, "SomeInfo"));   
            }
        }
    }
}
