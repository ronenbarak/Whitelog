using System;
using System.Collections.Generic;

namespace Whitelog.Barak.Common.DataStructures.Ring
{
    public class Consumer<T>
    {
        private readonly RingBuffer<T> m_ring;
        private readonly Action<IEnumerable<T>> m_consumer;
        private readonly RingEnumerator m_enumerator;
        private long m_currentSeq;
        public class RingEnumerator : IEnumerable<T>, IEnumerator<T>
        {
            private readonly RingBuffer<T> m_ring;
            private Entry<T> m_currentEntry;
            private long m_currentSeq;
            private long m_startSeq;
            public long LastSeq;
            
            // In case we need to reset the enumerable 
            public void SetStartSeq(long seq)
            {
                m_startSeq = seq;
                LastSeq = seq;
                Reset();
            }

            public RingEnumerator(RingBuffer<T> ring)
            {
                m_ring = ring;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this;
            }

            public T Current
            {
                get { return m_currentEntry.Data; }
            }

            public void Dispose()
            {
                Reset();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return m_currentEntry.Data; }
            }

            public bool MoveNext()
            {
                m_currentSeq++;
                m_currentEntry = m_ring[m_currentSeq];
                if (m_currentEntry.State == Entry<T>.StateOptions.Commited && m_currentEntry.Sequence == m_currentSeq)
                {
                    return true;
                }

                m_currentEntry = null;
                LastSeq = m_currentSeq;
                return false;
            }

            public void Reset()
            {
                m_currentSeq = m_startSeq -1;
                m_currentEntry = null;
            }
        }

        public Consumer(RingBuffer<T> ring,Action<IEnumerable<T>> consumer)
        {
            m_currentSeq = 0;
            m_enumerator = new RingEnumerator(ring);
            m_consumer = consumer;
            m_ring = ring;
        }

        public bool TryConsume()
        {
            var entry = m_ring[m_currentSeq];
            if (entry.State == Entry<T>.StateOptions.Commited)
            {
                m_enumerator.SetStartSeq(m_currentSeq);
                m_consumer.Invoke(m_enumerator);

                for (long seq = m_currentSeq; seq < m_enumerator.LastSeq;seq++)
                {
                    m_ring[seq].State = Entry<T>.StateOptions.Consumed;
                }

                m_currentSeq = m_enumerator.LastSeq;
                return true;
            }
            return false;
        }
    }
}