using System;
using System.Collections.Concurrent;
using Whitelog.Core.FileLog;
using Whitelog.Interface;

namespace Whitelog.Core.Serializer.MemoryBuffer
{
    internal class ThreadStaticBuffer
    {
        public class Buffer : IBuffer
        {
            private byte[] m_buffer;
            private int m_length;
            private readonly RawDataSerializer m_rawDataSerializer = new RawDataSerializer();

            public Buffer(int size)
            {
                m_buffer = new byte[size];
                m_rawDataSerializer.Init(this);
            }

            public void SetLength(int length)
            {
                m_length = length;
            }

            public byte[] IncressSize(int length, int required)
            {
                var buffer = new byte[required];
                System.Buffer.BlockCopy(m_buffer, 0, buffer, 0, length);
                m_length = length;
                m_buffer = buffer;
                return m_buffer;
            }

            public int Length
            {
                get { return m_length; }
            }

            byte[] IRawData.Buffer
            {
                get { return m_buffer; }
                set
                {
                    m_buffer = value;
                    m_length = 0;
                    m_rawDataSerializer.Init(this);
                }
            }

            public void Dispose()
            {
                m_length = 0;
                m_rawDataSerializer.Reset();
            }

            public ISerializer AttachedSerializer
            {
                get { return m_rawDataSerializer; }
            }


            public void Replace(IRawData data)
            {

            }
        }
    }

    internal class ThreadStaticBuffer1 : IBufferAllocator
    {

        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer1(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }

        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer2 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer2(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }

        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer3 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer3(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }


        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer4 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer4(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }


        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer5 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer5(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }


        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer6 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer6(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }


        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer7 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer7(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }


        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer8 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer8(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }


        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }

    internal class ThreadStaticBuffer9 : IBufferAllocator
    {
        [ThreadStatic]
        private static IBuffer m_buffer;

        private ConcurrentQueue<IBufferAllocator> m_bufferAllocators;

        public ThreadStaticBuffer9(ConcurrentQueue<IBufferAllocator> bufferAllocators)
        {
            m_bufferAllocators = bufferAllocators;
        }

        public int BufferSize { get { return BufferDefaults.Size; } }


        public IBuffer Allocate()
        {
            if (m_buffer == null)
            {
                m_buffer = new ThreadStaticBuffer.Buffer(BufferSize);
            }
            return m_buffer;
        }

        public void Dispose()
        {
            m_bufferAllocators.Enqueue(this);
        }
    }
}