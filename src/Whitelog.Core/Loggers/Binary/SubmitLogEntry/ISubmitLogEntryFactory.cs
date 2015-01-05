using Whitelog.Core.Binary;
using Whitelog.Core.Binary.Serializer;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Loggers.Binary.SubmitLogEntry
{
    public interface ISubmitLogEntryFactory
    {
        ISubmitEntry<IRawData> CreateSubmitLogEntry(IListWriter listWriter);
    }
}