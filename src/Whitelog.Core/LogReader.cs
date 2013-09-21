using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whitelog.Barak.Common.Events;
using Whitelog.Interface;
using Whitelog.Core.FileLog;
using Whitelog.Core.Generic;
using Whitelog.Core.ListWriter;
using Whitelog.Core.PakageDefinitions.Unpack;
using Whitelog.Core.Serializer;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core
{
    public class UnkownLogFileException : Exception
    {
        public UnkownLogFileException()
            : base("The Log is in invalid format")
        {
        }
    }

    public class LogReader : IDisposable
    {
        private Stream m_stream;
        private IListWriter m_logData;
        private Unpacker m_dataUnpacker = new Unpacker();
        private IBufferAllocatorFactory m_bufferAllocatorFactory;

        public LogReader(Stream stream, BinaryPackageDefinitionToGenericUnpackageDefinition packageDefinitionToGenericUnpackageDefinition,IBufferAllocatorFactory bufferAllocatorFactory)
        {
            m_bufferAllocatorFactory = bufferAllocatorFactory;
            packageDefinitionToGenericUnpackageDefinition.PackageDefinitionRegistred += new EventHandler<EventArgs<GenericUnpackageDefinition>>(OnPackageDefinitionRegistred);
            m_dataUnpacker.AddPackageDefinition(packageDefinitionToGenericUnpackageDefinition);
            m_dataUnpacker.AddPackageDefinition(new GenericPropertyUnpackageDefinition());
            m_dataUnpacker.AddPackageDefinition(new ConstGenericPropertyUnpackageDefinition());
            m_dataUnpacker.AddPackageDefinition(new CacheStringUnpackageDefinition());

            stream.Position = 0;
            m_stream = stream;
            byte[] guidBuffer = new byte[16];
            m_stream.Read(guidBuffer, 0, guidBuffer.Length);
            if (new Guid(guidBuffer) == ContinuesBinaryFileLogger.ID ||
                new Guid(guidBuffer) == InMemmoryBinaryFileLogger.ID)
            {
                PrepareContinuesExpendableListReader();
            }
            else
            {
                throw new UnkownLogFileException();
            }
        }

        public LogReader(string file, IBufferAllocatorFactory bufferAllocatorFactory)
            : this(System.IO.File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                   new BinaryPackageDefinitionToGenericUnpackageDefinition(),
                   bufferAllocatorFactory)
        {

        }

        public LogReader(Stream stream, IBufferAllocatorFactory bufferAllocatorFactory)
            : this(stream, new BinaryPackageDefinitionToGenericUnpackageDefinition(), bufferAllocatorFactory)
        {
        }

        void OnPackageDefinitionRegistred(object sender, EventArgs<GenericUnpackageDefinition> e)
        {
            m_dataUnpacker.AddPackageDefinition(e.Data);
        }

        private void PrepareContinuesExpendableListReader()
        {
            byte[] guidBuffer = new byte[16];
            if (m_stream.Read(guidBuffer, 0, guidBuffer.Length) != guidBuffer.Length)
            {
                throw new CorruptedDataException();
            }
            if (new Guid(guidBuffer) != ExpendableList.ID)
            {
                throw new CorruptedDataException();
            }
            byte[] longBuffer = new byte[sizeof(long)];           
            m_stream.Read(longBuffer, 0, longBuffer.Length);
            m_logData = new ExpendableList(m_stream.Position, m_stream, m_bufferAllocatorFactory.CreateBufferAllocator());
        }

        public IEnumerable<ILogEntryData> ReadAll()
        {
            var logReaderObjectObserver = new LogReaderObjectObserver(m_dataUnpacker);
            m_logData.ReadAll(logReaderObjectObserver);

            return logReaderObjectObserver.GetCollection().OfType<ILogEntryData>();
        }

        public void Dispose()
        {
            m_stream.Dispose();
        }
    }
}
