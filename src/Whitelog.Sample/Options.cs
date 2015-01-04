using Whitelog.Core.Binary;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Sample
{
    public class SubmiterOption
    {
        public SubmiterOption(ISubmitLogEntryFactory submitLogEntryFactory,string title)
        {
            SubmitLogEntryFactory = submitLogEntryFactory;
            Title = title;
        }

        public ISubmitLogEntryFactory SubmitLogEntryFactory { get; private set; }
        public string Title { get; private set; }

        public override string ToString()
        {
            return Title;
        }
    }

    public class BufferOption
    {
        public BufferOption(IBufferAllocatorFactory bufferAllocatorFactory,string title)
        {
            BufferAllocatorFactory = bufferAllocatorFactory;
            Title = title;
        }

        public IBufferAllocatorFactory BufferAllocatorFactory { get; private set; }
        public string Title { get; private set; }

        public override string ToString()
        {
            return Title;
        }
    }
}