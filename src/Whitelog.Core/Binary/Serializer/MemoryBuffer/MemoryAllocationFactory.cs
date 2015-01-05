namespace Whitelog.Core.Binary.Serializer.MemoryBuffer
{
    public class MemoryAllocationFactory : IBufferAllocatorFactory
    {
        class MemoryAllocation : IBufferAllocator
        {
            public void Dispose()
            {
            }

            public int BufferSize { get { return 256; } }

            public IBuffer Allocate()
            {
                return new ThreadStaticBuffer.Buffer(BufferSize);
            }
        }
        public IBufferAllocator CreateBufferAllocator()
        {
            return new MemoryAllocation();
        }
    }
}