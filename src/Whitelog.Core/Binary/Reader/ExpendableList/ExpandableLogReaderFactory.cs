using System;
using System.IO;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Loggers;

namespace Whitelog.Core.Binary.Reader.ExpendableList
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

            if (new Guid(guidBuffer) != ListWriter.ExpendableListWriter.ID)
            {
                throw new CorruptedDataException();
            }

            return new ExpendableListLogReader(consumer, new ExpendableListReader(data));

        }
    }
}