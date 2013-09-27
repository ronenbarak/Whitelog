using Whitelog.Core.Binary.FileLog;

namespace Whitelog.Core.Binary.Serializer.MemoryBuffer
{
    public class BufferPoolProxy:  IBufferAllocator
    {
        public static BufferPoolProxy Instance { get; private set; }
        private BufferPool m_bufferPool = new BufferPool();
        
        static  BufferPoolProxy()
        {
            Instance = new BufferPoolProxy();
        }

        public IBuffer Allocate()
        {
            return m_bufferPool.GetBuffer();
        }

        public void Dispose()
        {
        }

        public int BufferSize
        {
            get { return BufferPool.BufferLength; }
        }
    }
}