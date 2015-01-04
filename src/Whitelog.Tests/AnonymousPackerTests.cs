using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Whitelog.Barak.SystemDateTime;
using Whitelog.Core;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.Reader;
using Whitelog.Core.Binary.Reader.ExpendableList;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Interface;

namespace Whitelog.Tests
{
    [TestFixture]
    public class AnonymousPackerTests
    {
        private ILog m_log;
        private ContinuesBinaryFileLogger m_cfl;
        private TestConsumer m_testConsumer;
        private ILogReader m_logReader;

        protected virtual LogTunnel CreateLog()
        {
            return new LogTunnel(new SystemDateTime(), LogScopeSyncFactory.Create());
        }

        [TestFixtureSetUp]
        public void ActivateLog()
        {
            byte[] buffer = new byte[1024 * 1024 * 10];
            LogTunnel tunnelLog;
            m_log = tunnelLog = CreateLog();
            m_cfl = new ContinuesBinaryFileLogger(new InMemoryStreamProvider(new MemoryStream(buffer)), new SyncSubmitLogEntryFactory(), BufferPoolFactory.Instance);
            m_cfl.AttachToTunnelLog(tunnelLog);

            var readerFactory = new WhitelogBinaryReaderFactory();
            readerFactory.RegisterReaderFactory(new ExpandableLogReaderFactory());
            m_testConsumer = new TestConsumer();

            m_logReader = readerFactory.GetLogReader(new MemoryStream(buffer), m_testConsumer);
        }

        [TestFixtureTearDown]
        public void DeactivateLog()
        {
            m_cfl.Dispose();
        }

        [Test]
        public void CanWriteDataWithAnnunumonClassWithNoParameters()
        {
            m_log.Info("AnonymousTest", new
                                     {
                                     });

            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();

        }

        [Test]
        public void CanWriteDataWithAnnunumonClass()
        {
            m_log.Info("AnonymousTest", new
            {
                Item1 = 4,
                Item2 = "Ronen"
            });
            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
        }

        [Test]
        public void HittingTheSameAnonymousClassTwice()
        {
            for (int i = 0; i < 2; i++)
            {
                m_log.Info("AnonymousTest", new
                {
                    Item1 = 4 +i,
                    Item2 = "Ronen" +i
                });   
            }
            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
        }

        [Test]
        public void CanWriteDataWithAnnunumonClassWithArrays()
        {
            m_log.Info("AnonymousTest", new
            {
                Item1 = new List<int>{4,5,6},
                Item2 = (IEnumerable<int>) new List<int> { 1, 2, 3 }
            });
            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
        }
    }
}
