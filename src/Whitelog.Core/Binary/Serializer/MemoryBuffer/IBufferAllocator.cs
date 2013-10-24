using System;
using Whitelog.Core.Binary.FileLog;
using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary.Serializer.MemoryBuffer
{
    public interface IBufferAllocatorFactory
    {
        // the IListWriter paramater is a design flow and hopfuly will be removed when a new
        // design will not require it any more.
        // at the moment it is in use only for the ringbuffer allocator
        IBufferAllocator CreateBufferAllocator(IListWriter listWriter);
    }

    public interface IBufferAllocator : IDisposable
    {
        int BufferSize { get; }
        IBuffer Allocate();
    }
}
