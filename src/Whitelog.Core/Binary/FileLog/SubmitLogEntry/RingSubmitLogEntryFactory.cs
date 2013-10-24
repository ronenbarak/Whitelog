using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Whitelog.Barak.Common.DataStructures.Ring;
using Whitelog.Core.Binary.ListWriter;
using Whitelog.Core.Binary.Serializer.MemoryBuffer;

namespace Whitelog.Core.Binary.FileLog.SubmitLogEntry
{
    public enum RingConsumeOption
    {
        SpinWait
    }

    public class RingSubmitLogEntryFactory : ISubmitLogEntryFactory, IBufferAllocatorFactory,IDisposable
    {
        ConcurrentDictionary<IListWriter,Tuple<ISubmitLogEntry,IBufferAllocator>> m_factories = new ConcurrentDictionary<IListWriter, Tuple<ISubmitLogEntry, IBufferAllocator>>();
        private RingConsumeOption m_ringConsumeOption;
        private int m_ringSize;

        public RingSubmitLogEntryFactory(RingConsumeOption ringConsumeOption,int ringSize)
        {
            m_ringSize = ringSize;
            m_ringConsumeOption = ringConsumeOption;
        }

        public ISubmitLogEntry CreateSubmitLogEntry(IListWriter listWriter)
        {
            Tuple<ISubmitLogEntry, IBufferAllocator> factories = GetFactories(listWriter);
            return factories.Item1;
        }

        public IBufferAllocator CreateBufferAllocator(IListWriter listWriter)
        {
            Tuple<ISubmitLogEntry, IBufferAllocator> factories  = GetFactories(listWriter);
            return factories.Item2;
        }

        private Tuple<ISubmitLogEntry, IBufferAllocator> GetFactories(IListWriter listWriter)
        {
            Tuple<ISubmitLogEntry, IBufferAllocator> factories = m_factories.GetOrAdd(listWriter, writer =>
                                                  {
                                                      var submiterAndBuffer = new RingSubmitLogEntry(m_ringConsumeOption, m_ringSize, listWriter);
                                                      factories = new Tuple<ISubmitLogEntry, IBufferAllocator>(submiterAndBuffer, submiterAndBuffer);
                                                      return factories;
                                                  });
            return factories;
        }

        public void WaitForIdle()
        {
            foreach (var factory in m_factories.Values)
            {
                factory.Item1.WaitForIdle();
            }
        }

        public void Dispose()
        {
            foreach (var factory in m_factories.Values)
            {
                factory.Item2.Dispose();
            }
        }
    }

    class RingSubmitLogEntry : IBufferAllocator, ISubmitLogEntry
    {
        private RingBuffer<RingLogBuffer> m_ring;
        private Consumer<RingLogBuffer> m_consumer;
        private Thread m_thread;
        private bool m_dispose = false;
        private IListWriter m_listWriter;
        private bool m_isCheckIdeal = false;
        private bool m_isIdeal = false;

        public RingSubmitLogEntry(RingConsumeOption ringConsumeOption, int ringSize, IListWriter listWriter)
        {
            m_listWriter = listWriter;
            m_ring = new RingBuffer<RingLogBuffer>(ringSize, ring1 => new RingLogBuffer(BufferSize, ring1));
            m_consumer = new Consumer<RingLogBuffer>(m_ring, OnConsume);
            if (ringConsumeOption == RingConsumeOption.SpinWait)
            {
                m_thread = new Thread(ConsumeLoop);
                m_thread.IsBackground = true;
                m_thread.Start();
            }   
        }

        private void OnConsume(IEnumerable<RingLogBuffer> ringLogBuffers)
        {
            lock (m_listWriter.LockObject)
            {
                m_listWriter.WriteData(ringLogBuffers);
                m_listWriter.Flush();
            }
        }

        private void ConsumeLoop()
        {
            SpinWait spinWait = new SpinWait();
            while (!m_dispose)
            {
                if (!m_consumer.TryConsume())
                {
                    if (m_isCheckIdeal)
                    {
                        m_isCheckIdeal = false;
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

        public void Dispose()
        {
            m_dispose = true;
            m_thread.Join();
        }

        public int BufferSize { get { return 1024; } }

        public IBuffer Allocate()
        {
            RingLogBuffer ringLogBuffer;
            var seq = m_ring.GetNextEntry(out ringLogBuffer);
            ringLogBuffer.SetSequance(seq);
            return ringLogBuffer;
        }

        public void WaitForIdle()
        {
            lock (m_ring)
            {
                m_isIdeal = false;
                m_isCheckIdeal = true;
                while (!m_isIdeal)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        }

        public void AddLogEntry(IRawData buffer)
        {
            // Do nothing it will happedn in the buffer dispose
        }
    }
}