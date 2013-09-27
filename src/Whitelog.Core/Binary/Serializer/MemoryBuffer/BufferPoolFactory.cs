namespace Whitelog.Core.Binary.Serializer.MemoryBuffer
{
    public class BufferPoolFactory : IBufferAllocatorFactory
    {
        public static BufferPoolFactory Instance { get; private set; }

        static BufferPoolFactory()
        {
            Instance = new BufferPoolFactory();
        }

        private readonly BufferPoolProxy m_bufferPoolProxy = new BufferPoolProxy();



        private BufferPoolFactory()
        {
        }
        public IBufferAllocator CreateBufferAllocator()
        {
            return m_bufferPoolProxy;
        }
    }
}