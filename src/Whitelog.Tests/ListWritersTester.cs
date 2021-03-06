﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Whitelog.Barak.Common.ExtensionMethods;
using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Deserilizer;
using Whitelog.Core.Binary.Deserilizer.Reader;
using Whitelog.Core.Binary.Deserilizer.Unpack;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;
using Whitelog.Core.File;
using Whitelog.Core.Loggers.Binary;
using Whitelog.Core.PackageDefinitions;

namespace Whitelog.Tests
{
    public class PackData
    {
        public int IntData { get; set; }
        public string StringData { get; set; }
        public DateTime DateTimeData { get; set; }
        public List<PackData> ArrayPackData { get; set; }
    }

    public static class ListWriterExtensions
    {
        class BufferConsumer : IBufferConsumer
        {
            public IRawData m_buffer;

            public void Consume(IRawData buffer)
            {
                m_buffer = buffer;
            }
        }

        public static byte[] Read(this IListReader listWriter)
        {
            var buffer = new BufferConsumer();
            if (!listWriter.Read(buffer))
            {
                return null;
            }
            return buffer.m_buffer.Buffer;
        }
    }

    [TestFixture]
    public class ListWritersTester
    {
        [Test]
        public void TestContinuesExpendableList()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.SetLength(1024);
                ms.Position = 1024;
                ExpendableListWriter fixSizeWriter = new ExpendableListWriter(new InMemoryStreamProvider(ms),(writer, stream) => {});
                fixSizeWriter.WriteData(new CloneRawData(BitConverter.GetBytes(1)));
                fixSizeWriter.WriteData(new CloneRawData(BitConverter.GetBytes(2)));
                fixSizeWriter.WriteData(new CloneRawData(BitConverter.GetBytes(3)));

