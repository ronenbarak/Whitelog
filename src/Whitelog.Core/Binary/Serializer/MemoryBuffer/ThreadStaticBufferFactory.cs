using System.Collections.Concurrent;
using Whitelog.Core.Binary.ListWriter;

namespace Whitelog.Core.Binary.Serializer.MemoryBuffer
{
    public class ThreadStaticBufferFactory : IBufferAllocatorFactory
    {
        private readonly ConcurrentQueue<IBufferAllocator> m_bufferAllocators = new ConcurrentQueue<IBufferAllocator>();
        private readonly BufferPoolProxy m_bufferPoolProxy = new BufferPoolProxy();

        public static ThreadStaticBufferFactory Instance { get; private set; }
        
        static ThreadStaticBufferFactory()
        {
            Instance = new ThreadStaticBufferFactory();
        }

        private ThreadStaticBufferFactory()
        {
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer1(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer2(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer3(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer4(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer5(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer6(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer7(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer8(m_bufferAllocators));
            m_bufferAllocators.Enqueue(new ThreadStaticBuffer9(m_bufferAllocators));

        }

        public IBufferAllocator CreateBufferAllocator(IListWriter listWriter)
        {
            IBufferAllocator bufferAllocator;
            if (m_bufferAllocators.TryDequeue(out bufferAllocator))
            {
                return bufferAllocator;
            }

            return m_bufferPoolProxy;
        }
    }
}