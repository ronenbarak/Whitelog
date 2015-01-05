using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Binary.Deserilizer
{
    public interface IBufferConsumer
    {
        void Consume(IRawData buffer);
    }

    public interface IListReader
    {
        bool Read(IBufferConsumer bufferConsumer);
        bool ReadAll(IBufferConsumer objectObserver);
    }
}