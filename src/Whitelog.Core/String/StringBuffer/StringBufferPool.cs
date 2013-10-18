using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Whitelog.Core.String.StringBuffer
{
    public class StringBufferPool : IStringBuffer
    {
        class PoolItem : IStringBufferAllocation
        {
            private StringBuilder m_stringBuilder = new StringBuilder();
            private StringBufferPool m_stringBufferPool;
            private readonly int m_index;

            public PoolItem(StringBufferPool stringBufferPool, int index)
            {
                m_stringBufferPool = stringBufferPool;
                m_index = index;
            }

            public void Dispose()
            {
                m_stringBuilder.Clear();
                m_stringBufferPool.ReleaseBufferToPool(this, m_index);
            }

            public StringBuilder StringBuilder { get { return m_stringBuilder; } }
        }

        class ExternalItem : IStringBufferAllocation
        {
            private StringBuilder m_stringBuilder = new StringBuilder();

            public void Dispose()
            {
                m_stringBuilder.Clear();
            }

            public StringBuilder StringBuilder { get { return m_stringBuilder; } }
        }


        static StringBufferPool()
        {
            Instance = new StringBufferPool();
        }

        public static StringBufferPool Instance { get; private set; }

        const int PoolSize = 20;
        private readonly IStringBufferAllocation[] pool = new IStringBufferAllocation[PoolSize];

        public StringBufferPool()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = new PoolItem(this, i);
            }
        }

        public IStringBufferAllocation Allocate()
        {
            IStringBufferAllocation tmp;
            for (int i = 0; i < pool.Length; i++)
            {
                if ((tmp = Interlocked.Exchange(ref pool[i], null)) != null) return tmp;
            }

            return new ExternalItem();
        }

        internal void ReleaseBufferToPool(IStringBufferAllocation buffer, int index)
        {
            Interlocked.CompareExchange(ref pool[index], buffer, null);
        }
    }
}
