using System;
using System.IO;
using Whitelog.Core.FileLog;

namespace Whitelog.Core.Reader.ExpendableList
{
    public class ExpandableLogReaderFactory : ILogReaderFactory
    {
        public Guid Id
        {
            get { return ContinuesBinaryFileLogger.ID; }
        }

        public ILogReader Create(Stream data, ILogConsumer consumer)
        {
            byte[] guidBuffer = new byte[16];
            data.Read(guidBuffer, 0, guidBuffer.Length);

            if (new Guid(guidBuffer) != Whitelog.Core.ListWriter.ExpendableList.ID)
            {
                throw new CorruptedDataException();
            }

            return new ExpendableListLogReader(consumer, new Whitelog.Core.ListWriter.ExpendableList(data));

        }
    }

    public class InMemoryLogReaderFactory : ILogReaderFactory
    {
        public Guid Id
        {
            get { return InMemmoryBinaryFileLogger.ID; }
        }

        public ILogReader Create(Stream data, ILogConsumer consumer)
        {
            byte[] guidBuffer = new byte[16];
            data.Read(guidBuffer, 0, guidBuffer.Length);

            if (new Guid(guidBuffer) != Whitelog.Core.ListWriter.ExpendableList.ID)
            {
                throw new CorruptedDataException();
            }

            return new ExpendableListLogReader(consumer, new Whitelog.Core.ListWriter.ExpendableList(data));
        }
    }
}