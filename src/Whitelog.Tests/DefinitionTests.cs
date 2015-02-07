using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Whitelog.Core;
using Whitelog.Core.Binary.Deserilizer.Reader;
using Whitelog.Core.Binary.Deserilizer.Reader.ExpendableList;
using Whitelog.Core.Configuration.Fluent;
using Whitelog.Interface;

namespace Whitelog.Tests
{
    [TestFixture]
    public class DefinitionTests
    {
        class DummClass
        {
            public string DummyValue { get; set; }
        }

        private class AllTypesClass
        {
            public double DoubleValue { get; set; }
            public bool BoolValue { get; set; }
            public DateTime DateTimeValue { get; set; }
            public int IntValue { get; set; }
            public byte ByteValue { get; set; }
            public string StringValue { get; set; }
            public int[] EnumerableValue1 { get; set; }
            public IEnumerable<int> EnumerableValue2 { get; set; }
            public IEnumerable<string> EnumerableValue3 { get; set; }
            public IEnumerable<object> EnumerableValue4 { get; set; }
            public List<object> EnumerableValue5 { get; set; }
            public IEnumerable EnumerableValue6 { get; set; }
            public Guid GuidValue { get; set; }
            public uint UInt32Value { get; set; }
            public ushort UShortValue { get; set; }
            public sbyte SByte { get; set; }
            public short Short { get; set; }
            public ulong UInt64Value { get; set; }
            public float FloatValue { get; set; }
            public char CharValue { get; set; }
            public decimal DecimalValue { get; set; }
            public object ObjectValue { get; set; }
        }
        
        //public int VariantUInt32Value { get; set; }
        //public int ConstString { get; set; }

        [SetUp]
        [TearDown]
        public static void TestCleanUp()
        {
            System.IO.File.Delete("b.Data");
            System.IO.File.Delete("b.txt");
        }
       
        [Test]
        public void SerilizeAllTypes()
        {
            LogTunnel logger = Whilelog.Configure.Logger.BinaryFile(b => b.Mode.Sync
                                                                        .File(f => f.Path("b.Data") ))
                                                 .Logger.String(layout => layout.OutputTo.Sync.ColorConsole()
                                                                        .OutputTo.Sync.File(f =>f.Path("b.txt") ))
                        .CreateLog() as LogTunnel;

            Guid g = Guid.NewGuid();
            DateTime d = DateTime.UtcNow;
            logger.Debug("this Test Every thing {*}", new
            {
                DoubleValue = 1,
                BoolValue = true,
                DateTimeValue = d,
                IntValue = 3,
                ByteValue = (byte)4,
                StringValue = "5",
                EnumerableValue1 = new int[]{1,2,3},
                EnumerableValue2 = new int[]{1,2,3}.Take(2),
                EnumerableValue3 = Enumerable.Empty<string>(),
                EnumerableValue4 = (List<object>)null,
                EnumerableValue5 = new List<object>(){1,2,3},
                EnumerableValue6 = new List<int>(){1,2,3},
                GuidValue = g,
                UInt32Value = (UInt32)(UInt32.MaxValue -1),
                UShortValue = (ushort)(ushort.MaxValue -2),
                SByte = (sbyte)8,
                Short = (short) 7,
                UInt64Value = (UInt64) (Int64.MaxValue - 3),
                FloatValue = (float)8,
                CharValue = 'a',
                DecimalValue = (decimal) (decimal.MaxValue -4),
                ObjectValue = new DummClass() { DummyValue = "D For Dummy"}
            });

            logger.Info("Somthing elesae");
            /*var readerFactory = new WhitelogBinaryReaderFactory();
            readerFactory.RegisterReaderFactory(new ExpandableLogReaderFactory());
            var testConsumer = new TestConsumer();

            var m_logReader = readerFactory.GetLogReader(new MemoryStream(System.IO.File.ReadAllBytes("b.Data")), testConsumer);

            Assert.IsTrue(m_logReader.TryRead());*/

            //log.Info("MyMessage ",);

            logger = null;

            GC.Collect();
        }
    }
}
