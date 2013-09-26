using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whitelog.Core;
using Whitelog.Core.FileLog;
using Whitelog.Core.FileLog.SubmitLogEntry;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Core.Reader;
using Whitelog.Core.Reader.ExpendableList;
using Whitelog.Interface;

namespace Whitelog.Tests
{
    [TestClass]
    public class AnonymousPackerTests
    {
        private ILog m_log;
        private InMemmoryBinaryFileLogger m_cfl;
        private TestConsumer m_testConsumer;
        private ILogReader m_logReader;

        protected virtual LogTunnel CreateLog()
        {
            return new LogTunnel(new SystemDateTime(), new SingleLogPerApplicationScopeSync());
        }

        [TestInitialize]
        public void ActivateLog()
        {
            LogTunnel tunnelLog;
            m_log = tunnelLog = CreateLog();
            m_cfl = new InMemmoryBinaryFileLogger(new SyncSubmitLogEntryFactory());
            m_cfl.AttachToTunnelLog(tunnelLog);

            var readerFactory = new WhitelogBinaryReaderFactory();
            readerFactory.RegisterReaderFactory(new ExpandableLogReaderFactory());
            readerFactory.RegisterReaderFactory(new InMemoryLogReaderFactory());
            m_testConsumer = new TestConsumer();
            m_logReader = readerFactory.GetLogReader(m_cfl.Stream, m_testConsumer);
        }

        [TestCleanup]
        public void DeactivateLog()
        {
            m_cfl.Dispose();
        }

        [TestMethod]
        public void CanWriteDataWithAnnunumonClassWithNoParameters()
        {
            m_log.LogInfo("AnonymousTest", new
                                     {
                                     });

            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();

        }

        [TestMethod]
        public void CanWriteDataWithAnnunumonClass()
        {
            m_log.LogInfo("AnonymousTest", new
            {
                Item1 = 4,
                Item2 = "Ronen"
            });
            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
        }

        [TestMethod]
        public void HittingTheSameAnonymousClassTwice()
        {
            for (int i = 0; i < 2; i++)
            {
                m_log.LogInfo("AnonymousTest", new
                {
                    Item1 = 4 +i,
                    Item2 = "Ronen" +i
                });   
            }
            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
        }

        [TestMethod]
        public void CanWriteDataWithAnnunumonClassWithArrays()
        {
            m_log.LogInfo("AnonymousTest", new
            {
                Item1 = new List<int>{4,5,6},
                Item2 = (IEnumerable<int>) new List<int> { 1, 2, 3 }
            });
            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
        }
    }
}