                ms.Position = 1024;
                ExpendableListReader readerSizeWriter = new ExpendableListReader(ms);
                Assert.AreEqual(1, BitConverter.ToInt32(readerSizeWriter.Read(), 0));
                Assert.AreEqual(2, BitConverter.ToInt32(readerSizeWriter.Read(), 0));
                Assert.AreEqual(3, BitConverter.ToInt32(readerSizeWriter.Read(), 0));
                Assert.IsNull(readerSizeWriter.Read());
            }
        }

        [Test]
        public void TestInstanceCreatorPackUnpack()
        {
            using (IBuffer buffer = new SimpleBuffer())
            {
                RawDataSerializer ms = new RawDataSerializer(buffer);
                BinaryPackager packer = new BinaryPackager();

                RegisteredPackageDefinition registeredPackageDefinition;
                packer.RegisterDefinition(new PackageDefinition<PackData>(), out registeredPackageDefinition);

                packer.Pack(new PackData(), ms);

                using (StreamDeserilizer deserilizer = new StreamDeserilizer(buffer.Buffer))
                {
                    Unpacker unpacker = new Unpacker();
                    unpacker.AddPackageDefinition(new EmptyConstractorUnpackageDefinition<PackData>() { DefinitionId = registeredPackageDefinition.DefinitionId });

					Assert.IsNotNull(unpacker.Unpack<PackData>(deserilizer));
                }
            }
        }

        [Test]
        public void TestSimplePropertyPackUnpack()
        {
            using (IBuffer buffer = new SimpleBuffer())
            {
                RawDataSerializer ms = new RawDataSerializer(buffer);
                BinaryPackager packer = new BinaryPackager();

                RegisteredPackageDefinition registeredPackageDefinition;
                packer.RegisterDefinition(new PackageDefinition<PackData>()
                                                        .Define(x => x.IntData, x => x.IntData)
                                                        .Define(x => x.StringData, x => x.StringData)
                                                        .Define(x => x.DateTimeData, x => x.DateTimeData), out registeredPackageDefinition);
                var packData = new PackData()
                {
                    ArrayPackData = null,
                    DateTimeData = DateTime.Now,
                    IntData = 124,
                    StringData = "Some String",
                };

                packer.Pack(packData, ms);

                using (StreamDeserilizer deserilizer = new StreamDeserilizer(buffer.Buffer))
                {
                    Unpacker unpacker = new Unpacker();
                    unpacker.AddPackageDefinition(new EmptyConstractorUnpackageDefinition<PackData>()
                    {
                        DefinitionId = registeredPackageDefinition.DefinitionId
                    }
                                                      .DefineInt("IntData", (data, i) => data.IntData = i)
                                                      .DefineString("StringData", (data, s) => data.StringData = s)
                                                      .DefineDateTime("DateTimeData",
                                                                      (data, time) => data.DateTimeData = time));

                    var unpackData = unpacker.Unpack<PackData>(deserilizer);
					Assert.IsNotNull(unpackData);
                    CheckValidPackData(packData, unpackData);
                    Assert.AreEqual(packData.ArrayPackData, unpackData.ArrayPackData);
                }
            }
        }

        [Test]
        public void TestArrayWithSimplePropertyPackUnpack()
        {
            using (IBuffer buffer = new SimpleBuffer())
            {
                RawDataSerializer ms = new RawDataSerializer(buffer);
                BinaryPackager packer = new BinaryPackager();

                RegisteredPackageDefinition registeredPackageDefinition;
                packer.RegisterDefinition(new PackageDefinition<PackData>()
                                                        .Define(x => x.IntData, x => x.IntData)
                                                        .Define(x => x.StringData, x => x.StringData)
                                                        .Define(x => x.ArrayPackData, x => x.ArrayPackData)
                                                        .Define(x => x.DateTimeData, x => x.DateTimeData), out registeredPackageDefinition);
                var packData = new PackData()
                {
                    ArrayPackData = new List<PackData>()
                                        {
                                            new PackData()
                                            {
                                                IntData =1,
                                                StringData = "First String",
                                            },
                                            new PackData()
                                            {
                                                IntData =2,
                                                DateTimeData = DateTime.Now + TimeSpan.FromHours(2),
                                                ArrayPackData =  new List<PackData>(),
                                            },
                                            new PackData()
                                            {
                                                StringData = string.Empty
                                            },
                                        },
                    DateTimeData = DateTime.Now,
                    IntData = 124,
                    StringData = "Some String",
                };

                packer.Pack(packData, ms);

                using (StreamDeserilizer deserilizer = new StreamDeserilizer(buffer.Buffer))
                {

                    Unpacker unpacker = new Unpacker();
                    unpacker.AddPackageDefinition(new EmptyConstractorUnpackageDefinition<PackData>()
                    {
                        DefinitionId = registeredPackageDefinition.DefinitionId
                    }
                                                      .DefineInt("IntData", (data, i) => data.IntData = i)
                                                      .DefineString("StringData", (data, s) => data.StringData = s)
                                                      .DefineEnumerable("ArrayPackData",
                                                                        (data, enumerable) => data.ArrayPackData = enumerable.NullGurd(x => x.OfType<PackData>().ToList()))
                                                      .DefineDateTime("DateTimeData", (data, time) => data.DateTimeData = time));

                    var unpackData = unpacker.Unpack<PackData>(deserilizer);
					Assert.IsNotNull(unpackData);
                    CheckValidPackData(unpackData, packData);
                    Assert.IsNotNull(packData.ArrayPackData);

                    Assert.AreEqual(3, packData.ArrayPackData.Count);

                    CheckValidPackData(packData.ArrayPackData[0], unpackData.ArrayPackData[0]);
                    Assert.IsNull(unpackData.ArrayPackData[0].ArrayPackData);

                    CheckValidPackData(packData.ArrayPackData[1], unpackData.ArrayPackData[1]);
                    Assert.IsNotNull(unpackData.ArrayPackData[1].ArrayPackData);
                    Assert.AreEqual(0, unpackData.ArrayPackData[1].ArrayPackData.Count);

                    CheckValidPackData(packData.ArrayPackData[2], unpackData.ArrayPackData[2]);
                    Assert.IsNull(unpackData.ArrayPackData[2].ArrayPackData);
                }
            }
        }

        private static void CheckValidPackData(PackData unpackData, PackData packData)
        {
            Assert.AreEqual(packData.IntData, unpackData.IntData);
            Assert.AreEqual(packData.StringData, unpackData.StringData);
            Assert.AreEqual(packData.DateTimeData, unpackData.DateTimeData);
        }
    }
}
