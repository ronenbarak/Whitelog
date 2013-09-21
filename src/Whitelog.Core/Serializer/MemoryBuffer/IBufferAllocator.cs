using System;
using Whitelog.Core.FileLog;

namespace Whitelog.Core.Serializer.MemoryBuffer
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
