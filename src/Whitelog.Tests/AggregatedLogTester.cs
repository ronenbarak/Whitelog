using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whitelog.Barak.Common.ExtensionMethods;
using Whitelog.Core;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.FileLog.SubmitLogEntry;
using Whitelog.Core.Binary.Generic;
using Whitelog.Core.Binary.Reader;
using Whitelog.Core.Binary.Reader.ExpendableList;
using Whitelog.Core.Loggers;
using Whitelog.Core.LogScopeSyncImplementation;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Tests
{
    public static class ValidationExtentions
    {
        public static object GetValue(this ILogEntryData genericPackageData, string memberName)
        {
            var property = genericPackageData.GetProperties().FirstOrDefault(p => p.Name == memberName);
            return property.GetValue(genericPackageData);
        }

        public static ILogEntryData ValidateLogEntry<T, T2>(this ILogEntryData genericPackageData, Expression<Func<T, object>> expression, params Action<ILogEntryData>[] subItems)
        {
            var memberName = ObjectHelper.GetMemberName(expression);
            var property = genericPackageData.GetProperties().FirstOrDefault(p => p.Name == memberName);
            Assert.IsNotNull(property);
            Assert.AreEqual(typeof(object), property.Type);
            var value = property.GetValue(genericPackageData) as GenericPackageData;
            Assert.AreEqual(typeof(T2).FullName, value.GetEntryType().FullName);
            foreach (var subItem in subItems)
            {
                subItem.Invoke(value);
            }

            return genericPackageData;
        }

        public static ILogEntryData ValidateArrayLogEntry<T, T2>(this ILogEntryData genericPackageData, int index, Expression<Func<T, object>> expression, params Action<ILogEntryData>[] subItems)
        {
            var memberName = ObjectHelper.GetMemberName(expression);
            var property = genericPackageData.GetProperties().FirstOrDefault(p => p.Name == memberName);
            Assert.IsNotNull(property);
            Assert.AreEqual(typeof(Array), property.Type);
            var value = property.GetValue(genericPackageData) as object[];
            Assert.AreEqual(typeof(T2).FullName, (value.ElementAt(index) as GenericPackageData).GetEntryType().FullName);
            foreach (var subItem in subItems)
            {
                subItem.Invoke(value.ElementAt(index) as GenericPackageData);
            }

            return genericPackageData;
        }

        public static ILogEntryData ValidatePropertyLogEntry<T, TPropertyType>(this ILogEntryData genericPackageData, Expression<Func<T, TPropertyType>> expression, TPropertyType value)
        {
            var memberName = ObjectHelper.GetMemberName(expression);
            var property = genericPackageData.GetProperties().FirstOrDefault(p => p.Name == memberName);

            Assert.IsNotNull(property);
            Assert.AreEqual(typeof(TPropertyType), property.Type);

            Assert.AreEqual(value, property.GetValue(genericPackageData));

            return genericPackageData;
        }
    }

    [TestClass]
    public class AggregatedLogMultiAplicationTester : AggregatedLogSingleApplicationTester
    {
        protected override LogTunnel CreateLog()
        {
            return new LogTunnel(new SystemDateTime(), new MultiLogPerApplicationScopeSync());
        }
    }

    class TestConsumer : ILogConsumer
    {
        private List<ILogEntryData> m_logs = new List<ILogEntryData>();
        public void Consume(ILogEntryData entryData)
        {
            m_logs.Add(entryData);
        }

        public IEnumerable<ILogEntryData> Logs()
        {
            var list = m_logs.ToList();
            m_logs.Clear();
            return list;
        }
        
    }
    [TestClass]
    public class AggregatedLogSingleApplicationTester
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
        public void Can_Read_And_Write_Logs_Larger_Then_The_Buffer()
        {
            int messageSize = 1024 * 1024; // 1M
            string message = "";
            for (int i = 0; i < messageSize; i++)
            {
                message += i.ToString();
                if (message.Length > (1024 * 20))
                {
                    break;
                }
            }

            m_log.LogInfo("Some Title", message);
            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
            logEntries.ElementAt(0).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, 0)
                //.ValidatePropertyLogEntry<LogEntry, IEnumerable>(x => x.Paramaeters, null)
                .ValidateLogEntry<LogEntry, string>(entry => entry.Paramaeter, data =>
                                                                               {
                                                                                   Assert.AreEqual(message, data.GetValue("ToString"));
                                                                               });
        }

        [TestMethod]
        public void CanWriteSingleLogEntry()
        {
            m_log.LogInfo("SomeInfo");
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

        [TestMethod]
        public void CanWriteMultiLogEntry()
        {
            m_log.LogInfo("SomeInfo");
            m_log.LogError("SomeError");


            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();

            Assert.AreEqual(2, logEntries.Count());
            logEntries.ElementAt(0).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, 0)
                                    .ValidatePropertyLogEntry<LogEntry, object>(x => x.Paramaeter, null)
                                    .ValidateLogEntry<LogEntry, InfoLogTitle>(x => x.Title,
                                            x => x.ValidatePropertyLogEntry<InfoLogTitle, string>(p => p.Message, "SomeInfo"));

            logEntries.ElementAt(1).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, 0)
                                    .ValidatePropertyLogEntry<LogEntry, object>(x => x.Paramaeter, null)
                                    .ValidateLogEntry<LogEntry, ErrorLogTitle>(x => x.Title,
                                            x => x.ValidatePropertyLogEntry<InfoLogTitle, string>(p => p.Message, "SomeError"));
        }

        [TestMethod]
        public void CanReadTheSecondTime()
        {
            m_log.LogInfo("SomeInfo");

            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();

            Assert.AreEqual(1, logEntries.Count());
            logEntries.ElementAt(0).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, 0)
                                    .ValidatePropertyLogEntry<LogEntry, object>(x => x.Paramaeter, null)
                                    .ValidateLogEntry<LogEntry, InfoLogTitle>(x => x.Title,
                                            x => x.ValidatePropertyLogEntry<InfoLogTitle, string>(p => p.Message, "SomeInfo"));

            m_log.LogError("SomeError");

            Assert.IsTrue(m_logReader.TryRead());
            logEntries = m_testConsumer.Logs();

            Assert.AreEqual(1, logEntries.Count());
            logEntries.ElementAt(0).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, 0)
                                    .ValidatePropertyLogEntry<LogEntry, object>(x => x.Paramaeter, null)
                                    .ValidateLogEntry<LogEntry, ErrorLogTitle>(x => x.Title,
                                            x => x.ValidatePropertyLogEntry<ErrorLogTitle, string>(p => p.Message, "SomeError"));
        }

        [TestMethod]
        public void ScopeCreateOpenAndCloseScope()
        {
            int scopeid;
            using (var scope = m_log.CreateScope("ScopeTile"))
            {
                scopeid = scope.LogScopeId;
            }
            Assert.AreNotEqual(0, scopeid);

            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();

            Assert.AreEqual(2, logEntries.Count());

            logEntries.ElementAt(0).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, scopeid)
                                            .ValidateLogEntry<LogEntry, OpenLogScopeTitle>(x => x.Title,
                                                x => x.ValidatePropertyLogEntry<OpenLogScopeTitle, string>(p => p.Message, "ScopeTile"),
                                                x => x.ValidatePropertyLogEntry<OpenLogScopeTitle, int>(p => p.ParentLogId, 0));

            logEntries.ElementAt(1).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, scopeid)
                                            .ValidateLogEntry<LogEntry, CloseLogScopeTitle>(x => x.Title, x => { });
        }

        [TestMethod]
        public void DeepLogAggregationWorks()
        {
            int firstScopeId;
            int secondScopeId;
            int thirdScopeId;

            using (var firstScope = m_log.CreateScope("FirstScope"))
            {
                firstScopeId = firstScope.LogScopeId;
                using (var secondScope = m_log.CreateScope("SecondScope"))
                {
                    m_log.LogInfo("SomeLogData");
                    secondScopeId = secondScope.LogScopeId;
                }

                m_log.LogInfo("MiddleData");

                using (var thirdScope = m_log.CreateScope("thirdScope"))
                {
                    m_log.LogInfo("SomeOtherLogData");
                    thirdScopeId = thirdScope.LogScopeId;
                }
            }
            m_log.LogWarning("EndData");

            Assert.IsTrue(firstScopeId != secondScopeId && secondScopeId != thirdScopeId && firstScopeId != thirdScopeId);

            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
            Assert.AreEqual(10, logEntries.Count());

            logEntries.ElementAt(0).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, firstScopeId); // Open 1 

            // Open 2
            logEntries.ElementAt(1).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, secondScopeId)
                                                .ValidateLogEntry<LogEntry, OpenLogScopeTitle>(x => x.Title,
                                                    x => x.ValidatePropertyLogEntry<OpenLogScopeTitle, int>(p => p.ParentLogId, firstScopeId));

            logEntries.ElementAt(2).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, secondScopeId); // Data 2
            logEntries.ElementAt(3).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, secondScopeId); // Close 2
            logEntries.ElementAt(4).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, firstScopeId); // Data 1

            // Open 3
            logEntries.ElementAt(5).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, thirdScopeId)
                                        .ValidateLogEntry<LogEntry, OpenLogScopeTitle>(x => x.Title,
                                                    x => x.ValidatePropertyLogEntry<OpenLogScopeTitle, int>(p => p.ParentLogId, firstScopeId));

            logEntries.ElementAt(6).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, thirdScopeId); // Data 3
            logEntries.ElementAt(7).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, thirdScopeId); // Close 3
            logEntries.ElementAt(8).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, firstScopeId); // Close 1
            logEntries.ElementAt(9).ValidatePropertyLogEntry<LogEntry, int>(x => x.LogScopeId, 0); // EndData
        }

        [TestMethod]
        public void LogCanWorkFromMutliThreads()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            ManualResetEvent manualResetEvent2 = new ManualResetEvent(false);
            CountdownEvent countdownEvent = new CountdownEvent(2);
            int threadLogScopeID = -1;
            int normalLogScopeID = -1;
            Thread t = new Thread(() =>
            {
                using (var threadScope = m_log.CreateScope("ThreadScope"))
                {
                    threadLogScopeID = threadScope.LogScopeId;
                    manualResetEvent.Set();
                    m_log.LogInfo("In thread");
                    manualResetEvent2.WaitOne();
                }
                countdownEvent.Signal();
            });

            t.Start();

            using (var normalScope = m_log.CreateScope("NormalScope"))
            {
                normalLogScopeID = normalScope.LogScopeId;
                manualResetEvent.WaitOne();
                m_log.LogInfo("In Noraml");
                manualResetEvent2.Set();
            }
            countdownEvent.Signal();
            countdownEvent.Wait();

            Assert.AreNotEqual(normalLogScopeID, threadLogScopeID);
            Assert.AreNotEqual(-1, threadLogScopeID);
            Assert.AreNotEqual(-1, normalLogScopeID);

            Assert.IsTrue(m_logReader.TryRead());
            var logEntries = m_testConsumer.Logs();
            Assert.AreEqual(6, logEntries.Count());

            logEntries = logEntries.Where(p => (p.GetValue("Title") as GenericPackageData).GetEntryType().FullName == typeof(InfoLogTitle).FullName).ToList();

            int logScope = (int)(logEntries.First().GetValue("LogScopeId"));
            string data = (logEntries.First().GetValue("Title") as GenericPackageData).GetValue("Message") as string;
            int otherLogScope = (int)(logEntries.Last().GetValue("LogScopeId"));
            string otherData = (logEntries.Last().GetValue("Title") as GenericPackageData).GetValue("Message") as string; ;

            Assert.AreNotEqual(data, otherData);

            if (data == "In thread")
            {
                Assert.AreEqual(threadLogScopeID, logScope);
            }
            else if (data == "In Noraml")
            {
                Assert.AreEqual(normalLogScopeID, logScope);
            }
            else
            {
                Assert.Fail();
            }

            if (otherData == "In thread")
            {
                Assert.AreEqual(threadLogScopeID, otherLogScope);
            }
            else if (otherData == "In Noraml")
            {
                Assert.AreEqual(normalLogScopeID, otherLogScope);
            }
            else
            {
                Assert.Fail();
            }
        }


    }
}
