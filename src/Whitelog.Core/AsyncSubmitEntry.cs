using System.Collections.Generic;
using System.Threading;
using Whitelog.Barak.Common.DataStructures.BulkCollector;

namespace Whitelog.Core
{
    public interface ISubmitEntry<T>
    {
        void WaitForIdle();
        void AddEntry(T buffer);
    }

    public interface IAsyncActions<T,T2>
    {
        T2 Clone(T source);
        void HandleBulk(IEnumerable<T2> enumerable);
        void BulkEnded();
    }

    public class AsyncSubmitEntry<T,T2> : ISubmitEntry<T>
    {
        public AsyncSubmitEntry(IAsyncActions<T,T2> asyncActions)
        {
            m_asyncActions = asyncActions;
        }

        private QueuedBulkCollector<T2> m_queuedBulk = new QueuedBulkCollector<T2>();
        private int m_pendingBulksToCollect = 0;
        private object m_locker = new object();
        private IAsyncActions<T, T2> m_asyncActions;

        public void AddEntry(T entry)
        {
            bool isFirstInQueue =false;
            m_queuedBulk.Add(m_asyncActions.Clone(entry), out isFirstInQueue);
            if (isFirstInQueue)
            {
                int threadCount = Interlocked.Increment(ref m_pendingBulksToCollect);
                if (threadCount == 1)
                {
                    // the application might terminate befor the thread run and we will lose entries
                    // for that resean we have WaitForIdle to insure the entries has handled.
                    ThreadPool.QueueUserWorkItem(ExecuteAsync);
                }
            }
        }

        private void ExecuteAsync(object state)
        {
            lock (m_locker)
            {
                bool bContinue = true;
                while (bContinue)
                {
                    var entries = m_queuedBulk.GetBulk();
                    m_asyncActions.HandleBulk(entries);
                    bContinue = Interlocked.Decrement(ref m_pendingBulksToCollect) != 0;
                }

                m_asyncActions.BulkEnded();
            }
        }

        public void WaitForIdle()
        {
            lock (m_locker)
            {
                // This will wait until ExecuteAsync will finish
            }
        }
    }
}