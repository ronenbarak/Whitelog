using System;

namespace Whitelog.Core.Binary.Serializer.MemoryBuffer
{
    public interface IBufferAllocatorFactory
    {
        IBufferAllocator CreateBufferAllocator();
    }

    public interface IBufferAllocator : IDisposable
    {
        int BufferSize { get; }
        IBuffer Allocate();
    }
}
