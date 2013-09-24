using System.Collections.Generic;
using System.Threading;
using Whitelog.Barak.Common.DataStructures.Ring;
using Whitelog.Core.ListWriter;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.FileLog.SubmitLogEntry
{
    public enum RingConsumeOption
    {
        //BusySpin,
        SpinWait
    }

    /// <summary>
    /// Not Supported by the reader, dont use!!!
    /// this class is experamental 
    /// </summary>
    public class RingSubmitLogEntryFactory : ISubmitLogEntryFactory, IBufferAllocatorFactory, IBufferAllocator, ISubmitLogEntry
    {
        private RingBuffer<RingLogBuffer> m_ring;
        private IListWriter m_listWriter;
        private bool m_isIdeal = false;
        private bool m_isCheckIdeal = false;
        private Consumer<RingLogBuffer> m_consumer;

        public RingSubmitLogEntryFactory(RingConsumeOption ringConsumeOption,int ringSize)
        {
            m_ring = new RingBuffer<RingLogBuffer>(ringSize, ring1 => new RingLogBuffer(BufferSize, ring1));
            m_consumer = new Consumer<RingLogBuffer>(m_ring,OnConsume);
            if (ringConsumeOption == RingConsumeOption.SpinWait)
            {
                Thread t = new Thread(ConsumeLoop);
                t.IsBackground = true;
                t.Start();
            }
        }

        private void ConsumeLoop()
        {
            m_isIdeal = false;
            SpinWait spinWait = new SpinWait();
            while(true)
            {
                if (!m_consumer.TryConsume())
                {
                    if (m_isCheckIdeal)
                    {
                        m_isIdeal = true;
                    }
                    spinWait.SpinOnce();
                }
                else
                {
                    spinWait.Reset();   
                }
            }
        }

        private void OnConsume(IEnumerable<RingLogBuffer> ringLogBuffers)
        {
            lock(m_listWriter.LockObject)
            {
                m_listWriter.WriteData(ringLogBuffers);
            }
        }

        public ISubmitLogEntry CreateSubmitLogEntry(IListWriter listWriter)
        {
            m_listWriter = listWriter;
            return this;
        }

        public int BufferSize
        {
            get { return 1024*4; }
        }

        public IBuffer Allocate()
        {
            RingLogBuffer ringLogBuffer;
            var seq = m_ring.GetNextEntry(out ringLogBuffer);
            ringLogBuffer.SetSequance(seq);
            return ringLogBuffer;
        }

        public void Dispose()
        {
        }

        public void WaitForIdle()
        {
            m_isCheckIdeal = true;
            m_isIdeal = false;
            while(!m_isIdeal)
            {
                System.Threading.Thread.Sleep(1);
            }
        }

        public void AddLogEntry(IRawData buffer)
        {
            // Do nothing it will happedn in the buffer dispose
        }

        public IBufferAllocator CreateBufferAllocator()
        {
            return this;
        }
    }
}