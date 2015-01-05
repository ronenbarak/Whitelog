using System;
using System.IO;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Loggers.Binary;

namespace Whitelog.Core.Binary.Deserilizer.Reader.ExpendableList
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

            if (new Guid(guidBuffer) != ExpendableListWriter.ID)
            {
                throw new CorruptedDataException();
            }

            return new ExpendableListLogReader(consumer, new ExpendableListReader(data));

        }
    }
}